using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using AgroFirma.Models;
using Components;
using FastReport;
using FastReport.Export.Pdf;
using Microsoft.Office.Interop.Word;
using Model;
using Model.Engine.Repository.Interface;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using DataTable = System.Data.DataTable;

namespace AgroFirma.Controllers
{
    public class ContractController : ControllerInitializer
    {
        public ContractController(IServiceLayer serviceLayer) : base(serviceLayer){}

        public ActionResult List()
        {
            ViewBag.SuccessMessage = ViewBagMain.MessageSuccess.Look();

            ViewBag.ErrorMessage = ViewBagMain.MessageError.Look();

            return View(_serviceLayer.Get<IRContractService>()._Repository.GetAllList().ToList());
        }

        public ActionResult Details(int id)
        {
            if (!_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).Any())
            {
                ViewBagMain.MessageError.Init("Таких товаров нет");

                return RedirectToAction("List");
            }
            return View(_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).ToList());
        }


        public ActionResult Pay(int id)
        {//TODO: Переделать на POST
            //TODO: Вставить проверку

            _serviceLayer.Get<IRContractService>().Pay(id);

            ViewBagMain.MessageSuccess.Init("Оплачено");

             return RedirectToAction("List");
        }
        
        //Отказ от заказа
        public ActionResult RefuseContract(int id)
        {
            try
            {
                _serviceLayer.Get<IRContractService>().RefuseContract(id);

                ViewBagMain.MessageError.Init("Контракт расторгнут");
            }
            catch (Exception ex)
            {
                ViewBagMain.MessageError.Init(ex.Message);
                throw;
            }

            return RedirectToAction("List");
        }



        //FOR ADMIN
        ///////////////////////////////////////
        public ActionResult ListAdmin()
        {//TODO: Входить только админам

            return View(_serviceLayer.Get<IRContractService>()._Repository.GetAllList().ToList());
        }

        public ActionResult DetailsAdmin(int id)
        {
            if (!_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).Any())
            {
                ViewBagMain.MessageError.Init("Пока ни кто не заключил контрактов");

                return RedirectToAction("ListAdmin");
            }
            return View(_serviceLayer.Get<IROrderService>()._Repository.GetSortList(e => e.FK_ID_CONTRACT == id).ToList());
        }



















        public FileResult PrintDog(int id)
        {//TODO: Поменять на FastReport. Или пофиксить метод в движке
            string fileName = "Dogovor_Postavki_Tovara.doc";

            string filePathTemplate = Path.Combine(
                HttpContext.Server.MapPath(
                    String.Format("\\Template\\{0}", fileName)));

            string filePath = PrintDog(id, fileName, filePathTemplate);

            return File(filePath, "application/doc", fileName);
        }

        private string PrintDog(int id, string fileName, string filePathTemplate)
        {        
            var wordApp = new ApplicationClass();

            wordApp.Visible = true;

            var wordDocument = (DocumentClass)wordApp.Documents.Open(filePathTemplate);


            //Достаём договор
            rcontract itemContract = _serviceLayer.Get<IRContractService>()._Repository.GetItem(e => e.PK_ID == id);

            itemContract.ReplaseWordStub(
                e => 
                    new
                    {
                        ID = e.PK_ID,
                        DATE_REG = e.DATE.ToString("D")
                    }, wordDocument);

            //Достайм поставщика по контракту
            _serviceLayer.Get<IRContractor_infoService>()
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
            _serviceLayer.Get<IRUser_infoService>()
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



            string filePathTemplateSuccess = Path.Combine(
                HttpContext.Server.MapPath(
                    String.Format("\\Template\\Success\\{0}", fileName)));

            wordDocument.SaveAs(filePathTemplateSuccess);

            wordDocument.Close();

            wordApp.Quit();

            return filePathTemplateSuccess;
        }
































        public FileResult SpecificationToPDF(int id)
        {
            rcontract contract = _serviceLayer.Get<IRContractService>()._Repository.GetItem(e => e.PK_ID == id);

            DataTable dtContract = contract.ConvertToDataTable("CONTRACT", 
                e =>
                    new
                    {
                        NUMBER_CONTRACT = contract.PK_ID, 
                        contract.DATE
                    });


            DataTable dtContractorInfo = _serviceLayer.Get<IRContractor_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == contract.FK_ID_CONTRACT_CONTRACTOR)
                .ConvertToDataTable("CONTRACTOR_INFO", 
                e =>
                    new
                    {
                        e.NAME_COMPANY,
                        e.CITY_NAME,
                        e.LEGAL_ADDRESS,
                        e.PHONE
                    });


            DataTable dtUserInfo = _serviceLayer.Get<IRUser_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == contract.FK_ID_CONTRACT_USER)
                .ConvertToDataTable("USER_INFO", 
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
            DataTable dtOrder = _serviceLayer.Get<IROrderService>()
                ._Repository
                .GetSortList(e => e.FK_ID_CONTRACT == id)
                .ToList()
                .ConvertToDataTable("ORDER", 
                e => 
                    new
                    {
                        e.PK_ID,
                        NUMBER_PRODUCT = random.Next(0, 100),
                        MASSA = random.Next(0, 100),
                        e.QANTITY,
                        NAME_PRODUCT = e.rstock.NAME,
                        e.rstock.PRICE_ONE,
                        PRICE_SUM = e.QANTITY * e.rstock.PRICE_ONE,
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

            Report report = new Report();

            report.Report.RegisterData(ds, ds.DataSetName);

            ds.WriteXmlSchema(Server.MapPath("~/Template/PDF/Specification.xsd"));
            ds.WriteXml(Server.MapPath("~/Template/PDF/Specification.xml"));

            report.Load(Server.MapPath("~/Template/PDF/Specification.frx"));
            
            report.Report.Prepare();

            var stream = new MemoryStream();
            //report.Report.Export(new Word2007Export(), stream);
            report.Report.Export(new PDFExport(), stream);
            stream.Position = 0;

            return new FileStreamResult(stream, "application/pdf");
        }
    }
}
