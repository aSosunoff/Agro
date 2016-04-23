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


        public ActionResult AddedToBasket(int id, bool success = false)
        {
            //TODO: параметр success необходим для того что бы отображать положительное сообщение при добавлении товара
            //TODO: Так как если вызвать просто возврат View не обновляется колличество в корзине
            //TODO: А при Редиректе не выводиться ViewBag

            rstock rstock = _serviceLayer.Get<IRStockService>().GetItemToId(id);

            if (success)
                ViewBag.SuccessMessage = String.Format("Товар \"{0}\" успешно добавлен в корзину.", rstock.NAME);

            return View(rstock);
        }

        [HttpPost, ActionName("AddedToBasket")]
        public ActionResult Added(rstock rstock)
        {

            try
            {
                _serviceLayer.Get<IRStockService>().AddedProductToBasket(rstock);
                
                return RedirectToAction("AddedToBasket", new{ id = rstock.PK_ID, success = true});
            }
            catch (Exception ex)
            {
                ModelState["QANTITY"].Errors.Add(ex.Message);
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(_serviceLayer.Get<IRStockService>().GetItemToId(rstock.PK_ID));
        }

        //DETAILS
        public ActionResult Details(int id)
        {
            //TODO: везде IS_ACTIVE поменять на тип перечисление. Не понятно что за магическая единица
            return View(_serviceLayer.Get<IRStockService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1));
        }

        //CREATE
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

                return RedirectToAction("Add");
            }
            return View(rstock);
        }


    }
}
