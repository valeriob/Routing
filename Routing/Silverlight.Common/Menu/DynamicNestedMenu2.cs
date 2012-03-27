using System;
using System.Net;
using System.Linq;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace Silverlight.Common.Menu
{

    public class DynamicNestedMenu2 : UserControl
    {
        public ObservableCollection<AbstractNode> Nodes { get; set; }

        public ObservableCollection<AbstractNode> Path { get; set; }

        //public Node SelectedNode { get; set; }


        public Node SelectedNode
        {
            get { return (Node)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(Node), typeof(DynamicNestedMenu2), null);



        public DynamicNestedMenu2()
        {
            Nodes = new ObservableCollection<AbstractNode>();
            Path = new ObservableCollection<AbstractNode>();

            //SelectedNode = 
            var r1 = new Node() { Name = "Root 1" };
            r1.Children.Add(new Leaf() { Name = "Leaf" });
            r1.Children.Add(new Node() { Name = "Node 1" });

            var r2 = new Node() { Name = "Root 2" };
            r2.Children.Add(new Leaf() { Name = "Leaf 2" });
            r2.Children.Add(new Node() { Name = "Node 3" });

            Nodes.Add(r1);
            Nodes.Add(r2);

            SelectedNode = new Node() { Name = "MegaRoot" };
            SelectedNode.Children.Add(r1);
            SelectedNode.Children.Add(r2);
        }


        public void Navigate(AbstractNode destinationNode)
        {
            Node node = destinationNode as Node;
            Leaf leaf = destinationNode as Leaf;

            if (node != null)
            {
                Path.Add(destinationNode);
                SelectedNode = node;
            }
            if (leaf != null)
            {
                // NavigationService invoke
            }

        }

        public void Back()
        {
            if (SelectedNode == null || SelectedNode.Parent == null)
                return;

            Path.Remove(SelectedNode);
            SelectedNode = SelectedNode.Parent as Node;

        }


        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Primitives.ToggleButton;
            var node = button.DataContext as AbstractNode;
            Navigate(node);
        }
    }

}
