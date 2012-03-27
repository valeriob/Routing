using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Silverlight.Common.Controls
{
    public class CustomTreeView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            var tvItem = new CustomTreeViewItem(this);
            tvItem.Expanded += new RoutedEventHandler(OnExpanded);
            return tvItem;
        }

        internal void OnExpanded(object sender, RoutedEventArgs e)
        {
            if (NodeExpanded != null)
                NodeExpanded(sender, e);

            ExecuteNodeExpanded_Command(sender);
        }
        public event RoutedEventHandler NodeExpanded;



        public ICommand NodeExpanded_Command
        {
            get { return (ICommand)GetValue(NodeExpanded_CommandProperty); }
            set { SetValue(NodeExpanded_CommandProperty, value); }
        }
        public static readonly DependencyProperty NodeExpanded_CommandProperty = DependencyProperty.Register("NodeExpanded_Command", typeof(ICommand), typeof(CustomTreeView), null);




        private void ExecuteNodeExpanded_Command(object sender)
        {
            object commandParameter = null;
            var tvItem = sender as CustomTreeViewItem;
            if(tvItem != null)
                commandParameter = tvItem.DataContext;

            ICommand command = this.NodeExpanded_Command;
            if ((command != null) && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

 

 



 
    }

    public class CustomTreeViewItem : TreeViewItem
    {
        public CustomTreeView _customTreeView;

        public CustomTreeViewItem(CustomTreeView customTreeView)
        {

            _customTreeView = customTreeView;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var t = new CustomTreeViewItem(_customTreeView);
            t.Expanded += new RoutedEventHandler(t_Expanded);
            return t;
        }

        void t_Expanded(object sender, RoutedEventArgs e)
        {
            _customTreeView.OnExpanded(sender, e);
        }
    }




}
