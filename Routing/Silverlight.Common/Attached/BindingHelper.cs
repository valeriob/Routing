using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Common.Attached
{
    public class BindingHelper
    {
        public static bool GetUpdateSourceOnChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdateSourceOnChangeProperty);
        }

        public static void SetUpdateSourceOnChange(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateSourceOnChangeProperty, value);
        }

        public static readonly DependencyProperty UpdateSourceOnChangeProperty = DependencyProperty.RegisterAttached("UpdateSourceOnChange", typeof(bool), 
            typeof(BindingHelper), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox == null)
                return;
            if ((bool)e.NewValue)
            {
                textBox.TextChanged += OnTextChanged;
            }
            else
            {
                textBox.TextChanged -= OnTextChanged;
            }
        }

        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            var be = textBox.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }


        /*

        public static readonly DependencyProperty DelayedUpdateSourceOnChangeProperty = DependencyProperty.RegisterAttached("DelayedUpdateSourceOnChange", typeof(bool),
           typeof(BindingHelper), new PropertyMetadata(false, OnDelayedPropertyChanged));

        private static void OnDelayedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox == null)
                return;
            if ((bool)e.NewValue)
            {
                textBox.TextChanged += OnDelayedTextChanged;
            }
            else
            {
                textBox.TextChanged -= OnDelayedTextChanged;
            }
        }

        static void OnDelayedTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            var be = textBox.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }
         
        */

    }
}
