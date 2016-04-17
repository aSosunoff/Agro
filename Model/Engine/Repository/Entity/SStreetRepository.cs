using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class SStreetRepository : CRUDRepository<sstreet, agro_mydbEntities>, ISStreetRepository
    {
        public SStreetRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}