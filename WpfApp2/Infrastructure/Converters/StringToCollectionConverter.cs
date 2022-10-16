using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace WpfApp2.Infrastructure.Converters;

public class StringToCollectionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var collection = (List<int>)value;
        collection.Sort();
        return string.Join(",", collection);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = Regex.Replace(((string)value).Trim(), @"\s+", " ");
        return new List<int>(str.Split(",").Select(i => int.Parse(i)));
    }
}
