using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20328381.Models;
using System.Text.RegularExpressions;

namespace SinExWebApp20328381.Controllers
{
    public class InvoicesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.shipment);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,WaybillId,TotalCostCurrency,TotalCost,ShippingAccountId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,WaybillId,TotalCostCurrency,TotalCost,ShippingAccountId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", invoice.InvoiceId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
        //[Authorize(Roles = "Employee")]
        public ActionResult SearchWayBill()
        {
            var selectIetm = db.Shipments.Select(x => new SelectListItem() { Value = x.WaybillId.ToString(), Text = x.WaybillId.ToString() }).ToList();
            ViewBag.waybillID = selectIetm;
            Invoice invoice = new Invoice();
            return View(invoice);
        }
        [HttpGet]
        public ActionResult FillActualWeight(string WaybillId)
        {
            long waybillID = Convert.ToInt64(WaybillId);
            Shipment shipment=db.Shipments.Include("Packages").SingleOrDefault(s => s.WaybillId == waybillID);
            Invoice invoice = new Invoice();
            invoice.shipment = shipment;
            invoice.WaybillId = waybillID;
            ViewData["waybillID"] = WaybillId;
            return View(invoice);
        }
    
        [HttpPost]
         public ActionResult FillActualWeight(Invoice invoice)
        {
            long waybillID = Convert.ToInt64(invoice.WaybillId);
            Shipment shipment = db.Shipments.Include("Packages").SingleOrDefault(s => s.WaybillId == waybillID);
            invoice.ShippingAccountId = shipment.ShippingAccountId;
            int i = 0;
            foreach(var package in shipment.Packages)
            {
                package.ActualWeight = invoice.shipment.Packages.ToList()[i].ActualWeight;
                i = i + 1;
            }
            shipment.Tax = invoice.shipment.Tax;
            shipment.Duty = invoice.shipment.Duty;
            long shipmentShippingAccountId= Convert.ToInt64(shipment.ShipmentShippingAccountId);
            var provinceCode = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == shipmentShippingAccountId).MailingAddressProvinceCode;
            invoice.TotalCostCurrency = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode).CurrencyCode;
            long taxShippingAccountID = Convert.ToInt64(shipment.TaxAndDutyShippingAccountId );
            var provinceCode2 = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountId == taxShippingAccountID).MailingAddressProvinceCode;
            shipment.TaxCurreny = db.Destinations.SingleOrDefault(s => s.ProvinceCode == provinceCode2).CurrencyCode;
            shipment.DutyCurrency = shipment.TaxCurreny;
            //calculate the total fee
            if (shipment.Packages.Count()!=0)
            {
                var ServiceTypeID = db.ServiceTypes.SingleOrDefault(s => s.Type == shipment.ServiceType).ServiceTypeID;
                int PackageTypeID;
                ServicePackageFee servicePackageFee;
                string packageTypelimit;
                Decimal fee, TotalFee, decimalweight;
                TotalFee = 0;
                foreach (var package in shipment.Packages)
                {
                    if (package.PackageTypeID !=0 && package.ActualWeight != null)
                    {
                        PackageTypeID = package.PackageTypeID;
                        servicePackageFee = db.ServicePackageFees.SingleOrDefault(s => (s.PackageTypeID == PackageTypeID && s.ServiceTypeID == ServiceTypeID));
                        decimalweight = decimal.Round((decimal)package.Weight, 1);
                        fee = (decimalweight * servicePackageFee.Fee < servicePackageFee.MinimumFee ? servicePackageFee.MinimumFee : decimalweight * servicePackageFee.Fee);
                        package.Cost = fee;
                        Regex reg = new Regex(@"([0-9]*).*");

                        if (package.PackageTypeSizeID != null)
                        {
                            packageTypelimit = db.PackageTypeSizes.SingleOrDefault(s => s.PackageTypeSizeID == package.PackageTypeSizeID).limit;
                            var result = reg.Match(packageTypelimit).Groups;
                            if (result[1].Value == "")
                            {
                                fee = servicePackageFee.Fee;
                            }
                            else if (package.Weight > decimal.Parse(result[1].Value))
                            {
                                fee += 500;
                            }
                        }
                        TotalFee += fee;
                        
                    }
                    invoice.TotalCost = TotalFee;
                }
               
            }
            invoice.shipment = shipment;
            db.Invoices.Add(invoice);
            db.SaveChanges();
            return View(invoice);
        }
    }
}
