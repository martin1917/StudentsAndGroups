using System;
using WpfApp2.Models;
using WpfApp2.ViewModels.GroupDialogVM;
using WpfApp2.Views.Windows.GroupDialogs;

namespace WpfApp2.Services;

public class GroupDialogService
{
    public bool Edit(GroupModel group)
    {
        var vm = new GroupEditViewModel(group);
        var window = new GroupEditWindow
        {
            DataContext = vm
        };

        if(window.ShowDialog() == false)
        {
            return false;
        }

        group.Name = vm.Name;
        group.DateCreated = DateOnly.FromDateTime(vm.DateCreated);
        return true;
    }
}
