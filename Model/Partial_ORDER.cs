using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [MetadataType(typeof(order))]
    public partial class rorder
    {
    }

    public class order
    {
        [Display(Name = "Количество")]
        public int QANTITY { get; set; }
    }
}
