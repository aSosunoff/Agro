using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RContractor_infoService : BaseService<IRContractor_infoRepository>, IRContractor_infoService
    {
        public RContractor_infoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}