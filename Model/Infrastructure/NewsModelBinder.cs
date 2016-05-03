using System;
using System.Web;
using System.Web.Mvc;
using Components;

namespace Model.Infrastructure
{
    public class NewsModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            switch (propertyDescriptor.Name)
            {
                case "IMAGE_PATH":

                    FileOfWork.FileBase = (HttpPostedFileBase)bindingContext.ValueProvider.GetValue(propertyDescriptor.Name).ConvertTo(typeof(HttpPostedFileBase));

                    value = FileOfWork.GetPath(String.Format("\\tmp\\{0}\\", controllerContext.RouteData.Values["controller"]));

                    break;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}