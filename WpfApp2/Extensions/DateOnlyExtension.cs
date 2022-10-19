﻿using System;

namespace WpfApp2.Extensions;

public static class DateOnlyExtension
{
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