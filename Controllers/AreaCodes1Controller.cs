using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreDataInformation.Models;

namespace StoreDataInformation.Controllers
{
    public class AreaCodes1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AreaCodes1
        public ActionResult Index()
        {
            return View(db.AreaCodes.ToList());
        }

        // GET: AreaCodes1/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaCode areaCode = db.AreaCodes.Find(id);
            if (areaCode == null)
            {
                return HttpNotFound();
            }
            return View(areaCode);
        }

        // GET: AreaCodes1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AreaCodes1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AreaCode areaCode)
        {
            if (ModelState.IsValid)
            {
                db.AreaCodes.Add(areaCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(areaCode);
        }

        // GET: AreaCodes1/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaCode areaCode = db.AreaCodes.Find(id);
            if (areaCode == null)
            {
                return HttpNotFound();
            }
            return View(areaCode);
        }

        // POST: AreaCodes1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AreaCode areaCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(areaCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(areaCode);
        }

        // GET: AreaCodes1/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaCode areaCode = db.AreaCodes.Find(id);
            if (areaCode == null)
            {
                return HttpNotFound();
            }
            return View(areaCode);
        }

        // POST: AreaCodes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            AreaCode areaCode = db.AreaCodes.Find(id);
            db.AreaCodes.Remove(areaCode);
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
