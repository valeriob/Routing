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

namespace Routing.Silverlight.References
{
    public partial class Destinations : UserControl
    {
        public event EventHandler EntityActivated;
        //public event EventHandler<DialogResult_EventArgs> IsDone;

        Address_Validation_Helper validator;
        DestinationSearchViewModel ViewModel;

        public Destinations()
            : this(new DestinationSearchViewModel())
        {

        }

        public Destinations(DestinationSearchViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;

            //MessageBus.Current.Listen<Validation_Canceled>().Subscribe(s =>
            //{
            //    validator.Dispose();
            //});
            //MessageBus.Current.Listen<Address_Validated>().Subscribe(s =>
            //{
            //    validator.Dispose();
            //    ViewModel.NearBy = new Microsoft.Maps.MapControl.Location(s.Location);
            //});
        }

        private void Search_Address_TextChanged(object sender, global::Silverlight.Common.Controls.TextChangedEventArgs e)
        {
            var control = sender as Control;
            var padri = control.GetVisualAncestors().ToList();
            var parentGrid = control.Parent as Panel;

            validator = new Address_Validation_Helper(parentGrid);
            validator.Validate_Address(e.IsSilent, e.Text);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Refresh();
        }


        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)

                OnEntityActivated();
            //OnIsDone(true);
        }

        protected void OnEntityActivated()
        {
            if (EntityActivated != null)
                EntityActivated(this, new EventArgs());
        }

        //protected void OnIsDone(bool positive)
        //{
        //    if (IsDone != null)
        //    {
        //        var result = ViewModel.SelectedEntity;
        //        if (!positive)
        //            result = null;
        //        IsDone(this, new DialogResult_EventArgs(positive));
        //    }
        //}
    }


    //public class DialogResult_EventArgs : EventArgs
    //{
    //    public bool DialogResult { get; protected set; }

    //    public DialogResult_EventArgs(bool entity)
    //    {
    //        DialogResult = entity;
    //    }

    //}
  
}
