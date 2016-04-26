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
    public class ContractController : ControllerInitializer
    {
        public ContractController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List()
        {
            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            ViewBag.ErrorMessage = ViewBagMain.MessageError.Look();

            return View(_serviceLayer.Get<IRContractService>()._Repository.GetAllList().ToList());
        }

        public ActionResult Details(int id)
        {
            if (!_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).Any())
            {
                ViewBagMain.MessageError.Init("Таких товаров нет");

                return RedirectToAction("List");
            }
            return View(_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).ToList());
        }


        public ActionResult Pay(int id)
        {//TODO: Переделать на POST
            //TODO: Вставить проверку

            _serviceLayer.Get<IRContractService>().Pay(id);

            ViewBagMain.MessageSuccess.Init("Оплачено");

             return RedirectToAction("List");
        }
        
        //Отказ от заказа
        public ActionResult RefuseContract(int id)
        {
            try
            {
                _serviceLayer.Get<IRContractService>().RefuseContract(id);

                ViewBagMain.MessageError.Init("Контракт расторгнут");
            }
            catch (Exception ex)
            {
                ViewBagMain.MessageError.Init(ex.Message);
                throw;
            }

            return RedirectToAction("List");
        }



        //FOR ADMIN
        ///////////////////////////////////////
        public ActionResult ListAdmin()
        {//TODO: Входить только админам

            return View(_serviceLayer.Get<IRContractService>()._Repository.GetAllList().ToList());
        }

        public ActionResult DetailsAdmin(int id)
        {
            if (!_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).Any())
            {
                ViewBagMain.MessageError.Init("Пока ни кто не заключил контрактов");

                return RedirectToAction("ListAdmin");
            }
            return View(_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).ToList());
        }
    }
}
