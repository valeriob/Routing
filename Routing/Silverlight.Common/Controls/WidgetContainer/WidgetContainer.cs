using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Common.Helpers;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;


namespace Silverlight.Common.Controls.WidgetContainer
{
    public class WidgetContainer : UserControl
    {
        protected Canvas Canvas { get; set; }

        public List<WidgetItemContainer> Widgets { get; set; }


        public void Add_WidgetItemContainer(WidgetItemContainer container)
        {
            Canvas.Children.Add(container);
            Widgets.Add(container);
        }

        public void Remove_WidgetItemContainer(WidgetItemContainer container)
        {
            Canvas.Children.Remove(container);
            Widgets.Remove(container);
        }

        public WidgetContainer()
        {
            Init();
        }

        protected void Init()
        {
            Canvas = new Canvas();
            Widgets = new List<WidgetItemContainer>();
   
            Content = Canvas;
        }

     
       



    }




}
