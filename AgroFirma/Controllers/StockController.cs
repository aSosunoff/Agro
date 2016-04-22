using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AgroFirma.Controllers
{
    public class StockController : ControllerInitializer
    {
        public StockController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult Add()
        {
            return View(new rstock());
        }

        [HttpPost, ActionName("Add")]
        public ActionResult AddStock([Bind(Exclude = "IS_ACTIVE, DATE_ADDED")] rstock rstock)
        {
            if (ModelState.IsValid)
            {
                _serviceLayer.Get<IRStockService>().Create(rstock);

                return View(new rstock());
            }
            return View(rstock);
        }


    }
}
