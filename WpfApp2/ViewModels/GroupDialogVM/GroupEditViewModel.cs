using System;
using WpfApp2.Models;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels.GroupDialogVM;

public class GroupEditViewModel : BaseViewModel
{
	public GroupEditViewModel(GroupModel group)
	{
        Name = group.Name;
        if(group.DateCreated != default)
        {
            DateCreated = new DateTime(group.DateCreated.Year, group.DateCreated.Month, group.DateCreated.Day);
        }
    }

    private string _name;
    public string Name { get => _name; set => Set(ref _name, value); }

    private DateTime _dateCreated = DateTime.Now;
    public DateTime DateCreated { get => _dateCreated; set => Set(ref _dateCreated, value); }
}
