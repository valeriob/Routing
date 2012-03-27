using System;
using System.Net;
using System.Linq;
using System.Windows;

using ReactiveUI;
using System.ServiceModel.DomainServices.Client;
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

namespace Silverlight.Common.DynamicSearch
{
    public interface ISearchViewModel
    {
        void Refresh();
        void SetFilters(AbstractFilter rootFilter);

        void EditSelectedEntity();
        void CreateNewEntity();
        void DeleteSelectedEntity();

        ICommand DeleteSelectedEntityCmd { get; }
        ICommand EditSelectedEntityCmd { get; }
        ICommand CreateNewEntityCmd { get; }
    }

    public abstract class SearchViewModel<TEntity> : ReactiveObject, ISearchViewModel
        where TEntity : class
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
        
        

        private ObservableCollection<DataBindableFilter> _filters;
        public ObservableCollection<DataBindableFilter> Filters
        {
            get { return _filters ?? (_filters = new ObservableCollection<DataBindableFilter>()); }
            set { _filters = value; this.RaisePropertyChanged(v => v.Filters); }
        }

        public AbstractFilter RootFilter { get; set; }


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

            var canHitEdit = this.WhenAny(p => p.SelectedEntity, e => e.Value != null);
            var cmd = new ReactiveCommand(canHitEdit);
            cmd.Subscribe(a => EditSelectedEntity() );
            EditSelectedEntityCmd = cmd;

            var canHitCreate = this.WhenAny(p => p.CanCreateNewEntity, e => e.Value);
            cmd = new ReactiveCommand(canHitCreate);
            cmd.Subscribe(a => CreateNewEntity() );
            CreateNewEntityCmd = cmd;

            var canHitDelete = this.WhenAny(p => p.CanDeleteEntity, e => e.Value);
            cmd = new ReactiveCommand(canHitDelete);
            cmd.Subscribe(a => DeleteSelectedEntity());
            DeleteSelectedEntityCmd = cmd;

            Entities.OnRefresh += (sender, e) => Refresh();
        }
 
        public void Refresh()
        {
            Refresh(null);
        }
        public abstract void Refresh(Action<IEnumerable<TEntity>> onRefreshed);
        public abstract void DeleteEntity(TEntity entity);


        public void EditSelectedEntity()
        {
            EditSelectedEntity(null);
        }
        public void EditSelectedEntity(Action<TEntity> closingAction = null)
        {
            var childWindow = CreateChildWindow(SelectedEntity);

            if (childWindow == null)
                return;

            childWindow.Closed += (sender, e) =>
            {
                if (childWindow.DialogResult.Value)
                {
                    if(closingAction != null)
                        closingAction(SelectedEntity);
                    Refresh();
                }
            };
            childWindow.Show();
        }
        public void CreateNewEntity()
        {
            var childWindow = CreateChildWindow(null);
            if (childWindow == null)
                return;

            childWindow.Closed += (sender, e) =>
            {
                if (childWindow.DialogResult.Value)
                    Refresh();
            };
            childWindow.Show();
        }

        public void DeleteSelectedEntity()
        {
            if (MessageBox.Show("Conferma Eliminazione ?") == MessageBoxResult.OK)
            {
                DeleteEntity(SelectedEntity);
            }
        }


        public void SetFilters(AbstractFilter rootFilter)
        {
            try
            {
                var visitor = new IteratorFilterVisitor<TEntity, DataBindableFilter>((f) => { if (f != null && f.IsVisible) Filters.Add(f); });
                rootFilter.Accept(visitor);
            }
            catch(Exception)
            {
                Filters.Clear();
            }
            RootFilter = rootFilter;
        }

        public ExpressionDataBindableFilter BuildFilter<TResult>(Expression<Func<TEntity, TResult>> property)
        {
            var visitor = new FilterExpressionVisitor();
            var propName = visitor.GetPropertyName(property.Body);

            var filter = new ExpressionDataBindableFilter();
            filter.Expression = property;
            //filter.ValueType = typeof(TResult);

            var memberExp = property.Body as MemberExpression;
            filter.PropertyType = typeof(TResult);
            filter.PropertyName = propName;
            return filter;
        }

        protected abstract ChildWindow CreateChildWindow(TEntity selectedEntity);

    }

    public class DesignEntitySearchViewModel : SearchViewModel<DesignEntity>
    {
        public DesignEntitySearchViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                BuildDesignData();
                return;
            }
        }

        protected override ChildWindow CreateChildWindow(DesignEntity selectedEntity)
        {
            throw new NotImplementedException();
        }

        public override void Refresh(Action<IEnumerable<DesignEntity>> onRefreshed)
        {
            throw new NotImplementedException();
        }
        public override void DeleteEntity(DesignEntity entity)
        {
            throw new NotImplementedException();
        }

        protected void BuildDesignData()
        {
            for (int i = 0; i < 20; i++)
                Entities.Add(new DesignEntity { Id= i, Bool = i%2==0, DateTime = DateTime.Now, Name = "Name "+i });


            var filterA = BuildFilter(f => f.Name);
            filterA.Operator = Common.DynamicSearch.FilterOperator.Equals;
            filterA.Value = "Name";
            filterA.DisplayName = "Name";

            var filterD = BuildFilter(f => f.DateTime);
            filterD.Operator = Common.DynamicSearch.FilterOperator.Equals;
            filterD.Value = DateTime.Now;
            filterD.DisplayName = "DateTime";

            var filterE = BuildFilter(f => f.Bool);
            filterE.Operator = Common.DynamicSearch.FilterOperator.Equals;
            filterE.Value = true;
            filterE.DisplayName = "Bool";

            var filterB = BuildFilter(f => f.Id);
            filterB.Operator = Common.DynamicSearch.FilterOperator.Equals;
            filterB.Value = 123;
            filterB.DisplayName = "Id";

            var filter = (filterA & filterB & filterD & filterE);

            SetFilters(filter);
        }



    }

    public class DesignEntity : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public bool Bool { get; set; }

    }
}
