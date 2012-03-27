using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;


namespace Silverlight.Common.Controls.SnapGrid
{
    public class SnapGridItemContainer : ContentControl
    {
        public SnapGridItemContainer()
        {
            Background = new SolidColorBrush(Colors.Cyan);
            var t = DefaultStyleKey;

            Background = new SolidColorBrush(Colors.Cyan);
            MouseLeftButtonUp += new MouseButtonEventHandler(SnapGridItem_MouseLeftButtonUp);
        }

        void SnapGridItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
