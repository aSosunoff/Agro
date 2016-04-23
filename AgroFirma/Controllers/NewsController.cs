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
    public class NewsController : ControllerInitializer
    {
        public NewsController(IServiceLayer serviceLayer) : base(serviceLayer){}


        

        //DETAILS
        public ActionResult Details(int id)
        {
            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();
            //TODO: везде IS_ACTIVE поменять на тип перечисление. Не понятно что за магическая единица
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1));
        }

        //LIST
        public ActionResult List()
        {
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetSortList(e => e.IS_ACTIVE == 1).OrderByDescending(e => e.DATE));
        }
        
        

        #region Методы работы для администраторов системы
        //EDIT ADMIN PANEL

        public ActionResult ListAdmin()
        {//TODO: Доступна только администраторов
            //Выводим все новости

            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            ViewBag.ErrorMessage = ViewBagMain.MessageError.Look();

            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetAllList().OrderByDescending(e => e.DATE));
        }

        //DELETE FROM DB
        public ActionResult Remove(int id)
        {//TODO: Доступна только администраторов

            rnews item = _serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id);

            if (item != null)
            {
                string newsName = item.NAME;

                _serviceLayer.Get<IRNewsService>()._Repository.Delete(item);

                ViewBagMain.MessageSuccess.Init(String.Format("Новость \"{0}\" была удалена из Базы данных", newsName));
            }
            else
            {
                ViewBagMain.MessageError.Init("Такой новости нет для выполнения операции удаления");
            }

            return RedirectToAction("ListAdmin");
        }

        //DELETE
        public ActionResult Delete(int id)
        {//TODO: Доступна только администраторов
            _serviceLayer.Get<IRNewsService>().DeleteVirtual(id);

            ViewBagMain.MessageSuccess.Init(String.Format("Новость \"{0}\" не активна", _serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id).NAME));

            return RedirectToAction("ListAdmin");
        }

        //RECOVER
        public ActionResult Recover(int id)
        {//TODO: Доступна только администраторов
            _serviceLayer.Get<IRNewsService>().RecoverVirtual(id);

            ViewBagMain.MessageSuccess.Init(String.Format("Новость \"{0}\" восстановлена", _serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id).NAME));

            return RedirectToAction("ListAdmin");
        }

        //EDIT
        public ActionResult Edit(int id)
        {
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1));
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditItem([Bind(Exclude = "DATE")] rnews item)
        {
            if (ModelState.IsValid)
            {
                //TODO: Не добавляется большой текст
                //TODO: Пофиксить обновление изображения. Сейчас при обновлении вставляется картинка по дефолту
                _serviceLayer.Get<IRNewsService>().Update(item);

                ViewBagMain.MessageSuccess.Init(String.Format("Новость \"{0}\" обновлена", item.NAME));

                return RedirectToAction("Details", new { id = item.PK_ID });
            }
            return View(item);
        }

        //CREATE
        public ActionResult Add()
        {//TODO: Только для админов

            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            return View();
        }
        [HttpPost, ActionName("Add")]
        public ActionResult AddNews([Bind(Exclude = "IS_ACTIVE, DATE")] rnews item)
        {//TODO: Только для админов
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO: Не добавляется большой текст
                    _serviceLayer.Get<IRNewsService>().Create(item);

                    ViewBagMain.MessageSuccess.Init(String.Format("Новость {0} добавлена", item.NAME));

                    return RedirectToAction("Add");
                }
            }
            catch (Exception)
            {
                //TODO: Записать в таблицу ошибку для дальнейшего исправления
                ViewBag.ErrorMessage = "Не предвиденная ошибка";
            }

            ViewBag.ErrorMessage = "Ошибка ввида";

            return View(item);
        }
        #endregion
        
    }
}
