using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class RUserRepository : CRUDRepository<ruser, agro_mydbEntities>, IRUserRepository
    {
        public RUserRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}