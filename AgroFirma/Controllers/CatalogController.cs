using System;
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
    public class CatalogController : ControllerInitializer
    {
        public CatalogController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List(int id)
        {
            ViewBag.CategoryTitle = _serviceLayer.Get<ICCategoryService>()._Repository.GetItem(e => e.PK_ID == id).TEXT;

            ConnectByPriorInModel model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "PK_ID",
                    ColummValue = id
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "PK_ID",
                    Right = "PARENT_ID"
                }
            };
            //TODO: Сделать Reset модели 

            CategoryModel categoryModel = new CategoryModel();

            categoryModel.Ccategories = _serviceLayer
                .Get<ICCategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(model)
                .Where(e => e.LEVEL == 2)
                .RemoveWrapModel();

            model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "PK_ID",
                    ColummValue = id
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "PK_ID",
                    Right = "PARENT_ID"
                }
            };


            int[] arrayIdCategory = _serviceLayer
                .Get<ICCategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(model)
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
