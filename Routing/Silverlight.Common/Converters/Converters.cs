using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Silverlight.Common
{

    //public class NotConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        bool boolValue = (bool)value;

    //        return !boolValue;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return Convert(value, targetType, parameter, culture);
    //    }
    //}

    public class BooleanToVisibility : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool swap = true;
            if (parameter != null)
                swap = System.Convert.ToBoolean(parameter);
            if (swap)
                return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            else
                return (!(bool)value) ? Visibility.Visible : Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool swap = true;
            if (parameter != null)
                swap = System.Convert.ToBoolean(parameter);

            return ((Visibility)value == Visibility.Visible) && swap;
        }
    }

    //public class IsNullToVisibility : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (value == null)
    //            if (parameter == null)
    //                return Visibility.Collapsed;
    //            else
    //                return Visibility.Visible;
    //        else
    //            if (parameter == null)
    //                return Visibility.Visible;
    //            else
    //                return Visibility.Collapsed;

    //        //return (value == null && parameter!=null) ? Visibility.Collapsed : Visibility.Visible;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class NumberToNullValueConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        try
    //        {
    //            var numericValue = System.Convert.ToInt32(value);
    //            if (numericValue == 0)
    //                value = null;
    //        }
    //        catch (Exception)
    //        { }
    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return Convert(value, targetType, parameter, culture);
    //    }
    //}

    //public class ByteArrayToImageConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        var image = new Image();
    //        var array = value as byte[];
    //        var stream = new MemoryStream(array);
    //        var bitmap = new BitmapImage();
    //        bitmap.SetSource(stream);
    //        image.Source = bitmap;
    //        return image;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}