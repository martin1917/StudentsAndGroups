using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;
using WpfApp2.Infrastructure.Commands;

namespace WpfApp2.ViewModels.GroupDialogVM;

/// <summary> VM для диалога (Редактирование группы) </summary>
public class GroupEditViewModel : BaseViewModel
{
    private string _name;
    private DateTime _dateCreated = DateTime.Now;
    private ICommand _confirmCommand;

	public GroupEditViewModel(GroupModel group)
	{
        Name = group.Name ?? string.Empty;
        if(group.DateCreated != default)
        {
            DateCreated = new DateTime(group.DateCreated.Year, group.DateCreated.Month, group.DateCreated.Day);
        }
    }

    /// <summary> Название предмета </summary>
    public string Name { get => _name; set => Set(ref _name, value); }

    /// <summary> Дата создания </summary>
    public DateTime DateCreated { get => _dateCreated; set => Set(ref _dateCreated, value); }

    /// <summary> Валидация </summary>
    public ICommand ConfirmCommand => _confirmCommand
        ??= new Command(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted(object? param)
    {
        string error = string.Empty;

        if (!Regex.IsMatch(Name.Trim(), @"[a-zA-Zа-яА-Я]+"))
        {
            error += "• Имя должно содержать хотя бы одну букву\n";
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
