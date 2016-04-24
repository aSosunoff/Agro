using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IRBasketService : IBaseService<IRBasketRepository>
    {
        int Count();

        void OrderToPrepare(ruser_info userCustomer);
    }
}