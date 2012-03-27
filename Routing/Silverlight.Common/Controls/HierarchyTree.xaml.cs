using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Common.Controls
{

    public partial class HierarchyTree : UserControl
    {
		public enum Orientation { Branch, Root };

        private SolidColorBrush lineBrush;

        public HierarchyTree()
        {
            InitializeComponent();
            lineBrush = new SolidColorBrush(this.LineColor);
        }

        #region Properties

        /// <summary>
        /// List of HierarchyNodes to be displayed by the NodeControl
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<HierarchyNode> Nodes
        {
            get
            {
                if (base.GetValue(NodesProperty) == null)
                    base.SetValue(NodesProperty, new List<HierarchyNode>());

                return (List<HierarchyNode>)base.GetValue(NodesProperty);
            }

            set
            {
                base.SetValue(NodesProperty, value);
            }
        }
        public static DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(List<HierarchyNode>), typeof(HierarchyTree), null);


        /// <summary>
        /// The vertical spacings between each level
        /// </summary>
        public double LevelSpacing
        {
            get
            {
                return (double)base.GetValue(LevelSpacingProperty);
            }
            set
            {
                base.SetValue(LevelSpacingProperty, value);
            }
        }
        public static DependencyProperty LevelSpacingProperty = DependencyProperty.Register("LevelSpacing", typeof(double), typeof(HierarchyTree), new PropertyMetadata(20d));

        /// <summary>
        /// The horizontal spacing between each sibling
        /// </summary>
        public double ChildSpacing
        {
            get
            {
                return (double)base.GetValue(ChildSpacingProperty);
            }
            set
            {
                base.SetValue(ChildSpacingProperty, value);
            }
        }
        public static DependencyProperty ChildSpacingProperty = DependencyProperty.Register("ChildSpacing", typeof(double), typeof(HierarchyTree), new PropertyMetadata(20d));

        /// <summary>
        /// The color of the join lines between parents and children
        /// </summary>
        public Color LineColor
        {
            get
            {
                return (Color)base.GetValue(LineColorProperty);
            }
            set
            {
                base.SetValue(LineColorProperty, value);
            }
        }
        public static DependencyProperty LineColorProperty = DependencyProperty.Register("LineColor", typeof(Color), typeof(HierarchyTree), new PropertyMetadata((Color)Colors.Black, new PropertyChangedCallback(LineColorChanged)));

        /// <summary>
        /// The color of the join lines between parents and children
        /// </summary>
        public Orientation Direction
        {
            get
            {
                return (Orientation)base.GetValue(DirectionProperty);
            }
            set
            {
                base.SetValue(DirectionProperty, value);
            }
        }
        public static DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Orientation), typeof(HierarchyTree), new PropertyMetadata((Orientation)Orientation.Root, new PropertyChangedCallback(DirectionChanged)));



        #endregion Properties

        #region Events

		private static void LineColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HierarchyTree tree = d as HierarchyTree;
			Color NewColor = (Color)e.NewValue;
			tree.lineBrush.Color = NewColor;
		}

		private static void DirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HierarchyTree tree = d as HierarchyTree;
			Orientation orientation = (Orientation)e.NewValue;
			tree.Direction = orientation;
		}

		#endregion Events



        public void Display()
        {
            double top = base.Padding.Top;
            double left = base.Padding.Left;
            List<HierarchyNode> hierarchyItems = this.Nodes;

            LayoutRoot.Children.Clear();

            
            for (int i = 0; i < hierarchyItems.Count; i++)
            {
                left = AddControl(top, left, hierarchyItems[i], false);
            }

			if (Direction == Orientation.Branch) DisplayAsBranch();
			
        }

		private void DisplayAsBranch()
		{
			foreach (FrameworkElement element in LayoutRoot.Children)
			{
				switch (element.GetType().ToString())
				{
					case "System.Windows.Shapes.Line":
						Line line = element as Line;
						HierarchyNode node = line.Tag as HierarchyNode;
						line.Y1 = LayoutRoot.Height - line.Y1 + node.Control.Height;
						line.Y2 = LayoutRoot.Height - line.Y2 + node.Control.Height;
						break;

					default:
						element.Margin = new Thickness(element.Margin.Left, LayoutRoot.Height - element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
						break;
				}
			}

			if (Direction != Orientation.Branch) Direction = Orientation.Branch;
		}

        /// <summary>
        /// Recursive method which adds the HierarchyNode controls and lines used to create the tree
        /// </summary>
        /// <param name="top">Top margin of the NodeControl</param>
        /// <param name="left">Left margin of the NodeControl</param>
        /// <param name="node">Node used to reference the NodeControl and children to display</param>
        /// <param name="nodeHasParents">used to notify method that the node has no parents</param>
        /// <returns></returns>
        private double AddControl(double top, double left, HierarchyNode node, bool nodeHasParents)
        {
            double controlLeft = left;

            FrameworkElement nodeControl = node.Control;

            //Determine the placement and display all children first
            if (node.Children.Count > 0)
            {
                double lineTop = top + nodeControl.Height + (LevelSpacing / 2);
                double childTop = top + nodeControl.Height + LevelSpacing;
                double childLeft = left;

                //Add child controls to canvas first
                foreach(HierarchyNode child in node.Children)
                    childLeft = AddControl(childTop, childLeft, child, true);

                //Create horizontal join line to visually join children to parent
                double firstNodePoint = node.Children.First().Control.Margin.Left + (node.Children.First().Control.Width / 2);
                double lastNodePoint = node.Children.Last().Control.Margin.Left + (node.Children.Last().Control.Width / 2);
                AddLine(firstNodePoint, lastNodePoint, lineTop, lineTop, node);

                //Find center of join line to create parent line
                double centerPoint = firstNodePoint + ((lastNodePoint - firstNodePoint) / 2);
                controlLeft = centerPoint - (nodeControl.Width / 2);

                left = childLeft + ChildSpacing;
            }
            else
            {
                //Node has no children so no need to recalculate left position of NodeControl
                left += nodeControl.Width + ChildSpacing;
            }

            //Visually join node to parent
            if (nodeHasParents) 
                AddChildToParentLine(controlLeft, top, nodeControl, node);

            //Visually join node to children
            if (node.Children.Count > 0)
                AddParentToChildLine(controlLeft, top, nodeControl, node);

            //Finally add NodeControl to canvas
            nodeControl.Margin = new Thickness(controlLeft, top, 0, 0);
            LayoutRoot.Children.Add(node.Control);

			if (double.IsNaN(LayoutRoot.Height) || LayoutRoot.Height < top + nodeControl.Height + 10)
				LayoutRoot.Height = top + nodeControl.Height + ChildSpacing;

            return left;
        }

        /// <summary>
        /// Refactored method used to display lines in the tree
        /// </summary>
		private void AddLine(double x1, double x2, double y1, double y2, HierarchyNode node)
        {
            Line newLine = new Line();
			newLine.X1 = x1;
			newLine.X2 = x2;
			newLine.Y1 = y1;
			newLine.Y2 = y2;
			newLine.Stroke = this.lineBrush;
			newLine.Tag = node;
			LayoutRoot.Children.Add(newLine);

        }

        /// <summary>
        /// Creates a line which joins the child NodeControl to the join line
        /// </summary>
        private void AddChildToParentLine(double left, double top, FrameworkElement control, HierarchyNode node)
        {
            double center = left + (control.Width / 2);
            AddLine(center, center, top - (LevelSpacing / 2), top, node);
        }

        /// <summary>
        /// Creates a line which joins the parent NodeControl to the join line
        /// </summary>
		private void AddParentToChildLine(double left, double top, FrameworkElement control, HierarchyNode node)
        {
            double center = left + (control.Width / 2);
            double lineTop = top + control.Height;
            AddLine(center, center, lineTop, lineTop + (LevelSpacing / 2), node);
        }

    }

    public class HierarchyNode
    {

        private FrameworkElement control;
        private List<HierarchyNode> children = new List<HierarchyNode>();


        public HierarchyNode(FrameworkElement control, double width, double height)
        {
            this.control = control;
            control.Width = width;
            control.Height = height;
        }

        public FrameworkElement Control
        {
            get { return this.control; }
            set { control = value; }
        }

        public List<HierarchyNode> Children
        {
            get { return this.children; }
            set { children = value; }
        }

    }
}
