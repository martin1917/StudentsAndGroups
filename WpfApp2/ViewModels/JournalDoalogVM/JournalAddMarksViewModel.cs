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

public class JournalAddMarksViewModel : BaseViewModel
{
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

    public ObservableCollection<StudentAndMarkInDate> StudentsAndMarks { get; } = new();

    public List<int> NumInMonth { get; }

    private int? _day;
    public int? Day { get => _day; set => Set(ref _day, value); }

    private ICommand _confirmOperationCommand;
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
            var marks = Regex.Replace(item.Marks.Trim().ToUpper(), @"\s+", "").Split(",");

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
