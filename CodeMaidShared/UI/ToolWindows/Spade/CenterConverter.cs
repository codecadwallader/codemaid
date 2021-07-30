using System;
using System.Globalization;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    public class CenterConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (double)value / 2.0;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}