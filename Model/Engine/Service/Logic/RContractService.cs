using System;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class RContractService : BaseService<IRContractRepository>, IRContractService
    {
        public RContractService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

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
    }
}