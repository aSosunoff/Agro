using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Components;
using Model;

namespace AgroFirma.Helpers
{
    public static class MenuHelper
    {
        public static MvcHtmlString Menu(this HtmlHelper htmlHelper, string actionName, string controllerName, IEnumerable<WrapModel<ccategory>> wrapModels)
        {

            TagBuilder ul = new TagBuilder("ul");

            ul.AddCssClass("nav nav-pills nav-stacked");

            ul.InnerHtml += GetTagLi(wrapModels, wrapModels.Where(e => e.LEVEL == 1), actionName, controllerName);

            return new MvcHtmlString(ul.ToString());

        }

        private static string GetTagLi<T>(IEnumerable<WrapModel<T>> wrapModels, IEnumerable<WrapModel<T>> wrapModelsCopy, string actionName, string controllerName, string resLine = "")
        {

            foreach (var element in wrapModelsCopy)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder a = new TagBuilder("a");

                a.MergeAttribute("href", Path.Combine(String.Format("/{0}/{1}/?id={2}", controllerName, actionName, element.ITEM.GetValueInt("PK_ID"))));

                a.SetInnerText(element.ITEM.GetValueString("TEXT"));

                if (element.FLAG_TREE != true)
                {
                    a.AddCssClass("downSubItem");
                    a.InnerHtml += "<span class=\"caret\"></span>";
                    li.InnerHtml += a.ToString();

                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("dropdown-menu");

                    IEnumerable<WrapModel<T>> models =
                        wrapModels.Where(
                            e =>
                                e.ITEM.GetValueInt("PARENT_ID") == element.ITEM.GetValueInt("PK_ID"));
                    ul.InnerHtml += GetTagLi(wrapModels, models, actionName, controllerName);


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
    }
}