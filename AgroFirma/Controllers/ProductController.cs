using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace AgroFirma.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
