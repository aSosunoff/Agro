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
    public class BasketController : ControllerInitializer
    {
        public BasketController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List()
        {//TODO: Необходимо привязаться к определённому пользователю. Сейчас будут выводиться все товары из корзины
            //TODO: Проверить наличие продукта на складе если его уже нет чем заказанного то вывыести сообщение

            
            string errorMessage;
            if (_serviceLayer.Get<IRBasketService>().IsNullQantityProduct(out errorMessage))
                ViewBag.ErrorMessage = errorMessage;


            return View(_serviceLayer.Get<IRBasketService>()._Repository.GetAllList().ToList());
        }

        public ActionResult OrderToPrepare()
        {
            try
            {
                string errorMessage;
                if (_serviceLayer.Get<IRBasketService>().IsNullQantityProduct(out errorMessage))
                    return RedirectToAction("List");

                    //TODO: Сделать логику прекрипление заказчика компании. В данный момент прикрепляется тестовый пользователь заказчик
                    ruser user = _serviceLayer.Get<IRUserService>()._Repository.GetItem(e => e.PK_ID == 1);//TODO: 1 индекс тестового ползователя заказчика

                    //Оставляем данные пользователя заказчика компании на тот момент когда осуществлялся заказ. На тот случий если данные пользователя изменяться
                    //но прикрепим его ID что бы следить за информацией по этому пользователю
                    ruser_info userCustomer = new ruser_info()
                    {
                        FK_USER = user.PK_ID,
                        NAME = user.NAME,
                        SURNAME = user.SURNAME,
                        MIDDLE_NAME = user.MIDDLE_NAME,
                        NAME_COMPANY = user.NAME_COMPANY,
                        LEGAL_ADDRESS = user.LEGAL_ADDRESS,
                        MAIL_ADDRESS = user.MAIL_ADDRESS,
                        PHONE = user.PHONE,
                        FAX = user.FAX,
                        INN = user.INN,
                        KPP = user.KPP,
                        CHECKING_ACCOUNT = user.CHECKING_ACCOUNT,
                        BANK = user.BANK,
                        CORRESPONDENT_ACCOUNT = user.CORRESPONDENT_ACCOUNT,
                        BIK = user.BIK,
                        CITY_NAME = user.sstreet.scity.NAME
                    };

                    _serviceLayer.Get<IRUser_infoService>()._Repository.Create(userCustomer);

                    _serviceLayer.Get<IRBasketService>().OrderToPrepare(userCustomer);

                    return RedirectToAction("Index", "Home");
                
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View("List");
            //todo: Вывести детальное предупреждение. Предусмотреть удаление товара из корзины которого нет на складе. Так же итоговую сумму выводить без сложения с товаром которого нет на складе
        }
        
    }
}
