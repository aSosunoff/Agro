using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RUser_infoService : BaseService<IRUser_infoRepository>, IRUser_infoService
    {
        public RUser_infoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}