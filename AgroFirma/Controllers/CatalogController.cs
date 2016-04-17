using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class CatalogController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        private PageModel _pageModel { get; set; }

        public CatalogController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);

            _pageModel = new PageModel(serviceLayer);
        }   

        public ActionResult List(int id)
        {
            _pageModel.CatalogModel.Ccategories = _ServiceLayer.Get<ICCategoryService>()._Repository.GetSortList(e => e.PARENT_ID == id).ToList();
            _pageModel.CatalogModel.Rstocks = _ServiceLayer.Get<IRStockService>()._Repository.GetSortList(e => e.FK_ID_CATEGORY == id);
            return View(_pageModel);
        }

    }
}
