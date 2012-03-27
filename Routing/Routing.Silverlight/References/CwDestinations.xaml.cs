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
using Routing.Silverlight.Models.References;
using Routing.Silverlight.ServiceReferences;

namespace Routing.Silverlight.References
{
    public partial class CwDestinations : ChildWindow
    {
        public CwDestinations(DestinationSearchViewModel viewModel) 
        {
            InitializeComponent();

            var control = new Destinations(viewModel);
            control.EntityActivated += (sender, e) => 
            {
                DialogResult = true;
            };

            LayoutRoot.Children.Add(control);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}

