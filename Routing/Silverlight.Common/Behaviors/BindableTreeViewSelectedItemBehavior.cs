using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Silverlight.Common.Behaviors
{

    public class BindableTreeViewSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding", typeof(object), typeof(BindableTreeViewSelectedItemBehavior), new PropertyMetadata(BindingChanged));

        private static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(BindableTreeViewSelectedItemBehavior), new PropertyMetadata(SelectedItemChanged));

        private bool createdBindings;
        private bool isUpdatingTree;

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public object Binding
        {
            get
            {
                return this.GetValue(BindingProperty);
            }

            set
            {
                this.SetValue(BindingProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
            this.AssociatedObject.SelectedItemChanged += this.AssociatedObjectSelectedItemChanged;
            if (null != this.AssociatedObject.DataContext)
            {
                this.CreateBindings();
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectedItemChanged -= this.AssociatedObjectSelectedItemChanged;
        }

        private static void BindingChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            // establish bindings
            var behavior = source as BindableTreeViewSelectedItemBehavior;
            if (null == behavior)
            {
                return;
            }

            behavior.CreateBindings();
        }

        private static void SelectedItemChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            // find the container in the treeview for the selected item and set it's IsSelected property
            var behavior = source as BindableTreeViewSelectedItemBehavior;
            if (null == behavior)
            {
                return;
            }

            behavior.UpdateTreeViewSelectedItem();
        }

        private void UpdateTreeViewSelectedItem()
        {
            if (!this.createdBindings)
            {
                return;
            }

            if (this.isUpdatingTree)
            {
                // we started this one
                return;
            }

            if (null == this.AssociatedObject)
            {
                return;
            }

            var selectedItem = this.GetValue(SelectedItemProperty);
            var generator = this.AssociatedObject.ItemContainerGenerator;
            if (null == generator)
            {
                return;
            }

            // check for top level item
            
            this.AssociatedObject.ExpandAll();
            this.AssociatedObject.UpdateLayout();
            var node = this.AssociatedObject.GetContainerFromItem(selectedItem);

            if (null != node)
            {
                node.IsSelected = true;
                this.AssociatedObject.CollapseAllButSelectedPath();
            }
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.createdBindings)
            {
                this.CreateBindings();
            }
            else
            {
                this.UpdateTreeViewSelectedItem();
            }
        }

        private void CreateBindings()
        {
            // clear any existing binding
            this.ClearValue(SelectedItemProperty);

            var data = this.AssociatedObject.DataContext;
            if (null == data)
            {
                return;
            }

            // retrieve the developer's binding specification
            var expression = this.ReadLocalValue(BindingProperty) as BindingExpression;
            if (null == expression)
            {
                return;
            }

            // create our own Binding
            var binding = new Binding
            {
                Source = data,
                Path = new PropertyPath(expression.ParentBinding.Path.Path),
                Mode = BindingMode.TwoWay
            };

            // set our flag first since the call to SetBinding will call the change event handler
            this.createdBindings = true;
            BindingOperations.SetBinding(this, SelectedItemProperty, binding);
        }

        private void AssociatedObjectSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!this.createdBindings)
            {
                return;
            }

            var treeview = sender as TreeView;
            if (null == treeview)
            {
                return;
            }

            // update the value of our binding to the newly selected item which will get passed to the data context
            this.isUpdatingTree = true;
            try
            {
                this.SetValue(SelectedItemProperty, treeview.SelectedValue);
            }
            finally
            {
                this.isUpdatingTree = false;
            }
        }
    }
}