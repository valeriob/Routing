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
    public class EnterBehavior : Behavior<Control>
    {
        protected DispatcherTimer timer;

        public EnterBehavior()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(200000);

            timer.Tick += (sender, e) =>
            {

                ButtonAutomationPeer peer = new ButtonAutomationPeer(DestinationButton);
                IInvokeProvider ip = (IInvokeProvider)peer;
                ip.Invoke();

                lock (timer)
                    timer.Stop();
            };
        }

        public Button DestinationButton
        {
            get { return (Button)GetValue(DestinationButtonProperty); }
            set { SetValue(DestinationButtonProperty, value); }
        }

        public static readonly DependencyProperty DestinationButtonProperty = DependencyProperty.Register("DestinationButton", typeof(Button), typeof(EnterBehavior), null);

        


        protected override void OnAttached()
        {
            //AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.KeyUp += AssociatedObject_KeyUp;
        }

        void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DestinationButton!=null)
            {
                DestinationButton.Focus();
                timer.Start();
                //ButtonAutomationPeer peer = new ButtonAutomationPeer(DestinationButton); 
                //IInvokeProvider ip = (IInvokeProvider)peer; 
                //ip.Invoke();
            }
        }

        protected override void OnDetaching()
        {
            //AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
        }

        void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
