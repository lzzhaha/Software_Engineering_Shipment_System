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
    public class PackageTypeSizesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: PackageTypeSizes
        public ActionResult Index()
        {
            var packageTypeSizes = db.PackageTypeSizes.Include(p => p.PackageType);
            return View(packageTypeSizes.ToList());
        }

        // GET: PackageTypeSizes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageTypeSize packageTypeSize = db.PackageTypeSizes.Find(id);
            if (packageTypeSize == null)
            {
                return HttpNotFound();
            }
            return View(packageTypeSize);
        }

        // GET: PackageTypeSizes/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
            return View();
        }

        // POST: PackageTypeSizes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageTypeSizeID,size,limit,PackageTypeID")] PackageTypeSize packageTypeSize)
        {
            if (ModelState.IsValid)
            {
                db.PackageTypeSizes.Add(packageTypeSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", packageTypeSize.PackageTypeID);
            return View(packageTypeSize);
        }

        // GET: PackageTypeSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageTypeSize packageTypeSize = db.PackageTypeSizes.Find(id);
            if (packageTypeSize == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", packageTypeSize.PackageTypeID);
            return View(packageTypeSize);
        }

        // POST: PackageTypeSizes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageTypeSizeID,size,limit,PackageTypeID")] PackageTypeSize packageTypeSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageTypeSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", packageTypeSize.PackageTypeID);
            return View(packageTypeSize);
        }

        // GET: PackageTypeSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageTypeSize packageTypeSize = db.PackageTypeSizes.Find(id);
            if (packageTypeSize == null)
            {
                return HttpNotFound();
            }
            return View(packageTypeSize);
        }

        // POST: PackageTypeSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageTypeSize packageTypeSize = db.PackageTypeSizes.Find(id);
            db.PackageTypeSizes.Remove(packageTypeSize);
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
