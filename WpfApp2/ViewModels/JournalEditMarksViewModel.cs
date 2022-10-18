using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Entity;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels;

public class JournalEditMarksViewModel : BaseViewModel
{

    public JournalEditMarksViewModel(Student student, DateOnly date, string marks)
    {
        Student = student;
        Date = date;
        Marks = marks;
    }

    public List<int> NumsInMonth { get; set; }

    public Student Student { get; }
    public DateOnly Date { get; }

    private string _marks;
    public string Marks { get => _marks; set => Set(ref _marks, value); }

    private ICommand _confirmEditCommand; 
    public ICommand ConfirmEditCommand => _confirmEditCommand
        ??= new Command(OnConfirmEditCommandExecuted, CanConfirmEditCommandExecute);

    private bool CanConfirmEditCommandExecute(object? arg)
    {
        return App.CurrentWindow != null;
    }

    private void OnConfirmEditCommandExecuted(object? obj)
    {
        var error = string.Empty;

        var marks = Regex.Replace(Marks.Trim().ToUpper(), @"\s+", "").Split(",");

        if (marks.Length > 1 && marks.Contains("Н"))
        {
            error += $"Студент ({Student.Id}) не может остутствовать и иметь оценки за одну дату\n";
        }

        foreach (var mark in marks)
        {
            if (mark != "2" && mark != "3" && mark != "4" && mark != "5" && mark != "Н")
            {
                error += $"Студент ({Student.Id}). Оценки могут быть только [2, 3, 4, 5, Н] (Н - не был)\n";
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
