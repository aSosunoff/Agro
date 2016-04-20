using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Repository;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;
using Model.Infrastructure.JazzClass;

namespace AgroFirma.Component.Helpers
{
    public static class CatalogHelper
    {
        private static IServiceLayer _ServiceLayer { get; set; }
        static CatalogHelper()
        {
            _ServiceLayer = new ServiceLayer(new UnitOfWork());
        }

        private static string SubCatalog(string actionName, string controllerName, string resLine = "", int id = 0)
        {
            //todo: попробовать поменять на хранимую процедуру, что бы она возвращала
            var items = _ServiceLayer.Get<ICCategoryService>()._Repository.GetSortList(e => e.PARENT_ID == id).ToList();
            for (int i = 0; i < items.Count(); i++)
            {
                TagBuilder li = new TagBuilder("li");

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", Path.Combine(String.Format("/{0}/{1}/?id={2}", controllerName, actionName, items[i].PK_ID)));
                a.SetInnerText(items[i].TEXT);

                var iID = items[i].PK_ID;

                if (_ServiceLayer.Get<ICCategoryService>()._Repository.GetSortList(e => e.PARENT_ID == iID).ToList().Count != 0)
                {
                    a.AddCssClass("downSubItem");
                    a.InnerHtml += "<span class=\"caret\"></span>";
                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("dropdown-menu");
                    ul.InnerHtml += SubCatalog(actionName, controllerName, ul.InnerHtml, items[i].PK_ID);
                    li.InnerHtml += a.ToString();
                    li.InnerHtml += ul.ToString();
                }
                else
                {

                    li.InnerHtml += a.ToString();
                }
                resLine += li.ToString();
            }
            return resLine;
        }

        public static MvcHtmlString DisplayCatalogList(this HtmlHelper helper, string actionName, string controllerName, int idParent = 0)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-pills nav-stacked");

            ul.InnerHtml += SubCatalog(actionName, controllerName);

            return new MvcHtmlString(ul.ToString() == String.Empty ? "": ul.ToString());
        }


        public static MvcHtmlString DisplayCatalogListXXXXX(this HtmlHelper helper, string actionName, string controllerName)
        {
            ConnectByPriorInModel model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "PK_ID",
                    ColummValue = 0
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "PK_ID",
                    Right = "PARENT_ID"
                }
            };

            string li = _ServiceLayer.Get<ICCategoryService>()
                ._Repository.GetAllList()
                .ConnectByPriorAllElement(model)
                .Where(e => e.ITEM.IS_ACTIVE == 1)
                .DisplayCatalogListLoop(0);
                

            //TagBuilder ul = new TagBuilder("ul");
            //ul.AddCssClass("nav nav-pills nav-stacked");

            //ul.InnerHtml += SubCatalog(actionName, controllerName);

            return new MvcHtmlString(li);
        }

        private static string DisplayCatalogListLoop<T>(this IEnumerable<WrapModel<T>> wrapModels, int index)
        {
            var listHeahItem = wrapModels.Where(e => (int)e.ITEM.GetType().GetProperty("PARENT_ID").GetValue(e.ITEM, null) == index).ToList();

            foreach (var eelement in listHeahItem)
            {
                
            }

            return null;
        }
    }
}