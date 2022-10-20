using System;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

/// <summary> Ученик (Отображается на UI) </summary>
public class StudentModel : BaseViewModel
{
    private int _id;
    private string _firstName;
    private string _secondName;
    private string _patronymic;
    private DateOnly _birthDay;
    private int _groupId;
    private GroupModel _groupModel;

    public int Id { get => _id; set => Set(ref _id, value); }

    /// <summary> Имя </summary>
    public string FirstName { get => _firstName; set => Set(ref _firstName, value); }

    /// <summary> Фамилия </summary>
    public string SecondName { get => _secondName; set => Set(ref _secondName, value); }

    /// <summary> Отчество </summary>
    public string Patronymic { get => _patronymic; set => Set(ref _patronymic, value); }

    /// <summary> Дата рождения </summary>
    public DateOnly BirthDay { get => _birthDay; set => Set(ref _birthDay, value); }

    /// <summary> ID учебной группы, в которой учится студент </summary>
    public int GroupId { get => _groupId; set => Set(ref _groupId, value); }

    /// <summary> Группа, в которой учится студент </summary>
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }
}