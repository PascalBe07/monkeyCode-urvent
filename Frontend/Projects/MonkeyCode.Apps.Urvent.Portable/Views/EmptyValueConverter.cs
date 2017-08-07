﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    /// <summary>
    /// For XAML debugging only
    /// </summary>
    internal class EmptyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
