﻿using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface ICCategoryService : IBaseService<ICCategoryRepository>
    {
        void Create(ccategory item);
    }
}