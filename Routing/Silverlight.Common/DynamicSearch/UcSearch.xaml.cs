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
using OnEnergy.Silverlight.Common;
using System.Windows.Markup;
using System.Text;
using System.Windows.Data;
using System.Linq.Expressions;
using Silverlight.Common.DynamicSearch;


namespace Silverlight.Common.DynamicSearch
{
    public partial class UcSearch : UserControl
    {
        public event EventHandler EntityActivated;

        public ISearchViewModel ViewModel
        {
            get { return DataContext as ISearchViewModel; }
        }
        
        public UcSearch()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonHelper.IsDoubleClick(sender, e))
            {
                if (ViewModel != null)
                {
                    OnEntityActivated();
                   
                }
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Refresh();
            }
        }

        protected void OnEntityActivated()
        {
            if (EntityActivated != null)
                EntityActivated(this, new EventArgs());
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

   



 
   

}
