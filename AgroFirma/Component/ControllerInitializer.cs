using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Engine.Service;

namespace AgroFirma.Component
{
    public class ControllerInitializer : Controller
    {
        public IServiceLayer _serviceLayer { get; set; }
        public ViewBagMain ViewBagMain { get; set; }

        public ControllerInitializer(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            ViewBagMain = new ViewBagMain(_serviceLayer);

            ViewBag.CountElementToBasket = ViewBagMain.CountElementToBasket;
            ViewBag.CountElementToContract = ViewBagMain.CountElementToContract;
            ViewBag.WrapModels = ViewBagMain.WrapModels;
            ViewBag.NewsList = ViewBagMain.NewsList;

        }
    }
}