using System;
using System.Net;
using ReactiveUI;


namespace Routing.Silverlight.Models
{
    public class Amount : ReactiveObject
    {
        private double _Value;
        public double Value
        {
            get { return _Value; }
            set { _Value = value; this.RaisePropertyChanged(a => a.Value); }
        }
        
        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; this.RaisePropertyChanged(a => a.Unit); }
        }

        private string _Display;
        public string Display
        {
            get { return _Display; }
            set { _Display = value; this.RaisePropertyChanged(a => a.Display);}
        }

        public Amount()
        {
            this.WhenAny(p => p.Value, q => q.Unit, (a, b) => { return a; }).Subscribe(s =>
            {
                Display = ToString();
            });
        }
        

        public override string ToString()
        {
            return string.Format("{0} {1}", Value, Unit);
        }
    }
}
