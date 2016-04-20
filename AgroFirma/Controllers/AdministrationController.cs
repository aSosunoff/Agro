using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
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
            
            return View(new rstock());
        }

        [HttpPost, ActionName("AddProductInStock")]
        public ActionResult AddProductInStockPost([Bind(Exclude = "IS_ACTIVE, DATE_ADDED")] rstock rstock)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<IRStockService>().Create(rstock);

                return View(new rstock());
            }
            return View(rstock);
        }

        public ActionResult AddCategory()
        {
            //Создаём тут что бы наше свойство с выбором родителя активировалось
            return View(new ccategory());
        }

        [HttpPost, ActionName("AddCategory")]
        public ActionResult AddCategory([Bind(Exclude = "IS_ACTIVE")] ccategory ccategory)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<ICCategoryService>().Create(ccategory);

                return View(new ccategory());
            }
            return View(ccategory);
        }

    }
}
