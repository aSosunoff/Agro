using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.Infrastructure
{
    public class StockModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            switch (propertyDescriptor.Name)
            {
                //case "IMG_FILE_PATH":
                //    //http://stackoverflow.com/questions/2083645/how-can-i-use-a-modelbinder-to-correct-values-that-will-then-be-visible-to-the-u
                //    //Формируем путь до файла и сохраняем

                //    FileOfWork.FileBase = (HttpPostedFileBase)bindingContext.ValueProvider.GetValue(propertyDescriptor.Name).ConvertTo(typeof(HttpPostedFileBase));

                //    value = FileOfWork.GetPath(String.Format("\\tmp\\{0}\\", controllerContext.RouteData.Values["controller"]));

                //    break;
                case "PRICE_ONE":

                    //Проверяем цену на лишние символы
                    string priceValue = (string)bindingContext.ValueProvider.GetValue(propertyDescriptor.Name).ConvertTo(typeof(string));
                    
                    //Проверяем на нули
                    Regex regex = new Regex(@"^0+$");
                    if (regex.IsMatch(priceValue))
                        bindingContext.ModelState.AddModelError(propertyDescriptor.Name, "Стоимость книги может быть только больше 0");
                    
                    
                    decimal price;
                    //todo: Поменять на проверку без использования out параметра
                    if (!(decimal.TryParse(priceValue, out price)) //Прверяем ввели число или нет
                        &&
                        !(String.IsNullOrEmpty(priceValue))) //Исключаем пустую строку. Так как проверку на пустоту осуществляет DefaultModelBinder
                    {
                        bindingContext.ModelState.AddModelError(propertyDescriptor.Name, "Не допустимые символы");
                    }
                    break;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
