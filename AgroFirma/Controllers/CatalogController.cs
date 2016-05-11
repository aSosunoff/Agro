using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using AgroFirma.Models;
using Components;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class CatalogController : ControllerInitializer
    {
        public CatalogController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List(int id)
        {
            ccategory item = _serviceLayer.Get<ICCategoryService>()._Repository.GetItem(e => e.PK_ID == id);
            if (item != null)
            {
                ViewBag.CategoryTitle = item.TEXT;

                CategoryModel categoryModel = new CategoryModel();

                categoryModel.Ccategories = _serviceLayer
                    .Get<ICCategoryService>()
                    ._Repository
                    .GetAllList()
                    .ConnectByPrior(
                    e => 
                        new
                            {
                                e.PK_ID, e.PARENT_ID, ROOT = id
                            })
                    .Where(e => e.LEVEL == 2)
                    .RemoveWrapModel();

                int[] arrayIdCategory = _serviceLayer
                    .Get<ICCategoryService>()
                    ._Repository
                    .GetAllList()
                    .ConnectByPrior(
                    e =>
                        new
                        {
                            e.PK_ID,
                            e.PARENT_ID,
                            ROOT = id
                        })
                    .Where(e => e.FLAG_TREE)
                    .RemoveWrapModel().Select(e => e.PK_ID)
                    .ToArray();

                categoryModel.Rstocks = _serviceLayer
                    .Get<IRStockService>()
                    ._Repository
                    .GetSortList(e => arrayIdCategory
                        .Contains(e.FK_ID_CATEGORY))
                    .ToList();

                return View(categoryModel);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Add()
        {//TODO:СДЕЛАТЬ Только для АДМИНИСТРАТОРОВ
            
            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            //Создаём тут что бы наше свойство с выбором родителя активировалось
            return View(new ccategory());
        }

        [HttpPost, ActionName("Add")]
        public ActionResult AddCategory([Bind(Exclude = "IS_ACTIVE")] ccategory ccategory)
        {//TODO:СДЕЛАТЬ Только для АДМИНИСТРАТОРОВ
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceLayer.Get<ICCategoryService>().Create(ccategory);

                    ViewBagMain.MessageSuccess.Init("Категория добавлена");

                    return RedirectToAction("Add");
                }
                catch (Exception)
                {
                    //TODO: Записать в таблицу ошибку для дальнейшего исправления
                    ViewBag.ErrorMessage = "Не предвиденная ошибка";
                }    
            }

            ViewBag.ErrorMessage = "Ошибка ввида";

            return View(ccategory);
        }


    }
}
