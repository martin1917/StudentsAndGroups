using Microsoft.EntityFrameworkCore;
using WpfApp2.Entity;

namespace WpfApp2.Data;

public class Context : DbContext
{
	public Context(DbContextOptions<Context> options) : base(options) { }

	public DbSet<Group> Groups { get; set; }

	public DbSet<Student> Students { get; set; }

	public DbSet<Subject> Subjects { get; set; }

	public DbSet<SubjectGroup> SubjectGroups { get; set; }

	public DbSet<AcademicPerformanceLog> AcademicPerformanceLogs { get; set; }
}
