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

namespace Silverlight.Common.Helpers
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }

    
    // Example usage:
    /*
    public class CustomTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template1
        {
            get;
            set;
        }

        public DataTemplate Template2
        {
            get;
            set;
        }

        public DataTemplate Template3
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Entity1)
                return Template1;
            if (item is Entity2)
                return Template2;
            if (item is Entity3)
                return Template3;

            return base.SelectTemplate(item, container);
        }
    }
     */

}
