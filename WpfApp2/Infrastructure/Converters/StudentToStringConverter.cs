﻿using System;
using System.Globalization;
using System.Windows.Data;
using WpfApp2.Models;

namespace WpfApp2.Infrastructure.Converters;

public class StudentToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var studentModel = (StudentModel)value;
        return $"{studentModel.SecondName} {studentModel.FirstName[0]}.{studentModel.Patronymic[0]}.";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}