using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class RNewsRepository : CRUDRepository<rnews, agro_mydbEntities>, IRNewsRepository
    {
        public RNewsRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}
