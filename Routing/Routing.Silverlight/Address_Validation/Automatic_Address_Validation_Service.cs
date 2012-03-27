using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using ReactiveUI;
using maps = Silverlight.Common.Maps;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;

namespace Routing.Silverlight.Address_Validation
{
    public class Automatic_Address_Validation_Service
    {
        protected static HashSet<object> Locks { get; set; }

        static Automatic_Address_Validation_Service()
        {
            Locks = new HashSet<object>();
            MessageBus.Current.Listen<Automatically_Validate_Address>().Subscribe(Automatically_Validate_Address);
        }

        public static void Init() { }


        public static void Automatically_Validate_Address(Automatically_Validate_Address search_Address)
        {
            if (!search_Address.IsValid())
                return;

            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.GeocodeCompleted += (sender, e) =>
            {
                if (e.Result.Results.Count == 1)
                {
                    MessageBus.Current.SendMessage(new Address_Validated(e.Result.Results.First(), search_Address));
                }
                else
                    MessageBus.Current.SendMessage(search_Address.To_Manual());
            };

            geocoding.GeocodeAsync(new maps.GeocodeService.GeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Query = search_Address.Search_Address
            });
        }

    

    }



}
