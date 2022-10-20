using System;
using WpfApp2.Models;
using WpfApp2.ViewModels.GroupDialogVM;
using WpfApp2.Views.Windows.GroupDialogs;

namespace WpfApp2.Services;

/// <summary> Диалоговый сервис для ГРУПП </summary>
public class GroupDialogService
{
    /// <summary>
    /// Отображение диалогового окна - для редактирование
    /// </summary>
    /// <param name="group">редактируемая группа</param>
    /// <returns></returns>
    public bool Edit(GroupModel group)
    {
        var vm = new GroupEditViewModel(group);
        var window = new GroupEditWindow { DataContext = vm };

        if(window.ShowDialog() == false)
        {
            return false;
        }

        group.Name = vm.Name;
        group.DateCreated = DateOnly.FromDateTime(vm.DateCreated);
        return true;
    }
}
