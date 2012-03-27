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


namespace Silverlight.Common.Controls.SnapGrid
{
    public class SnapGridItemsTemplate : ItemsControl
    {
        
    }

    public class SnapGrid : Grid
    {
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }

        public double XUnit { get; set; }
        public double YUnit { get; set; }

        public FrameworkElement DraggingElement { get; set; }
        public Popup MovingOverlay { get; set; }

        public SnapGrid()
        {
            LayoutUpdated += new EventHandler(SnapGrid_LayoutUpdated);
            SizeChanged += new SizeChangedEventHandler(SnapGrid_SizeChanged);
            MouseLeftButtonDown += new MouseButtonEventHandler(SnapGrid_MouseLeftButtonDown);
            MouseLeftButtonUp += new MouseButtonEventHandler(SnapGrid_MouseLeftButtonUp);
            LostMouseCapture += new MouseEventHandler(SnapGrid_LostMouseCapture);
            MouseMove += new MouseEventHandler(SnapGrid_MouseMove);
            Loaded += new RoutedEventHandler(SnapGrid_Loaded);

            ColumnCount = 20;
            RowCount = 20;
            ShowGridLines = true;

            Init();
        }

        void SnapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (DraggingElement != null)
            {
                var position = e.GetSafePosition(this);
                MoveOverlay(position);
            }
        }

        void SnapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UIElement parent = VisualTreeHelper.GetParent(this) as UIElement;
            parent = VisualTreeHelper.GetParent(parent) as UIElement;
            parent = VisualTreeHelper.GetParent(parent) as UIElement;

            parent.MouseLeftButtonUp += (from, ev) =>
            {
                SnapGrid_MouseLeftButtonUp(sender, ev);
            };

        }

        void SnapGrid_LostMouseCapture(object sender, MouseEventArgs e)
        {
            
        }



        void SnapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingElement == null)
                return;

            try 
            {
                var point = e.GetPosition(this);

                Grid.SetRow(DraggingElement, GetRow(point));
                Grid.SetColumn(DraggingElement, GetColumn(point));
            }
            finally
            {
                DraggingElement = null;
                MovingOverlay.IsOpen = false;
            }
        }

        void SnapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(position, this);

            var item = elements.OfType<SnapGridItemContainer>().FirstOrDefault();

            DraggingElement = VisualTreeHelper.GetParent(item) as FrameworkElement;
            MovingOverlay.Child = new Image { Source = Render_Control(DraggingElement) };
            MovingOverlay.IsOpen = true;
            
        }

        protected void Init()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                ColumnDefinitions.Add( new ColumnDefinition{ });
            }

            for (int i = 0; i < RowCount; i++)
            {
                RowDefinitions.Add( new RowDefinition{ });
            }

            MovingOverlay = new Popup();

        }

        void SnapGrid_LayoutUpdated(object sender, EventArgs e)
        {
          
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        void SnapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            YUnit = e.NewSize.Height / RowCount;
            XUnit = e.NewSize.Width / ColumnCount;
        }



        protected int GetRow(Point point)
        {
            return (int)Math.Floor( point.Y / YUnit );
        }

        protected int GetColumn(Point point)
        {
            return (int)Math.Floor(point.X / XUnit);
        }

        public int GetRowSpan(SnapGridItemContainer item)
        {
            return 1;
        }

        public int GetColumnSpan(SnapGridItemContainer item)
        {
            return 1;
        }

        protected ImageSource Render_Control(FrameworkElement control)
        {
            var bitmap = new WriteableBitmap((int)control.ActualWidth, (int)control.ActualHeight);
            //var bitmap = new WriteableBitmap((int)control.RenderSize.Width, (int)control.RenderSize.Height);
            bitmap.Render(control,null);
            bitmap.Invalidate();

            return bitmap;
        }

        
        protected void MoveOverlay(Point position)
        {
            Grid.SetColumn(MovingOverlay, GetColumn(position));
            Grid.SetRow(MovingOverlay, GetRow(position));
        }

    }




}
