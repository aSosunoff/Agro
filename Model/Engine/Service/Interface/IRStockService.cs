using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IRStockService : IBaseService<IRStockRepository>
    {
        void Create(rstock item);
        rstock GetItemToId(decimal id);
        void AddedProductToBasket(rstock productToBasket);
    }
}