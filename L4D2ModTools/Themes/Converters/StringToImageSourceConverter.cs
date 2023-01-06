namespace L4D2ModTools.Themes.Converters;

public class StringToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var path = (string)value;
        if (path.StartsWith("http"))
        {
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
        else
        {
            if (File.Exists(path))
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            else
                return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
