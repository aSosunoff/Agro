﻿using System;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RNewsService : BaseService<IRNewsRepository>, IRNewsService
    {
        public RNewsService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
     
        public void Create(rnews item)
        {
            //вновь созданный товар всегда активный
            item.IS_ACTIVE = 1;
            //фмксируем дату добавления
            item.DATE = DateTime.Now;
            //TODO: зафиксировать кто добавил
            _Repository.Create(item);
        }

        public void Update(rnews item)
        {
            //фмксируем дату добавления
            item.DATE = DateTime.Now;
            //TODO: зафиксировать кто добавил
            _Repository.Update(item);
        }

        public void DeleteVirtual(int id)
        {
            rnews item = _Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 1);

            if (item != null)
            {
                item.IS_ACTIVE = 0;

                Update(item);
            }
        }

        public void RecoverVirtual(int id)
        {
            rnews item = _Repository.GetItem(e => e.PK_ID == id && e.IS_ACTIVE == 0);

            if (item != null)
            {
                item.IS_ACTIVE = 1;

                Update(item);
            }
        }
    }
}