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
            //Создаём тут что бы наше свойство с шагом(измерение единици товара) активировалось
            rstock rstock = new rstock();
            //Выбираем только те категории которые не имеют дочерние элементы
            //var t = _ServiceLayer.Get<ICCategoryService>()._Repository.GetSortList()
            return View(rstock);
        }

        [HttpPost, ActionName("AddProductInStock")]
        public ActionResult AddProductInStockPost([Bind(Exclude = "IS_ACTIVE, DATE_ADDED")] rstock rstock)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<IRStockService>().Create(rstock);
                return View();
            }
            return View(rstock);
        }

    }
}
