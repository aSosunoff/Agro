namespace Model.Engine.Service.Interface
{
    public interface IBaseService<TRepository> where TRepository : class
    {
        TRepository _Repository { get; set; }
        void SetRootService(IServiceLayer serviceLayer);
    }
}