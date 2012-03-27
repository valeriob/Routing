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
    public class Search_Scenario_Helper : Use_Case_Manager<AbstractScenarioDto>
    {
        public SearchScenarioViewModel ViewModel { get; protected set;}
        public Search_Scenario_Helper()
        {
            ViewModel = new SearchScenarioViewModel();
        }

        public override void Search(object objectState, Action<DestinationDto> onOneResult, Action onCancel, Action onNoResults)
        {
            var cwSearch = new CwDestinations(ViewModel);

            cwSearch.Closed += (a, b) =>
            {
                try
                {
                    if (cwSearch.DialogResult.Value)
                    {
                        if (ViewModel.SelectedEntity != null && onOneResult != null)
                            onOneResult(ViewModel.SelectedEntity);
                        else
                            if (onNoResults != null)
                                onNoResults();
                    }
                    else
                    {
                        if (onCancel != null)
                            onCancel();
                    }
                }
                finally
                {
                    lock (Locks)
                        Locks.Remove(objectState);
                }
            };
            cwSearch.Show();
        }

        protected override void Refresh_ViewModel(Action<IEnumerable<DestinationDto>> onRefreshed)
        {
            ViewModel.Refresh(onRefreshed);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }


}
