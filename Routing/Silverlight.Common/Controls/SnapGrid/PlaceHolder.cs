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

namespace Silverlight.Common.Controls.SnapGrid
{
    public class PlaceHolder : Control
    {
        public PlaceHolder()
        {
            Background = new SolidColorBrush(Colors.Cyan);
            MouseLeftButtonUp += new MouseButtonEventHandler(PlaceHolder_MouseLeftButtonUp);
        }

        void PlaceHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
