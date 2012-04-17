using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Routing.Domain.Dto.Command;
using System.Threading.Tasks;
using Routing.Domain.Dto.Abstracts;
using Routing.Domain.ReadModel;
using Routing.Web.Models.Scenario;

namespace Routing.Web.Controllers
{
    public class ScenarioController : Controller
    {
        References_ReadModel References_ReadModel;

        public ScenarioController(References_ReadModel _References_ReadModel)
        {
            References_ReadModel = _References_ReadModel;
        }
        //
        // GET : /Scenario/ 

        public ActionResult Index(int? page, int? pageSize)
        {
            var query = new Routing.Domain.Dto.Query.SearchScenarios 
            { 
                UserId = "users/1",
                PageIndex = page.GetValueOrDefault(1), 
                PageSize = pageSize.GetValueOrDefault(10)
            };

            var scenari = References_ReadModel.Search_Scenarios(query);
            var viewModel = new Search_Scenario_ViewModel { Scenari = scenari };
            return View(viewModel);
        }

        //public JsonResult Search(SearchDestinations query)
        //{
        //    var clients = DocumentSession.Query<Destination>()
        //        .To_DestinationDto()
        //        //.Apply_Sort_And_Paging(query, s => s.Id)
        //        .ToList();

        //    return Json(clients, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public ActionResult Nuovo_Scenario()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create_Scenario(Create_Scenario cmd)
        {
            return View();
        }


        //public Task<ViewResult> Index()
        //{
        //    return Task.Factory.StartNew(() => View());
        //}

    }
}
