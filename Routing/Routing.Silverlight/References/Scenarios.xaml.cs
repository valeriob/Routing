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
using ReactiveUI;
using Routing.Silverlight.Address_Validation;
using System.Windows.Controls.Primitives;
using Routing.Silverlight.Models;
using Routing.Silverlight.Models.References;
using Routing.Silverlight.ServiceReferences;
using Silverlight.Common;

namespace Routing.Silverlight.References
{
    public partial class Scenarios : UserControl
    {
        public event EventHandler EntityActivated;

        Address_Validation_Helper validator;
        public SearchScenarioViewModel ViewModel { get; protected set; }

        public Scenarios()
            : this(new SearchScenarioViewModel())
        {

        }

        public Scenarios(SearchScenarioViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;
        }



        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Refresh();
        }


        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonHelper.IsDoubleClick(sender, e))
                OnEntityActivated();
        }

        protected void OnEntityActivated()
        {
            if (EntityActivated != null)
                EntityActivated(this, new EventArgs());
        }

    }

  
}
