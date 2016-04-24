using System;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RBasketService : BaseService<IRBasketRepository>, IRBasketService
    {
        public RBasketService(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public int Count()
        {
            return _Repository.GetAllList().Count();
        }

        private bool IsNullQantityProduct(out string errorMessage)
        {
            bool errorFlag = false;
            errorMessage = String.Empty;

            foreach (var element in _Repository.GetAllList())
            {
                if (RootServiceLayer.Get<IRStockService>().GetItemToId(element.FK_ID_STOCK).QANTITY < element.QANTITY)
                {
                    errorFlag = true;
                    errorMessage += String.Format("Продукта \"{0}\" нет на складе\n", element.rstock.NAME);
                }
                    
            }
            return errorFlag;
        }

        public void OrderToPrepare(ruser_info userCustomer)
        {
            //проверяем есть ли добавленый ранее товар в корзине на складе
            //если нет то выводим предупреждающее сообщение
            ////string errorMessage;
            ////if(IsNullQantityProduct(out errorMessage))
            ////    throw new Exception(errorMessage);
            //создаём контракт который прикрепим к заказу
            //добавляем информацию о клиенте и о поставщике
            //TODO: продумать индификатор поставщика

            //TODO: Необходимо прикрепить к контракту пользователя кто осуществляет заказ. 
            rcontract contract = new rcontract()
            {
                FK_ID_CONTRACT_USER = userCustomer.PK_ID
            };

            RootServiceLayer.Get<IRContractService>().Create(contract);

            //создаём заказ. и перемещаем товары из корзины в заказ
            foreach (var element in _Repository.GetAllList().ToList())
            {

                rorder order = new rorder()
                {
                    FK_ID_STOCK = element.FK_ID_STOCK,
                    FK_ID_CONTRACT = contract.PK_ID,
                    QANTITY = element.QANTITY
                };

                RootServiceLayer.Get<IROrderService>()._Repository.Create(order);

                //вычитаем из склада
                //TODO: предусмотреть проверку на остаток на складе
                //TODO: недопустить что бы на складе вышло колличество в минус
                RootServiceLayer.Get<IRStockService>()
                    .GetItemToId(element.FK_ID_STOCK).QANTITY -= element.QANTITY;

                RootServiceLayer.Get<IRStockService>()
                    ._Repository
                    .Update(RootServiceLayer.Get<IRStockService>().GetItemToId(element.FK_ID_STOCK));

                //удаляем из корзины
                _Repository.Delete(element);
            }
        }
    }
}