using System;
using WpfApp2.ViewModels;

namespace WpfApp2.Models;

public class StudentModel : BaseViewModel
{
    private int _id;
    public int Id { get => _id; set => Set(ref _id, value); }

    private string _firstName;
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    private string _secondName;
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    private string _patronymic;
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }

    private DateOnly _birthDay;
    public DateOnly BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    private int _groupId;
    public int GroupId { get => _groupId; set => Set(ref _groupId, value); }

    private GroupModel _groupModel;
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }
}