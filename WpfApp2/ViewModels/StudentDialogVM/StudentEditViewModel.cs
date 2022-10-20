using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using WpfApp2.Entity;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;
using WpfApp2.Infrastructure.Commands;

namespace WpfApp2.ViewModels.StudentDialogVM;

/// <summary> VM для диалога (редактирование ученика) </summary>
public class StudentEditViewModel : BaseViewModel
{
    public List<GroupModel> AllGroups { get; set; }
    private string _firstName;
    private string _secondName;
    private string _patronymic;
    private DateTime _birthDay = DateTime.Now;
    private GroupModel _groupModel;
    private ICommand _confirmCommand;

    public StudentEditViewModel(StudentModel student, List<GroupModel> allGroups)
    {
        AllGroups = allGroups;
        FirstName = student.FirstName;
        SecondName = student.SecondName;
        Patronymic = student.Patronymic;
        BirthDay = new DateTime(student.BirthDay.Year, student.BirthDay.Month, student.BirthDay.Day);
        GroupModel = allGroups.First(g => g.Id == student.GroupId);
    }

    /// <summary> Имя ученика </summary>
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    /// <summary> Фамилия ученика </summary>
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    /// <summary> Отчество ученика </summary>
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }

    /// <summary> Дата рождения ученика </summary>
    public DateTime BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    /// <summary> Учебная группа ученика </summary>
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }

    /// <summary> Валидация </summary>
    public ICommand ConfirmCommand => _confirmCommand
        ??= new Command(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted(object? param)
    {
        string error = string.Empty;
        string stringPatter = @"^[a-zA-Zа-яА-Я]+$";

        if (!Regex.IsMatch(FirstName.Trim(), stringPatter))
        {
            error += "• Имя должно состоять только из букв\n";
        }

        if (!Regex.IsMatch(SecondName.Trim(), stringPatter))
        {
            error += "• Фамилия должно состоять только из букв\n";
        }

        if (!Regex.IsMatch(Patronymic.Trim(), stringPatter))
        {
            error += "• Отчество должно состоять только из букв\n";
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
