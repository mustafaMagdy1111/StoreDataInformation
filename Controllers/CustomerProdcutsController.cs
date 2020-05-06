using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StoreDataInformation.Models;

namespace StoreDataInformation.Controllers
{
    public class CustomerProdcutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerProdcuts
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? CustomerId, int? page)
        {
            //ViewBag.CustomerId = CustomerId;
            //ViewBag.CustomerName = CustomerName;
            //ViewBag.AreaCodeId = AreaCodeId;
            int pageSize = 25;
            int pageNumber = page ?? 1;
            List<CustomerProdcut> customerProdcuts = new List<CustomerProdcut>();
            StaticPagedList<CustomerProdcut> resultAsPagedList;
            int superSetCount = 0;

            try
            {
                if (CustomerId != null)
                {
                    customerProdcuts = db.CustomerProdcut.Where(c => c.Id == CustomerId).ToList();
                    superSetCount = db.CustomerProdcut.Where(c => c.Id == CustomerId).Count();
                }
                else
                {
                    //customerProdcuts = db.CustomerProdcut.Include(a => a.customer).Include(a => a.CategoryProduct).ToList();
                    customerProdcuts = db.CustomerProdcut.ToList();
                    superSetCount = db.CustomerProdcut.ToList().Count();
                }

                resultAsPagedList = new StaticPagedList<CustomerProdcut>(customerProdcuts, pageNumber, pageSize, superSetCount);
                return PartialView("_CustomerProdcut", resultAsPagedList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        // GET: CustomerProdcuts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProdcut customerProdcut = db.CustomerProdcut.Find(id);
            if (customerProdcut == null)
            {
                return HttpNotFound();
            }
            return View(customerProdcut);
        }

        // GET: CustomerProdcuts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.ProdcutId = new SelectList(db.CategoryProduct, "Id", "Name");
            return View();
        }

        // POST: CustomerProdcuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create (CustomerProdcut CustomerProductToBeAdded)
        {
            if (ModelState.IsValid)
            {
                var CustProdDb = db.CategoryProduct.Where(a => a.Id == CustomerProductToBeAdded.CategoryProductId).FirstOrDefault();
                if(CustProdDb.NumberInStock==0)
                {
                    return Json("Stock Not Found That Product");
                }
                CustProdDb.NumberInStock= CustProdDb.NumberInStock-1;
                db.CategoryProduct.AddOrUpdate(CustProdDb);
                db.SaveChanges();
                db.CustomerProdcut.Add(CustomerProductToBeAdded);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", CustomerProductToBeAdded.CustomerId);
            ViewBag.ProdcutId = new SelectList(db.CategoryProduct, "Id", "Name", CustomerProductToBeAdded.CategoryProductId);
            return View(CustomerProductToBeAdded);
        }

        // GET: CustomerProdcuts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProdcut customerProdcut = db.CustomerProdcut.Find(id);
            if (customerProdcut == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", customerProdcut.CustomerId);
            ViewBag.ProdcutId = new SelectList(db.CategoryProduct, "Id", "Name", customerProdcut.CategoryProductId);
            return View(customerProdcut);
        }

        // POST: CustomerProdcuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,ProdcutId")] CustomerProdcut customerProdcut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerProdcut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", customerProdcut.CustomerId);
            ViewBag.ProdcutId = new SelectList(db.CategoryProduct, "Id", "Name", customerProdcut.CategoryProductId);
            return View(customerProdcut);
        }

        // GET: CustomerProdcuts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProdcut customerProdcut = db.CustomerProdcut.Find(id);
            if (customerProdcut == null)
            {
                return HttpNotFound();
            }
            return View(customerProdcut);
        }

        // POST: CustomerProdcuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerProdcut customerProdcut = db.CustomerProdcut.Find(id);
            db.CustomerProdcut.Remove(customerProdcut);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult SearchInCustomer(string searchText, int?searchId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if(searchId!=null)
            {
                var Customers = db.Customers.Where(a => a.Id == searchId).Select(p => new { ID = p.Id, NAME = p.Name }).ToList();
                return Json(Customers, JsonRequestBehavior.AllowGet);
            }
            if ( searchText != "")
            {
                var Customers = db.Customers.Where(a=>a.Name.Contains(searchText)).Select(p => new { ID = p.Id, NAME = p.Name }).Take(25).ToList();
                return Json(Customers, JsonRequestBehavior.AllowGet);
            }
            return Json(db.Customers.Select(p => new { ID = p.Id, NAME = p.Name }).Take(25).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchInProduct(string searchText, int?searchId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if(searchId!=null)
            {
                var Products = db.CategoryProduct.Where(a => a.Code == searchId.ToString()).Select(p => new { ID = p.Code, NAME = p.Name ,RealId=p.Id}).ToList();
                return Json(Products, JsonRequestBehavior.AllowGet);
            }
            if ( searchText != "")
            {
                var Products = db.CategoryProduct.Where(a=>a.Name.Contains(searchText)).Select(p => new { ID = p.Code, NAME = p.Name, RealId = p.Id }).Take(25).ToList();
                return Json(Products, JsonRequestBehavior.AllowGet);
            }
            return Json(db.CategoryProduct.Select(p => new { ID = p.Code, NAME = p.Name, RealId = p.Id }).Take(25).ToList(), JsonRequestBehavior.AllowGet);
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
