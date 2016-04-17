using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class RContractor_infoRepository : CRUDRepository<rcontractor_info, agro_mydbEntities>, IRContractor_infoRepository
    {
        public RContractor_infoRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}
