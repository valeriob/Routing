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

namespace Silverlight.Common.Controls.ActionQueue
{
    public partial class ActionQueue : UserControl
    {
        public string Category { get; set; }

        ActionQueueViewModel ViewModel;

        public ActionQueue()
        {
            InitializeComponent();

            Loaded += (sender, e) => 
            {
                DataContext = ViewModel = new ActionQueueViewModel(Category);
            };
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.PopupOpen = !ViewModel.PopupOpen;
        }

        private void PopUpButton_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
