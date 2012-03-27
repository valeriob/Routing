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

namespace Routing.Silverlight.Views
{
    public partial class SearchScenario : Page
    {
        public SearchScenario()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Scenarios_EntityActivated(object sender, EventArgs e)
        {
            var entity = scenario.ViewModel.SelectedEntity;
            var uri = new Uri("/SimulateScenario?Id="+entity.Id, UriKind.RelativeOrAbsolute);

            this.NavigationService.Navigate(uri);
        }

    }
}
