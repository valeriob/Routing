using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Common.Helpers;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;


namespace Silverlight.Common.Controls.WidgetContainer
{
    public partial class WidgetBoard : UserControl
    {
        static Size maxSize = new Size { Width = 1000, Height = 1000 };

        public double XUnit { get; set; }
        public double YUnit { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        protected EditAction Action { get; set; }
        protected WidgetItemContainer Selected_Element { get; set; }
        protected Point OriginalPosition { get; set; }
        protected Point MouseToElementRelativePosition { get; set; }

        public List<WidgetItemContainer> Widgets { get; set; }
        protected DelayedAction Rearrange { get; set; }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(WidgetBoard), new PropertyMetadata(null,
             new PropertyChangedCallback(OnItemsSourceChanged)));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var items = e.NewValue as IEnumerable;
            var widgets = d as WidgetBoard;

            foreach (var item in items)
            {
                var container = new WidgetItemContainer();
                container.DataContext = item;
                container.ContentTemplate = widgets.ItemTemplate;
                container.Content = item;

                widgets.Add_WidgetItemContainer(container);
            }
        }

        public Dictionary<WidgetItemContainer, Delegate> Handlers = new Dictionary<WidgetItemContainer, Delegate>(); 

        public void Add_WidgetItemContainer(WidgetItemContainer container)
        {
            var handler = new MouseButtonEventHandler(SnapGrid_MouseLeftButtonUp);
            Handlers[container] = handler;

            container.AddHandler(UserControl.MouseLeftButtonUpEvent, handler, true);

            //container.ContentTemplate = ItemTemplate;

            //container.SizeChanged += (from, ev) =>
            //{
            //    container.Measure(maxSize);
            //    container.Width = widgets.Calculate_Discrete_X(container.DesiredSize.Width);
            //    container.Height = widgets.Calculate_Discrete_X(container.DesiredSize.Height);
            //};

            Canvas.Children.Add(container);
            Widgets.Add(container);
        }

        public void Remove_WidgetItemContainer(WidgetItemContainer container)
        {
            var handler = Handlers[container];
            container.RemoveHandler(UserControl.MouseLeftButtonUpEvent, handler);
            Handlers.Remove(container);

            Canvas.Children.Remove(container);
            Widgets.Remove(container);
        }



        public WidgetBoard()
        {
            InitializeComponent();

            Init();
        }

        protected void Init()
        {
            MouseMove += new MouseEventHandler(OnMouseMove);
            Loaded += (sender, e) => OnLoaded();
            SizeChanged += (sender, e) => Rearrange.Postpone();

            Widgets = new List<WidgetItemContainer>();
            Action = EditAction.None;
            Rearrange = new DelayedAction();

            XUnit = 50;
            YUnit = 50;

            Rearrange.Action = () =>
            {
                var map = GetMap();
                //map.AutoEnlargeX = true;
                map.AutoEnlargeY = true;
                foreach (var container in Widgets)
                {
                    var spot = map.FindArea(container.DWidth(XUnit), container.DHeight(YUnit));
                    if (spot != null)
                    {
                        Canvas.SetTop(container, spot.Y * YUnit);
                        Canvas.SetLeft(container, spot.X * XUnit);
                        map.Set_ActualBusy(container, XUnit, YUnit);
                    }
                }
                InvalidateArrange();
            };

            var colors = new Color[] { Colors.Red, Colors.Blue, Colors.Gray, Colors.Green};
            var rnd = new Random(DateTime.Now.Millisecond);
            Background = new SolidColorBrush(colors[rnd.Next(4)]);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var container in Widgets)
            {
                container.Width = Calculate_Discrete_X(container.DesiredSize.Width);
                container.Height = Calculate_Discrete_X(container.DesiredSize.Height);
            }
            ScrollViewer.Arrange(new Rect { Height = finalSize.Height, Width= finalSize.Width });


            if (Widgets.Any())
            {
                var farRight = Widgets.First();
                var farBottom = Widgets.First();

                foreach (var w in Widgets.Skip(1))
                    if (Canvas.GetLeft(w) > Canvas.GetLeft(farRight))
                        farRight = w;

                foreach (var w in Widgets.Skip(1))
                    if (Canvas.GetTop(w) > Canvas.GetTop(farRight))
                        farBottom = w;

                Canvas.Width = Canvas.GetLeft(farRight) + farRight.Width;
                Canvas.Height = Canvas.GetTop(farBottom) + farBottom.Height;
            }

            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var widget in Widgets)
                widget.Measure(availableSize);
            ScrollViewer.Measure(availableSize);
            Canvas.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }


        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Selected_Element == null)
                return;

            if (Action == EditAction.Moving)
                Animate_Moving(e.GetPosition(Canvas));
            if (Action == EditAction.Resizing)
                Animate_Resizing(e.GetPosition(Selected_Element));
        }

        void OnLoaded()
        {
            var parent = VisualTreeHelper.GetParent(this) as UIElement;
            parent = VisualTreeHelper.GetParent(parent) as UIElement;
  
            parent.AddHandler(UserControl.MouseLeftButtonUpEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonUp), true);
            parent.AddHandler(UserControl.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonDown), true);
        }

        void SnapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (Selected_Element == null)
                return;

            try 
            {
                var point = e.GetPosition(Canvas);
                VisualStateManager.GoToState(Selected_Element, "NotDragging", true);
                Selected_Element.ReleaseMouseCapture();

                if (Action == EditAction.Moving)
                {
                    Selected_Element.Traslate.X = 0;
                    Selected_Element.Traslate.Y = 0;

                    var x = point.X - MouseToElementRelativePosition.X;
                    var y = point.Y - MouseToElementRelativePosition.Y;

                    Move_Item(Selected_Element, new Point { X = x, Y = y });
                }

                if (Action == EditAction.Resizing)
                {
                    Selected_Element.Scale.ScaleX = 1;
                    Selected_Element.Scale.ScaleY = 1;

                    Resize_Item(Selected_Element, e.GetPosition(Selected_Element));
                }
            }
            finally
            {
                Selected_Element = null;
                Action = EditAction.None;
            }
        }

        void SnapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var position = e.GetPosition(this);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(position, this);


            var itemContainer = elements.OfType<WidgetItemContainer>().FirstOrDefault();
            if (itemContainer == null)
                itemContainer = Widgets.Where(w => IsInside(w, e)).FirstOrDefault();
            if (itemContainer == null)
                return;

            Selected_Element = itemContainer;

            OriginalPosition = e.GetPosition(Canvas);
            MouseToElementRelativePosition = e.GetPosition(itemContainer);

            Selected_Element.CaptureMouse();

            Bring_To_Front(itemContainer);


            if (elements.OfType<Grid>().Any(i => i.Name == "resizeAncor"))
            {
                Action = EditAction.Resizing;
                VisualStateManager.GoToState(Selected_Element, "Dragging", true);
            }
            else
            {
                Action = EditAction.Moving;
                VisualStateManager.GoToState(Selected_Element, "Dragging", true);
            }
        }



        protected void Bring_To_Front(UIElement item)
        {
            var maxZ = Widgets.Max(w => Canvas.GetZIndex(w));
            Canvas.SetZIndex(item, maxZ + 1);
        }

        protected int Calculate_Discrete_X(double width)
        {
            return (int)(Math.Round(width / XUnit) * XUnit);
        }

        protected int Calculate_Discrete_Y(double height)
        {
            return (int)(Math.Round(height / YUnit) * YUnit);
        }

        protected bool IsInside(WidgetItemContainer item, MouseButtonEventArgs e)
        {
            var relative = e.GetPosition(item);
            return relative.X > 0 && relative.Y > 0 && relative.X < item.ActualWidth && relative.Y < item.ActualHeight;
        }


        protected void Animate_Resizing(Point position)
        {
            if (position.X < 0 || position.Y < 0)
                return;

            Selected_Element.Scale.ScaleX += -Calculate_Discrete_X(MouseToElementRelativePosition.X - position.X) / Selected_Element.ActualWidth;
            Selected_Element.Scale.ScaleY += -Calculate_Discrete_Y(MouseToElementRelativePosition.Y - position.Y) / Selected_Element.ActualHeight;
        }

        protected void Resize_Item(FrameworkElement item, Point position)
        {
            if (position.X < 0 || position.Y < 0)
                return;

            item.Width = Calculate_Discrete_X(position.X);
            item.Height = Calculate_Discrete_Y(position.Y);
            item.InvalidateMeasure();
        }

        protected void Animate_Moving(Point position)
        {
            Selected_Element.Traslate.X = Calculate_Discrete_X(position.X - OriginalPosition.X);
            Selected_Element.Traslate.Y = Calculate_Discrete_Y(position.Y - OriginalPosition.Y);
        }

        protected void Move_Item(UIElement item, Point position)
        {
            Canvas.SetTop(item, Calculate_Discrete_Y(position.Y));
            Canvas.SetLeft(item, Calculate_Discrete_X(position.X));
        }



        protected AutoEnlarging_BusyMap GetMap()
        {
            var x = (int)(ActualWidth / XUnit);
            var y =  (int)(ActualHeight / YUnit);

            return new AutoEnlarging_BusyMap(x, y);

            //foreach (var widget in Widgets)
            //    busy.Set_Busy(widget.DLeft(XUnit), widget.DTop(YUnit), widget.DWidth(XUnit), widget.DHeight(YUnit), widget);
        }
    }

   



 


}
