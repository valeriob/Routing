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

namespace Silverlight.Common.Controls.WidgetContainer
{
    public static class WidgetBoardExtensions
    {
        public static int DTop(this Control item, double unit)
        {
            return Calculate_Discrete(Canvas.GetTop(item), unit);
        }
        public static int DLeft(this Control item, double unit)
        {
            return Calculate_Discrete(Canvas.GetLeft(item), unit);
        }
        public static int DActualWidth(this Control item, double unit)
        {
            return Calculate_Discrete(item.ActualWidth, unit);
        }
        public static int DActualHeight(this Control item, double unit)
        {
            return Calculate_Discrete(item.ActualHeight, unit);
        }

        public static int DWidth(this Control item, double unit)
        {
            return Calculate_Discrete(item.Width, unit);
        }
        public static int DHeight(this Control item, double unit)
        {
            return Calculate_Discrete(item.Height, unit);
        }

        public static int DMaxWidth(this Control item, double unit)
        {
            return Calculate_Discrete(Math.Max(item.Width, item.ActualWidth), unit);
        }
        public static int DMaxHeight(this Control item, double unit)
        {
            return Calculate_Discrete(Math.Max(item.Height, item.ActualHeight), unit);
        }




        public static int Calculate_Discrete(double width, double unit)
        {
            return (int)Math.Round(width / unit);
        }



        public static void Set_ActualBusy(this BusyMap map, Control item, double xUnit, double yUnit)
        {
            map.Set_Busy(item.DLeft(xUnit), item.DTop(yUnit), item.DActualWidth(xUnit), item.DActualHeight(yUnit), item);
        }

        public static void Set_ActualBusy(this AutoEnlarging_BusyMap map, Control item, double xUnit, double yUnit)
        {
            map.Set_Busy(item.DLeft(xUnit), item.DTop(yUnit), item.DActualWidth(xUnit), item.DActualHeight(yUnit), item);
        }

        public static void Set_ActualBusy(this PageBusyMap map, Control item, double xUnit, double yUnit, Control page)
        {
            map.Set_Busy(item.DLeft(xUnit), item.DTop(yUnit), item.DActualWidth(xUnit), item.DActualHeight(yUnit), item, page);
        }





        public static void Set_Busy(this PageBusyMap map, Control item, double xUnit, double yUnit, Control page)
        {
            map.Set_Busy(item.DLeft(xUnit), item.DTop(yUnit), item.DWidth(xUnit), item.DHeight(yUnit), item, page);
        }

        public static void Set_MaxBusy(this PageBusyMap map, Control item, double xUnit, double yUnit, Control page)
        {
            map.Set_Busy(item.DLeft(xUnit), item.DTop(yUnit), item.DMaxWidth(xUnit), item.DMaxHeight(yUnit), item, page);
        }



        public static double MaxHeight(this Control control)
        {
            return Math.Max(control.Height, control.ActualHeight);
        }

        public static double MaxWidth(this Control control)
        {
            return Math.Max(control.Width, control.ActualWidth);
        }

        public static void MoveTo(this Panel control, Panel destination)
        {
            var pPanel = control.Parent as Panel;
            if (pPanel != null)
            {
                pPanel.Children.Remove(control);
            }
            control.Children.Add(destination);

            //var pBorder = control.Parent as Border;
            //var pContentControl = control.Parent as ContentControl;
            //var pItemsControl = control.Parent as ItemsControl;
        }


    }

   
}
