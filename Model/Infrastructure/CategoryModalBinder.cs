using System;
using System.Web;
using System.Web.Mvc;
using Components;


namespace Model.Infrastructure
{
    public class CategoryModalBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            switch (propertyDescriptor.Name)
            {
                case "IMAGE_PATH":
                    //http://stackoverflow.com/questions/2083645/how-can-i-use-a-modelbinder-to-correct-values-that-will-then-be-visible-to-the-u
                    //Формируем путь до файла и сохраняем

                    FileOfWork.FileBase = (HttpPostedFileBase)bindingContext.ValueProvider.GetValue(propertyDescriptor.Name).ConvertTo(typeof(HttpPostedFileBase));

                    value = FileOfWork.GetPath(String.Format("\\tmp\\{0}\\", controllerContext.RouteData.Values["controller"]));

                    break;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
