using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.StudentDialogVM;

public class StudentCreateViewModel : BaseViewModel
{
    public StudentCreateViewModel(GroupModel group)
    {
        GroupModel = group;
        BirthDay = DateTime.Now;
    }

    private string _firstName = string.Empty;
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    private string _secondName = string.Empty;
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    private string _patronymic = string.Empty;
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }

    private DateTime _birthDay;
    public DateTime BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    private GroupModel _groupModel;
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }

    private ICommand _confirmCommand;
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
