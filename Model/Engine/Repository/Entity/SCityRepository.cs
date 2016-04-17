using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class SCityRepository : CRUDRepository<scity, agro_mydbEntities>, ISCityRepository
    {
        public SCityRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}