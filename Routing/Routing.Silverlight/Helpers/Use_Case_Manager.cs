using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using Routing.Silverlight.References;
using Routing.Silverlight.ServiceReferences;
using System.Windows.Controls;
using System.Collections.Generic;
using Routing.Silverlight.Models.References;

namespace Routing.Silverlight
{
    public abstract class Use_Case_Manager<TEntity> : IDisposable
    {
        //public static void Init() { }

        //protected Use_Case_Manager() { }

        //static Use_Case_Manager()
        //{
        //    MessageBus.Current.Listen<Begin_Search_Destination>()
        //        .Subscribe(Begin_Search_Destination);
        //}

        //protected static void Begin_Search_Destination(Begin_Search_Destination source)
        //{
        //    var cw = new CwDestinations();
        //    cw.Show();
        //}

        //public static void Begin_Search_Destination(Action<DestinationDto> onResult)
        //{
        //    var cw = new CwDestinations();
        //    cw.Show();
        //    MessageBus.Current.SendMessage(new Begin_Search_Destination());
        //    var subscription = MessageBus.Current.Listen<End_Search_Destination>().Subscribe(s => 
        //    {
        //        onResult(s.Chosen);
                
        //        //subscription.Dispose();
        //    });
        //}


        //public static void Search_Destination(Action<DestinationDto> onResult)
        //{
        //    var cw = new CwDestinations();
        //    cw.Closed += (sender, e) => 
        //    {
        //        var selection = cw.Get_Selected_Destination();
        //        if (selection != null && onResult != null)
        //            onResult(selection);
        //    };
        //    cw.Show();
        //}



        public static HashSet<object> Locks = new HashSet<object>();


        public abstract void Search(object objectState, Action<TEntity> onOneResult, Action onCancel, Action onNoResults);


        public void Validate(object objectState, Action<TEntity> onOneResult, Action onNoResults, Action onCancel, bool silent = true)
        {
            Action<IEnumerable<TEntity>> onRefreshed = (entities) =>
            {
                int count = entities.Count();

                if (count == 0 && onNoResults != null)
                    try
                    {
                        onNoResults();
                    }
                    finally
                    {
                        lock (Locks)
                            Locks.Remove(objectState);
                    }

                if (count == 1 && onOneResult != null && silent)
                    try
                    {
                        onOneResult(entities.Single());
                    }
                    finally
                    {
                        lock (Locks)
                            Locks.Remove(objectState);
                    }

                if (count > 1 || (!silent && count == 1))
                {
                    Search(objectState, onOneResult, onCancel, onNoResults);
                }
            };

            lock (Locks)
                if (!Locks.Contains(objectState))
                    Locks.Add(objectState);

            Refresh_ViewModel(onRefreshed);
            //ViewModel.Refresh(onRefreshed);
        }

        protected abstract void Refresh_ViewModel(Action<IEnumerable<TEntity>> onRefreshed);

        public virtual void Dispose()
        {
           
        }
    }



}
