using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class HomeController : ControllerInitializer
    {
        public HomeController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult Index()
        {

            return View();
        }
    }
}
