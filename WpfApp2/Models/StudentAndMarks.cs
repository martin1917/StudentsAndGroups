using System.Collections.Generic;
using System;
using System.Linq;

namespace WpfApp2.Models;

/// <summary>
/// Все оценки ученика
/// </summary>
/// <param name="StudentId">ID ученикаа</param>
/// <param name="MarkDetails">Оценки за все даты</param>
public record StudentAndMarks(int StudentId, List<MarkDetail> MarkDetails)
{
    /// <summary>
    /// Проверить есть ли оценки за дату
    /// </summary>
    /// <param name="date">дата</param>
    /// <returns>true - есть; false - нет</returns>
    public bool ContainDate(DateOnly date)
    {
        return MarkDetails.FirstOrDefault(md => md.Date == date) != null;
    }

    /// <summary>
    /// Получить оценки за дату
    /// </summary>
    /// <param name="date">дата</param>
    /// <returns>Оценки за дату</returns>
    public MarkDetail? GetByDate(DateOnly date)
    {
        return MarkDetails.FirstOrDefault(md => md.Date == date);
    }

    /// <summary>
    /// Получить все даты
    /// </summary>
    /// <returns>Даты, за которые есть оценки</returns>
    public List<DateOnly> GetDates()
    {
        return MarkDetails.Select(md => md.Date).ToList();
    }
}
