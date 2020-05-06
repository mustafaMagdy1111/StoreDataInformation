using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StoreDataInformation.Models;

namespace StoreDataInformation.Controllers
{
    public class CategoryProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryProducts
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? ProdcutCode, int? page)
        {
            //ViewBag.CustomerId = CustomerId;
            //ViewBag.CustomerName = CustomerName;
            //ViewBag.AreaCodeId = AreaCodeId;
            int pageSize = 25;
            int pageNumber = page ?? 1;
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            StaticPagedList<CategoryProduct> resultAsPagedList;
            int superSetCount = 0;

            try
            {
                //if (CustomerId != null)
                //{
                //    customers = db.Customers.Where(c => c.Id == CustomerId).ToList();
                //    superSetCount = db.Customers.Where(c => c.Id == CustomerId).Count();
                //}
                //else if (AreaCodeId != null)
                //{
                //    customers = db.Customers.Where(c => c.AreaCodeId == AreaCodeId).ToList();
                //    superSetCount = db.Customers.Where(c => c.AreaCodeId == AreaCodeId).Count();
                //}
                //else if (CustomerName != "")
                //{
                //    customers = db.Customers.Where(c => c.Name.Contains(CustomerName)).ToList();
                //    superSetCount = db.Customers.Where(c => c.Name.Contains(CustomerName)).Count();
                //}
                //else
                //{
                //    customers = db.Customers.ToList();
                //    superSetCount = db.Customers.Where(c => c.Name.Contains(CustomerName)).Count();
                //}

                categoryProducts = db.CategoryProduct.ToList();
                resultAsPagedList = new StaticPagedList<CategoryProduct>(categoryProducts, pageNumber, pageSize, superSetCount);
                return PartialView("_CategoryProductDataTable", resultAsPagedList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: CategoryProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProduct categoryProduct = db.CategoryProduct.Find(id);
            if (categoryProduct == null)
            {
                return HttpNotFound();
            }
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: CategoryProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Size,PriceInLE,Color,NumberInStock,CategoryId")] CategoryProduct categoryProduct)
        {
            if (ModelState.IsValid)
            {
                db.CategoryProduct.Add(categoryProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", categoryProduct.CategoryId);
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProduct categoryProduct = db.CategoryProduct.Find(id);
            if (categoryProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", categoryProduct.CategoryId);
            return View(categoryProduct);
        }

        // POST: CategoryProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,Size,PriceInLE,Color,NumberInStock,CategoryId")] CategoryProduct categoryProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", categoryProduct.CategoryId);
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProduct categoryProduct = db.CategoryProduct.Find(id);
            if (categoryProduct == null)
            {
                return HttpNotFound();
            }
            return View(categoryProduct);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryProduct categoryProduct = db.CategoryProduct.Find(id);
            db.CategoryProduct.Remove(categoryProduct);
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
