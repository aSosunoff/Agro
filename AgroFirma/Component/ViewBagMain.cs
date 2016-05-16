using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Components;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace AgroFirma.Component
{
    public class ViewBagMain
    {
        public static class MessageError
        {
            private static bool flag = false;
            private static string message = null;

            public static void Init(string mess)
            {
                flag = true;
                message = mess;
            }

            public static string Look()
            {
                if (flag)
                {
                    string mess = message;
                    Reset();
                    return mess;
                }
                else
                    return null;
            }

            private static void Reset()
            {
                flag = false;
                message = null;
            }

        };

        public static class MessageSuccess
        {
            private static bool flag = false;
            private static string message = null;

            public static void Init(string mess)
            {
                flag = true;
                message = mess;
            }

            public static string Look()
            {
                if (flag)
                {
                    string mess = message;
                    Reset();
                    return mess;
                }
                else
                    return null;
            }

            private static void Reset()
            {
                flag = false;
                message = null;
            }

        };


        public IServiceLayer _serviceLayer { get; set; }
        public ViewBagMain(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            CountElementToBasket = _serviceLayer.Get<IRBasketService>().Count();
            CountElementToContract = _serviceLayer.Get<IRContractService>().Count();

            var wrapModelList = _serviceLayer
                .Get<ICCategoryService>()
                ._Repository
                .GetAllList()
                .ConnectByPrior(
                e => 
                    new
                        {
                            e.PK_ID, 
                            e.PARENT_ID, 
                            ROOT = 0
                        });
            WrapModels = wrapModelList.Count > 0 ? wrapModelList : null;

            //TODO: Предусмотреть настройку в админке сколько новостей выводить сейчас 3 Take(3)
            //TODO: Предусмотреть настройку в админке в какой последовательности выводить сейчас последние добавленные по дате OrderByDescending(e => e.DATE)
            var newsList = _serviceLayer.Get<IRNewsService>()._Repository.GetSortList(e => e.IS_ACTIVE == 1).Take(3);
            NewsList = newsList.Count() > 0 ? newsList.OrderByDescending(e => e.DATE) : null;


        }

        public IEnumerable<rnews> NewsList{ get; set; }

        public List<WrapModel<ccategory>> WrapModels { get; set; }

        /// <summary>
        /// Колличество продуктов в корзине
        /// </summary>
        public int CountElementToBasket { get; set; }
        /// <summary>
        /// Колличество оформленных заказов
        /// </summary>
        public int CountElementToContract { get; set; }
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
        private bool IsNullQantityProduct(out string errorMessage)
        {
            bool errorFlag = false;
            errorMessage = String.Empty;

            foreach (var element in _serviceLayer.Get<IRBasketService>()._Repository.GetAllList())
            {
                if (_serviceLayer.Get<IRStockService>().GetItemToId(element.PK_ID).QANTITY < element.QANTITY)
                {
                    errorFlag = true;
                    errorMessage += String.Format("Продукта \"{0}\" нет на складе\n", element.rstock.NAME);
                }

            }
            return errorFlag;
        }
    }
}