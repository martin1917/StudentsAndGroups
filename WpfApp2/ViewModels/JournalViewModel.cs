using WpfApp2.ViewModels.Base;
using WpfApp2.State;
using System.Data;
using WpfApp2.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WpfApp2.Data;
using System.Windows.Input;
using WpfApp2.Infrastructure.Commands;
using System;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Entity;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WpfApp2.ViewModels.JournalDoalogVM;
using WpfApp2.Views.Windows.JournalDialogs;
using WpfApp2.Extensions;
using System.Windows;
using System.Collections.ObjectModel;

namespace WpfApp2.ViewModels;

public class JournalViewModel : BaseViewModel
{
	private readonly IMapper _mapper;

	public JournalViewModel(IMapper mapper) : base(ViewModelType.AcademyJournal)
	{
		_mapper = mapper;
        LoadData();
    }

	private void LoadData()
	{
		var ctx = ContextFactory.CreateContext();
        GroupModels = _mapper.Map<List<GroupModel>>(ctx.Groups);
		//SubjectModels = _mapper.Map<List<SubjectModel>>(ctx.Subjects);
	}

    public ObservableCollection<SubjectModel> SubjectModels { get; private set;  } = new();

    private SubjectModel _selectedSubject;
    public SubjectModel SelectedSubject { get => _selectedSubject; set => Set(ref _selectedSubject, value); }

	public List<GroupModel> GroupModels { get; set; }

    private GroupModel _selectedGroup;
	public GroupModel SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

    public List<string> Months { get; set; } = new()
    {
        "Январь",
        "Февраль",
        "Март",
        "Апрель",
        "Май",
        "Июнь",
        "Июль",
        "Август",
        "Сентябрь",
        "Октябрь",
        "Ноябрь",
        "Декабрь",
    };

    private string _selectedMonth;
	public string SelectedMonth { get => _selectedMonth; set => Set(ref _selectedMonth, value); }

    private string _year = $"{DateTime.Now.Year}";
	public string Year { get => _year; set => Set(ref _year, value); }

    private DataTable? _dataTable;
    public DataTable? DataTable { get => _dataTable; set => Set(ref _dataTable, value); }

    private List<DateOnly> uniqueDates;

    private ICommand _loadSubjectCommand;
    public ICommand LoadSubjectCommand => _loadSubjectCommand
        ??= new Command(OnLoadSubjectCommandExecuted);

    private void OnLoadSubjectCommandExecuted(object? param)
    {
        var ctx = ContextFactory.CreateContext();

        var deltaYear = DateTime.Now.Year - SelectedGroup.DateCreated.Year;
        var deltaMonth = DateTime.Now.Month - SelectedGroup.DateCreated.Month;
        var num = deltaMonth > 0 ? deltaYear + 1 : deltaYear;

        SubjectModels.Clear();
        foreach(var subject in ctx.SubjectGroups.Where(sg => sg.NumGroup == num).Select(sg => sg.Subject))
        {
            SubjectModels.Add(_mapper.Map<SubjectModel>(subject));
        }
    }

    private ICommand _loadMarksCommand;
	public ICommand LoadMarksCommand => _loadMarksCommand
        ??= new Command(OnLoadMarksCommandExecuted, CanLoadMarksCommandExecute);

	private bool CanLoadMarksCommandExecute(object? param)
	{
        return SelectedSubject != null
            && SelectedGroup != null
            && SelectedMonth != null;
	}

	private void OnLoadMarksCommandExecuted(object? param)
    {
        if (!int.TryParse(Year, out _))
        {
            var error = "Год должен быть числом";
            MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        LoadMarks();
    }

    private void LoadMarks()
    {
        _ = int.TryParse(Year, out int year);
        var month = Months.IndexOf(SelectedMonth) + 1;
        DateOnly startDate = new DateOnly(year, month, 1);
        DateOnly endDate = month != 12
            ? new DateOnly(year, month + 1, 1)
            : new DateOnly(year + 1, 1, 1);

        var ctx = ContextFactory.CreateContext();

        // запрос на получение всех оценок по предмету для группы за указанный месяц
        var effectiveLogs = ctx.AcademicPerformanceLogs.FromSqlRaw("SELECT " +
            "apl.Id as Id, " +
            "apl.SubjectId as SubjectId, " +
            "apl.GroupId as GroupId, " +
            "apl.StudentId as StudentId, " +
            "apl.Mark as Mark, " +
            "apl.Date as Date " +
            "from AcademicPerformanceLogs apl " +
            $"where apl.GroupId == {SelectedGroup.Id} and " +
            $"apl.SubjectId == {SelectedSubject.Id} and " +
            $"apl.Date >= date('{startDate.ConvertToSQLiteFormat()}') and " +
            $"apl.Date < date('{endDate.ConvertToSQLiteFormat()}')");

        // уникальные даты
        uniqueDates = effectiveLogs.AsEnumerable()
            .Select(i => i.Date)
            .Distinct()
            .OrderBy(i => i)
            .ToList();

        var SetOfStudentMarks = effectiveLogs.AsEnumerable()
            .GroupBy(i => i.StudentId)
            .Select(gr => new StudentAndMarks(
                gr.Key,
                gr.GroupBy(i => i.Date).Select(i => new MarkDetail(
                    i.Select(x => x.Mark).ToList(),
                    i.Key)).ToList()));

        DataTable tmpTable = new();

        var students = ctx.Students.Where(s => s.GroupId == SelectedGroup.Id);

        tmpTable.Columns.Add("ID", typeof(int));
        tmpTable.Columns.Add("Студент", typeof(string));
        foreach (var date in uniqueDates)
        {
            tmpTable.Columns.Add($"{date.Day} {date.Month} {date.Year}", typeof(string));
        }

        foreach (var student in students)
        {
            var row = tmpTable.NewRow();
            row["ID"] = student.Id;
            row["Студент"] = $"{student.SecondName} {student.FirstName[0]}.{student.Patronymic[0]}.";

            var marksForStudent = SetOfStudentMarks.FirstOrDefault(el => el.StudentId == student.Id);

            if (marksForStudent == null)
            {
                foreach (var date in uniqueDates)
                {
                    row[$"{date.Day} {date.Month} {date.Year}"] = string.Empty;
                }
            }
            else
            {
                foreach (var date in uniqueDates)
                {
                    var marksInDay = marksForStudent.MarkDetails.FirstOrDefault(i => i.Date == date);
                    var marks = marksInDay != null ? marksInDay.MarksToString() : string.Empty;
                    row[$"{date.Day} {date.Month} {date.Year}"] = marks;
                }
            }

            tmpTable.Rows.Add(row);
        }

        DataTable?.Dispose();
        DataTable = tmpTable;
    }

    private ICommand _addMarksCommand;
    public ICommand AddMarksCommand => _addMarksCommand
        ??= new Command(OnAddMarksCommandExecuted, CanAddMarksCommandExecute);

    private bool CanAddMarksCommandExecute(object? arg)
    {
        return DataTable != null;
    }

    private void OnAddMarksCommandExecuted(object? obj)
    {
        if (!int.TryParse(Year, out int year))
        {
            return;
        }

        var ctx = ContextFactory.CreateContext();

        var month = Months.IndexOf(SelectedMonth) + 1;
        var studentModels = _mapper.Map<List<StudentModel>>(ctx.Students.Where(s => s.GroupId == SelectedGroup.Id));
        var vm = new JournalAddMarksViewModel(studentModels, year, month, uniqueDates);
        var window = new JournalAddMarks { DataContext = vm };
        if (window.ShowDialog() == false) return;

        foreach (var item in vm.StudentsAndMarks)
        {
            var entityId = item.StudentModel.Id;
            var marksString = Regex.Replace(item.Marks.Trim(), @"\s+", " ").Split(", ");
            if (string.IsNullOrEmpty(marksString[0])) continue;
            var marks = marksString.Select(i => int.TryParse(i, out int res) ? res : -1);
            foreach (var mark in marks)
            {
                var data = new AcademicPerformanceLog
                {
                    Date = new DateOnly(year, month, vm.Day.Value),
                    Mark = mark == -1 ? null : mark,
                    StudentId = entityId,
                    GroupId = SelectedGroup.Id,
                    SubjectId = SelectedSubject.Id
                };

                ctx.AcademicPerformanceLogs.Add(data);
            }
        }

        ctx.SaveChanges();
        LoadMarks();
    }

    private ICommand _addCommand;
    public ICommand AddCommand => _addCommand
        ??= new Command(OnAddCommandExecuted);

    private void OnAddCommandExecuted(object? obj)
    {
        var dataGrid = (DataGrid)((MouseButtonEventArgs)obj!).Source;

        var itemsSource = dataGrid.ItemsSource;
        if (itemsSource == null) return;

        var columnHeader = (dataGrid.CurrentCell.Column.Header as string);
        var item = (DataRowView)dataGrid.CurrentCell.Item;

        var partsColumnHeader = columnHeader
            .Split(" ")
            .Select(i => int.Parse(i))
            .ToList();

        var ctx = ContextFactory.CreateContext();
        var date = new DateOnly(partsColumnHeader[2], partsColumnHeader[1], partsColumnHeader[0]);
        var id = (int)item.Row["ID"];
        var marks = (string)item.Row[columnHeader];
        var student = ctx.Students.First(s => s.Id == id);

        var vm = new JournalEditMarksViewModel(student, date, marks);
        var window = new JournalEditMarks { DataContext = vm };
        if (window.ShowDialog() == false) return;

        item.Row[columnHeader] = vm.Marks;

        var marksBefore = Regex.Replace(marks.Trim().ToUpper(), @"\s+", "").Split(",");
        var marksAfter = Regex.Replace(vm.Marks.Trim().ToUpper(), @"\s+", "").Split(",");

        var prevMarks = marksBefore.Select(i => i == "Н" ? (int?)null : int.Parse(i));
        var newMarks = marksAfter.Select(i => i == "Н" ? (int?)null : int.Parse(i));

        // remove
        foreach (var prevMark in prevMarks.Except(newMarks))
        {
            var entity = ctx.AcademicPerformanceLogs
                .First(apl => apl.SubjectId == SelectedSubject.Id
                    && apl.GroupId == student.GroupId
                    && apl.StudentId == student.Id
                    && apl.Date == date
                    && apl.Mark == prevMark);

            ctx.Entry(entity).State = EntityState.Deleted;
        }

        // add
        foreach (var newMark in newMarks.Except(prevMarks))
        {
            ctx.AcademicPerformanceLogs.Add(new AcademicPerformanceLog
            {
                SubjectId = SelectedSubject.Id,
                GroupId = student.GroupId,
                StudentId = student.Id,
                Date = date,
                Mark = newMark
            });
        }

        ctx.SaveChanges();
    }
}