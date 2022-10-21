using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.StudentDialogVM;

/// <summary> VM для диалога (создание ученика) </summary>
public class StudentCreateViewModel : BaseViewModel
{
    private string _firstName = string.Empty;
    private string _secondName = string.Empty;
    private string _patronymic = string.Empty;
    private DateTime _birthDay;
    private GroupModel _groupModel;
    private ICommand _confirmCommand;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="group">Группа</param>
    public StudentCreateViewModel(GroupModel group)
    {
        GroupModel = group;
        BirthDay = DateTime.Now;
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

    /// <summary> валидация </summary>
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
