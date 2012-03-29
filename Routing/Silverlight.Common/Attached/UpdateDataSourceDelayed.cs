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
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;

namespace Silverlight.Common
{
    public class UpdateDataSourceDelayed
    {
        protected static Dictionary<object, DispatcherTimer> Timers { get; set; }
        static UpdateDataSourceDelayed()
        {
            Timers = new Dictionary<object, DispatcherTimer>();
        }


        public static int GetUpdateDataSourceDelay(DependencyObject obj)
        {
            return (int)obj.GetValue(UpdateDataSourceDelayProperty);
        }

        public static void SetUpdateDataSourceDelay(DependencyObject obj, int value)
        {
            obj.SetValue(UpdateDataSourceDelayProperty, value);
        }
        public static readonly DependencyProperty UpdateDataSourceDelayProperty =
            DependencyProperty.RegisterAttached("UpdateDataSourceDelay", typeof(int), typeof(UpdateDataSourceDelayed), new PropertyMetadata(0, OnUpdateDataSourceDelayPropertyChanged));


        private static void OnUpdateDataSourceDelayPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
                return;
            if (obj is TextBox)
            {
                var textBox = (TextBox)obj;
                textBox.TextChanged += (from, ev) => PropertyChanged(textBox, TextBox.TextProperty);
                //textBox.KeyUp += (from, ev) => ForceDataBind(textBox, TextBox.TextProperty);
            }
            else if (obj is DatePicker)
            {
                var datePicker = (DatePicker)obj;
                datePicker.SelectedDateChanged += (from, ev) => PropertyChanged(datePicker, DatePicker.SelectedDateProperty);
            }
        }

        static void ForceDataBind(FrameworkElement sender, DependencyProperty dependencyProperty)
        {
            lock (Timers)
            {
                if( Timers.ContainsKey(sender))
                {
                    var timer = Timers[sender];
                    timer.Stop();
                    sender.GetBindingExpression(dependencyProperty).UpdateSource();
                }
            }
        }


        static void PropertyChanged(FrameworkElement sender, DependencyProperty dependencyProperty)
        {
            if (sender == null)
                return;

            lock (Timers)
            {
                if (Timers.ContainsKey(sender))
                {
                    var timer = Timers[sender];
                    timer.Start();
                }
                else
                {
                    var delay = GetUpdateDataSourceDelay(sender);
                    var timer = BuildTimer(sender, sender.GetBindingExpression(dependencyProperty), delay);
                    Timers[sender] = timer;
                    timer.Start();
                }
            }
        }

        static DispatcherTimer BuildTimer(object control, BindingExpression exp, int milliseconds)
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            timer.Tick += (sender, e) =>
            {
                exp.UpdateSource();

                lock (Timers)
                {
                    timer.Stop();
                    Timers.Remove(control);
                }
            };
            return timer;
        }

    }
}
