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
using Newtonsoft.Json;
using System.Collections;

namespace SinExWebApp20328381.Controllers
{
    public class ServicePackageFeesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: ServicePackageFees
        public ActionResult Index()
        {
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            return View(servicePackageFees.ToList());
        }
        public ActionResult Index2()
        {
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            return View(servicePackageFees.ToList());
        }

        // GET: ServicePackageFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type");
            return View();
        }

        // POST: ServicePackageFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.ServicePackageFees.Add(servicePackageFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePackageFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            db.ServicePackageFees.Remove(servicePackageFee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get
        public ActionResult FeeCheck(string ServiceType, ICollection<FeeCheckListViewModel> Packages)
        {
            if (Packages.Count == 0)
            {
                var FeeCheckInput = new FeeCheckGenerateViewModel();
                FeeCheckInput.Destinations = PopulateDestinationsDropdownList().ToList();
                FeeCheckInput.ServiceTypes = PopulateServiceTypesDropdownList().ToList();
                FeeCheckInput.PackageTypes = PopulatePackageTypesDropdownList().ToList();

                return View(FeeCheckInput);
            }
            else
            {
                var ServiceTypeID = db.ServiceTypes.SingleOrDefault(s => s.Type == ServiceType).ServiceTypeID;
                ArrayList fees = new ArrayList();
                int PackageTypeID;
                ServicePackageFee servicePackageFee;
                foreach (var i in Packages)
                {
                    PackageTypeID = db.PackageTypes.SingleOrDefault(s => s.Type == i.PackageType).PackageTypeID;
                    servicePackageFee = db.ServicePackageFees.SingleOrDefault(s => (s.PackageTypeID == PackageTypeID && s.ServiceTypeID == ServiceTypeID));
                    fees.Add((i.Weight * servicePackageFee.Fee < servicePackageFee.MinimumFee ? i.Weight * servicePackageFee.Fee : servicePackageFee.MinimumFee));

                }
            }
        }

        private SelectList PopulatePackageTypesDropdownList()
        {
            var PackageTypeQuery = db.PackageTypes.Select(s => s.Type).Distinct().OrderBy(s => s);
            return new SelectList(PackageTypeQuery);
        }

        private SelectList PopulateServiceTypesDropdownList()
        {
            var ServiceTypeQuery = db.ServiceTypes.Select(s => s.Type).Distinct().OrderBy(s => s);
            return new SelectList(ServiceTypeQuery);
        }

        private SelectList PopulateDestinationsDropdownList()
        {
            var DestinationQuery = db.Destinations.Select(s => s.City).Distinct().OrderBy(s => s);
            return new SelectList(DestinationQuery);
        }

        [HttpPost]
        public JsonResult GetSizeOfPackage(FeeCheckPackageJson jsonPackageName)
        {
            //var PackageName = JsonConvert.DeserializeObject(jsonPackageName);
            PackageType package = db.PackageTypes.SingleOrDefault(s => s.Type == jsonPackageName.PackageName);
            if (package == null ) return Json(0);
            var sizeQuery = db.PackageTypeSizes.Where(s => s.PackageTypeID == package.PackageTypeID).Select(s => s.size).Distinct();
            return Json(sizeQuery.ToArray());
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
