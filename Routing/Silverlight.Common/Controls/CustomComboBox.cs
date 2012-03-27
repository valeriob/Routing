using System;
using System.Net;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;


namespace Silverlight.Common.Controls
{
    public class CustomComboBox : System.Windows.Controls.ComboBox
    {
        public CustomComboBox()
        {
            this.Loaded += new RoutedEventHandler(ComboBox_Loaded);
            this.SelectionChanged += new SelectionChangedEventHandler(ComboBox_SelectionChanged);
        }

        void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            SetSelectionFromValue();
        }

        private object _selection;

        void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _selection = e.AddedItems[0];
                SelectedValue = GetMemberValue(_selection);
            }
            //else
            //{
            //    _selection = null;
            //    SelectedValue = null;
            //}
        }

        private object GetMemberValue(object item)
        {
            //return item.GetType().GetProperty(ValueMemberPath).GetValue(item, null);
            return GetValue(item, ValueMemberPath);
        }

        public static object GetValue(object source, string path)
        {
            object result = source;
            if (string.IsNullOrWhiteSpace(path))
                return source;

            var queue = new Queue<string>(path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
            while (queue.Any())
            {
                var relativePath = queue.Dequeue();

                var pi = result.GetType().GetProperty(relativePath);
                if (pi == null)
                    throw new Exception("Property not found");

                result = pi.GetValue(result, null);
            }
            return result;
        }



        public static DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(CustomComboBox), null);
        public string ValueMemberPath
        {
            get
            {
                return ((string)(base.GetValue(CustomComboBox.ValueMemberPathProperty)));
            }
            set
            {
                base.SetValue(CustomComboBox.ValueMemberPathProperty, value);
            }
        }

        public static DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(CustomComboBox),
          new PropertyMetadata((o, e) => { ((CustomComboBox)o).SetSelectionFromValue(); }));

        public object SelectedValue
        {
            get
            {
                return ((object)(base.GetValue(CustomComboBox.SelectedValueProperty)));
            }
            set
            {
                base.SetValue(CustomComboBox.SelectedValueProperty, value);
            }
        }

        private void SetSelectionFromValue()
        {
            var value = SelectedValue;
            if (Items.Count > 0 && value != null)
            {
                var sel = (from item in Items
                           //where GetMemberValue(item).Equals(value)
                           where item != null && value.Equals(GetMemberValue(item))
                           select item).SingleOrDefault();
                _selection = sel;
                SelectedItem = sel;
            }
            else
            {
                _selection = null;
                SelectedItem = null;
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            SetSelectionFromValue();
        }
    }

}
