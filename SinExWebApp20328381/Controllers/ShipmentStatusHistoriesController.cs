using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;

namespace SinExWebApp20328381.Controllers
{
    public class ShipmentStatusHistoriesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();


        public ActionResult Search(string errorMessage) {
            ViewBag.Error = errorMessage;
            return View();
        }
        // GET: ShipmentStatusHistories
        /*
         Shipment Tracking Functionality
         Corresponding Shipment status will be displayed when customer enters the shipment waybill number
         */
        public ActionResult Index(long? WaybillId)
        {
            if (WaybillId ==null ) {
                return RedirectToAction("Search",new { errorMessage = "You should enter a waybill number"});
            }
            //Retrieve Shipment 
            var shipment = db.Shipments.Include("Packages").Include("Packages.PackageType").Where(s => s.WaybillId == WaybillId).FirstOrDefault();
            //Pass the property of the shipment to ViewData

            if (shipment==null) {
                return RedirectToAction("Search", new { errorMessage = "No such a waybill" });
            }
            
            ViewData["WaybillNumber"] = shipment.WaybillId;

            ViewData["DeliveredPerson"] = shipment.DeliveredPerson==null? "Not Delivered": shipment.DeliveredPerson;

            ViewData["Company"] = shipment.RecipientCompanyName;

            ViewData["DeliveredPlace"] = shipment.DeliveredPlace==null? "Not Delivered": shipment.DeliveredPlace;

            ViewData["Status"] = shipment.Status;

            ViewData["ServiceType"] = shipment.ServiceType;

            List<Package> Packages = shipment.Packages.ToList();
            ViewData["Packages"] = Packages;

            
            //Retrieve shipmentStatusHistory
            var statusQuery = from status in db.ShipmentStatusHistories
                              where status.WaybillId == WaybillId
                              orderby status.DateAndTime descending
                              select status;

         
            return View(statusQuery.ToList());
        }

        // GET: ShipmentStatusHistories/Details/5
       /* public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatusHistory shipmentStatusHistory = db.ShipmentStatusHistories.Find(id);
            if (shipmentStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatusHistory);
        }
        */
        // GET: ShipmentStatusHistories/Create
        public ActionResult Create(long? WaybillId, string Status)
        {
            ViewData["WaybillId"] = WaybillId;
            ViewData["Status"] = Status;
            return View();
        }

        // POST: ShipmentStatusHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WaybillId,ShipmentStatusHistoryId,Status,DateAndTime,Description,Location,Remark,DeliveredPerson,DeliveredPlace")] ShipmentStatusHistory shipmentStatusHistory)
        {
           
            if (ModelState.IsValid)
            {
                Shipment shipment = db.Shipments.Where(s => s.WaybillId == shipmentStatusHistory.WaybillId).SingleOrDefault();
                shipment.Status = shipmentStatusHistory.Status;
                if (shipmentStatusHistory.Status == "Delivered")
                {
                    shipment.DeliveredDate = shipmentStatusHistory.DateAndTime;
                    shipment.DeliveredPlace = shipmentStatusHistory.DeliveredPlace;
                    shipment.DeliveredPerson = shipmentStatusHistory.DeliveredPerson;
                
                }
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                db.ShipmentStatusHistories.Add(shipmentStatusHistory);
             
                db.SaveChanges();
                return RedirectToAction("Index",new { WaybillId = shipmentStatusHistory.WaybillId });
            }

            return View(shipmentStatusHistory);
        }

        // GET: ShipmentStatusHistories/Edit/5
        public ActionResult Edit(long? WaybillId)
        {
            if (WaybillId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatusHistory shipmentStatusHistory = db.ShipmentStatusHistories.Find(WaybillId);
            if (shipmentStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatusHistory);
        }

        // POST: ShipmentStatusHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShipmentStatusHistoryId,WaybillId,Status,DateAndTime,Description,Location,Remark")] ShipmentStatusHistory shipmentStatusHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentStatusHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipmentStatusHistory);
        }

        // GET: ShipmentStatusHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatusHistory shipmentStatusHistory = db.ShipmentStatusHistories.Find(id);
            if (shipmentStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatusHistory);
        }

        // POST: ShipmentStatusHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentStatusHistory shipmentStatusHistory = db.ShipmentStatusHistories.Find(id);
            db.ShipmentStatusHistories.Remove(shipmentStatusHistory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
