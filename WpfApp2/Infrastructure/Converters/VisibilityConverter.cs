using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp2.Infrastructure.Converters;

/// <summary> Класс конвертирующий объект в елемент перечисления типа Visibility </summary>
public class VisibilityConverter : IValueConverter
{
    /// <summary> Ковертировать для отображение на UI </summary>
    /// <returns>Visibility.Collapsed - если объект == null; Visibility.Visible - иначе</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Visibility.Collapsed;
        }

        return Visibility.Visible;
    }

    /// <summary> Не используется (Необходим из-за интерфейса IValueConverter) </summary>
    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return default;
    }
}
