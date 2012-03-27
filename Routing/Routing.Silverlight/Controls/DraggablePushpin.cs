using System;
using Microsoft.Maps.MapControl;
using System.Windows.Input;

namespace Routing.Silverlight.Controls
{
    public class DraggablePushpin : Pushpin
    {
        public bool IsDragging { get; set; }
        EventHandler<MapMouseDragEventArgs> ParentMapMousePanHandler;
        MouseButtonEventHandler ParentMapMouseLeftButtonUpHandler;
        MouseEventHandler ParentMapMouseMoveHandler;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var parentLayer = Parent as MapLayer;
            if (parentLayer != null)
            {
                var parentMap = parentLayer.ParentMap;
                if (parentMap != null)
                {
                    if (this.ParentMapMousePanHandler == null)
                    {
                        ParentMapMousePanHandler = new EventHandler<MapMouseDragEventArgs>(ParentMap_MousePan);
                        parentMap.MousePan += ParentMapMousePanHandler;
                    }
                    if (ParentMapMouseLeftButtonUpHandler == null)
                    {
                        ParentMapMouseLeftButtonUpHandler = new MouseButtonEventHandler(ParentMap_MouseLeftButtonUp);
                        parentMap.MouseLeftButtonUp += ParentMapMouseLeftButtonUpHandler;
                    }
                    if (ParentMapMouseMoveHandler == null)
                    {
                        ParentMapMouseMoveHandler = new MouseEventHandler(ParentMap_MouseMove);
                        parentMap.MouseMove += ParentMapMouseMoveHandler;
                    }
                }
            }

            IsDragging = true;

            base.OnMouseLeftButtonDown(e);
        }

        #region "Mouse Event Handler Methods"

        void ParentMap_MousePan(object sender, MapMouseDragEventArgs e)
        {
            if (IsDragging)
                e.Handled = true;
        }

        void ParentMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
        }

        void ParentMap_MouseMove(object sender, MouseEventArgs e)
        {
            var map = sender as Map;
            if (IsDragging)
            {
                var mouseMapPosition = e.GetPosition(map);
                var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition);
                Location = mouseGeocode;
            }
        }

        #endregion
    }

}