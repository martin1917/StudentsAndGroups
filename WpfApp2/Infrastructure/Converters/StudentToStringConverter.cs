using System;
using System.Globalization;
using System.Windows.Data;
using WpfApp2.Models;

namespace WpfApp2.Infrastructure.Converters;

/// <summary> Класс конвертирующий объект в класс типа Brushes </summary>
public class StudentToStringConverter : IValueConverter
{
    /// <summary> Возвращает строку = (Имя + Фамилия + Отчество) для объекта типа StudentModel </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var studentModel = (StudentModel)value;
        return $"{studentModel.SecondName} {studentModel.FirstName[0]}.{studentModel.Patronymic[0]}.";
    }

    /// <summary> Не используется (Необходим из-за интерфейса IValueConverter) </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
