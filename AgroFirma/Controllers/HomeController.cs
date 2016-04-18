using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Engine.Service;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class HomeController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        private PageModel PageModels { get; set; }

        public HomeController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);

            PageModels = new PageModel(serviceLayer);
        }   

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
