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

        [HttpPost, ActionName("Details")]
        public ActionResult Added(rstock rstock)
        {
            int qantity;
            if (int.TryParse(rstock.QANTITY.ToString(), out qantity))
            {
                try
                {
                    _serviceLayer.Get<IRStockService>().AddedProductToBasket(rstock);

                    return RedirectToAction("Details", new { id = rstock.PK_ID, success = true });
                }
                catch (Exception ex)
                {
                    ModelState["QANTITY"].Errors.Add(ex.Message);
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            else
                ViewBag.ErrorMessage = "Введено недопустимое значение";

            return View(_serviceLayer.Get<IRStockService>().GetItemToId(rstock.PK_ID));
        }

        //DETAILS
        public ActionResult Details(int id, bool success = false)
        {
            //TODO: параметр success необходим для того что бы отображать положительное сообщение при добавлении товара
            //TODO: Так как если вызвать просто возврат View не обновляется колличество в корзине
            //TODO: А при Редиректе не выводиться ViewBag

            //TODO: везде IS_ACTIVE поменять на тип перечисление. Не понятно что за магическая единица
            rstock rstock = _serviceLayer.Get<IRStockService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1);

            if (success)
                ViewBag.SuccessMessage = String.Format("Товар \"{0}\" успешно добавлен в корзину.", rstock.NAME);

            return View(rstock);
        }

        //CREATE
        public ActionResult Create()
        {
            return View(new rstock());
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreateStock([Bind(Exclude = "IS_ACTIVE, DATE_ADDED")] rstock rstock)
        {
            if (ModelState.IsValid)
            {
                _serviceLayer.Get<IRStockService>().Create(rstock);

                return RedirectToAction("Create");
            }
            return View(rstock);
        }


    }
}
