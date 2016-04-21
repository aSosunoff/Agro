using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class CatalogController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        public CatalogController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);
        }

        public ActionResult List(int id)
        {
            ViewBag.CategoryTitle = _ServiceLayer.Get<ICCategoryService>()._Repository.GetItem(e => e.PK_ID == id).TEXT;

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

            categoryModel.Ccategories = _ServiceLayer
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


            int[] arrayIdCategory = _ServiceLayer
                .Get<ICCategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(model)
                .Where(e => e.FLAG_TREE)
                .RemoveWrapModel().Select(e => e.PK_ID)
                .ToArray();

            categoryModel.Rstocks = _ServiceLayer
                .Get<IRStockService>()
                ._Repository
                .GetSortList(e => arrayIdCategory
                    .Contains(e.FK_ID_CATEGORY))
                .ToList();

            return View(categoryModel);
        }

        public ActionResult Add()
        {
            //Создаём тут что бы наше свойство с выбором родителя активировалось
            return View(new ccategory());
        }

        [HttpPost, ActionName("Add")]
        public ActionResult AddCategory([Bind(Exclude = "IS_ACTIVE")] ccategory ccategory)
        {
            if (ModelState.IsValid)
            {
                _ServiceLayer.Get<ICCategoryService>().Create(ccategory);

                return View(new ccategory());
            }
            return View(ccategory);
        }
    }
}
