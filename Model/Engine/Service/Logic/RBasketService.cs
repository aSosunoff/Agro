using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RBasketService : BaseService<IRBasketRepository>, IRBasketService
    {
        public RBasketService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}