using System;
using System.Net;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using Silverlight.Common.Helpers;

namespace Silverlight.Common.Controls.WidgetContainer
{
    [TemplateVisualState(GroupName = "DraggingGroup", Name = "Dragging")]
    [TemplateVisualState(GroupName = "DraggingGroup", Name = "NotDragging")]
    [TemplateVisualState(GroupName = "Common", Name = "Normal")] 
    [TemplateVisualState(GroupName = "Common", Name = "MouseOver")] 
    public class WidgetItemContainer : ContentControl
    {
        public TranslateTransform Traslate { get; set; }
        public ScaleTransform Scale { get; set; }


        public WidgetItemContainer()
        {
            DefaultStyleKey = typeof(WidgetItemContainer);

            var tGroup = new TransformGroup();
            tGroup.Children.Add(Traslate = new TranslateTransform());
            tGroup.Children.Add(Scale = new ScaleTransform());
            RenderTransform = tGroup;

            Background = new SolidColorBrush(Colors.Cyan);

            VisualStateManager.GoToState(this, "NotDragging", false);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "MouseOver", false);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "Normal", false);
        }


        public override string ToString()
        {
            return Content + "";
        }


    }
}
