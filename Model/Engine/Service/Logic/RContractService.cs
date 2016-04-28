using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using AgroFirma.Component;
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

        

        private Contract_Print_Model getContractPrintModel(int id)
        {
            //Достаём договор
            rcontract itemContract = _Repository.GetItem(e => e.PK_ID == id);

            //Достайм поставщика по контракту
            rcontractor_info itemContractor =
                RootServiceLayer.Get<IRContractor_infoService>()
                    ._Repository.GetItem(e => e.PK_ID == itemContract.FK_ID_CONTRACT_CONTRACTOR);

            //Достайм заказчика по контракту
            ruser_info itemUser =
                RootServiceLayer.Get<IRUser_infoService>()
                    ._Repository.GetItem(e => e.PK_ID == itemContract.FK_ID_CONTRACT_USER);

            //Создаём модель для печати договора и заполняем
            return new Contract_Print_Model()
            {
                ID = Convert.ToString(itemContract.PK_ID),
                NAME_COMPANY_CITY = itemContractor.CITY_NAME,
                DATE_REG = itemContract.DATE.ToString("D"),
                NAME_COMPANY_CONTRACTOR = itemContractor.NAME_COMPANY,
                FIO_CONTRACTOR = String.Format("{0} {1} {2}", itemContractor.SURNAME, itemContractor.NAME, itemContractor.MIDDLE_NAME),
                NAME_COMPANY_USER = itemUser.NAME_COMPANY,
                FIO_USER = String.Format("{0} {1} {2}", itemUser.SURNAME, itemUser.NAME, itemUser.MIDDLE_NAME),
                LEGAL_ADDRESS_CONTRACTOR = itemContractor.LEGAL_ADDRESS,
                LEGAL_ADDRESS_USER = itemUser.LEGAL_ADDRESS,
                MAIL_ADDRESS_CONTRACTOR = itemContractor.MAIL_ADDRESS,
                MAIL_ADDRESS_USER = itemUser.MAIL_ADDRESS,
                PHONE_CONTRACTOR = itemContractor.PHONE,
                FAX_CONTRACTOR = itemContractor.FAX,
                PHONE_USER = itemUser.PHONE,
                FAX_USER = itemUser.FAX,
                INN_CONTRACTOR = Convert.ToString(itemContractor.INN),
                INN_USER = Convert.ToString(itemUser.INN),
                KPP_CONTRACTOR = Convert.ToString(itemContractor.KPP),
                KPP_USER = Convert.ToString(itemUser.KPP),
                CHECKING_ACCOUNT_CONTRACTOR = Convert.ToString(itemContractor.CHECKING_ACCOUNT),
                CHECKING_ACCOUNT_USER = Convert.ToString(itemUser.CHECKING_ACCOUNT),
                BANK_CONTRACTOR = itemContractor.BANK,
                BANK_USER = itemUser.BANK,
                CORRESPONDENT_ACCOUNT_CONTRACTOR = Convert.ToString(itemContractor.CORRESPONDENT_ACCOUNT),
                CORRESPONDENT_ACCOUNT_USER = Convert.ToString(itemUser.CORRESPONDENT_ACCOUNT),
                BIK_CONTRACTOR = Convert.ToString(itemContractor.BIK),
                BIK_USER = Convert.ToString(itemUser.BIK)
            };
        }

        private void ReplaseWordStub(string stubToReplace, string text, DocumentClass wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private class Val
        {
            public string Key { get; set; }

            public string Value{ get; set; }
        }
        public string PrintDog(int id, string fileName)
        {

            Contract_Print_Model contractPrintModel = getContractPrintModel(id);    

            string filePathTemplate = Path.Combine(
                HttpContext.Current.Server.MapPath(
                    String.Format("\\Template\\{0}", fileName)));

            var wordApp = new ApplicationClass();

            wordApp.Visible = true;

            var wordDocument = (DocumentClass)wordApp.Documents.Open(filePathTemplate);

            ReplaseWordStub("{ID}", contractPrintModel.ID, wordDocument);
            ReplaseWordStub("{NAME_COMPANY_CITY}", contractPrintModel.NAME_COMPANY_CITY, wordDocument);
            ReplaseWordStub("{DATE_REG}", contractPrintModel.DATE_REG, wordDocument);
            ReplaseWordStub("{NAME_COMPANY_CONTRACTOR}", contractPrintModel.NAME_COMPANY_CONTRACTOR, wordDocument);
            ReplaseWordStub("{FIO_CONTRACTOR}", contractPrintModel.FIO_CONTRACTOR, wordDocument);
            ReplaseWordStub("{NAME_COMPANY_USER}", contractPrintModel.NAME_COMPANY_USER, wordDocument);
            ReplaseWordStub("{FIO_USER}", contractPrintModel.FIO_USER, wordDocument);
            ReplaseWordStub("{LEGAL_ADDRESS_CONTRACTOR}", contractPrintModel.LEGAL_ADDRESS_CONTRACTOR, wordDocument);
            ReplaseWordStub("{LEGAL_ADDRESS_USER}", contractPrintModel.LEGAL_ADDRESS_USER, wordDocument);
            ReplaseWordStub("{MAIL_ADDRESS_CONTRACTOR}", contractPrintModel.MAIL_ADDRESS_CONTRACTOR, wordDocument);
            ReplaseWordStub("{MAIL_ADDRESS_USER}", contractPrintModel.MAIL_ADDRESS_USER, wordDocument);
            ReplaseWordStub("{PHONE_CONTRACTOR}", contractPrintModel.PHONE_CONTRACTOR, wordDocument);
            ReplaseWordStub("{FAX_CONTRACTOR}", contractPrintModel.FAX_CONTRACTOR, wordDocument);
            ReplaseWordStub("{PHONE_USER}", contractPrintModel.PHONE_USER, wordDocument);
            ReplaseWordStub("{FAX_USER}", contractPrintModel.FAX_USER, wordDocument);
            ReplaseWordStub("{INN_CONTRACTOR}", contractPrintModel.INN_CONTRACTOR, wordDocument);
            ReplaseWordStub("{INN_USER}", contractPrintModel.INN_USER, wordDocument);
            ReplaseWordStub("{KPP_CONTRACTOR}", contractPrintModel.KPP_CONTRACTOR, wordDocument);
            ReplaseWordStub("{KPP_USER}", contractPrintModel.KPP_USER, wordDocument);
            ReplaseWordStub("{CHECKING_ACCOUNT_CONTRACTOR}", contractPrintModel.CHECKING_ACCOUNT_CONTRACTOR, wordDocument);
            ReplaseWordStub("{CHECKING_ACCOUNT_USER}", contractPrintModel.CHECKING_ACCOUNT_USER, wordDocument);
            ReplaseWordStub("{BANK_CONTRACTOR}", contractPrintModel.BANK_CONTRACTOR, wordDocument);
            ReplaseWordStub("{BANK_USER}", contractPrintModel.BANK_USER, wordDocument);
            ReplaseWordStub("{CORRESPONDENT_ACCOUNT_CONTRACTOR}", contractPrintModel.CORRESPONDENT_ACCOUNT_CONTRACTOR, wordDocument);
            ReplaseWordStub("{CORRESPONDENT_ACCOUNT_USER}", contractPrintModel.CORRESPONDENT_ACCOUNT_USER, wordDocument);
            ReplaseWordStub("{BIK_CONTRACTOR}", contractPrintModel.BIK_CONTRACTOR, wordDocument);
            ReplaseWordStub("{BIK_USER}", contractPrintModel.BIK_USER, wordDocument);
            ReplaseWordStub("{ID}", contractPrintModel.ID, wordDocument);


            string filePathTemplateSuccess = Path.Combine(
                HttpContext.Current.Server.MapPath(
                    String.Format("\\Template\\Success\\{0}", fileName)));

            wordDocument.SaveAs(filePathTemplateSuccess);

            wordDocument.Close();

            wordApp.Quit();

            return filePathTemplateSuccess;

            /*
                         AGRO_PRODUCT item = _serviceLayer.Get<IProductService>()._Repository.GetItem(e => e.ID == 1);

            List<string> namecolumns = new List<string>();

            foreach (PropertyInfo element in item.GetType().GetProperties())
            {
                namecolumns.Add(String.Format("{-{0}-}",element.Name));
            }





            

            

            



            return File(filePath, "application/doc", "tmpGood.doc");
             */
        }

        public DataSet GetSpecification(int id)
        {
            


            return null;
        }
    }
}