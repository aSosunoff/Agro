using System;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RContractService : BaseService<IRContractRepository>, IRContractService
    {
        public RContractService(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public int Count()
        {
            return _Repository.GetAllList().Count();
        }

        public void Create(rcontract item)
        {
            //TODO: Сделать логику прекрипление директора компании. В данный момент прикрепляется тестовый пользователь директор
            ruser user = RootServiceLayer.Get<IRUserService>()._Repository.GetItem(e => e.PK_ID == 2);//TODO: 2 индекс тестового ползователя директора

            //Оставляем данные пользователя директора компании на тот момент когда осуществлялся заказ. На тот случий если данные пользователя изменяться
            //но прикрепим его ID что бы следить за информацией по этому пользователю
            rcontractor_info userContractor = new rcontractor_info()
            {
                FK_USER = user.PK_ID,
                NAME = user.NAME,
                SURNAME = user.SURNAME,
                MIDDLE_NAME = user.MIDDLE_NAME,
                NAME_COMPANY = user.NAME_COMPANY,
                LEGAL_ADDRESS = user.LEGAL_ADDRESS,
                MAIL_ADDRESS = user.MAIL_ADDRESS,
                PHONE = user.PHONE,
                FAX = user.FAX,
                INN = user.INN,
                KPP = user.KPP,
                CHECKING_ACCOUNT = user.CHECKING_ACCOUNT,
                BANK = user.BANK,
                CORRESPONDENT_ACCOUNT = user.CORRESPONDENT_ACCOUNT,
                BIK = user.BIK,
                CITY_NAME = user.sstreet.scity.NAME     
            };

            RootServiceLayer.Get<IRContractor_infoService>()._Repository.Create(userContractor);


            item.FK_ID_CONTRACT_CONTRACTOR = userContractor.PK_ID;

            item.DATE = DateTime.Now;
            item.FLAG_SIGNATURE = 0;
            item.FLAG_PAYMENT = 0;
            item.IS_ACTIVE = 1;
            item.IS_READY = 0;

            _Repository.Create(item);
        }

        public void Pay(int id)
        {
            rcontract item = _Repository.GetItem(e => e.PK_ID == id);

            item.FLAG_PAYMENT = 1;

            _Repository.Update(item);
        }

        public void RefuseContract(int id)
        {//TODO: Привязаться к пользователю кто вызвал метод
            //Находим контракт который пользователь хочет вернуть
            rcontract item = _Repository.GetItem(e => e.PK_ID == id);
            if (item != null)
            {
                foreach (var element in RootServiceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == item.PK_ID).ToList())
                {
                    rstock stockItem = RootServiceLayer.Get<IRStockService>()._Repository.GetItem(e => e.PK_ID == element.FK_ID_STOCK);

                    stockItem.QANTITY += element.QANTITY;

                    RootServiceLayer.Get<IROrderService>()._Repository.Delete(element);
                }

                ruser_info userInfo = RootServiceLayer.Get<IRUser_infoService>()._Repository.GetItem(e => e.PK_ID == item.FK_ID_CONTRACT_USER);

                rcontractor_info contractorInfo = RootServiceLayer.Get<IRContractor_infoService>()._Repository.GetItem(e => e.PK_ID == item.FK_ID_CONTRACT_CONTRACTOR);

                _Repository.Delete(item);

                RootServiceLayer.Get<IRUser_infoService>()._Repository.Delete(userInfo);

                RootServiceLayer.Get<IRContractor_infoService>()._Repository.Delete(contractorInfo);
            }
            else 
                throw new Exception("Такого договора нет");
            
        }
    }
}