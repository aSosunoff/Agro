using System;
using System.Data;
using System.Linq;
using Components;
using Microsoft.Office.Interop.Word;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using DataTable = System.Data.DataTable;

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

        public void ContractToWord(int id, DocumentClass wordDocument)
        {
            //Достаём договор
            rcontract itemContract = _Repository.GetItem(e => e.PK_ID == id);

            itemContract.ReplaseWordStub(
                e =>
                    new
                    {
                        ID = e.PK_ID,
                        DATE_REG = e.DATE.ToString("D")
                    }, wordDocument);

            //Достайм поставщика по контракту
            RootServiceLayer.Get<IRContractor_infoService>()
                ._Repository.GetItem(e => e.PK_ID == itemContract.FK_ID_CONTRACT_CONTRACTOR)
                .ReplaseWordStub(
                    e =>
                        new
                        {
                            NAME_COMPANY_CITY = e.CITY_NAME,
                            NAME_COMPANY_CONTRACTOR = e.NAME_COMPANY,
                            FIO_CONTRACTOR = String.Format("{0} {1} {2}", e.SURNAME, e.NAME, e.MIDDLE_NAME),
                            LEGAL_ADDRESS_CONTRACTOR = e.LEGAL_ADDRESS,
                            MAIL_ADDRESS_CONTRACTOR = e.MAIL_ADDRESS,
                            PHONE_CONTRACTOR = e.PHONE,
                            FAX_CONTRACTOR = e.FAX,
                            INN_CONTRACTOR = Convert.ToString(e.INN),
                            CHECKING_ACCOUNT_CONTRACTOR = Convert.ToString(e.CHECKING_ACCOUNT),
                            BANK_CONTRACTOR = e.BANK,
                            CORRESPONDENT_ACCOUNT_CONTRACTOR = Convert.ToString(e.CORRESPONDENT_ACCOUNT),
                            BIK_CONTRACTOR = Convert.ToString(e.BIK),
                            KPP_CONTRACTOR = Convert.ToString(e.KPP)

                        }, wordDocument);


            //Достайм заказчика по контракту
            RootServiceLayer.Get<IRUser_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == itemContract.FK_ID_CONTRACT_USER)
                .ReplaseWordStub(
                    e =>
                        new
                        {
                            NAME_COMPANY_USER = e.NAME_COMPANY,
                            FIO_USER = String.Format("{0} {1} {2}", e.SURNAME, e.NAME, e.MIDDLE_NAME),
                            LEGAL_ADDRESS_USER = e.LEGAL_ADDRESS,
                            MAIL_ADDRESS_USER = e.MAIL_ADDRESS,
                            PHONE_USER = e.PHONE,
                            FAX_USER = e.FAX,
                            INN_USER = Convert.ToString(e.INN),
                            KPP_USER = Convert.ToString(e.KPP),
                            CHECKING_ACCOUNT_USER = Convert.ToString(e.CHECKING_ACCOUNT),
                            BANK_USER = e.BANK,
                            CORRESPONDENT_ACCOUNT_USER = Convert.ToString(e.CORRESPONDENT_ACCOUNT),
                            BIK_USER = Convert.ToString(e.BIK)
                        }, wordDocument);
        }

        public DataSet PDF(int id)
        {
            rcontract contract = _Repository.GetItem(e => e.PK_ID == id);

            DataTable dtContract = contract.ToDataTable("CONTRACT",
                e =>
                    new
                    {
                        NUMBER_CONTRACT = contract.PK_ID,
                        contract.DATE
                    });


            DataTable dtContractorInfo = RootServiceLayer.Get<IRContractor_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == contract.FK_ID_CONTRACT_CONTRACTOR)
                .ToDataTable("CONTRACTOR_INFO",
                e =>
                    new
                    {
                        e.NAME_COMPANY,
                        e.CITY_NAME,
                        e.LEGAL_ADDRESS,
                        e.PHONE
                    });


            DataTable dtUserInfo = RootServiceLayer.Get<IRUser_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == contract.FK_ID_CONTRACT_USER)
                .ToDataTable("USER_INFO",
                e =>
                    new
                    {
                        e.NAME_COMPANY,
                        e.CITY_NAME,
                        e.LEGAL_ADDRESS,
                        e.PHONE,
                        e.CHECKING_ACCOUNT
                    });


            //TODO: NUMBER_LABLE Номер упаковочного ярлыка задан рамдомно
            //TODO: BRUTTO брутто задано рамдомно
            //TODO: TARA Тара задано рамдомно

            Random random = new Random();

            //TODO: Код продукции (номенкла-турный номер) рандомное число -> изменить в дальнейшем
            //TODO: Масса, т выведено рандомное число исправить в дальнейшем
            DataTable dtOrder = RootServiceLayer.Get<IROrderService>()
                ._Repository
                .GetSortList(e => e.FK_ID_CONTRACT == id)
                .ToList()
                .ToDataTable("ORDER",
                e =>
                    new
                    {
                        e.PK_ID,
                        NUMBER_PRODUCT = random.Next(0, 100),
                        MASSA = random.Next(0, 100),
                        e.QANTITY,
                        NAME_PRODUCT = e.rstock.NAME,
                        e.rstock.PRICE_ONE,
                        PRICE_SUM = (e.QANTITY * e.rstock.PRICE_ONE),
                        STEP = e.rstock.STEP_GetValue,
                        NUMBER_LABLE = random.Next(0, 100),
                        BRUTTO = random.Next(0, 100),
                        TARA = random.Next(0, 100)
                    });

            DataSet ds = new DataSet("N");
            ds.Tables.Add(dtContract);
            ds.Tables.Add(dtContractorInfo);
            ds.Tables.Add(dtUserInfo);
            ds.Tables.Add(dtOrder);

            return ds;
        }
    }
}