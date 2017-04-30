using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;
using SinExWebApp20328381.ViewModels;
using System.Net.Mail;
namespace SinExWebApp20328381.Controllers
{
    public class ShipmentStatusHistoriesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: ShipmentStatusHistories
        public ActionResult Search(string errorMessage) {
            SearchViewModel search_View = new SearchViewModel();
            ViewData["Error"] = errorMessage;
            return View(search_View);
        }

        
        [HttpPost]
        public ActionResult Search(SearchViewModel search_view) {
            if (ModelState.IsValid)
            {
                long? _WaybillId = Int64.Parse(search_view.WaybillId);
                return RedirectToAction("Index", new { WaybillId = _WaybillId });
            }
            else {
                return View(search_view);
            }
        }
        /*
         Shipment Tracking Functionality
         Corresponding Shipment status will be displayed when customer enters the shipment waybill number
         */
 
        public ActionResult Index(long? WaybillId)
        {
            
                //Retrieve Shipment 
            
                var shipment = db.Shipments.Include("Packages").Include("Packages.PackageType").Where(s => s.WaybillId == WaybillId).FirstOrDefault();
                //Pass the property of the shipment to ViewData

                if (shipment == null)
                {
                    return RedirectToAction("Search", new { errorMessage = "No such a waybill" });
                }

                ViewData["WaybillNumber"] = shipment.WaybillId.ToString().PadLeft(12, '0'); ;

                ViewData["DeliveredPerson"] = shipment.DeliveredPerson == null ? "Not Delivered" : shipment.DeliveredPerson;

                ViewData["Company"] = shipment.RecipientCompanyName;

                ViewData["DeliveredPlace"] = shipment.DeliveredPlace == null ? "Not Delivered" : shipment.DeliveredPlace;

                ViewData["Status"] = shipment.Status;

                ViewData["ServiceType"] = shipment.ServiceType;

                List<Package> Packages = shipment.Packages.ToList();
                ViewData["Packages"] = Packages;


                //Retrieve shipmentStatusHistory
                var statusQuery = from status in db.ShipmentStatusHistories
                                  where status.WaybillId == WaybillId
                                  orderby status.Date descending, status.Time descending
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
        public ActionResult Create([Bind(Include = "WaybillId,ShipmentStatusHistoryId,Status,Date,TimeValue,Description,Location,Remarks,DeliveredPerson,DeliveredPlace")] ShipmentStatusHistory shipmentStatusHistory)
        {

           
            if (ModelState.IsValid)
            {
                Shipment shipment = db.Shipments.Where(s => s.WaybillId == shipmentStatusHistory.WaybillId).SingleOrDefault();
                shipment.Status = shipmentStatusHistory.Status;
                if (shipmentStatusHistory.Status == "Delivered")
                {
                    shipment.DeliveredDate = shipmentStatusHistory.Date;
                    shipment.DeliveredPlace = shipmentStatusHistory.DeliveredPlace;
                    shipment.DeliveredPerson = shipmentStatusHistory.DeliveredPerson;

                    //Send Email to sender
                   
                    MailMessage DMail = new MailMessage();
                    DMail.From = new MailAddress("comp3111_team105@cse.ust.hk");
                    DMail.To.Add(shipment.ShippingAccount.EmailAddress);
                    DMail.Subject = "Delivered Notification";
                    DMail.Body = "<!doctype html><html><head><meta charset = 'UTF-8'></head>";
                    DMail.Body += "<div>Your shipment has been successfully delivered to:&nbsp " + shipment.RecipientName + "</div> <br /> " +
                        "<div>Delivered Location:</div><br />"+
                       "<div>City:&nbsp"+ shipment.RecipientCityAddress + ",</div> <br />" +
                       "<div>Street:&nbsp" + shipment.RecipientStreetAddress + "</div><br />"+
                       " <div>Building:&nbsp" + shipment.RecipientBuildingAddress + "</div><br />"+
                       "<div>Delivery Date:&nbsp"+ shipment.DeliveredDate.ToString("dd-mm-yyyy")+"</div>";
                    DMail.Body += "< body ></ body ></ html >";
                    sendEmail(DMail);
                  } else if (shipmentStatusHistory.Status=="PickedUp") {

                    //Send Email to recipient

                    bool IsPerson;
                    if (shipment.ShippingAccount is PersonalShippingAccount) {
                        IsPerson = true;
                    } else {
                        IsPerson = false;
                    }
                    string SenderName;
                    if (IsPerson) {
                        PersonalShippingAccount sender = new PersonalShippingAccount();
                        SenderName = sender.FirstName + " " + sender.LastName;
                    } else {
                        BusinessShippingAccount sender = new BusinessShippingAccount();
                        SenderName = "Company Name: " + sender.CompanyName + ", Contact Person: " + sender.ContactPersonName;
                    }

                    MailMessage DMail = new MailMessage();
                    DMail.From = new MailAddress("comp3111_team105@cse.ust.hk");
                    DMail.To.Add(shipment.RecipientEmailAddress);
                    DMail.Subject = "PickedUp Notification";




                    DMail.Body = "<!doctype html><html><head><meta charset = 'UTF-8'></head>";
                    DMail.Body += "<div>Your shipment has been successfully delivered to:&nbsp " + shipment.RecipientName + "</div> <br /> " +
                        "<div>A shipment (WaybillId:&nbsp"+ shipment.WaybillId+ ") for you has been successfully picked up from: </div><br />" +
                        "<div>Sender:&nbsp"+ SenderName+"</div><br />"+
                       "<div>City:&nbsp" + shipment.ShippingAccount.MailingAddressCity + ",</div> <br />" +
                       "<div>Street:&nbsp" + shipment.ShippingAccount.MailingAddressStreet + "</div><br />" +
                       " <div>Building:&nbsp" + shipment.ShippingAccount.MailingAddressBuilding + "</div><br />" +
                       "<div>Delivery Date:&nbsp" + shipment.ShippedDate.ToString("dd-mm-yyyy") + "</div>";
                    DMail.Body += "< body ></ body ></ html >";

                    sendEmail(DMail);

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
