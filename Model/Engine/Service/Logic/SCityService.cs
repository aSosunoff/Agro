using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class SCityService : BaseService<ISCityRepository>, ISCityService
    {
        public SCityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}