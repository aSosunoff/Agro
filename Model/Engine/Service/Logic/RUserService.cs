using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RUserService : BaseService<IRUserRepository>, IRUserService
    {
        public RUserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}