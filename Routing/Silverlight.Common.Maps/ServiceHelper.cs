using System;
using System.Net;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using Silverlight.Common.Maps.GeocodeService;

namespace Silverlight.Common.Maps
{
    public static class ServiceHelper
    {

        public static readonly string GeocodeServiceCredentials = "Aml_hW0EpwSkTsHB5Lnp5cZrKFE2U7McBgKK5gxFwtaTnwilp03rgIcNLNpgkxfm";


        public static bool IsDesignMode(DependencyObject obj = null)
        {
            return System.ComponentModel.DesignerProperties.IsInDesignTool;
            if (obj != null)
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(obj);
            else
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual);
        }



        public static GeocodeServiceClient GetGeocodeService(DependencyObject obj = null)
        {
            GeocodeServiceClient service = null;
            if (IsDesignMode(obj))
            {
                Binding binding = new BasicHttpBinding();
                EndpointAddress address = new EndpointAddress("http://dev.virtualearth.net/webservices/v1/geocodeservice/GeocodeService.svc");
                service = new GeocodeServiceClient(binding, address);
            }
            else
            {
                service = new GeocodeServiceClient("CustomBinding_IGeocodeService");
            }
           
            return service;
        }


        //public static ReverseGeocodeRequest CreateReverseGeocodeRequest(Domain.HomeBudget.Entities.Location location)
        //{
        //    var request = new ReverseGeocodeRequest();
        //    request.Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = GeocodeServiceCredentials };
        //    request.Location = new Microsoft.Maps.MapControl.Location(location.Latitude, location.Longitude, location.Altitude);
        //    return request;
        //}

        public static ReverseGeocodeRequest CreateReverseGeocodeRequest(Microsoft.Maps.MapControl.Location location )
        {
            return CreateReverseGeocodeRequest(location, GeocodeServiceCredentials);
        }

        public static ReverseGeocodeRequest CreateReverseGeocodeRequest(Microsoft.Maps.MapControl.Location location, string credentials)
        {
            var request = new ReverseGeocodeRequest();
            request.Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = credentials };
            request.Location = new Microsoft.Maps.MapControl.Location() { Latitude = location.Latitude, Longitude = location.Longitude, Altitude =location.Altitude };
            return request;
        }

        public static GeocodeRequest CreateReverseGeocodeRequest(string query)
        {
            return CreateReverseGeocodeRequest(query, GeocodeServiceCredentials);
        }

        public static GeocodeRequest CreateReverseGeocodeRequest(string query, string credentials)
        {
            var request = new GeocodeRequest();
            request.Query = query;
            request.Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = credentials };
            return request;

        }




        public static RouteService.RouteServiceClient GetRouteService(DependencyObject obj = null)
        {
            RouteService.RouteServiceClient service = null;
            if (IsDesignMode(obj))
            {
                Binding binding = new BasicHttpBinding();
                EndpointAddress address = new EndpointAddress("http://dev.virtualearth.net/webservices/v1/geocodeservice/GeocodeService.svc");
                service = new RouteService.RouteServiceClient(binding, address);
            }
            else
            {
                service = new RouteService.RouteServiceClient("CustomBinding_IRouteService");
            }

            return service;
        }

    }

}

