using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Engine.Service.Logic;

namespace Model.Engine.Service
{
    class ServiceLayer : Engine, IServiceLayer
    {
        public static IServiceLayer Instance(IServiceLayer serviceLayer)
        {
            serviceLayer.Get<ICCategoryService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRBasketService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRContractor_infoService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRContractService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRNewsService>().SetRootService(serviceLayer);
            serviceLayer.Get<IROrderService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRStockService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRUser_infoService>().SetRootService(serviceLayer);
            serviceLayer.Get<IRUserService>().SetRootService(serviceLayer);
            serviceLayer.Get<ISCityService>().SetRootService(serviceLayer);
            serviceLayer.Get<ISStreetService>().SetRootService(serviceLayer);

            return serviceLayer;
        }
        public ServiceLayer(IUnitOfWork unitOfWork)
        {
            Objects.Add(typeof(ICCategoryService), new CCategoryService(unitOfWork));
            Objects.Add(typeof(IRBasketService), new RBasketService(unitOfWork));
            Objects.Add(typeof(IRContractor_infoService), new RContractor_infoService(unitOfWork));
            Objects.Add(typeof(IRContractService), new RContractService(unitOfWork));
            Objects.Add(typeof(IRNewsService), new RNewsService(unitOfWork));
            Objects.Add(typeof(IROrderService), new ROrderService(unitOfWork));
            Objects.Add(typeof(IRStockService), new RStockService(unitOfWork));
            Objects.Add(typeof(IRUser_infoService), new RUser_infoService(unitOfWork));
            Objects.Add(typeof(IRUserService), new RUserService(unitOfWork));
            Objects.Add(typeof(ISCityService), new SCityService(unitOfWork));
            Objects.Add(typeof(ISStreetService), new SStreetService(unitOfWork));
        }
    }
}
