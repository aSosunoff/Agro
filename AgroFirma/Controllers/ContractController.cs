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
            rcontract rcontractItem = _serviceLayer.Get<IRContractService>()._Repository.GetItem(e => e.PK_ID == id);

            DataTable dt0 = Components.Convert.ConvertToDataTable("CONTRACT", 
                new
                {
                    NUMBER_CONTRACT = rcontractItem.PK_ID, 
                    DATE = rcontractItem .DATE
                });

            rcontractor_info rcontractorInfoItem = _serviceLayer.Get<IRContractor_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == rcontractItem.FK_ID_CONTRACT_CONTRACTOR);

            DataTable dt1 = Components.Convert.ConvertToDataTable("CONTRACTOR_INFO",
                new
                {
                    rcontractorInfoItem.NAME_COMPANY,
                    rcontractorInfoItem.CITY_NAME,
                    rcontractorInfoItem.LEGAL_ADDRESS,
                    rcontractorInfoItem.PHONE
                });

            ruser_info ruserInfoItem = _serviceLayer.Get<IRUser_infoService>()
                ._Repository
                .GetItem(e => e.PK_ID == rcontractItem.FK_ID_CONTRACT_USER);

            DataTable dtUserInfo = Components.Convert.ConvertToDataTable("USER_INFO",
                new
                {
                    ruserInfoItem.NAME_COMPANY,
                    ruserInfoItem.CITY_NAME,
                    ruserInfoItem.LEGAL_ADDRESS,
                    ruserInfoItem.PHONE,
                    ruserInfoItem.CHECKING_ACCOUNT
                });




            IEnumerable<rorder> rorderList = _serviceLayer.Get<IROrderService>()
                ._Repository
                .GetSortList(e => e.FK_ID_CONTRACT == id)
                .ToList();

            //TODO: NUMBER_LABLE Номер упаковочного ярлыка задан рамдомно
            //TODO: BRUTTO брутто задано рамдомно
            //TODO: TARA Тара задано рамдомно
            Random random = new Random();
            var r = rorderList
                    .Select(e =>
                        new
                        {
                            QANTITY = e.QANTITY,
                            NAME_PRODUCT = e.rstock.NAME,
                            NUMBER_LABLE = random.Next(0, 100),
                            BRUTTO = random.Next(0, 100),
                            TARA = random.Next(0, 100)
                        })
                        .ToList();

            DataTable dt2 = Components.Convert.ConvertToDataTable("ORDER", r.Select(e => 
                new
                {
                    e.QANTITY,
                    e.NAME_PRODUCT,
                    e.NUMBER_LABLE,
                    e.BRUTTO,
                    e.TARA
                }));

            DataTable dt3 = Components.Convert.ConvertToDataTable("ORDER_SUM",
                new
                {
                    SUM_QANTITY = r.Sum(e => e.QANTITY),
                    SUM_BRUTTO = r.Sum(e => e.BRUTTO),
                    SUM_TARA = r.Sum(e => e.TARA)
                });


            DataSet ds = new DataSet("N");
            ds.Tables.Add(dt0);
            ds.Tables.Add(dt1);
            ds.Tables.Add(dtUserInfo);
            ds.Tables.Add(dt2);
            ds.Tables.Add(dt3);

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

        public FileResult WaybillToPDF(int id)
        {
            rcontract rcontractItem = _serviceLayer.Get<IRContractService>()._Repository.GetItem(e => e.PK_ID == id);

            return null;
        }
    }
}
