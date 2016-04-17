using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class RUser_infoRepository : CRUDRepository<ruser_info, agro_mydbEntities>, IRUser_infoRepository
    {
        public RUser_infoRepository(agro_mydbEntities entities) : base(entities)
        {
        }
    }
}