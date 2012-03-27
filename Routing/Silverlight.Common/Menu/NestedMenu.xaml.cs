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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Silverlight.Common.Menu
{

    public partial class NestedMenu : UserControl
    {
        public NestedMenu()
        {
            DataContext = this;
            Nodes = new ObservableCollection<AbstractNode>();
            Path = new ObservableCollection<Node>();


            InitializeComponent();

            var r1 = new Node() { Name = "Root 1" };
            r1.Children.Add(new Leaf() { Name = "Leaf", Parent=r1 });
            r1.Children.Add(new Node() { Name = "Node 1", Parent=r1 });

            var r2 = new Node() { Name = "Root 2" };
            r2.Children.Add(new Leaf() { Name = "Leaf 2", Parent=r2 });
            r2.Children.Add(new Node() { Name = "Node 3", Parent=r2 });



            Nodes.Add(r1);
            Nodes.Add(r2);

            var megaroot = new Node() { Name = "Home" };
            megaroot.Children.Add(r1);
            megaroot.Children.Add(r2);
            r1.Parent = megaroot;
            r2.Parent = megaroot;

            SelectedNode = megaroot;
            Path.Add(megaroot);
        }

        public Binding Binding { get; set; }

        public Binding ChildrenBinding { get; set; }

        public Binding UrlBinding { get; set; }


        //public Binding Binding
        //{
        //    get { return (Binding)GetValue(BindingProperty); }
        //    set { SetValue(BindingProperty, value); }
        //}
        //public static readonly DependencyProperty BindingProperty =  DependencyProperty.Register("Binding", typeof(Binding), typeof(CustomControl), null);

        public ObservableCollection<AbstractNode> Nodes
        {
            get { return (ObservableCollection<AbstractNode>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }
        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(ObservableCollection<AbstractNode>), typeof(NestedMenu), new PropertyMetadata(NodesChanged));
        private static void NodesChanged(object sender, DependencyPropertyChangedEventArgs e)
        { 
            var oldValue = e.OldValue as ObservableCollection<AbstractNode>;
            if(oldValue!=null)
                oldValue.CollectionChanged -= CollectionChanged;
            var newValue = e.NewValue as ObservableCollection<AbstractNode>;
            if(newValue!=null)
                newValue.CollectionChanged += CollectionChanged;

            var control = sender as NestedMenu;
            if (control != null && control.Path != null)
            {
                var root = new Node() { Name = "Home" };
                foreach (var node in newValue)
                    root.Children.Add(node);

                control.Path.Clear();
                control.SelectedNode = root;
                control.Path.Add(root);
            }
        }

        static void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
            //INavigate n; n.Navigate(new Uri());
        }

        public  ObservableCollection<Node> Path
        {
            get { return (ObservableCollection<Node>)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(ObservableCollection<Node>), typeof(NestedMenu), null);

        public Node SelectedNode
        {
            get { return (Node)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(Node), typeof(NestedMenu), null);




        public string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(NestedMenu), null);

        


        public void Navigate(AbstractNode destinationNode)
        {
            Node node = destinationNode as Node;
            Leaf leaf = destinationNode as Leaf;

            if (node != null)
            {
                Path.Add(node);
                SelectedNode = node;
            }

            if (leaf != null)
            { 
                // NavigationService invoke
                TryInternalNavigate(new Uri("/About", UriKind.Relative));
            }

        }

        public void Back(Node destinationNode) 
        {
            foreach (var node in Path.ToList())
                if (IsAnchestor(destinationNode, node))
                    Path.Remove(node);

            SelectedNode = destinationNode;
        }

        public bool IsAnchestor(Node anchestor, Node node)
        {
            var parent = node.Parent as Node;
            if (parent != null && parent == anchestor)
                return true;
            else
                if (node.Parent == null)
                    return false;
                else
                    return IsAnchestor(anchestor, parent);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Primitives.ToggleButton;
            var node = button.DataContext as AbstractNode;
            Navigate(node);
        }

        private void ToggleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Primitives.ToggleButton;
            var node = button.DataContext as Node;
            Back(node);
        }

        private INavigate FindNavigator(FrameworkElement baseFE, DependencyObject lastSearchedSubtree)
        {
            if (baseFE != null)
            {
                if ((baseFE is INavigate) && (string.Equals(baseFE.Name, this.TargetName) || string.IsNullOrEmpty(this.TargetName)))
                {
                    return (baseFE as INavigate);
                }
                bool flag = baseFE is Popup;
                int num = flag ? 1 : VisualTreeHelper.GetChildrenCount(baseFE);
                for (int i = 0; i < num; i++)
                {
                    DependencyObject objA = flag ? ((Popup)baseFE).Child : VisualTreeHelper.GetChild(baseFE, i);
                    if (!object.ReferenceEquals(objA, lastSearchedSubtree))
                    {
                        INavigate navigate = this.FindNavigator(objA as FrameworkElement, lastSearchedSubtree);
                        if (navigate != null)
                        {
                            return navigate;
                        }
                    }
                }
            }
            return null;
        }

        private bool TryInternalNavigate(Uri uri)
        {
            INavigate navigate = null;
            DependencyObject navigationStart = this;
            DependencyObject reference = null;
            DependencyObject lastSearchedSubtree = this;
            do
            {
                reference = VisualTreeHelper.GetParent(navigationStart);
                if ((reference == null) && (navigationStart is FrameworkElement))
                {
                    reference = ((FrameworkElement)navigationStart).Parent;
                }
                if ((reference != null) && ((reference is INavigate) || (VisualTreeHelper.GetParent(reference) == null)))
                {
                    navigate = this.FindNavigator(reference as FrameworkElement, lastSearchedSubtree);
                    if (navigate != null)
                    {
                        return navigate.Navigate(uri);
                    }
                    lastSearchedSubtree = reference;
                }
                navigationStart = reference;
            }
            while (navigationStart != null);
            return false;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && Binding!=null)
                textBlock.SetBinding(TextBlock.TextProperty, Binding);
        }



 

 

    }


}
