using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RNewsService : BaseService<IRNewsRepository>, IRNewsService
    {
        public RNewsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}