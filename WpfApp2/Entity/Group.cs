using System;
using System.Collections.Generic;

namespace WpfApp2.Entity;

/// <summary> Учебная группа </summary>
public class Group
{ 
    public int Id { get; set; }

    /// <summary> Название учебной группы </summary>
    public string Name { get; set; }

    /// <summary> Дата создания учебной группы </summary>
    public DateOnly DateCreated { get; set; }

    /// <summary> Ученики в учебной группе </summary>
    public List<Student> Students { get; set; }
}
