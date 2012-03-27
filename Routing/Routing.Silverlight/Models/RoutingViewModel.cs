using System;
using System.Net;
using System.Linq;
using ReactiveUI;

namespace Routing.Silverlight.Models
{
    public class RoutingViewModel : ReactiveObject
    {
        public static RoutingViewModel _Instance;
        public static RoutingViewModel Instance { get { return _Instance ?? (_Instance = new RoutingViewModel()); } protected set { _Instance = value; } }

        protected RoutingViewModel()
        {

        }

        private string _UserId;
        public string UserId
        {
            get { return _UserId; }
            protected set { _UserId = value; this.RaisePropertyChanged(r => r.UserId); }
        }
        


        public void Login(string userId)
        {
            UserId = userId;
        }
    }
}



