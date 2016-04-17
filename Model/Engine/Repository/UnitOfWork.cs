using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Entity;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository
{
    class UnitOfWork : Engine, IUnitOfWork
    {
        private readonly agro_mydbEntities _agroMydbEntities = new agro_mydbEntities();

        public UnitOfWork()
        {
            Objects.Add(typeof(ICCategoryRepository), new CCategoryRepository(_agroMydbEntities));
            Objects.Add(typeof(IRBasketRepository), new RBasketRepository(_agroMydbEntities));
            Objects.Add(typeof(IRContractor_infoRepository), new RContractor_infoRepository(_agroMydbEntities));
            Objects.Add(typeof(IRContractRepository), new RContractRepository(_agroMydbEntities));
            Objects.Add(typeof(IRNewsRepository), new RNewsRepository(_agroMydbEntities));
            Objects.Add(typeof(IROrderRepository), new ROrderRepository(_agroMydbEntities));
            Objects.Add(typeof(IRStockRepository), new RStockRepository(_agroMydbEntities));
            Objects.Add(typeof(IRUser_infoRepository), new RUser_infoRepository(_agroMydbEntities));
            Objects.Add(typeof(IRUserRepository), new RUserRepository(_agroMydbEntities));
            Objects.Add(typeof(ISCityRepository), new SCityRepository(_agroMydbEntities));
            Objects.Add(typeof(ISStreetRepository), new SStreetRepository(_agroMydbEntities));
        }
    }
}
