using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class RBasketRepository : CRUDRepository<rbasket, agro_mydbEntities>, IRBasketRepository
    {
        public RBasketRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}
