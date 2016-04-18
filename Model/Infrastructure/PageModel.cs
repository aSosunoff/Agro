using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace Model.Infrastructure
{
    public class CatalogModel
    {
        public IEnumerable<ccategory> Ccategories { get; set; }
        public IEnumerable<rstock> Rstocks { get; set; } 
    }

    public class AdministrationModel
    {
        public rstock Rstock { get; set; }
    }


    public class PageModel
    {
        private IServiceLayer _serviceLayer { get; set; }
        public PageModel(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);
        }

        /// <summary>
        /// Колличество продуктов в корзине
        /// </summary>
        public int CountElementToBasket
        {
            set { }
            get { return _serviceLayer.Get<IRBasketService>().Count(); }
        }
        /// <summary>
        /// Колличество оформленных заказов
        /// </summary>
        public int CountElementToContract
        {
            set { }
            get { return _serviceLayer.Get<IRContractService>().Count(); }
        }
        /// <summary>
        /// Все продукты в системе
        /// </summary>
        public IEnumerable<rstock> Products
        {
            set { }
            get { return _serviceLayer.Get<IRStockService>()._Repository.GetAllList(); }
        }

        /// <summary>
        /// Товары которые лежат в корзине
        /// </summary>
        public IEnumerable<rbasket> ProductsToBascet
        {
            get
            {
                //проверяем есть ли добавленый ранее товар в корзину на складе
                //если нет то выводим предупреждающее сообщение            

                return _serviceLayer.Get<IRBasketService>()._Repository.GetAllList();
            }
            set { }
        }
        public void IsProductsToBascet()
        {
            try
            {
                string errorMessage;
                if (IsNullQantityProduct(out errorMessage))
                    throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Итоговая сумма товаров которые лежат в корзине
        public decimal ResultAllSumToBasket { get { return ProductsToBascet.Sum(x => x.QANTITY * x.rstock.PRICE_ONE); } }

        //Итоговая сумма товаров которые заказаны
        public decimal ResultAllSumToOrder { get { return Orders.Sum(x => x.QANTITY * x.rstock.PRICE_ONE); } }

        public IEnumerable<rcontract> Contracts
        {
            get { return _serviceLayer.Get<IRContractService>()._Repository.GetAllList(); }
            set { }
        }

        public IEnumerable<rorder> Orders { get; set; }

        public rstock Product { get; set; }
        public rbasket Basket { get; set; }
        public rorder Order { get; set; }

        /// <summary>
        /// Поле сообщения об ошибки. Выводиться на главной страницы просмотра
        /// </summary>
        public string ErrorMessage { get; set; }


        public CatalogModel CatalogModel;

        public AdministrationModel AdministrationModel;








        private bool IsNullQantityProduct(out string errorMessage)
        {
            bool errorFlag = false;
            errorMessage = String.Empty;

            foreach (var element in _serviceLayer.Get<IRBasketService>()._Repository.GetAllList())
            {
                if (_serviceLayer.Get<IRStockService>()._Repository.GetItem(element.FK_ID_STOCK).QANTITY < element.QANTITY)
                {
                    errorFlag = true;
                    errorMessage += String.Format("Продукта \"{0}\" нет на складе\n", element.rstock.NAME);
                }

            }
            return errorFlag;
        }
    }
}
