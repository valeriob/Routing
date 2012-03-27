using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

using search = Routing.Silverlight.Models.References;
using System.ComponentModel;
using Routing.Silverlight.ServiceReferences;
using Silverlight.Common.DynamicSearch;
using ReactiveUI;
using Microsoft.Maps.MapControl;
using System.Windows;

namespace Routing.Silverlight.Models.References
{
    public class DestinationSearchViewModel : search.SearchViewModel<DestinationDto> 
    {
        //IDisposable subscription;
        public DestinationSearchViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
                return;
            //subscription = MessageBus.Current.Listen<Begin_Search_Destination>().Subscribe(Init);
        }


        //Begin_Search_Destination Source;
        //public void Init(Begin_Search_Destination source)
        //{
        //    Source = source;
        //    Refresh();
        //}

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; this.RaisePropertyChanged(r => r.Name);}
        }
        

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; this.RaisePropertyChanged(r => r.Address); }
        }

        private string _Search_Address;
        public string Search_Address
        {
            get { return _Search_Address; }
            set { _Search_Address = value; this.RaisePropertyChanged(r => r.Address); }
        }
        

        private string _ExternalId;
        public string ExternalId
        {
            get { return _ExternalId; }
            set { _ExternalId = value; this.RaisePropertyChanged(r => r.ExternalId);}
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; this.RaisePropertyChanged(r => r.Id); }
        }
        
        private Location _NearBy;
        public Location NearBy
        {
            get { return _NearBy; }
            set { _NearBy = value; this.RaisePropertyChanged(r => r.NearBy); }
        }
        

        public override void Refresh(Action<IEnumerable<DestinationDto>> onRefreshed)
        {
            var service = new ReferencesClient();
            service.Known_DestinationsCompleted += (sender, e) => 
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else
                {
                    Entities.Clear();
                    Entities.AddRange(e.Result);

                    if (onRefreshed != null)
                        onRefreshed(e.Result);
                }
            };
            var query = new SearchDestinations 
            { 
                Address = Address, 
                ExternalId = ExternalId,
                Id = Id, 
                Name = Name
            };

            if (NearBy != null)
            {
                query.NearBy_Latitude = NearBy.Latitude;
                query.NearBy_Longitude = NearBy.Longitude;
                query.NearBy_Radius = 50;
            }
            query.Set_Paging(Entities);

            service.Known_DestinationsAsync(query);
        }

        public override void DeleteEntity(DestinationDto entity)
        {
            throw new NotImplementedException();
        }

        //public void Destination_Found()
        //{
        //    if (Source != null)
        //    {
        //        subscription.Dispose();
        //        MessageBus.Current.SendMessage(new End_Search_Destination(Source, SelectedEntity));
        //    }
        //}

        //public void Search_Canceled()
        //{
        //    if (Source != null)
        //    {
        //        subscription.Dispose();
        //        MessageBus.Current.SendMessage(new End_Search_Destination(Source));
        //    }
        //}

    }
}
