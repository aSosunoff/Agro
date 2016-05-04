using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroFirma.Component;
using Components;
using FastReport;
using FastReport.Export.Pdf;
using Model;
using Model.Engine.Repository.Interface;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

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

            string filePath = _serviceLayer.Get<IRContractService>().PrintDog(id, fileName);

            return File(filePath, "application/doc", fileName);
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
