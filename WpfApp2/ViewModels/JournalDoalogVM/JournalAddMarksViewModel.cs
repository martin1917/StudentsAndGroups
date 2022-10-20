using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.JournalDoalogVM;

/// <summary> VM для диалога (добавление оценок) </summary>
public class JournalAddMarksViewModel : BaseViewModel
{
    private int? _day;
    private ICommand _confirmOperationCommand;

    public JournalAddMarksViewModel(List<StudentModel> studentModels, int year, int month, List<DateOnly> alreadyAddedDate)
    {
        foreach (var student in studentModels)
        {
            StudentsAndMarks.Add(new StudentAndMarkInDate(student));
        }

        var nums = Enumerable
            .Range(1, DateTime.DaysInMonth(year, month))
            .Where(n => !alreadyAddedDate.Select(i => i.Day).Contains(n));

        NumInMonth = new(nums);
    }

    /// <summary> Список студентов и их всех оценок </summary>
    public ObservableCollection<StudentAndMarkInDate> StudentsAndMarks { get; } = new();

    /// <summary> оставшиеся даты указанного месяца </summary>
    public List<int> NumInMonth { get; }

    /// <summary> Выбранный день месяца </summary>
    public int? Day { get => _day; set => Set(ref _day, value); }

    /// <summary> Валидация </summary>
    public ICommand ConfirmOperationCommand => _confirmOperationCommand
        ??= new Command(OnConfirmOperationCommandExecuted, CanConfirmOperationCommandExecute);

    private bool CanConfirmOperationCommandExecute(object? param)
    {
        return Day != null && App.CurrentWindow != null;
    }

    private void OnConfirmOperationCommandExecuted(object? param)
    {
        if (App.CurrentWindow == null) return;

        string error = string.Empty;
        foreach (var item in StudentsAndMarks)
        {
            var marks = Regex.Replace(item.Marks.Trim().ToUpper(), @"\s+", " ").Split(" ");

            if (string.IsNullOrEmpty(marks[0])) continue;

            if (marks.Length > 1 && marks.Contains("Н"))
            {
                error += $"Студент ({item.StudentModel.Id}) не может остутствовать и иметь оценки за одну дату\n";
            }

            foreach (var mark in marks)
            {
                if (mark != "2" && mark != "3" && mark != "4" && mark != "5" && mark != "Н")
                {
                    error += $"Студент ({item.StudentModel.Id}). Оценки могут быть только [2, 3, 4, 5, Н] (Н - не был)\n";
                }
            }
        }

        if (string.IsNullOrEmpty(error))
        {
            var window = App.CurrentWindow;
            window.DialogResult = true;
            window.Close();
        }
        else
        {
            MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
