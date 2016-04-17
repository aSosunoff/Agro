using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class RStockRepository : CRUDRepository<rstock, agro_mydbEntities>, IRStockRepository
    {
        public RStockRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}