using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model
{
    [MetadataType(typeof(stock))]
    public partial class rstock
    {
        public IEnumerable<SelectListItem> STEP_OBJECT
        {
            get
            {
                return new[]
                    {
                        new SelectListItem()
                        {
                            Value = "0",
                            Text = "кг"
                        },
                            new SelectListItem()
                        {
                            Value = "1",
                            Text = "шт"
                        }
                    };
            }
        }
    }

    public class stock
    {
        [Required(ErrorMessage = "Заполните наименование")]
        [Display(Name = "Наименование продукта")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "Заполните количество")]
        [Display(Name = "Количество")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Только числа больше 0")]
        public int QANTITY { get; set; }

        [Required(ErrorMessage = "Выбирете единицу")]
        [Display(Name = "Единица продукта")]
        public int STEP { get; set; }

        [Required(ErrorMessage = "Заполните стоимость")]
        [Display(Name = "Стоимость")]
        public decimal PRICE_ONE { get; set; }

        [Required(ErrorMessage = "Заполните информацию о продукте")]
        [Display(Name = "Информация")]
        public string INFO { get; set; }

        [Required(ErrorMessage = "Загрузите фотографию")]
        [Display(Name = "Изображение")]
        public string IMAGE_PATH { get; set; }

        [Required(ErrorMessage = "Загрузите фотографию")]
        [Display(Name = "Категория")]
        public int FK_ID_CATEGORY { get; set; }
    }
}
