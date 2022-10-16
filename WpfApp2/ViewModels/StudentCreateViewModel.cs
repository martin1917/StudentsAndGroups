﻿using System;
using WpfApp2.Models;

namespace WpfApp2.ViewModels;

public class StudentCreateViewModel : BaseViewModel
{
    public StudentCreateViewModel(GroupModel group)
    {
        GroupModel = group;
    }

    private string _firstName;
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    private string _secondName;
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    private string _patronymic;
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }

    private DateTime _birthDay = DateTime.Now;
    public DateTime BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    private GroupModel _groupModel;
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }
}