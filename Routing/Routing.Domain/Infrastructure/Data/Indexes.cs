using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Routing.Domain.Aggregates;

namespace Routing.Domain
{
    public class Destination_ByLocation : AbstractIndexCreationTask<Destination>
    {
        public Destination_ByLocation()
        {
            Map = resource =>   from path in resource
                                select new { _ = SpatialIndex.Generate(path.Location.Latitude, path.Location.Longitude) };

            
            //Index(x => x.BudgetId, FieldIndexing.Analyzed);
        }
    }


    //public class LocalizedPayment_ByLocation : AbstractIndexCreationTask<LocalizedPayment>
    //{
    //    public LocalizedPayment_ByLocation()
    //    {
    //        Map = parkingPayment => from path in parkingPayment
    //                          select new { _ = SpatialIndex.Generate(path.Location.Latitude, path.Location.Longitude) };


    //        //Index(x => x.BudgetId, FieldIndexing.Analyzed);
    //    }
    //}

    //public class Country_ByName : AbstractIndexCreationTask<Country>
    //{
    //    public Country_ByName()
    //    {
    //        Map = countries => from country in countries
    //                          select new { country.Name };


    //        Index(x => x.Id, FieldIndexing.Analyzed);
    //    }
    //}

    //public class Users_CountByCountry : AbstractIndexCreationTask<User>
    //{
    //    public Users_CountByCountry()
    //    {
    //        Map = users => from user in users
    //              select new {user.Country, Count = 1};
    //        Reduce= results => from result in results
    //                           group result by result.Country into g
    //                           select new { Country = g.Key, Count = g.Sum(x=>x.Count)}

    //    }
    //}
}
