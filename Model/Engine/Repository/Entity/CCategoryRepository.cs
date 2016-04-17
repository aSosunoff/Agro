using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class CCategoryRepository : CRUDRepository<ccategory, agro_mydbEntities>, ICCategoryRepository
    {
        public CCategoryRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}
