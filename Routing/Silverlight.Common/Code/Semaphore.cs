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
using System.Threading;

namespace Silverlight.Common
{
    public class Semaphore
    {
        protected int _count;
        protected object _lock = new object();
        public event EventHandler CountedZero;

        public Semaphore(int count) 
        {
            lock (_lock)
                _count = count;
        }

        public void WaitAll() 
        {
            lock (_lock) 
            {
                while (_count > 0)
                {
                    Monitor.Wait(_lock);
                }
            }
            OnCountedZero();
        }

        public void Signal() 
        {
            lock (_lock) 
            {
                _count--;
                Monitor.Pulse(_lock);
                if(_count==0)
                    OnCountedZero();
            }

        }

        public void Reset(int count) 
        {
            lock (_lock)
            {
                if (_count == 0)
                    _count = count;
            }
        }

        protected void OnCountedZero() 
        {
            if (CountedZero != null)
                CountedZero(this, new EventArgs());
        }

        
    }
}
