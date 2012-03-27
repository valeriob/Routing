using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using maps = Silverlight.Common.Maps;
using System.Threading;
using System.Diagnostics;

namespace Routing.Silverlight.Models
{
    public abstract class RouteCalculator
    {
        public MyLocation Starting { get; protected set; }
        public MyLocation Ending { get; protected set; }

        protected List<MyLocation> _Tours;
        public IEnumerable<MyLocation> Tours { get { return _Tours; } }

        public IEnumerable<MyLocation> Result { get; protected set; }

        public RouteCalculator()
        {
            _Tours = new List<MyLocation>();
        }

        public void Set_Starting_Point(double latitude, double longitude)
        {
            Starting = new MyLocation(latitude, longitude);
        }
        public void Set_Ending_Point(double latitude, double longitude)
        {
            Ending = new MyLocation(latitude, longitude);
        }

        public void Add_Tour(double latitude, double longitude)
        {
            _Tours.Add(new MyLocation(latitude, longitude));
        }

        public Task Calculate_Routes()
        {
            var result =Task.Factory.StartNew(() => 
            {
                var total = _Tours.ToList();
                total.Insert(0, Starting);
                total.Add(Ending);

                var tasks = new List<Task>();
                var legs = new List<Leg>();
            
            
                for (int i = 0; i < total.Count -1; i++)
                {
                    var leg = new Leg(total[i], total[i + 1]);
                    legs.Add(leg);
                    
                    var task = Calculate_Route(leg);
 
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());

                Result = legs.SelectMany(l => l.SubTour);
            });

            return result;
        }

        protected abstract Task Calculate_Route(Leg leg);
    
    }

    public class MyLocation
    {
        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }

        public MyLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class Leg
    {
        public MyLocation From { get; protected set; }
        public MyLocation To { get; protected set; }

        public Leg(MyLocation from, MyLocation to)
        {
            From = from;
            To = to;
        }

        public IEnumerable<MyLocation> SubTour { get; protected set; }
        public void Resolve(IEnumerable<MyLocation> subTour)
        {
            SubTour = subTour;
        }
    }


    public class BingRouteCalculator : RouteCalculator
    {
        public BingRouteCalculator() { }


        protected override Task Calculate_Route(Leg leg)
        {
            var service = maps.ServiceHelper.GetRouteService();// as maps.RouteService.IRouteService;

            var tcs = new TaskCompletionSource<Leg>(leg);

            service.CalculateRouteCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    tcs.TrySetException(e.Error);
                else
                {
                    var currentLeg = e.UserState as Leg;
                    currentLeg.Resolve(e.Result.Result.RoutePath.Points.Select(p => new MyLocation(p.Latitude, p.Longitude)));
                    tcs.TrySetResult(currentLeg);
                }
            };

            var request = new maps.RouteService.RouteRequest
            {
                Credentials = new maps.RouteService.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Waypoints = new maps.RouteService.Waypoint[] { ToWp(leg.From), ToWp(leg.To) }.To_ObservableCollection()

            };
            request.Options = new maps.RouteService.RouteOptions();
            request.Options.RoutePathType = maps.RouteService.RoutePathType.Points;

            service.CalculateRouteAsync(request, leg);

            //var result = service.BeginCalculateRoute(request, null, null);
            //return Task.Factory.FromAsync<maps.RouteService.RouteResponse>(result, service.EndCalculateRoute, TaskCreationOptions.None);

            return tcs.Task;
        }

        protected maps.RouteService.Waypoint ToWp(MyLocation location)
        {
            return new maps.RouteService.Waypoint { Location = new maps.RouteService.Location { Latitude = location.Latitude, Longitude = location.Longitude } };
        }
        

    }

    public static class MyTaskFactory
    {
        public static Task<TResult> FromAsync<TResult>(
            IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
        {
            var completionSource = new TaskCompletionSource<TResult>();
            if (asyncResult.IsCompleted)
            {
                completionSource.TrySetResult(asyncResult, endMethod);
            }
            else
            {
                System.Threading.ThreadPool.RegisterWaitForSingleObject(
                    asyncResult.AsyncWaitHandle,
                    (state, timeOut) =>
                    {
                        completionSource.TrySetResult(asyncResult, endMethod);
                    }, null, -1, true);
            }

            return completionSource.Task;
        }

        static void TrySetResult<TResult>(
            this TaskCompletionSource<TResult> completionSource,
            IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
        {
            try
            {
                var result = endMethod(asyncResult);
                completionSource.TrySetResult(result);
            }
            catch (OperationCanceledException)
            {
                completionSource.TrySetCanceled();
            }
            catch (Exception genericException)
            {
                completionSource.TrySetException(genericException);
            }
        }
    }
}
