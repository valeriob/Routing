//using System;
//using System.Linq;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Silverlight.Common.Helpers;
//using System.Windows.Media.Imaging;
//using System.Windows.Controls.Primitives;
//using System.Collections;
//using System.Collections.Generic;


//namespace Silverlight.Common.Controls.WidgetContainer
//{

//    public class WidgetContainer : UserControl
//    {
//        public int ColumnCount { get; set; }
//        public int RowCount { get; set; }

//        public double XUnit { get; set; }
//        public double YUnit { get; set; }

//        public DataTemplate ItemTemplate { get; set; }
//        public Grid Grid { get; set; }
//        public Canvas Canvas { get; set; }

//        public WidgetItemContainer DraggingElement { get; set; }
//        public Point OriginalPosition { get; set; }
//        public Point MouseToElementRelativePosition { get; set; }
//        protected List<WidgetItemContainer> Widgets { get; set; }

//        public IEnumerable ItemsSource
//        {
//            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
//            set { SetValue(ItemsSourceProperty, value); }
//        }
//        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(WidgetContainer), new PropertyMetadata(null,
//             new PropertyChangedCallback(OnItemsSourceChanged)));

//        static Size maxSize = new Size { Width = 1000, Height = 1000 };
//        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var items = e.NewValue as IEnumerable;
//            var widgets = d as WidgetContainer;

//            int i = 0;
//            foreach (var item in items)
//            {
//                var container = new WidgetItemContainer();
//                container.AddHandler(UserControl.MouseLeftButtonUpEvent, new MouseButtonEventHandler(widgets.SnapGrid_MouseLeftButtonUp), true);

//                container.Content = item;
//                container.ContentTemplate = widgets.ItemTemplate;
//                container.UpdateLayout();

//                container.SizeChanged += (from, ev) => 
//                {
//                    container.Measure(maxSize);
//                    Grid.SetColumnSpan(container, widgets.GetColumnSpan(container) +1);
//                    Grid.SetRowSpan(container, widgets.GetRowSpan(container) +1);
//                };

//                Grid.SetColumn(container, i);
//              //  widgets.Grid.Children.Add(container);
//                widgets.Canvas.Children.Add(container);

//                widgets.Widgets.Add(container);
//                i++;
//            }
//        }
        


//        public WidgetContainer()
//        {
//            Init();
//        }

//        void SnapGrid_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (DraggingElement != null)
//            {
//                var position = e.GetPosition(this);

//                Animate_Dragging(position);
//            }
//        }

//        void SnapGrid_Loaded(object sender, RoutedEventArgs e)
//        {
//            UIElement parent = VisualTreeHelper.GetParent(this) as UIElement;
//            parent = VisualTreeHelper.GetParent(parent) as UIElement;
//            //parent = VisualTreeHelper.GetParent(parent) as UIElement;

//            parent.MouseLeftButtonUp += (from, ev) =>
//            {
//                SnapGrid_MouseLeftButtonUp(sender, ev);
//            };

//        }

//        void SnapGrid_LostMouseCapture(object sender, MouseEventArgs e)
//        {
            
//        }



//        void SnapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            e.Handled = true;

//            if (DraggingElement == null)
//                return;

//            try 
//            {
//                VisualStateManager.GoToState(DraggingElement, "NotDragging", true);
//                DraggingElement.ReleaseMouseCapture();
//                DraggingElement.Traslate.X = 0;
//                DraggingElement.Traslate.Y = 0;

//                var point = e.GetPosition(this);

//                var x = point.X - MouseToElementRelativePosition.X;
//                var y = point.Y - MouseToElementRelativePosition.Y;

//                Move_Item(DraggingElement, new Point { X= x, Y= y });
               
//            }
//            finally
//            {
//                DraggingElement = null;
//            }
//        }

//        void SnapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//        {
//            e.Handled = true;
//            Point position = e.GetPosition(this);
//            var elements = VisualTreeHelper.FindElementsInHostCoordinates(position, this);

//            var item = elements.OfType<WidgetItemContainer>().FirstOrDefault();
//            if (item == null)
//                return;

//            DraggingElement = item;

//            OriginalPosition = e.GetPosition(this);
//            MouseToElementRelativePosition = e.GetPosition(e.OriginalSource as UIElement);
//            DraggingElement.CaptureMouse();

//            Bring_To_Front(item);

//            VisualStateManager.GoToState(DraggingElement, "Dragging", true);
//        }

//        protected void Init()
//        {
//            LayoutUpdated += new EventHandler(SnapGrid_LayoutUpdated);
//            SizeChanged += new SizeChangedEventHandler(SnapGrid_SizeChanged);
//            LostMouseCapture += new MouseEventHandler(SnapGrid_LostMouseCapture);
//            MouseMove += new MouseEventHandler(SnapGrid_MouseMove);
//            Loaded += new RoutedEventHandler(SnapGrid_Loaded);

//            AddHandler(UserControl.MouseLeftButtonUpEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonUp), true);
//            AddHandler(UserControl.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonDown), true);

//            Grid = new Grid();
//            Canvas = new Canvas();
//            Widgets = new List<WidgetItemContainer>();

//            ColumnCount = 50;
//            RowCount = 50;
//            Grid.ShowGridLines = true;

//            for (int i = 0; i < ColumnCount; i++)
//            {
//                Grid.ColumnDefinitions.Add(new ColumnDefinition { });
//            }

//            for (int i = 0; i < RowCount; i++)
//            {
//                Grid.RowDefinitions.Add(new RowDefinition { });
//            }
            
//            //Content = Grid;
//            Content = Canvas;
//        }

//        void SnapGrid_LayoutUpdated(object sender, EventArgs e)
//        {
          
//        }
        
//        public override void OnApplyTemplate()
//        {
//            base.OnApplyTemplate();
//        }

//        void SnapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
//        {
//            YUnit = e.NewSize.Height / RowCount;
//            XUnit = e.NewSize.Width / ColumnCount;
//        }




//        protected int GetRow(double height)
//        {
//            if (height < 0)
//                return 0;
//            return (int)Math.Floor(height / YUnit);
//        }
//        protected int GetColumn(double width)
//        {
//            if (width < 0)
//                return 0;
//            return(int)Math.Floor(width / XUnit);
//        }
//        public int GetRowSpan(WidgetItemContainer item)
//        {
//            return Math.Max(GetRow(item.DesiredSize.Height), 1);
//        }
//        public int GetColumnSpan(WidgetItemContainer item)
//        {
//            return Math.Max(GetColumn(item.DesiredSize.Width), 1);
//        }

//        public void Bring_To_Front(UIElement item)
//        {
//            var maxZ = Widgets.Max(w => Canvas.GetZIndex(w));
//            Canvas.SetZIndex(item, maxZ + 1);
//        }

//        public int Calculate_Discrete_X(double width)
//        {
//            return (int)(Math.Floor(width / XUnit) * XUnit);
//        }

//        public int Calculate_Discrete_Y(double height)
//        {
//            return (int)(Math.Floor(height / YUnit) * YUnit);
//        }


//        protected void Animate_Dragging(Point position)
//        {
//            DraggingElement.Traslate.X = Calculate_Discrete_X(position.X - OriginalPosition.X);
//            DraggingElement.Traslate.Y = Calculate_Discrete_Y(position.Y - OriginalPosition.Y);
//        }

//        protected void Move_Item(UIElement item, Point position)
//        {
//            Canvas.SetTop(item, Calculate_Discrete_Y(position.Y));
//            Canvas.SetLeft(item, Calculate_Discrete_X(position.X));

//            //Grid.SetRow(DraggingElement, GetRow(position.Y));
//            //Grid.SetColumn(DraggingElement, GetColumn(position.X));
//        }
       

//    }




//}
