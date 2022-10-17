using System.Collections.Generic;
using System;
using System.Linq;

namespace WpfApp2.Models;

public record StudentAndMarks(int StudentId, List<MarkDetail> MarkDetails)
{
    public bool ContainDate(DateOnly date)
    {
        return MarkDetails.FirstOrDefault(md => md.Date == date) != null;
    }

    public MarkDetail? GetByDate(DateOnly date)
    {
        return MarkDetails.FirstOrDefault(md => md.Date == date);
    }

    public List<DateOnly> GetDates()
    {
        return MarkDetails.Select(md => md.Date).ToList();
    }
}
