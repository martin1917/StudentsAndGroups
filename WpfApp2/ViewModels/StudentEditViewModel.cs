﻿using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Entity;
using WpfApp2.Models;

namespace WpfApp2.ViewModels;

public class StudentEditViewModel : BaseViewModel
{
    public List<GroupModel> AllGroups { get; set; }

    public StudentEditViewModel(StudentModel student, List<GroupModel> allGroups)
    {
        AllGroups = allGroups;
        FirstName = student.FirstName;
        SecondName = student.SecondName;
        Patronymic = student.Patronymic;
        BirthDay = student.BirthDay;
        GroupModel = allGroups.First(g => g.Id == student.GroupId);
    }

    private string _firstName;
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    private string _secondName;
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    private string _patronymic;
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }
    
    private DateOnly _birthDay;
    public DateOnly BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    private GroupModel _groupModel;
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }
}
