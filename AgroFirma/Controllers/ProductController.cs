using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AgroFirma.Controllers
{
    public class ProductController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }
        public ProductController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer); 
        }   

        public ActionResult Index()
        {
            return View();
        }

    }
}
