using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RStockService : BaseService<IRStockRepository>, IRStockService
    {
        public RStockService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}