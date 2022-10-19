﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using WpfApp2.Data;
using WpfApp2.Extensions;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

public class AvgMarksViewModel : BaseViewModel
{
    private IMapper _mapper;

    public AvgMarksViewModel(IMapper mapper)
	{
        _mapper = mapper;
        
        LoadData();
    }

    private void LoadData()
    {
        var ctx = ContextFactory.CreateContext();
        GroupModels = _mapper.Map<List<GroupModel>>(ctx.Groups);
    }

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

    private ICommand _loadAvgMarks;
    public ICommand LoadAvgMarks => _loadAvgMarks
        ??= new Command(OnLoadAvgMarksExecuted, CanLoadAvgMarksExecute);

    private bool CanLoadAvgMarksExecute(object? param)
    {
        return SelectedGroup != null && SelectedMonth != null;
    }

    private void OnLoadAvgMarksExecuted(object? param)
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
        var num = deltaMonth >= 0 ? deltaYear + 1 : (deltaYear + 1) + 1;

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
}

public record AvgMark(int StudentId, int SubjectId, double Mark);
