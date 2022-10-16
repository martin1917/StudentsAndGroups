using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Data;

public class ContextFactory
{
    public static Context CreateContext()
    {
        var opt = new DbContextOptionsBuilder<Context>().UseSqlite("Data Source=GroupsStudents.db").Options;
        return new Context(opt);
    }
}
