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
using System.Windows.Interactivity;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;

namespace Silverlight.Common.Behaviors
{
    public class SelectTextBehavior : Behavior<TextBox>
    {
        public SelectTextBehavior()
        {

        }


        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        }

        void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }



        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }

    }
}
