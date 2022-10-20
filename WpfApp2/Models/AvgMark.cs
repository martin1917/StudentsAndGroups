namespace WpfApp2.Models;

/// <summary>
/// Запись отображающая средний балл ученика по предмету
/// </summary>
/// <param name="StudentId">ID ученика</param>
/// <param name="SubjectId">ID предмету</param>
/// <param name="Mark">Средний балл</param>
public record AvgMark(int StudentId, int SubjectId, double Mark);