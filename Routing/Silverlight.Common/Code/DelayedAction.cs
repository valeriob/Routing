using System;
using System.Net;
using System.Windows.Threading;


namespace Silverlight.Common
{
    public class DelayedAction
    {
        protected DispatcherTimer Timer;
        public Action Action { get; set; }
        public TimeSpan Delay { get; set; }

        public DelayedAction()
        {
            Delay = TimeSpan.FromMilliseconds(400);

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = Delay;
        }

        protected virtual void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            if(Action!= null)
                Action();
        }

        public void Postpone()
        {
            Timer.Start();
        }
    }


    public class DelayedAction<T> 
    {
        protected DispatcherTimer Timer;
        public Action<T> Action { get; set; }
        public TimeSpan Delay { get; set; }
        public T Parameter { get; protected set; }

        public DelayedAction()
        {
            Delay = TimeSpan.FromMilliseconds(400);

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = Delay;
        }

        protected virtual void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            if (Action != null)
                Action(Parameter);
        }

        public void Postpone(T parameter)
        {
            Parameter = parameter;
            Timer.Start();
        }
    }


    public class CancellationToken
    {
        public bool Canceled { get; set; }
    }
}
