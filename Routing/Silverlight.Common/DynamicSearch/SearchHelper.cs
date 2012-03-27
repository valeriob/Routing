using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.ServiceModel.DomainServices.Client;
using Silverlight.Common.Helpers;

namespace Silverlight.Common.DynamicSearch
{
    public abstract class SearchHelper
    {
        public abstract AbstractFilter BuildFilters();
        public abstract void BuildColumns(DataGrid datagrid);
        public abstract ISearchViewModel GetViewModel();

        public static SearchHelper Build(Type entityType)
        {
            // Find ViewModel subtype
            var baseViewModelType = typeof(SearchViewModel<>);
            var typedBaseViewModel = baseViewModelType.MakeGenericType(entityType);
          
            //var viewModelType = baseViewModelType.Assembly.GetTypes().Where(t => typedBaseViewModel.IsAssignableFrom(t)).FirstOrDefault();
            var viewModelType = ReflectionHelper.GetTypes().Where(t => typedBaseViewModel.IsAssignableFrom(t)).FirstOrDefault();
            var viewModelInstance = Activator.CreateInstance(viewModelType);


            var baseSearchHelperType = typeof(SearchHelper<>);
            var typedSearchHelperType = baseSearchHelperType.MakeGenericType(entityType);
            //var searchHelperType = baseViewModelType.Assembly.GetTypes().Where(t => typedSearchHelperType.IsAssignableFrom(t)).FirstOrDefault();
            var searchHelperType = ReflectionHelper.GetTypes().Where(t => typedSearchHelperType.IsAssignableFrom(t)).FirstOrDefault();

            var searchHelperInstance = (SearchHelper)Activator.CreateInstance(searchHelperType, viewModelInstance);


            return searchHelperInstance;
        }
    }

    public abstract class SearchHelper<TEntity> : SearchHelper where TEntity : Entity
    {
        public static HashSet<object> Locks { get; set; }

        static SearchHelper()
        {
            Locks = new HashSet<object>();
        }

        public SearchViewModel<TEntity> ViewModel { get; set; }

        private AbstractFilter _AbstractFilter;
        public AbstractFilter AbstractFilter
        {
            get { return _AbstractFilter; }
            set { _AbstractFilter = value; }
        }
        

        public SearchHelper(SearchViewModel<TEntity> viewModel)
        {
            ViewModel = viewModel;
            AbstractFilter = BuildFilters();

            SetFilters(AbstractFilter);
        }

        public override ISearchViewModel GetViewModel()
        {
            return ViewModel;
        }

        public static SearchHelper<TEntity> Build()
        {
            var entityType = typeof(TEntity);

            return (SearchHelper<TEntity>)SearchHelper.Build(entityType);
        }



        public CwSearch BuildNewCwSearch()
        {
            var cwSearch = new CwSearch();

            BuildColumns(cwSearch.ucSearch.dataGridEntities);

            cwSearch.DataContext = ViewModel;
            return cwSearch;
        }

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
            };

            lock(Locks)
                if (!Locks.Contains(objectState))
                    Locks.Add(objectState);
            
            ViewModel.Refresh(onRefreshed);
        }

        public void SetFilters(AbstractFilter rootFilter)
        {
            ViewModel.SetFilters(rootFilter);
        }

        public void SetFilterValue<TResult>(Expression<Func<TEntity,TResult>> selector, TResult value)
        {
            var visitor = new FilterExpressionVisitor();
            var propName = visitor.GetPropertyName(selector.Body);

            var filter = ViewModel.Filters.OfType<ExpressionDataBindableFilter>().SingleOrDefault(f => f.PropertyName == propName);
            if(filter!= null)
                filter.Value = value;
        }

        public void AddColumn<TResult>(DataGrid dataGrid, Expression<Func<TEntity, TResult>> selector, string columnHeader, string formatString = null, bool fillWidth = false, bool canSort = true)
        {
            var mv = new FilterExpressionVisitor();
            string propertyName = mv.GetPropertyName(selector.Body);

            var column = new DataGridTextColumn();
            if (fillWidth)
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            column.CanUserSort = canSort;
            if (!string.IsNullOrEmpty(columnHeader))
                column.Header = columnHeader;
            else
                column.Header = propertyName;

            column.Binding = new Binding(propertyName);
            column.Binding.StringFormat = formatString;

            dataGrid.Columns.Add(column);

            //return dataGrid;
        }

        public void ShowCwSearch()
        {
            BuildNewCwSearch().Show();
        }


    }

}
