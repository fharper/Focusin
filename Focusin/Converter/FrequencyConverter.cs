using System;
using System.Globalization;
using System.Windows.Data;

namespace Focusin.Converter
{
    public class FrequencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var frequency = (int) value;
            return frequency - 2; // frequency 2 is index 0, so we have to rest 2 to our frequency to match the index.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var frequency = (int) value;
            return frequency + 2; // When we change the frequency we have to convert it back to a real frequency to save it.
        }
    }
}