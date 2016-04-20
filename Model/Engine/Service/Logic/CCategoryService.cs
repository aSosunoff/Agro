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
    }
}