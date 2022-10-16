using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace WpfApp2.Infrastructure.Converters;

public class StringToCollectionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var collection = (ObservableCollection<int>)value;
        return string.Join(",", collection);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = Regex.Replace(((string)value).Trim(), @"\s+", " ");

        ObservableCollection<int> collection = new();
        var nums = str.Split(",").Select(i => int.Parse(i));
        foreach (var num in nums)
        {
            collection.Add(num);
        }
        return collection;
    }
}
