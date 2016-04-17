using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class BaseService<TRepository> : IBaseService<TRepository>
        where TRepository : class
    {
        public TRepository _Repository{ get; set; }


        public IServiceLayer RootServiceLayer { get; set; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            _Repository = unitOfWork.Get<TRepository>();
        }


        public void SetRootService(IServiceLayer serviceLayer)
        {
            RootServiceLayer = serviceLayer;
        }
    }
}