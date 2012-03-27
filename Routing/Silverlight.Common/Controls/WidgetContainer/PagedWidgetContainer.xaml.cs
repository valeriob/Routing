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
using System.Collections.ObjectModel;


namespace Silverlight.Common.Controls.WidgetContainer
{
    public partial class PagedWidgetContainer : UserControl
    {
        static Size maxSize = new Size { Width = 1000, Height = 1000 };

        public double XUnit { get; set; }
        public double YUnit { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        public ObservableCollection<PageViewModel> Pages { get; set; }
        protected DelayedAction Rearrange { get; set; }

        protected EditAction Action { get; set; }
        protected WidgetItemContainer Selected_Element { get; set; }
        protected Point OriginalPosition { get; set; }
        protected Point MouseToElementRelativePosition { get; set; }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PagedWidgetContainer), new PropertyMetadata(null,
             new PropertyChangedCallback(OnItemsSourceChanged)));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var items = e.NewValue as IEnumerable;
            var pagedContainer = d as PagedWidgetContainer;

            pagedContainer.MainGrid.Children.Clear();
            pagedContainer.Pages.Clear();

            var container = new WidgetContainer();
            foreach (var item in items)
            {
                var itemContainer = new WidgetItemContainer();
                itemContainer.DataContext = item;
                itemContainer.ContentTemplate = pagedContainer.ItemTemplate;
                itemContainer.Content = item;

                container.Add_WidgetItemContainer(itemContainer);
            }

            pagedContainer.Pages.Add(new PageViewModel( container));
            pagedContainer.MainGrid.Children.Add(container);
        }


        public PagedWidgetContainer()
        {
            Pages = new ObservableCollection<PageViewModel>();
            Rearrange = new DelayedAction();

            InitializeComponent();

            Init();
        }


        protected void Init()
        {
            LayoutRoot.DataContext = this;
            Loaded += (sender, e) => OnLoaded();
            SizeChanged += (sender, e) => Rearrange.Postpone();
            MouseMove += (sender, e) => OnMouseMove(sender,e);

            XUnit = 50;
            YUnit = 50;

            Rearrange.Action = () =>
            {
                var map = GetMap();

                foreach (var container in Pages.Select(p => p.Container))
                    map.Add_New_Page(container);

                var combos = Pages.Select(p => p.Container).SelectMany(c => c.Widgets.Select(w => new { c, w })).ToList();
                foreach (var wc in combos)
                {
                    var container = wc.c;
                    var widget = wc.w;

                    var spot = map.FindArea((int)widget.DMaxWidth(XUnit), (int)widget.DMaxHeight(YUnit));
                    if (spot == null)
                    {
                        var toBePlaced = new WidgetContainer();
                        Pages.Add(new PageViewModel(toBePlaced));
                        MainGrid.Children.Add(toBePlaced);
                        map.Add_New_Page(toBePlaced);
                        spot = map.FindArea((int)widget.DMaxWidth(XUnit), (int)widget.DMaxHeight(YUnit));
                    }

                    container.Remove_WidgetItemContainer(widget);
                    ((WidgetContainer)spot.Page).Add_WidgetItemContainer(widget);


                    Canvas.SetTop(widget, spot.Y * YUnit);
                    Canvas.SetLeft(widget, spot.X * XUnit);

                    map.Set_MaxBusy(widget, XUnit, YUnit, spot.Page);
                }

                foreach (var page in Pages.Where(p=> !p.Container.Widgets.Any()).ToList())
                    Pages.Remove(page);
                Pages.First().Container.Visibility = Visibility.Visible;
            };

        }


        void OnLoaded()
        {
            var parent = VisualTreeHelper.GetParent(this) as UIElement;
            parent = VisualTreeHelper.GetParent(parent) as UIElement;
            //parent = VisualTreeHelper.GetParent(parent) as UIElement;

            parent.AddHandler(UserControl.MouseLeftButtonUpEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonUp), true);
            parent.AddHandler(UserControl.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SnapGrid_MouseLeftButtonDown), true);
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Selected_Element == null)
                return;

            if (Action == EditAction.Moving)
                Animate_Moving(e.GetPosition(this));
            if (Action == EditAction.Resizing)
                Animate_Resizing(e.GetPosition(Selected_Element));
        }
        void SnapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Handle_Mouse_Up(e);
            // Edit.Postpone(e);
        }
        void SnapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Handle_Mouse_Down(e);
        }
        protected void Handle_Mouse_Down(MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;

            e.Handled = true;
            var position = e.GetPosition(this);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(position, this);


            var itemContainer = elements.OfType<WidgetItemContainer>().FirstOrDefault();
            if (itemContainer == null)
                itemContainer = GetWidgets().Where(w => IsInside(w, e)).FirstOrDefault();
            if (itemContainer == null)
                return;

            Selected_Element = itemContainer;

            OriginalPosition = e.GetPosition(this);
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
        protected void Handle_Mouse_Up(MouseButtonEventArgs e)
        {
            if (Selected_Element == null)
                return;

            try
            {
                var point = e.GetPosition(this);
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
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            var widgets = Pages.SelectMany(c => c.Container.Widgets).ToList();
            foreach (var widget in widgets)
            {
                widget.Height = Calculate_Discrete_X(widget.DesiredSize.Height);
                widget.Width = Calculate_Discrete_X(widget.DesiredSize.Width);
            }
            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var container in Pages.SelectMany(c => c.Container.Widgets))
                container.Measure(maxSize);
            
            return base.MeasureOverride(availableSize);
        }

        protected void Bring_To_Front(UIElement item)
        {
            var maxZ = GetWidgets().Max(w => Canvas.GetZIndex(w));
            Canvas.SetZIndex(item, maxZ + 1);
        }
        protected IEnumerable<WidgetItemContainer> GetWidgets()
        {
            return Pages.SelectMany(p => p.Container.Widgets);
        }
        protected PageBusyMap GetMap()
        {
            var x =  (int)(MainGrid.ActualWidth / XUnit);
            var y = (int)(MainGrid.ActualHeight / YUnit);

            return new PageBusyMap(x, y);
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


        private void Show_Page_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var container = control.DataContext as PageViewModel;

            foreach (var c in Pages)
                c.Container.Visibility = System.Windows.Visibility.Collapsed;

            container.Container.Visibility = System.Windows.Visibility.Visible;
        }

    }

    //public class PagedWidgetdBoard_ViewModel
    //{
    //    private ObservableCollection<WidgetBoard_ViewModel> _Pages;
    //    public ObservableCollection<WidgetBoard_ViewModel> Pages
    //    {
    //        get { return _Pages; }
    //        set { _Pages = value; }
    //    }
        
    //}

    //public class WidgetBoard_ViewModel
    //{
    //    public ObservableCollection<object> Items { get; set; }
    //}


    public class PageViewModel
    {
        public PageViewModel(WidgetContainer container)
        {
            Container = container;
        }
        public WidgetContainer Container { get; set; }
    }

}
