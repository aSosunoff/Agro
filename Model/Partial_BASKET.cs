using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [MetadataType(typeof(basket))]
    public partial class rbasket
    {
    }

    public class basket
    {
        [Display(Name = "Колличество в корзине")]
        public int QANTITY { get; set; }
        [Display(Name = "Дата добавления в корзину")]
        public System.DateTime DATE_ADD_PRODUCT { get; set; }
    }
}
