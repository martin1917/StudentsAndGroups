using System;
using System.Collections.ObjectModel;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

/// <summary> (Отображается на UI) Учебная группа </summary>
public class GroupModel : BaseViewModel
{
    private int _id;
    private string _name;
    private DateOnly _dateCreated;
    private ObservableCollection<StudentModel> _studentModels = new();

    public int Id { get => _id; set => Set(ref _id, value); }

    /// <summary> Название учебной группы </summary>
    public string Name { get => _name; set => Set(ref _name, value); }

    /// <summary> Дата создания учебной группы </summary>
    public DateOnly DateCreated { get => _dateCreated; set => Set(ref _dateCreated, value); }

    /// <summary> Ученики в учебной группе </summary>
    public ObservableCollection<StudentModel> StudentModels { get => _studentModels; set => Set(ref _studentModels, value); }
}


