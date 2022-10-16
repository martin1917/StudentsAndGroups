using System;
using System.Collections.ObjectModel;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

public class GroupModel : BaseViewModel
{
    private int _id;
    public int Id { get => _id; set => Set(ref _id, value); }

    private string _name;
    public string Name { get => _name; set => Set(ref _name, value); }

    private DateOnly _dateCreated;
    public DateOnly DateCreated { get => _dateCreated; set => Set(ref _dateCreated, value); }

    private ObservableCollection<StudentModel> _studentModels = new();
    public ObservableCollection<StudentModel> StudentModels { get => _studentModels; set => Set(ref _studentModels, value); }
}


