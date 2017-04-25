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
using System.Text.RegularExpressions;

namespace SinExWebApp20328381.Controllers
{
    public class ServicePackageFeesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: ServicePackageFees
        public ActionResult Index(string CurrencyCode)
        {
            //var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            //return View(servicePackageFees.ToList());
            SelectCurrency sc = new SelectCurrency();
            sc.Currencies = PopulateCurrenciesDropdownList().ToList();
            if (CurrencyCode == null)
            {
                if (Session["LastCurrency"] == null)
                    Session["LastCurrency"] = "CNY";
            }
            else
            {
                Session["LastCurrency"] = CurrencyCode;
            }
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            sc.ServicePackageFees = servicePackageFees.ToList();
            foreach (var servicePackageFee in sc.ServicePackageFees)
            {
                servicePackageFee.Fee = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.Fee);
                servicePackageFee.MinimumFee = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.MinimumFee);
                servicePackageFee.Penalty = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.Penalty);
            }
            return View(sc);
        }
        public ActionResult Index2()
        {
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            return View(servicePackageFees.ToList());
        }

        public ActionResult Index3(string CurrencyCode)
        {
            SelectCurrency sc = new SelectCurrency();
            sc.Currencies = PopulateCurrenciesDropdownList().ToList();
            if (CurrencyCode == null)
            {
                if (Session["LastCurrency"] == null)
                    Session["LastCurrency"] = "CNY";
            }
            else
            {
                Session["LastCurrency"] = CurrencyCode;
            }
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            sc.ServicePackageFees = servicePackageFees.ToList();
            foreach (var servicePackageFee in sc.ServicePackageFees)
            {
                servicePackageFee.Fee = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.Fee);
                servicePackageFee.MinimumFee = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.MinimumFee);
                servicePackageFee.Penalty = ConvertCurrency((string)Session["LastCurrency"], servicePackageFee.Penalty);
            }
            return View(sc);
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
            
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID,Penalty")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(servicePackageFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
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
        public List<Decimal> ProcessFeeCheck (string ServiceType, ICollection<PackageInputViewModel> Packages)
        {
            List<Decimal> fees = new List<Decimal>();
            if (Packages != null)
            {
                var ServiceTypeID = db.ServiceTypes.SingleOrDefault(s => s.Type == ServiceType).ServiceTypeID;
                int PackageTypeID;
                ServicePackageFee servicePackageFee;
                string packageTypelimit;
                Decimal fee, TotalFee, decimalweight;
                TotalFee = 0;
                foreach (var i in Packages)
                {
                    if (i.PackageType != null && i.Weight != null)
                    {
                        PackageTypeID = db.PackageTypes.SingleOrDefault(s => s.Type == i.PackageType).PackageTypeID;
                        servicePackageFee = db.ServicePackageFees.SingleOrDefault(s => (s.PackageTypeID == PackageTypeID && s.ServiceTypeID == ServiceTypeID));
                        decimalweight = (i.ActualWeight != null && i.ActualWeight > 0)? decimal.Round((decimal)i.ActualWeight, 1) : decimal.Round((decimal)i.Weight, 1);
                        fee = (decimalweight * servicePackageFee.Fee < servicePackageFee.MinimumFee ? servicePackageFee.MinimumFee : decimalweight * servicePackageFee.Fee);
                        Regex reg = new Regex(@"([0-9]*).*");
                        if (i.Size != null)
                        {
                            packageTypelimit = db.PackageTypeSizes.SingleOrDefault(s => s.size == i.Size).limit;
                            var result = reg.Match(packageTypelimit).Groups;
                            if (result[1].Value == "")
                            {
                                fee = servicePackageFee.Fee;
                            }
                            else if (i.Weight > decimal.Parse(result[1].Value))
                            {
                                fee += 500;
                            }
                        }
                        TotalFee += fee;
                        fees.Add(fee);
                    }
                }
                fees.Add(TotalFee);
            }
            return fees;
        }
        public ActionResult FeeCheck(string ServiceType, ICollection<PackageInputViewModel> Packages)
        {
            var FeeCheckInput = new FeeCheckGenerateViewModel();
            FeeCheckInput = (FeeCheckGenerateViewModel)PopulateDrownLists(FeeCheckInput);
            FeeCheckInput.Fees = ProcessFeeCheck(ServiceType, Packages);
            return View(FeeCheckInput);
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
