using System;

namespace WpfApp2.Extensions;

/// <summary> Методы расширения для класса DateOnly </summary>
public static class DateOnlyExtension
{
    /// <summary>
    /// Конвертировать объект типа DateOnly к строковому формату, удовлетворяющему SQLite
    /// </summary>
    /// <returns> Дата в строковом формате, которая удовлетворяют синтаксису SQLite </returns>
    public static string ConvertToSQLiteFormat(this DateOnly date)
    {
        (int year, int month, int day)
            = (date.Year, date.Month, date.Day);

        string result = $"{year}-";
        result += month < 10 ? $"0{month}-" : $"{month}-";
        result += day < 10 ? $"0{day}" : $"{day}";
        return result;
    }
}
