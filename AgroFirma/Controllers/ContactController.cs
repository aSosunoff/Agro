using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Model.Engine.Service;

namespace AgroFirma.Controllers
{
    public class ContactController : ControllerInitializer
    {
        public ContactController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult Index()
        {
            return View();
        }


    }
}
