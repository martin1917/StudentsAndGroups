using System;
using System.Collections.Generic;
using WpfApp2.Entity;

namespace WpfApp2.ViewModels;

public class StudentEditViewModel : BaseViewModel
{
    public List<Group> AllGroups { get; set; }

    public StudentEditViewModel(Student student, List<Group> allGroups)
    {
        AllGroups = allGroups;

        FirstName = student.FirstName;
        SecondName = student.SecondName;
        Patronymic = student.Patronymic;
        BirthDay = student.BirthDay;
        Group = student.Group;
    }

    private string _firstName;
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    private string _secondName;
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    private string _patronymic;
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }
    
    private DateOnly _birthDay;
    public DateOnly BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    private Group _group;
    public Group Group { get => _group; set => Set(ref _group, value); }
}
