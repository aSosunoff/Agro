using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class ROrderRepository : CRUDRepository<rorder, agro_mydbEntities>, IROrderRepository
    {
        public ROrderRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}
