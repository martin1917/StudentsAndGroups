using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp2.Infrastructure.Converters;

/// <summary> Класс конвертирующий объект в класс типа Brushes </summary>
public class ValueCellToBackgroundConverter : IValueConverter
{
    /// <summary> Ковертировать для отображение на UI </summary>
    /// <returns> Brushes.LightCoral - если содержимое ячейки DataGrid сожержит 'Н' </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string str;
        try
        {
            DataGridCell dgc = (DataGridCell)value;
            System.Data.DataRowView rowView = (System.Data.DataRowView)dgc.DataContext;
            str = (string)rowView.Row.ItemArray[dgc.Column.DisplayIndex];
        }
        catch (InvalidCastException)
        {
            return DependencyProperty.UnsetValue;
        }

        if (str!.Equals("Н")) return Brushes.LightCoral;

        return DependencyProperty.UnsetValue;
    }

    /// <summary> Не используется (Необходим из-за интерфейса IValueConverter) </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
