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
using System.Collections;
using System.Collections.Specialized;

namespace Silverlight.Common.Menu
{
 
    public partial class DynamicNestedMenu : UserControl
    {
        public DynamicNestedMenu()
        {
            //DataContext = this;
            //Nodes = new ObservableCollection<object>();
            Path = new ObservableCollection<object>();

            InitializeComponent();

            LayoutRoot.DataContext = this;

            //Loaded += (sender, e) => 
            //{ 
            //    listBoxLeafs.SetBinding(ListBox.ItemsSourceProperty, new Binding("SelectedNode."+ChildrenBinding.Path)); 
            //};
            /*
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
            Path.Add(megaroot);*/
        }

        public Binding Binding { get; set; }

        private Binding _childrenBinding;
        public Binding ChildrenBinding 
        { 
            get { return _childrenBinding; } 
            set 
            { 
                _childrenBinding = value;
                listBoxLeafs.SetBinding(ListBox.ItemsSourceProperty, new Binding("SelectedNode." + ChildrenBinding.Path.Path)); 
            } 
        }

        public Binding UriBinding { get; set; }

        public ICollection Nodes
        {
            get { return (ObservableCollection<object>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }
        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(ICollection), typeof(DynamicNestedMenu), new PropertyMetadata(NodesChanged));
        private static void NodesChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //e.NewValue as INotifyCollectionChanged
            //ObservableCollection<object>

            var oldValue = e.OldValue as INotifyCollectionChanged;
            if (oldValue != null)
                oldValue.CollectionChanged -= CollectionChanged;

            var newValue = e.NewValue as INotifyCollectionChanged;
            if (newValue != null)
                newValue.CollectionChanged += CollectionChanged;

            var control = sender as DynamicNestedMenu;
            if (control != null && control.Path != null && newValue!=null)
            {

                control.Path.Clear();
                var collection = newValue as IEnumerable;
                foreach (var node in collection)
                    control.Path.Add(node);

                control.SelectedNode = collection.Cast<object>().FirstOrDefault();
            }
        }

        static void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
            //INavigate n; n.Navigate(new Uri());
        }

        public  ObservableCollection<object> Path
        {
            get { return (ObservableCollection<object>)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(ObservableCollection<object>), typeof(DynamicNestedMenu), null);

        public object SelectedNode
        {
            get { return (Node)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(object), typeof(DynamicNestedMenu), null);




        public string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(DynamicNestedMenu), null);




        public void Navigate(object node)
        {
            var propertyInfo = node.GetType().GetProperty(ChildrenBinding.Path.Path);
            if (node != null && propertyInfo!=null)
            {
                Path.Add(node);
                SelectedNode = node;
            }

            // TODO
            try
            {
                propertyInfo = node.GetType().GetProperty(UriBinding.Path.Path);
                Uri uri = new Uri(propertyInfo.GetValue(node, null) as string);
                TryInternalNavigate(new Uri("/About", UriKind.Relative));
            }
            catch (Exception)
            { }

            //if (leaf != null)
            //{ 
            //    // NavigationService invoke
            //    TryInternalNavigate(new Uri("/About", UriKind.Relative));
            //}

        }

        public void Back(object destinationNode) 
        {
            foreach (var node in Path.ToList())
                if (IsAnchestor(destinationNode, node))
                    Path.Remove(node);

            SelectedNode = destinationNode;
        }

        public bool IsAnchestor(object anchestor, object node)
        {
            var children = GetChildren(anchestor);
            if (children.Contains(node))
                return true;
            else
                return children.Any(c => IsAnchestor(c, node));
        }
        
        public IEnumerable<object> GetChildren(object source)
        {
            var propertyInfo = source.GetType().GetProperty(ChildrenBinding.Path.Path);
            if (propertyInfo == null)
                return new List<object>();
            var value = propertyInfo.GetValue(source, null);
            if(value==null)
                return new List<object>();
            return (value as System.Collections.IEnumerable).Cast<object>();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Primitives.ToggleButton;
            var node = button.DataContext;// as AbstractNode;
            Navigate(node);
        }

        private void ToggleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Primitives.ToggleButton;
            var node = button.DataContext;// as Node;
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
