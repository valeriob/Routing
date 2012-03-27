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
using System.Windows.Data;
using System.Reactive.Linq;
using Silverlight.Common.Helpers;

namespace Silverlight.Common.DynamicSearch
{
    public partial class Search : Page
    {
        public ISearchViewModel SearchViewModel { get; set; }

        public Search()
        {
            InitializeComponent();

            ucSearch.EntityActivated += (sender, e) =>
            {
                ucSearch.ViewModel.EditSelectedEntity();
            };

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var entityName = NavigationContext.QueryString["entity"];

            var abstractType = typeof(SearchViewModel<>);
            var entityType = ReflectionHelper.GetAssemblyType(entityName);
            var helper = SearchHelper.Build(entityType);

            helper.BuildColumns(ucSearch.dataGridEntities);
            SearchViewModel = helper.GetViewModel();

            SearchViewModel.Refresh();

            DataContext = SearchViewModel;

        }

        protected void NavigationClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }





    }
}
