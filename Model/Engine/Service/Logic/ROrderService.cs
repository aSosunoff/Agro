using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class ROrderService : BaseService<IROrderRepository>, IROrderService
    {
        public ROrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}