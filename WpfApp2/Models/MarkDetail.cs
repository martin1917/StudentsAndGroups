using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp2.Models;

/// <summary>
/// Оценки за дату
/// </summary>
/// <param name="Marks">Оценки</param>
/// <param name="Date">Дата</param>
public record MarkDetail(List<int?> Marks, DateOnly Date)
{
    /// <summary>
    /// Все оценки за дату в строку
    /// </summary>
    /// <returns></returns>
    public string? MarksToString()
    {
        return string.Join(", ", Marks.Select(i => i == null ? "Н" : $"{i}"));
    }
}
