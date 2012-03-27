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

namespace Silverlight.Common.DynamicSearch
{
    public abstract class RiaSearchViewModel<TEntity> : SearchViewModel<TEntity>
        where TEntity : System.ServiceModel.DomainServices.Client.Entity
    {
        public DomainContext Service { get; set; }
     
        public RiaSearchViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            Service = GetDomainContext();

            //Entities.OnRefresh += (sender, e) => Refresh();
        }
 
        public override void Refresh(Action<IEnumerable<TEntity>> onRefreshed)
        {
            GetEntitySet().Clear();

            var sorting = Entities.SortDescriptions.Select(s => s.PropertyName).FirstOrDefault();
            var desc = Entities.SortDescriptions.Select(s => s.Direction == ListSortDirection.Descending).FirstOrDefault();

            var query = GetEntityQuery();

            // Filtering
            if (RootFilter != null)
            {
                var visitor = new FilterVisitor<TEntity>();
                var filterPredicate = visitor.Visit(RootFilter);

                if(filterPredicate!= null)
                    query = query.Where(filterPredicate);
            }

            // Sorting
            try
            {
                var propertyInfo = typeof(TEntity).GetProperty(sorting);
                var lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(typeof(TEntity), propertyInfo.PropertyType, sorting);

                var orderBy = typeof(EntityQueryable).GetMethod("OrderBy");
                orderBy = orderBy.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType );

                var orderByDescending = typeof(EntityQueryable).GetMethod("OrderByDescending");
                orderByDescending = orderByDescending.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType);

                if (desc)
                    query = (EntityQuery<TEntity>)orderByDescending.Invoke(null, new object[] { query, lambda });
                else
                    query = (EntityQuery<TEntity>)orderBy.Invoke(null, new object[] { query, lambda });
            }
            catch (Exception) 
            {
                query = ApplyDefaultSorting(query);
            }

            // Paging
            query = query.Skip(Entities.PageIndex * Entities.PageSize).Take(Entities.PageSize);
            query.IncludeTotalCount = true;

            //Executing
            IsBusy = true;
            var loadOp = Service.Load<TEntity>(query);
            loadOp.Completed += (sender, e) =>
            {
                IsBusy = false;
                if (loadOp.HasError)
                {
                    MessageBox.Show(loadOp.Error.Message);
                    loadOp.MarkErrorAsHandled();
                }
                else
                {
                    Entities.Clear();
                    Entities.TotalItemCount = loadOp.TotalEntityCount;
                    if(Entities.PageIndex > Entities.PageCount)
                        Entities.MoveToPage(0);
                    foreach (var en in loadOp.Entities)
                        Entities.Add(en);

                    if(onRefreshed!= null)
                        onRefreshed(loadOp.Entities);
                }
            };

        }

        protected abstract DomainContext GetDomainContext();
        protected abstract EntityQuery<TEntity> ApplyDefaultSorting(EntityQuery<TEntity> query);
        protected abstract EntitySet<TEntity> GetEntitySet();
        protected abstract EntityQuery<TEntity> GetEntityQuery();


        public override void DeleteEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }

}
