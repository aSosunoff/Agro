using System.Linq;
using Components;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class CCategoryService : BaseService<ICCategoryRepository>, ICCategoryService
    {
        public CCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork){}
        public void Create(ccategory item)
        {
            //Вновь созданную категорию делаем активной
            item.IS_ACTIVE = 1;

            _Repository.Create(item);
        }

        public void VirtualDeleteTreeDown(int id)
        {
            VirtualDR(id, 0);
        }

        public void VirtualRecoverTreeDown(int id)
        {
            VirtualDR(id, 1);
        }

        private void VirtualDR(int id, int activFlag)
        {
            //находим все дочерние элементы и ставим флаг
            var category = _Repository
                .GetAllList()
                .ConnectByPrior(
                e =>
                    new
                    {
                        e.PK_ID,
                        e.PARENT_ID,
                        ROOT = id
                    });

            foreach (var element in category)
            {
                element.ITEM.IS_ACTIVE = activFlag;
                _Repository.Update(element.ITEM);
            }

            //Находим родителя и тоже ставим
            var parentElement = _Repository.GetItem(e => e.PK_ID == id);
            parentElement.IS_ACTIVE = activFlag;

            _Repository.Update(parentElement);
        }
    }
}