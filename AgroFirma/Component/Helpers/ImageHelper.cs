using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgroFirma.Component.Helpers
{
    public static class ImageHelper
    {
        //new { src = element.IMAGE_PATH, @class = "img-thumbnail img-responsive center-block", alt = "Responsive image" }
        //element.TEXT, "List", "Catalog", new { id = element.PK_ID }, new { @class = "btn btn-primary btn-xs btn-block", role = "button" }
        //TODO: Поменять id на нормальную реализацию из первой сборки
        public static MvcHtmlString DisplayActionLincImage(this HtmlHelper html, string actionName, string controllerName, int id, object routeValueDictionary)
        {
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", Path.Combine(String.Format("/{0}/{1}/?id={2}", controllerName, actionName, id)));

            IDictionary<string, object> attributesDictionary = routeValueDictionary.GetType()
                .GetProperties()
                .ToDictionary(
                    x => x.Name,
                    x => x.GetValue(routeValueDictionary, null));

            TagBuilder img = new TagBuilder("img");

            img.MergeAttributes(attributesDictionary);

            a.InnerHtml += img.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(a.ToString());
        }


        
        public static MvcHtmlString DisplayImage(this HtmlHelper html, object routeValueDictionary)
        {
            IDictionary<string, object> attributesDictionary = routeValueDictionary.GetType()
                .GetProperties()
                .ToDictionary(
                    x => x.Name,
                    x => x.GetValue(routeValueDictionary, null));

            TagBuilder img = new TagBuilder("img");

            img.MergeAttributes(attributesDictionary);

            return new MvcHtmlString(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}