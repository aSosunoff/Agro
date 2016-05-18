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
                    //(string)bindingContext.ValueProvider.GetValue("IMAGE_FILE_PATH_SERVER").ConvertTo(typeof(string))
                    //TODO: добавить усливие если пользователь не выбрал фото, то загрузить фото загруженное ранее. Для этого в Partial модели предусмотрено поле в котором скопировано поле ранее загруженного изображения
                    value = FileOfWork.GetPath(String.Format("\\tmp\\{0}\\", controllerContext.RouteData.Values["controller"]));

                    break;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}