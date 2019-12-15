// Niklas Strasser, Felix Brandstetter, Yannick Gruber

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Client.Converter
{
    /// <summary>
    /// 
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a visibility. If true, visibility is collapsed, else visible.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>A visibility for XAML objects.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
