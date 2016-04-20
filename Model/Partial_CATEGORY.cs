using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model.Engine.Repository;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace Model
{
    [MetadataType(typeof(category))]
    public partial class ccategory
    {
        private IServiceLayer _ServiceLayer { get; set; }
        public IEnumerable<SelectListItem> PARENT_ID_SelectList
        {
            get
            {
                IServiceLayer serviceLayer = new ServiceLayer(new UnitOfWork());

                _ServiceLayer = ServiceLayer.Instance(serviceLayer);

                ConnectByPriorInModel model = new ConnectByPriorInModel()
                {
                    StartWith = new StartWith()
                    {
                        ColummName = "PK_ID",
                        ColummValue = 0
                    },
                    ConnectByPrior = new ConnectByPrior()
                    {
                        Left = "PK_ID",
                        Right = "PARENT_ID"
                    }
                };


                return _ServiceLayer.Get<ICCategoryService>()
                    ._Repository.GetAllList()
                    .ConnectByPriorAllElement(model)
                    .Where(e => e.ITEM.IS_ACTIVE == 1)
                    .PackSelectListItem("PK_ID", "TEXT")
                    .OrderBy(e => e.Text)
                    .AddedFirstItem("0", "Верхний уровень");
            }
        }
    }

    public class category
    {
        [Required(ErrorMessage = "Выбирете родителя")]
        [Display(Name = "Родитель")]
        public int PARENT_ID { get; set; }

        [Required(ErrorMessage = "Заполните наименование")]
        [Display(Name = "Наименование категории")]
        public string TEXT { get; set; }

        [Required(ErrorMessage = "Выбирете изображение")]
        [Display(Name = "Изображение")]
        public string IMAGE_PATH { get; set; }
    }
}
