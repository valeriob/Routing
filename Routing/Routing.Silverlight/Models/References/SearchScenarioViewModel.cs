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
    public class SearchScenarioViewModel : search.SearchViewModel<AbstractScenarioDto> 
    {
        public SearchScenarioViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
                return;
        }

        private DateTime? _From;
        public DateTime? From
        {
            get { return _From; }
            set { _From = value; this.RaisePropertyChanged(r => r.From); }
        }

        private DateTime? _To;
        public DateTime? To
        {
            get { return _To; }
            set { _To = value; this.RaisePropertyChanged(r => r.To); }
        }

        public override void DeleteEntity(AbstractScenarioDto entity)
        {
            throw new NotImplementedException();
        }
        

        public override void Refresh(Action<IEnumerable<AbstractScenarioDto>> onRefreshed)
        {
            var service = new ReferencesClient();
            service.Search_ScenariosCompleted += (sender, e) => 
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
            var query = new  SearchScenarios
            { 
                UserId = RoutingViewModel.Instance.UserId,
                From = From, 
                To = To
            };


            query.Set_Paging(Entities);

            service.Search_ScenariosAsync(query);
        }

    

    }
}
