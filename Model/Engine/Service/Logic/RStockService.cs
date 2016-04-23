using System;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RStockService : BaseService<IRStockRepository>, IRStockService
    {
        public RStockService(IUnitOfWork unitOfWork) : base(unitOfWork){}


        public void AddedProductToBasket(rstock rstock)
        {
            //количество которое пользователь хочет положить в корзину
            int qantity = rstock.QANTITY;

            //проверяем на ноль выбранных товаров
            if (qantity == 0)
                throw new Exception("Необходимо выбрать хотябы одну единицу товара");

            //id товара которое кладут в корзину
            var idProductToBasket = rstock.PK_ID;

            rstock agroProduct = _Repository.GetItem(e => e.PK_ID == idProductToBasket);

            //проверяем какое колличество добавил пользователь в корзину. Если оно привышает колличество доступное на складе, то выводим сообщение
            if (qantity > agroProduct.QANTITY)
                throw new Exception("Нельзя добавить такое колличество товара");

            //проверяем есть ли такой товар в корзине
            //для этого проверяем его по ID товара пришедшего в объекте корзина
            var prod = RootServiceLayer.Get<IRBasketService>()._Repository.GetItem(x => x.FK_ID_STOCK == idProductToBasket);

            //проверяем есть ли такой товар в корзине
            if (prod == null)
            {
                rbasket rbasket = new rbasket()
                {
                    DATE_ADD_PRODUCT = DateTime.Now,
                    QANTITY = qantity,
                    FK_ID_STOCK = idProductToBasket,
                    FK_ID_USER = 1
                    //TODO: пока пользователей нет поэтому записывае все добавленные товара на 1 тестового пользователя
                };
                RootServiceLayer.Get<IRBasketService>()._Repository.Create(rbasket);
            }
            else
            {
                //колличество товаров которое должно оказаться в корзине
                int sumQantity = prod.QANTITY + qantity;

                //товар который доступен на складе


                //проверяем колличество товара которое должно оказаться в корзине после суммирования 
                //с доступным на складе
                if (sumQantity > agroProduct.QANTITY)
                {
                    throw new Exception(String.Format(@"Вы не можете добавить {0} единиц товара ""{1}"" в корзину.
                                                        Так как после добавления колличество товара привысит допустимое, которое имеется на складе.
                                                        Вы можете максимально ещё добавить {2} единиц этого товара",
                                                                                                                    qantity,
                                                                                                                    prod.rstock.NAME,
                                                                                                                    agroProduct.QANTITY - prod.QANTITY));
                }


                prod.DATE_ADD_PRODUCT = DateTime.Now;
                prod.QANTITY = sumQantity;

                RootServiceLayer.Get<IRBasketService>()._Repository.Update(prod);
            }
        }

        public void Create(rstock item)
        {
            //вновь созданный товар всегда активный
            item.IS_ACTIVE = 1;
            //фмксируем дату добавления
            item.DATE_ADDED = DateTime.Now;
            //TODO: зафиксировать кто добавил
            _Repository.Create(item);
        }

        public rstock GetItemToId(decimal id)
        {
            return _Repository.GetItem(e => e.PK_ID == id);
        }
    }
}