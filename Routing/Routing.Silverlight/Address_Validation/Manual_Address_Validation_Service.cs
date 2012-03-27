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
    //public class Manual_Address_Validation_Service 
    //{
    //    private static Manual_Address_Validation_Service _Instance;
    //    public static Manual_Address_Validation_Service Instance
    //    {
    //        get { return _Instance ?? (_Instance = new Manual_Address_Validation_Service()); }
    //    }

    //    static Manual_Address_Validation_Service()
    //    {
    //        MessageBus.Current.Listen<Manually_Validate_Address>().Subscribe(Manually_Validate_Address);

    //        //Search_LocationViewModel = new Search_LocationViewModel();
    //        //Search_LocationViewModel.ObservableForProperty(v => v.PositionConfirmed).Value().Subscribe(PositionConfirmed);
    //    }

    //    public static Manually_Validate_Address Serving { get; set; }
    //    //protected static Search_LocationViewModel Search_LocationViewModel { get; set; }

    //    protected static void Manually_Validate_Address(Manually_Validate_Address toBeValidated)
    //    {
    //        if (!toBeValidated.IsValid())
    //            return;
    //        Debug.WriteLine("Manual validating " + toBeValidated);
    //        //Search_LocationViewModel.Initialize( toBeValidated.Search_Address);
    //        Instance.Popup.IsOpen = true;
    //        Serving = toBeValidated;
    //    }

    //    protected static void PositionConfirmed(bool confirmed)
    //    {
    //        if (confirmed)
    //            MessageBus.Current.SendMessage(new Address_Validated(Search_LocationViewModel.SelectedResult, Serving));
    //        else
    //            MessageBus.Current.SendMessage(new Validation_Canceled(Serving));

    //        Instance.Popup.IsOpen = false;

    //        Serving = null;
    //    }

    //    public Popup Popup { get; set; }


    //    public void Init_Popup(Grid parent)
    //    {
    //        Popup = new Popup { Child = new GeoSearchPopup() };
    //        parent.Children.Add(Popup);
    //    }

        
        

    //}
    
    //public class Address_Validation_Helper
    //{
    //    public static Popup Init_Popup(Grid parent)
    //    {
    //        var Popup = new Popup { Child = new GeoSearchPopup() };
    //        parent.Children.Add(Popup);
    //        return Popup;
    //    }

    //    public static void Validate_Address(bool silent, string textAddress)
    //    {
    //        if (silent)
    //        {
    //            var request = new Automatically_Validate_Address { Search_Address = textAddress };
    //            MessageBus.Current.SendMessage(request);
    //        }
    //        else
    //        {
    //            var request = new Manually_Validate_Address { Search_Address = textAddress };
    //            MessageBus.Current.SendMessage(request);
    //        }
    //    }
    //}

   
    
}
