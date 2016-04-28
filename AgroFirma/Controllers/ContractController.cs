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
            //TODO:  Возможно вынести в отдельный класс
            Report report = new Report();

            rcontract rcontractItem = _serviceLayer.Get<IRContractService>()._Repository.GetItem(e => e.PK_ID == id);

            DataTable dt0 = _serviceLayer.Get<IRContractService>()
                ._Repository
                .GetSortList(e => e.PK_ID == id)
                .Select( e => 
                    new
                    {
                        NUMBER_CONTRACT = e.PK_ID,
                        DATE = e.DATE
                    })
                .ConvertToDataTable("INFO_CONTRACT");


            DataTable dt1 = _serviceLayer.Get<IRContractor_infoService>()
                ._Repository
                .GetSortList( e => e.PK_ID == rcontractItem.FK_ID_CONTRACT_CONTRACTOR)
                .Select(r => 
                    new 
                    {
                        NAME_COMPANY = r.NAME_COMPANY, 
                        CITY = r.CITY_NAME, 
                        ADDRESS = r.LEGAL_ADDRESS, 
                        PHONE = r.PHONE
                    })
                    .ConvertToDataTable("INFO_COMPANY");

            DataTable dt2 = _serviceLayer.Get<IROrderService>()
                ._Repository
                .GetSortList(e => e.FK_ID_CONTRACT == id)
                .Select(r => 
                    new
                    {
                        QANTITY = r.QANTITY, 
                        NAME_PRODUCT = r.rstock.NAME
                    })
                    .ConvertToDataTables("ORDER");

            DataSet ds = new DataSet("N");
            ds.Tables.Add(dt0);
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            report.Report.RegisterData(ds, "N");

            ds.WriteXmlSchema(Server.MapPath("~/Template/PDF/Sertification.xsd"));
            ds.WriteXml(Server.MapPath("~/Template/PDF/Sertification.xml"));


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
