using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AgroFirma.Controllers
{
    public class BasketController : ControllerInitializer
    {
        public BasketController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List()
        {//TODO: Необходимо привязаться к определённому пользователю. Сейчас будут выводиться все товары из корзины

            return View(_serviceLayer.Get<IRBasketService>()._Repository.GetAllList().ToList());
        }

        
    }
}
