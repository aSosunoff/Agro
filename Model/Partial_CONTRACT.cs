using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [MetadataType(typeof(contract))]
    public partial class rcontract
    {
    }

    public class contract
    {
        [Display(Name = "Флаг подписания")]
        public int FLAG_SIGNATURE { get; set; }

        [Display(Name = "Статус")]
        public int FLAG_PAYMENT { get; set; }

        [Display(Name = "Дата составления")]
        public System.DateTime DATE { get; set; }

        [Display(Name = "Дата подписания")]
        public System.DateTime DATE_SIGNATURE { get; set; }

        [Display(Name = "Активность")]
        public int IS_ACTIVE { get; set; }

        [Display(Name = "Готовность")]
        public int IS_READY { get; set; }
    }
}
