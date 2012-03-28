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
using maps = Silverlight.Common.Maps;
using System.Threading.Tasks;

namespace Routing.Silverlight.Address_Validation
{
    public class Address_Validation_Helper : IDisposable
    {
        Popup Popup;
        Panel Parent;
        GeoSearchPopup SearchControl;
        Location_Reference Location;

        public Address_Validation_Helper(Location_Reference location, Panel parent)
        {
            Location = location;
            Parent = parent;

            SearchControl = new GeoSearchPopup();
            Popup = new Popup { Child = SearchControl };
            parent.Children.Add(Popup);
        }


        public Task<Address_Validated> Validate_Address(bool silent, string text)
        {
            return Task.Factory.StartNew(() =>
                {
                    if (silent)
                        return Automatically_Validate_Address(text).ContinueWith(c =>
                        {
                            if (c.IsCanceled)
                                return Manually_Validate_Address(text).Result;
                            else
                                return c.Result;
                        }).Result;
                    else
                        return Manually_Validate_Address(text).ContinueWith(v => { return v.Result; }).Result;
                }).ContinueWith(t => 
                {
                    Dispose();
                    return t.Result;
                });
        }


        //public Task<Address_Validated> Validate_Address(bool silent, string textAddress)
        //{
        //    if (silent)
        //        return Manually_Validate_Address(textAddress);
        //    else
        //        return Automatically_Validate_Address(textAddress);
        //}

        public Task<Address_Validated> Manually_Validate_Address(string textAddress)
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Popup.IsOpen = true;
                Popup.Child.Focus();
            });

            //var tokenSource = new System.Threading.CancellationTokenSource();
            
            return SearchControl.ViewModel.Search(textAddress)
                .ContinueWith(f => 
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Popup.IsOpen = false;
                    });

                    if (f.IsCanceled)
                        return null;

                    return f.Result;
                });
        }


        public Task<Address_Validated> Automatically_Validate_Address(string textAddress)
        {
            var tcs = new TaskCompletionSource<Address_Validated>();

            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.GeocodeCompleted += (sender, e) =>
            {
                //if (e.Error != null)
                //    tcs.SetCanceled();

                if (e.Error == null && e.Result.Results.GroupBy(f=> f.Address.FormattedAddress).Count() == 1)
                {
                    var validated = new Address_Validated(e.Result.Results.First(), null);
                    //MessageBus.Current.SendMessage(new Address_Validated(e.Result.Results.First(), search_Address));
                    tcs.TrySetResult(validated);
                }
                else
                {
                    tcs.SetCanceled();
                    //MessageBus.Current.SendMessage(search_Address.To_Manual());
                    //var task = Manually_Validate_Address(textAddress);
                    //task.Wait();
                }
            };

            geocoding.GeocodeAsync(new maps.GeocodeService.GeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Query = textAddress
            });

            return tcs.Task;
        }

        public void Dispose()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Popup.IsOpen = false;
                Parent.Children.Remove(Popup);
                Popup = null;
                Parent = null;
            });
        }
    }
    
}
