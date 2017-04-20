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

        // GET: ShipmentStatusHistories
        /*
         Shipment Tracking Functionality
         Corresponding Shipment status will be displayed when customer enters the shipment waybill number
         */
        public ActionResult Index(long? WaybillId)
        {
            //Retrieve Shipment 
            IEnumerable<Shipment> shipment = db.Shipments.Where(s => s.WaybillId == WaybillId);
            //Pass the property of the shipment to ViewData

            ViewData["WaybillNumber"] = shipment.ElementAt(0).WaybillId;

            ViewData["RecipientName"] = shipment.ElementAt(0).RecipientName;

            ViewData["Company"] = shipment.ElementAt(0).RecipientCompanyName;

            ViewData["DeliveredPlace"] = shipment.ElementAt(0).DeliveredPlace;

            ViewData["Status"] = shipment.ElementAt(0).Status;

            ViewData["ServiceType"] = shipment.ElementAt(0).ServiceType;

            ViewData["Packages"] = shipment.ElementAt(0).Packages;

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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShipmentStatusHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShipmentStatusHistoryId,WaybillId,Status,DateAndTime,Description,Location,Remark")] ShipmentStatusHistory shipmentStatusHistory)
        {
            if (ModelState.IsValid)
            {
                db.ShipmentStatusHistories.Add(shipmentStatusHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
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
