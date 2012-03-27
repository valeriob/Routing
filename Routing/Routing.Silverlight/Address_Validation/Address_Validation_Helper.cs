using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using Routing.Silverlight.Models;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Diagnostics;
using System.Collections.Generic;

namespace Routing.Silverlight.Address_Validation
{
    public class Address_Validation_Helper : IDisposable
    {
        Popup Popup;
        Panel Parent;

        public Address_Validation_Helper(Panel parent)
        {
            Popup = new Popup { Child = new GeoSearchPopup() };
            parent.Children.Add(Popup);
            Parent = parent;
        }

        public void Validate_Address(bool silent, string textAddress)
        {
            if (silent)
            {
                var request = new Automatically_Validate_Address { Search_Address = textAddress };
                MessageBus.Current.SendMessage(request);
            }
            else
            {
                Popup.IsOpen = true;
                Popup.Child.Focus();
 
                var request = new Manually_Validate_Address { Search_Address = textAddress };
                MessageBus.Current.SendMessage(request);
            }
        }

        public void Dispose()
        {
            Popup.IsOpen = false;
            Parent.Children.Remove(Popup);
            Popup = null;
            Parent = null;
        }
    }
    
}
