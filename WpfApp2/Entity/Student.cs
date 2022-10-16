using System;

namespace WpfApp2.Entity;

public class Student : BaseEntity
{
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public string Patronymic { get; set; }

    public DateOnly BirthDay { get; set; }

    public int GroupId { get; set; }

    public Group Group { get; set; }
}
