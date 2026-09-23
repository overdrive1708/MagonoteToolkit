using System;
using System.Globalization;
using System.Windows.Data;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// Bool型をVisibility型に変換するコンバーター
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility returnVisibility;

            if ((bool)value == true)
            {
                returnVisibility = System.Windows.Visibility.Visible;
            }
            else
            {
                returnVisibility = System.Windows.Visibility.Collapsed;
            }

            return returnVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool型をVisibility型に変換するコンバーター（反転版）
    /// </summary>
    public class InvertedBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility returnVisibility;

            if ((bool)value == true)
            {
                returnVisibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                returnVisibility = System.Windows.Visibility.Visible;
            }

            return returnVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
