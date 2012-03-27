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
    public partial class CwSearch : ChildWindow
    {
        public CwSearch()
        {
            InitializeComponent();

            ucSearch.EntityActivated += (sender, e) =>
            {
                DialogResult = true;
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }




 
   

}
