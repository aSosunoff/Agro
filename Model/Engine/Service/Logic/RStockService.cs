using System;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RStockService : BaseService<IRStockRepository>, IRStockService
    {
        public RStockService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Create(rstock item)
        {
            //вновь созданный товар всегда активный
            item.IS_ACTIVE = 1;
            //фмксируем дату добавления
            item.DATE_ADDED = DateTime.Now;
            //TODO: зафиксировать кто добавил
            _Repository.Create(item);
        }
    }
}