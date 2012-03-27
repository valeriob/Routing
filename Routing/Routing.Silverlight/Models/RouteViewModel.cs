using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using System.Collections.ObjectModel;
using Routing.Silverlight.ScenarioService;
using System.Collections.Generic;
using maps = Silverlight.Common.Maps;

namespace Routing.Silverlight.Models
{
    public class RouteViewModel : ReactiveObject
    {
        public ObservableCollection<Location_Reference> Locations { get; set; }

        // public RouteViewModel(VoyageDto voyage, IEnumerable<maps.RouteService.ItineraryItem> itinerary)
        public RouteViewModel(VoyageDto voyage, IEnumerable<maps.RouteService.Location> points)
        {
            Locations = new ObservableCollection<Location_Reference>();

            foreach (var point in points)
            {
                Locations.Add(new Location_Reference
                {
                    Location = new Microsoft.Maps.MapControl.Location {  Latitude = point.Latitude, Longitude = point.Longitude }, 

                   // Display = point.Text
                });
            }

        }


    }
}
