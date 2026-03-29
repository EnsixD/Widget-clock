using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WidgetClock.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is Visibility.Visible;
}

[ValueConversion(typeof(double), typeof(string))]
public class DoubleToPercentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is double d ? $"{(int)(d * 100)}%" : "100%";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => double.TryParse(value?.ToString()?.TrimEnd('%'), out var v) ? v / 100.0 : 1.0;
}

[ValueConversion(typeof(double), typeof(string))]
public class DoubleToIntStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is double d ? $"{(int)d}" : "0";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => double.TryParse(value?.ToString(), out var v) ? v : 0.0;
}
