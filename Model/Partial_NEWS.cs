using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [MetadataType(typeof(news))]
    public partial class rnews
    {
    }


    public class news
    {
        [Required(ErrorMessage = "Необходимо ввести название новости")]
        [Display(Name = "Наименование")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "Необходимо ввести текст новости")]
        [Display(Name = "Текст")]
        public string TEXT { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать картинку")]
        [Display(Name = "Картинка")]
        public string IMAGE_PATH { get; set; }
    }
}
