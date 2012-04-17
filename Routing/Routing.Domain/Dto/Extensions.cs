using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Routing.Domain.Aggregates;
using Routing.Domain.Dto.Query;
using System.Linq.Expressions;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Domain.Dto
{
    public static class Extensions
    {
        public static IEnumerable<DestinationDto> To_DestinationDto(this IEnumerable<Destination> query)
        {
            return query.Select(c => new DestinationDto 
            { 
                 Id = c.Id, 
                 ExternalId = c.ExternalId,
                 Name = c.Name,
                 Address = c.Address.City, 

                 Latitude = c.Location.Latitude, 
                 Longitude = c.Location.Longitude, 
                 Radius = c.Location.Approximation
            });
        }

        public static IQueryable<AbstractScenarioDto> To_AbstractScenarioDto(this IQueryable<Scenario> query)
        {
            return query.Select(c => new AbstractScenarioDto
            {
                Id = c.Id,
                UserId = c.UserId, 
                Date = c.Date, 
                Name = c.Name, 
                //OrdersCount = c.Orders.Count(),
            });
        }

        //public static IQueryable<AbstractSimulationDto> To_AbstractScenarioDto(this IQueryable<Scenario> query)
        //{
        //    return query.SelectMany(c => new AbstractSimulationDto
        //    {
        //        c.Simulations.SelectMany
        //    });
        //}

        

        public static IQueryable<T> Apply_Sort_And_Paging<T>(this IQueryable<T> query, Paging paging, Expression<Func<T,object>> defaultSorting, bool descending = false)
        {
            if (paging.Must_Sort())
            {
                query = query.OrderBy( paging.Get_Ordering_String() );
            }
            else
                if(descending)
                    query = query.OrderByDescending(defaultSorting);
                else
                    query = query.OrderBy(defaultSorting);

            return query.Apply_Paging(paging);
        }

        public static IQueryable<T> Apply_Paging<T>(this IQueryable<T> query, Paging paging)
        {

            if (paging.PageIndex * paging.PageSize >= 0 && paging.PageSize >= 0)
                query = query.Skip(paging.PageIndex * paging.PageSize).Take(paging.PageSize);
            else
                throw new ArgumentException("Wrong paging parameters");

            return query;
        }
    }
}
