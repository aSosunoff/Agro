using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RContractService : BaseService<IRContractRepository>, IRContractService
    {
        public RContractService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}