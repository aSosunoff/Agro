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
        public string IMAGE_FILE_PATH_SERVER
        {//Добавлено поле для копирования пути загруженного ранее изображения. Необходимо в том случае если пользователь при изменении элемента не меняет изображение. 
            get
            {
                return IMAGE_PATH;    
            }
            private set { }
        }
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

        [Display(Name = "Дата добавления")]
        public System.DateTime DATE { get; set; }

        [Display(Name = "Статус")]
        public int IS_ACTIVE { get; set; }
    }
}
