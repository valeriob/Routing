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
using Routing.Silverlight.Models;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;
using ReactiveUI;

namespace Routing.Silverlight.Address_Validation
{
    public partial class GeoSearchPopup : UserControl
    {
        public Search_LocationViewModel ViewModel;

        //public GeoSearchPopup(Search_LocationViewModel viewModel)
        //{
        //    InitializeComponent();
        //    DataContext = ViewModel = viewModel;
        //}
    

        public GeoSearchPopup()
        {
            InitializeComponent();
            DataContext =   ViewModel = new Search_LocationViewModel();
        }

        public void Initialize(string searchAddress)
        {
            ViewModel.Initialize(searchAddress);
        }



        private void Search_Box_Key_Pressed(object sender, KeyEventArgs e)
        {

        }

        private void buttonCloseResults_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBoxResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Confirm_Position()
        {

        }

        private void Conferma_Positione(object sender, RoutedEventArgs e)
        {
            ViewModel.Confirm_Position();
        }

        private void listBoxResults_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                ViewModel.Confirm_Position();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
        }

        private void LayoutRoot_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                ViewModel.Cancel();
        }
    }
}
