using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AgroFirma.Controllers
{
    public class NewsController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        public NewsController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost, ActionName("Add")]
        public ActionResult AddNews([Bind(Exclude = "IS_ACTIVE, DATE")] rnews rnews)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<IRNewsService>().Create(rnews);

                return View(new rnews());
            }
            return View(rnews);
        }

    }
}
