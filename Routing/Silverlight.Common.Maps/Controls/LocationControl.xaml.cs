using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.ComponentModel;
using System.Windows.Data;

using System.Windows.Threading;
using Microsoft.Maps.MapControl;
//using Silverlight.Common.Maps.GeocodeService;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using GeocodeResult = Silverlight.Common.Maps.GeocodeService.GeocodeResult;
using Silverlight.Common.Maps.GeocodeService;

namespace Silverlight.Common.Maps.Controls
{
    public partial class LocationControl : UserControl, INotifyPropertyChanged
    {
        protected Silverlight.Common.Maps.GeocodeService.GeocodeServiceClient GeoService { get; set; }
        protected DispatcherTimer Timer;

        #region Public properties not bindable
        public bool IsDraggablePushpinEnabled
        {
            get
            {
                return draggablePushpin.IsEnabled;
            }
            set
            {
                draggablePushpin.IsEnabled = value;
            }
        }
        public bool IsSearchEnabled
        {
            get
            {
                return SearchRow.Visibility == System.Windows.Visibility.Visible;
            }
            set
            {
                if (value)
                {
                    SearchRow.Visibility = Visibility.Visible;
                }
                else
                {
                    SearchRow.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region Dependencies properties
        public Location Location
        {
            get { return (Location)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(LocationControl), new PropertyMetadata(LocationChanged));
        private static void LocationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var newLocation = e.NewValue as Location;
            var locationControl = sender as LocationControl;
            locationControl.map.Center = newLocation;
            locationControl.Timer.Start();
        }
        public string GeocodeServiceCredentials
        {
            get { return (string)GetValue(GeocodeServiceCredentialsProperty); }
            set { SetValue(GeocodeServiceCredentialsProperty, value); }
        }
        public static readonly DependencyProperty GeocodeServiceCredentialsProperty = DependencyProperty.Register("GeocodeServiceCredentials", typeof(string), typeof(LocationControl), new PropertyMetadata(GeocodeServiceCredentialsChanged));
        private static void GeocodeServiceCredentialsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var locationControl = sender as LocationControl;
            locationControl.map.CredentialsProvider = new ApplicationIdCredentialsProvider((string)e.NewValue);
        }



        public GeocodeResult CurrentLocation
        {
            get { return (GeocodeResult)GetValue(CurrentLocationProperty); }
            set { SetValue(CurrentLocationProperty, value); }
        }
        public static readonly DependencyProperty CurrentLocationProperty = 
            DependencyProperty.Register("CurrentLocation", typeof(GeocodeResult), typeof(LocationControl), null);




        //public string Credentials
        //{
        //    get { return (string)GetValue(CredentialsProperty); }
        //    set { SetValue(CredentialsProperty, value); }
        //}
        //public static readonly DependencyProperty CredentialsProperty = DependencyProperty.Register("Credentials", typeof(string), typeof(LocationControl), null);

        
        
        
        //public Object MyDataContext
        //{
        //    get { return (Object)GetValue(MyDataContextProperty); }
        //    set { SetValue(MyDataContextProperty, value); }
        //}
        //public static readonly DependencyProperty MyDataContextProperty = DependencyProperty.Register("MyDataContext", typeof(Object), typeof(LocationControl), new PropertyMetadata(DataContextChanged));
        //private static void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    LocationControl locControl = (LocationControl)sender;
        //    locControl.draggablePushpin.DataContext = locControl;
        //    locControl.textBoxSearchAddress.DataContext = locControl;
        //    locControl.searchResultRow.DataContext = locControl;
        //    locControl.listBoxResults.DataContext = locControl;
        //    locControl.DataContext = e.NewValue;
        //}
        #endregion

        public LocationControl()
        {
            InitializeComponent();

            if (DesignerProperties.IsInDesignTool)
            {
                BuildDesignData();
                return;
            }

            GeoService = ServiceHelper.GetGeocodeService();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(500000);
            
            Timer.Tick += (sender, e) =>
            {
                if (Location != null)
                {
                    var request = ServiceHelper.CreateReverseGeocodeRequest(Location, GeocodeServiceCredentials);
                    request.Credentials.Token = GeocodeServiceCredentials;
                    GeoService.ReverseGeocodeAsync(request);
                }
                lock (Timer)
                    Timer.Stop();
            };

            GeoService.GeocodeCompleted += (sender, e) =>
            {
                GeocodeResult = null;
                if (e.Error == null)
                {
                    GeocodeResult = e.Result.Results;
                }
            };

            GeoService.ReverseGeocodeCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    AddressSearchQuery = e.Result.Results.Select(r => r.DisplayName).FirstOrDefault();
                    CurrentLocation = e.Result.Results.FirstOrDefault();
                }
            };

            //DataContext = this;
            draggablePushpin.DataContext = this;
            textBoxSearchAddress.DataContext = this;
            searchResultRow.DataContext = this;
            listBoxResults.DataContext = this;

        }
            

        protected string _addressSearchQuery;
        public string AddressSearchQuery
        {
            get { return _addressSearchQuery; }
            set { _addressSearchQuery = value; OnPropertyChanged("AddressSearchQuery"); }
        }

        //protected GeocodeResult _currentLocation;
        //public GeocodeResult CurrentLocation
        //{
        //    get { return _currentLocation; }
        //    set { _currentLocation = value; OnPropertyChanged("CurrentLocation"); }
        //}

        protected IEnumerable<GeocodeResult> _geocodeResult;
        public IEnumerable<GeocodeResult> GeocodeResult
        {
            get { return _geocodeResult; }
            set { _geocodeResult = value; OnPropertyChanged("GeocodeResult"); OnPropertyChanged("HasResults"); }
        }

        public bool HasResults { get { if (GeocodeResult == null) return false; return GeocodeResult.Count() != 0; } }

        public void SearchForAddresses(string query)
        {
            GeocodeResult = null;
            GeoService.GeocodeAsync(ServiceHelper.CreateReverseGeocodeRequest(query, GeocodeServiceCredentials));
        }

        private void listBoxResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var geocodeResult = listBoxResults.SelectedItem as GeocodeResult;
            if (geocodeResult == null)
                return;

            var location = geocodeResult.Locations.FirstOrDefault();
            if (location == null)
                return;

            Location = location;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchForAddresses(textBoxSearchAddress.Text);
        }

        private void textBoxSearchAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchForAddresses(textBoxSearchAddress.Text);
        }

        private void buttonCloseResults_Click(object sender, RoutedEventArgs e)
        {
            GeocodeResult = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BuildDesignData()
        {
            List<GeocodeResult> res = new List<GeocodeResult>();
            GeocodeResult r = new GeocodeResult
            {
                DisplayName = "pippo",
            };
            //res.Add(r);
            GeocodeResult = res;
        }
    }
}

