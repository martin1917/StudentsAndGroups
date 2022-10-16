using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WpfApp2.Data;

public class ContextFactory
{
    public static Context CreateContext()
    {
        var builder = new DbContextOptionsBuilder<Context>();
        var opt = builder.UseSqlite("Data Source=GroupsStudents.db").Options;
        return new Context(opt);
    }
}
