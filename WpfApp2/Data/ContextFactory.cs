using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Data;

/// <summary> Фабрика по созданию контекста </summary>
public class ContextFactory
{
    /// <summary> Создание контекста </summary>
    /// <returns> Готовый контекст для работы с БД </returns>
    public static Context CreateContext()
    {
        var builder = new DbContextOptionsBuilder<Context>();
        var opt = builder.UseSqlite("Data Source=GroupsStudents.db").Options;
        return new Context(opt);
    }
}
