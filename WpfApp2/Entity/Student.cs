using System;

namespace WpfApp2.Entity;

/// <summary> Ученик </summary>
public class Student
{
    public int Id { get; set; }

    /// <summary> Имя </summary>
    public string FirstName { get; set; }

    /// <summary> Фамилия </summary>
    public string SecondName { get; set; }

    /// <summary> Отчество </summary>
    public string Patronymic { get; set; }

    /// <summary> Дата рождения </summary>
    public DateOnly BirthDay { get; set; }

    /// <summary> ID учебной группы, в которой учится студент </summary>
    public int GroupId { get; set; }

    /// <summary> Группа, в которой учится студент </summary>
    public Group Group { get; set; }
}
