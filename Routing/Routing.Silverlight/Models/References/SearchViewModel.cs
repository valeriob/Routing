using System;
using System.Net;
using System.Linq;
using System.Windows;

using ReactiveUI;
using OnEnergy.Silverlight.Common;
using System.Windows.Controls;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using Silverlight.Common.DynamicSearch;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI.Xaml;
using Silverlight.Common;

namespace Routing.Silverlight.Models.References
{
    public abstract class SearchViewModel<TEntity> : ReactiveObject where TEntity : class
    {
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; this.RaisePropertyChanged(v => v.IsBusy); }
        }

        public ICommand EditSelectedEntityCmd { get; set; }
        public ICommand CreateNewEntityCmd { get; set; }
        public ICommand DeleteSelectedEntityCmd { get; set; }

        private bool _canCreateNewEntity;
        public bool CanCreateNewEntity
        {
            get { return _canCreateNewEntity; }
            set { _canCreateNewEntity = value; this.RaisePropertyChanged(c => c.CanCreateNewEntity); }
        }

        private bool _canDeleteEntity;
        public bool CanDeleteEntity
        {
            get { return _canDeleteEntity; }
            set { _canDeleteEntity = value; this.RaisePropertyChanged(c => c.CanDeleteEntity); }
        }
        
       

        private PagedSortableCollectionView<TEntity> _entities;
        public PagedSortableCollectionView<TEntity> Entities
        {
            get { return _entities ?? (_entities = new PagedSortableCollectionView<TEntity>()); }
            set { _entities = value;  this.RaisePropertyChanged(a => a.Entities); }
        }

   
        private TEntity _selectedEntity;
        public TEntity SelectedEntity
        {
            get { return _selectedEntity; }
            set { _selectedEntity = value; this.RaisePropertyChanged(a => a.SelectedEntity); }
        }

        public SearchViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
                return;
            RxApp.DeferredScheduler = System.Reactive.Concurrency.DispatcherScheduler.Instance; 
            CanCreateNewEntity = true;

            //var canHitEdit = this.WhenAny(p => p.SelectedEntity, e => e.Value != null);
            //var cmd = new ReactiveCommand(canHitEdit);
            //cmd.Subscribe(a => EditSelectedEntity() );
            //EditSelectedEntityCmd = cmd;

            //var canHitCreate = this.WhenAny(p => p.CanCreateNewEntity, e => e.Value);
            //cmd = new ReactiveCommand(canHitCreate);
            //cmd.Subscribe(a => CreateNewEntity() );
            //CreateNewEntityCmd = cmd;

            //var canHitDelete = this.WhenAny(p => p.CanDeleteEntity, e => e.Value);
            //cmd = new ReactiveCommand(canHitDelete);
            //cmd.Subscribe(a => DeleteSelectedEntity());
            //DeleteSelectedEntityCmd = cmd;

            Entities.OnRefresh += (sender, e) => Refresh();
        }
 

        public void Refresh()
        {
            Refresh(null);
        }

        public abstract void Refresh(Action<IEnumerable<TEntity>> onRefreshed);
        public abstract void DeleteEntity(TEntity entity);

/*
        public static HashSet<object> Locks = new HashSet<object>();
        public void Verify(object objectState, Action<TEntity> onOneResult, Action onNoResults, Action onCancel, bool silent = true)
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
                    var cwSearch = BuildNewCwSearch();

                    cwSearch.Closed += (a, b) =>
                    {
                        try
                        {
                            if (cwSearch.DialogResult.Value)
                            {
                                if (SelectedEntity != null && onOneResult != null)
                                    onOneResult(SelectedEntity);
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
            };

            lock (Locks)
                if (!Locks.Contains(objectState))
                    Locks.Add(objectState);

            Refresh(onRefreshed);
        }
        */

    }

   


}
