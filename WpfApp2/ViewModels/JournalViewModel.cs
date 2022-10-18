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
using WpfApp2.Views.Windows;
using WpfApp2.Entity;
using System.Text.RegularExpressions;

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
		SubjectModels = _mapper.Map<List<SubjectModel>>(ctx.Subjects);
	}

	public List<SubjectModel> SubjectModels { get; set; }

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

    private string _year;
	public string Year { get => _year; set => Set(ref _year, value); }

    private DataTable? _dataTable;
    public DataTable? DataTable { get => _dataTable; set => Set(ref _dataTable, value); }

    private bool dataIsLoaded = false;
    private List<DateOnly> uniqueDates;

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
        LoadMarks();
    }

    private void LoadMarks()
    {
        var resultParsing = int.TryParse(Year, out int year);
        if (!resultParsing)
        {
            return;
        }

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
            $"apl.Date >= date('{startDate.Year}-{startDate.Month}-0{startDate.Day}') and " +
            $"apl.Date < date('{endDate.Year}-{endDate.Month}-0{endDate.Day}')").ToList();

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

        var extendedTable = new List<StudentAndMarks>();
        foreach (var studentMarks in SetOfStudentMarks)
        {
            var marks = new List<MarkDetail>();

            foreach (var date in uniqueDates)
            {
                var markDetailInDate = studentMarks.GetByDate(date);
                marks.Add(new MarkDetail(markDetailInDate != null
                    ? markDetailInDate.Marks
                    : new List<int?>(), date));
            }

            extendedTable.Add(new StudentAndMarks(studentMarks.StudentId, marks));
        }

        DataTable tmpTable = new();

        tmpTable.Columns.Add("ID", typeof(int));
        tmpTable.Columns.Add("Студент", typeof(string));
        foreach (var date in uniqueDates)
        {
            tmpTable.Columns.Add($"{date.Day} {date.Month} {date.Year}", typeof(string));
        }

        foreach (var studentAndMarks in extendedTable)
        {
            var student = ctx.Students.First(s => s.Id == studentAndMarks.StudentId);

            var row = tmpTable.NewRow();
            row["ID"] = student.Id;
            row["Студент"] = $"{student.SecondName} {student.FirstName[0]}.{student.Patronymic[0]}"; ;
            foreach (var markDetail in studentAndMarks.MarkDetails)
            {
                row[$"{markDetail.Date.Day} {markDetail.Date.Month} {markDetail.Date.Year}"] = markDetail.MarksToString();
            }

            tmpTable.Rows.Add(row);
        }

        DataTable?.Dispose();
        DataTable = tmpTable;
        dataIsLoaded = true;
    }

    private ICommand _addMarksCommand;
    public ICommand AddMarksCommand => _addMarksCommand
        ??= new Command(OnAddMarksCommandExecuted, CanAddMarksCommandExecute);

    private bool CanAddMarksCommandExecute(object? arg)
    {
        return dataIsLoaded;
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
}
