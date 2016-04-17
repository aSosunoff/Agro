using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class SStreetService : BaseService<ISStreetRepository>, ISStreetService
    {
        public SStreetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}