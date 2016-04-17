using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class CCategoryService : BaseService<ICCategoryRepository>, ICCategoryService
    {
        public CCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork){}
    }
}