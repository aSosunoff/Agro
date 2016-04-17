using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class RContractRepository : CRUDRepository<rcontract, agro_mydbEntities>, IRContractRepository
    {
        public RContractRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}