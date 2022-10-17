using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp2.Models;

public record MarkDetail(List<int?> Marks, DateOnly Date)
{
    public string? MarksToString()
    {
        return string.Join(", ", Marks.Select(i => i == null ? "Н" : $"{i}"));
    }
}
