using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using WpfApp2.Entity;

namespace WpfApp2.Data;

/// <summary> Контекст базы данных </summary>
public class Context : DbContext
{
	public Context(DbContextOptions<Context> options) : base(options) { }

    /// <summary> Поле для взаимодействия с таблицей Groups в БД </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary> Поле для взаимодействия с таблицей Students в БД </summary>
	public DbSet<Student> Students { get; set; }

    /// <summary> Поле для взаимодействия с таблицей Subjects в БД </summary>
	public DbSet<Subject> Subjects { get; set; }

    /// <summary> Поле для взаимодействия с таблицей SubjectGroups в БД </summary>
	public DbSet<SubjectGroup> SubjectGroups { get; set; }

    /// <summary> Поле для взаимодействия с таблицей AcademicPerformanceLogs в БД </summary>
	public DbSet<AcademicPerformanceLog> AcademicPerformanceLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }
}
