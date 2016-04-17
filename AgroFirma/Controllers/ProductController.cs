using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class ProductController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        private PageModel PageModels { get; set; }

        public ProductController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);

            PageModels = new PageModel(serviceLayer);
        }   

        public ActionResult Index()
        {
            return View(PageModels);
        }

    }
}
