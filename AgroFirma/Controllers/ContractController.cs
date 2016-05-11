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

            var wordApp = new ApplicationClass();

            wordApp.Visible = true;

            var wordDocument = (DocumentClass)wordApp.Documents.Open(filePathTemplate);

            _serviceLayer.Get<IRContractService>().ContractToWord(id, wordDocument);

            string filePathTemplateSuccess = Path.Combine(
                HttpContext.Server.MapPath(
                    String.Format("\\Template\\Success\\{0}", fileName)));

            wordDocument.SaveAs(filePathTemplateSuccess);

            wordDocument.Close();

            wordApp.Quit();

            return File(filePathTemplateSuccess, "application/doc", fileName);
        }

        public FileResult SpecificationToPDF(int id)
        {
            DataSet ds = _serviceLayer.Get<IRContractService>().PDF(id);

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
