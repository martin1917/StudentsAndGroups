using System;
using System.Collections.Generic;

namespace WpfApp2.Entity;

public class Group : BaseEntity
{
    public string Name { get; set; }

    public DateOnly DateCreated { get; set; }


    public List<Student> Students { get; set; }
}
