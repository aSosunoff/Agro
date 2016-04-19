﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Controllers
{
    public class HomeController : Controller
    {
        private IServiceLayer _ServiceLayer { get; set; }

        private PageModel PageModels { get; set; }

        public HomeController(IServiceLayer serviceLayer)
        {
            _ServiceLayer = ServiceLayer.Instance(serviceLayer);

            PageModels = new PageModel(serviceLayer);
        }   

        public ActionResult Index()
        {
            


            var c = _ServiceLayer.Get<ICCategoryService>()
                ._Repository.GetAllList()
                .ToList()
                .ConnectByPrior("PK_ID", "PARENT_ID", 5);


            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
