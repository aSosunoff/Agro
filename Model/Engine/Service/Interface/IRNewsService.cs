using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IRNewsService : IBaseService<IRNewsRepository>
    {
        void Create(rnews item);
        void Update(rnews item);
        void DeleteVirtual(rnews item);
    }
}