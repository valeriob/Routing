using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Routing.Silverlight.Models;
using System.Collections.Specialized;
using Microsoft.Maps.MapControl;
using Routing.Silverlight.Models.SimulateScenario;

namespace Routing.Silverlight.Views
{
    public partial class Simulate : Page
    {
        SimulateScenarioViewModel ViewModel;

        public Simulate()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var scenarioId = e.Uri.GetValue("Id");

            DataContext = ViewModel = new SimulateScenarioViewModel(scenarioId);
            ViewModel.Routes.CollectionChanged += new NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
        }

        

        void Routes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var routeLayers = Map.Children.OfType<MapLayer>().Where(m => m.DataContext != null && m.DataContext.GetType() == typeof(RouteViewModel));

            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (RouteViewModel route in e.NewItems)
                {
                    var layer = Build_Layer_For_Route(route);
                    Map.Children.Add(layer);
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (RouteViewModel route in e.OldItems)
                {
                    var layer = routeLayers.SingleOrDefault(r => r.DataContext == route);
                    if (layer == null)
                        continue;
                    Map.Children.Remove(layer);
                }
            }
        }

        protected MapLayer Build_Layer_For_Route(RouteViewModel route)
        {
            var layer = new MapLayer ();
            Color routeColor = Colors.Blue;
            SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
            var routeLine = new MapPolyline();
            routeLine.Locations = new LocationCollection();
            routeLine.Stroke = routeBrush;
            routeLine.Opacity = 0.65;
            routeLine.StrokeThickness = 5.0;

            foreach (var p in route.Locations)
                routeLine.Locations.Add(new Location(p.Location.Latitude, p.Location.Longitude));
            layer.Children.Add(routeLine);


            foreach (var location in route.Locations)
            {
                var point = DrawPoint(location);
                layer.Children.Add(point);
            }

            return layer;
        }

        protected Ellipse DrawPoint(Location_Reference locationRef)
        {
            //return new Ellipse();

            var point = new Ellipse();
            point.Width = 10;
            point.Height = 10;
            point.Fill = new SolidColorBrush(Colors.Red);
            point.Opacity = 0.65;
            var location = new Location(locationRef.Location.Latitude, locationRef.Location.Longitude);
            MapLayer.SetPosition(point, location);
            MapLayer.SetPositionOrigin(point, PositionOrigin.Center);
            //point.Tag = itineraryItem;
            //point.MouseEnter += new System.Windows.Input.MouseEventHandler(point_MouseEnter);
            //point.MouseLeave += new System.Windows.Input.MouseEventHandler(point_MouseLeave);

            return point;
        }

    }

   
}
