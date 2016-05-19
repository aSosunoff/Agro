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

                    ViewBagMain.MessageSuccess.Init(String.Format("Товар \"{0}\" успешно добавлен в корзину.", rstock.NAME));

                    return RedirectToAction("Details", new { id = rstock.PK_ID });
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
        public ActionResult Details(int id)
        {
            //TODO: везде IS_ACTIVE поменять на тип перечисление. Не понятно что за магическая единица
            rstock rstock = _serviceLayer.Get<IRStockService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1);

            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            return View(rstock);
        }

        //CREATE
        public ActionResult Create()
        {//TODO: Только для администраторов
            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            return View(new rstock());
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreateStock([Bind(Exclude = "IS_ACTIVE, DATE_ADDED")] rstock rstock)
        {//TODO: Только для администраторов
            if (ModelState.IsValid)
            {
                _serviceLayer.Get<IRStockService>().Create(rstock);

                ViewBagMain.MessageSuccess.Init("Товар добавлен на склад");

                return RedirectToAction("Create");
            }

            ViewBag.ErrorMessage = "Ошибка ввида";

            return View(rstock);
        }


    }
}
