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


        //EDIT
        public ActionResult Edit(int id)
        {
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id));
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditItem([Bind(Exclude = "DATE")] rnews item)
        {
            if (ModelState.IsValid)
            {
                //TODO: Не добавляется большой текст
                //TODO: Пофиксить обновление изображения. Сейчас при обновлении вставляется картинка по дефолту
                _serviceLayer.Get<IRNewsService>().Update(item);

                return RedirectToAction("Details", new { id = item .PK_ID});
            }
            return View(item);
        }

        //DELETE
        public ActionResult Delete(int id)
        {
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteItem(rnews item)
        {
            //TODO: Пофиксить виртуальное удаление. Не получилось удалить по пришедшей модели. И поэтомк удаление происходит через поиск элемента в базе. НЕХОРОШО
            _serviceLayer.Get<IRNewsService>().DeleteVirtual(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == item.PK_ID));
            //ViewBag.ErrorMessage = String.Format("Вы удалили новость {0}", item.NAME);
            //TODO: Не выводиться ViewBag.ErrorMessage
            return RedirectToAction("List");
        }

        //DETAILS
        public ActionResult Details(int id)
        {
            //TODO: везде IS_ACTIVE поменять на тип перечисление. Не понятно что за магическая единица
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1));
        }

        //LIST
        public ActionResult List()
        {
            return View(_serviceLayer.Get<IRNewsService>()._Repository.GetSortList(e => e.IS_ACTIVE == 1).OrderByDescending(e => e.DATE));
        }
        
        //CREATE
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost, ActionName("Add")]
        public ActionResult AddNews([Bind(Exclude = "IS_ACTIVE, DATE")] rnews item)
        {
            if (ModelState.IsValid)
            {
                //TODO: Не добавляется большой текст
                _serviceLayer.Get<IRNewsService>().Create(item);

                return RedirectToAction("Add");
            }
            return View(item);
        }
    }
}
