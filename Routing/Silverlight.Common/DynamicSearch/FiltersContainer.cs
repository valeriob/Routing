using System;
using System.Net;
using Silverlight.Common.DynamicSearch;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;

namespace Silverlight.Common.DynamicSearch
{
    public class FiltersContainer : ItemsControl
    {
        public DataTemplate StringDataTemplate
        {
            get { return (DataTemplate)GetValue(StringDataTemplateProperty); }
            set { SetValue(StringDataTemplateProperty, value); }
        }
        public static readonly DependencyProperty StringDataTemplateProperty = DependencyProperty.Register("StringDataTemplate", typeof(DataTemplate), typeof(FiltersContainer), null);

        public DataTemplate DateTimeDataTemplate
        {
            get { return (DataTemplate)GetValue(DateTimeDataTemplateProperty); }
            set { SetValue(DateTimeDataTemplateProperty, value); }
        }
        public static readonly DependencyProperty DateTimeDataTemplateProperty = DependencyProperty.Register("DateTimeDataTemplate", typeof(DataTemplate), typeof(FiltersContainer), null);

        public DataTemplate BoolDataTemplate
        {
            get { return (DataTemplate)GetValue(BoolDataTemplateProperty); }
            set { SetValue(BoolDataTemplateProperty, value); }
        }
        public static readonly DependencyProperty BoolDataTemplateProperty = DependencyProperty.Register("BoolDataTemplate", typeof(DataTemplate), typeof(FiltersContainer), null);

       

        public DataTemplate IntDataTemplate
        {
            get { return (DataTemplate)GetValue(IntDataTemplateProperty); }
            set { SetValue(IntDataTemplateProperty, value); }
        }
        public static readonly DependencyProperty IntDataTemplateProperty =  DependencyProperty.Register("IntDataTemplate", typeof(DataTemplate), typeof(FiltersContainer), null);

        



        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {

            if (!object.ReferenceEquals(element, item))
            {
                ContentPresenter presenter = element as ContentPresenter;
                ContentControl control = null;
                if (presenter == null)
                {
                    control = element as ContentControl;
                    if (control == null)
                        return;
                }

                DataTemplate itemTemplate = null;
                if ((this.ItemTemplate != null) && (this.DisplayMemberPath != null))
                {
                    throw new InvalidOperationException("Cannot set ItemTemplate and DisplayMemberPath simultaneously");
                }

                var filter = item as DataBindableFilter;
                if (!(item is UIElement) && filter != null)
                {
                    if(filter.PropertyType == typeof(string))
                        itemTemplate = StringDataTemplate;

                    if (filter.PropertyType == typeof(DateTime) ||filter.PropertyType == typeof(DateTime?))
                        itemTemplate = DateTimeDataTemplate;

                    if (filter.PropertyType == typeof(bool) || filter.PropertyType == typeof(bool?))
                        itemTemplate = BoolDataTemplate;

                    if (filter.PropertyType == typeof(int) || filter.PropertyType == typeof(int?))
                        itemTemplate = IntDataTemplate;
                }
                if (presenter != null)
                {
                    if (itemTemplate != null)
                    {
                        presenter.Content = item;
                        presenter.ContentTemplate = itemTemplate;
                    }
                    else
                    {
                        if (DisplayMemberPath != null)
                            presenter.SetBinding(ContentControl.ContentProperty, new Binding(this.DisplayMemberPath));
                    }
                }
                else
                {
                    control.Content = item;
                    control.ContentTemplate = itemTemplate;
                }
            }

        }

    }

}
