using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.State;
using WpfApp2.Extensions;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

/// <summary> VM со средними оценками по предметам </summary>
public class AvgMarksViewModel : BaseViewModel
{
    private IMapper _mapper;
    private GroupModel _selectedGroup;
    private string _selectedMonth;
    private string _year = $"{DateTime.Now.Year}";
    private DataTable? _dataTable;
    private ICommand _loadAvgMarks;
    private ICommand _saveExcelFileCommand;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="mapper">Маппер, для отображения одних сущностей на другие</param>
    public AvgMarksViewModel(IMapper mapper) : base (ViewModelType.AvgMarks)
	{
        _mapper = mapper;        
        LoadData();
    }

    private void LoadData()
    {
        var ctx = ContextFactory.CreateContext();
        GroupModels = _mapper.Map<List<GroupModel>>(ctx.Groups);
    }

    /// <summary> Группы </summary>
    public List<GroupModel> GroupModels { get; set; }

    /// <summary> Выбаррная группа </summary>
    public GroupModel SelectedGroup { get => _selectedGroup; set => Set(ref _selectedGroup, value); }

    /// <summary> Месяца </summary>
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

    /// <summary> Выбранный месяц </summary>
    public string SelectedMonth { get => _selectedMonth; set => Set(ref _selectedMonth, value); }

    /// <summary> Год </summary>
    public string Year { get => _year; set => Set(ref _year, value); }

    /// <summary> Таблица с данными </summary>
    public DataTable? DataTable { get => _dataTable; set => Set(ref _dataTable, value); }

    /// <summary> Загрузка средних оценок </summary>
    public ICommand LoadAvgMarks => 
        _loadAvgMarks ??= new Command(OnLoadAvgMarksExecuted, CanLoadAvgMarksExecute);

    private bool CanLoadAvgMarksExecute(object? param)
        => SelectedGroup != null && SelectedMonth != null;

    private void OnLoadAvgMarksExecuted(object? param)
    {
        var resultParsing = int.TryParse(Year, out int year);
        if (!resultParsing)
        {
            var error = "Год должен быть числом";
            MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var month = Months.IndexOf(SelectedMonth) + 1;
        DateOnly startDate = new DateOnly(year, month, 1);
        DateOnly endDate = month != 12
            ? new DateOnly(year, month + 1, 1)
            : new DateOnly(year + 1, 1, 1);

        var query =
            "SELECT " +
            "apl.Id as Id, " +
            "apl.SubjectId as SubjectId, " +
            "apl.GroupId as GroupId, " +
            "apl.StudentId as StudentId, " +
            "apl.Mark as Mark, " +
            "apl.Date as Date " +
            "FROM AcademicPerformanceLogs apl " +
            $"WHERE apl.GroupId == {SelectedGroup.Id} AND " +
            $"apl.Date >= date('{startDate.ConvertToSQLiteFormat()}') AND " +
            $"apl.Date < date('{endDate.ConvertToSQLiteFormat()}') ";

        var ctx = ContextFactory.CreateContext();
        var data = ctx.AcademicPerformanceLogs
            .FromSqlRaw(query)
            .GroupBy(apl => new { apl.StudentId, apl.SubjectId })
            .Select(i => new AvgMark(
                i.Key.StudentId,
                i.Key.SubjectId,
                i.Average(i => i.Mark) ?? 0
            )).ToList();


        var deltaYear = DateTime.Now.Year - SelectedGroup.DateCreated.Year;
        var deltaMonth = DateTime.Now.Month - SelectedGroup.DateCreated.Month;
        var num = deltaMonth > 0 ? deltaYear + 1 : deltaYear;

        var subjects = ctx.SubjectGroups.Where(sg => sg.NumGroup == num).Select(sg => sg.Subject);
        var students = ctx.Students.Where(s => s.GroupId == SelectedGroup.Id);

        DataTable tmpTable = new();

        tmpTable.Columns.Add("ID", typeof(int));
        tmpTable.Columns.Add("Студент", typeof(string));
        foreach (var sub in subjects)
        {
            tmpTable.Columns.Add(sub.Name, typeof(double));
        }

        foreach (var student in students)
        {
            var row = tmpTable.NewRow();
            row["ID"] = student.Id;
            row["Студент"] = $"{student.SecondName} {student.FirstName[0]}.{student.Patronymic[0]}.";

            var cells = data.Where(d => d.StudentId == student.Id);
            foreach (var subject in subjects)
            {
                var cell = cells.FirstOrDefault(c => c.SubjectId == subject.Id);
                var mark = cell != null ? cell.Mark : 0;
                
                row[subject.Name] = mark;
            }
            tmpTable.Rows.Add(row);
        }

        DataTable = tmpTable;
    }

    /// <summary> Сохранение данных в Excel </summary>
    public ICommand SaveExcelFileCommand => 
        _saveExcelFileCommand ??= new Command(OnSaveExcelFileCommandExecuted, CanSaveExcelFileCommandExecute);

    private bool CanSaveExcelFileCommandExecute(object? arg)
        => DataTable != null;

    private void OnSaveExcelFileCommandExecuted(object? param)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Excel file (*.xlsx)|*.xlsx",
            ValidateNames = false,
            CheckFileExists = true,
            CheckPathExists = true,
        };

        if (saveFileDialog.ShowDialog() == false)
        {
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Avg marks");


        sheet.Cells[2, 2].Value = "Средние оценки";
        sheet.Cells[3, 2].Value = "Группа:";
        sheet.Cells[3, 3].Value = SelectedGroup.Name;
        sheet.Cells[4, 2].Value = "Месяц:";
        sheet.Cells[4, 3].Value = SelectedMonth;

        sheet.Cells[2, 2, 4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        sheet.Cells[2, 2, 4, 3].AutoFitColumns();

        var initRow = 2;
        var initCol = 5;
        if (DataTable != null)
        {
            for (int i = 0; i < DataTable.Columns.Count; i++)
            {
                var col = DataTable.Columns[i];
                sheet.Cells[initRow, initCol + i].Value = col.ColumnName;
            }

            for (int i = 0; i < DataTable.Rows.Count; i++)
            {
                var row = DataTable.Rows[i];
                int column = initCol;
                
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    var value = row.ItemArray[j];
                    sheet.Cells[initRow + i + 1, column + j].Value = value;
                }
            }

            var endRow = initRow + DataTable.Rows.Count;
            var endCol = initCol + DataTable.Columns.Count;

            sheet.Cells[initRow, initCol, endRow, endCol].AutoFitColumns();
            sheet.Cells[initRow, initCol, endRow, endCol]
                .Style.Border.BorderAround(ExcelBorderStyle.Thick);
        }

        File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
    }
}
