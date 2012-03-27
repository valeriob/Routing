using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing.Domain.Infrastructure;
using Autofac;
using Routing.Domain.ReadModel;
//using Utilities.DynamicSearch;
using Routing.Domain.Dto.Query;

namespace Routing.Domain.Test.Read_Model
{
    [TestClass]
    public class References_Test
    {
        //[TestMethod]
        //public void Query_Known_Destinations()
        //{
        //    var read = Container.Instance.Resolve<References_ReadModel>();


        //    var filterA = new ExpressionDataBindableFilter { Operator = FilterOperator.Contains, 
        //        PropertyName = "Address", PropertyType=typeof(string), Value = "a" };
        //    var filterB = new ExpressionDataBindableFilter { Operator = FilterOperator.Equals, 
        //        PropertyName = "Address", PropertyType=typeof(string), Value = "Russi" };
        //    var filter = filterA | filterB;

        //    var query = new SearchQuery
        //    {
        //         PageIndex =0, PageSize = 10, Filter = filter
        //    };

        //    var searchDestinations = new SearchDestinations { Address="russi" };

        //    var result = read.Known_Destinations(searchDestinations);
        //}

        [TestMethod]
        public void Search_Scenario()
        {
            //var container = Container.Init_Container();
            var read = Container.Instance.Resolve<References_ReadModel>();


            var query = new SearchScenarios
            {
                 PageIndex = 0, 
                 PageSize = 10,
                 UserId = "users/1",
                 OrderBy = "Date", 
                 Descending = true
            };
            read.Search_Scenarios(query);
        }
    }
}
