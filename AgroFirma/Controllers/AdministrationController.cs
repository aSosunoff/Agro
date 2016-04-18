using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class AdministrationController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        public AdministrationController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);
        }   

        public ActionResult AddProductInStock()
        {
            rstock rstock = new rstock();
            return View(rstock);
        }

        [HttpPost, ActionName("AddProductInStock")]
        public ActionResult AddProductInStockPost(rstock rstock)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<IRStockService>()._Repository.Create(rstock);

                return RedirectToAction("Index", "Home");
            }

            return View(rstock);

        }

    }
}
