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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? CustomerId, string CustomerName, int? AreaCodeId, int? page)
        {
            ViewBag.CustomerId = CustomerId;
            ViewBag.CustomerName = CustomerName;
            ViewBag.AreaCodeId = AreaCodeId;
            int pageSize = 10;
            int pageNumber = page ?? 1;
            List<Customer> customers = new List<Customer>();
            StaticPagedList<Customer> resultAsPagedList;
            int superSetCount = 0;

            try
            {
                if (CustomerId != null)
                {
                    customers = db.Customers.Where(c => c.Id == CustomerId)
                        .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    superSetCount = db.Customers.Where(c => c.Id == CustomerId)
                          .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                        .Count();
                }
               else if (AreaCodeId != null)
                {
                    customers = db.Customers.Where(c => c.AreaCodeId == AreaCodeId)
                        .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    superSetCount = db.Customers.Where(c => c.AreaCodeId == AreaCodeId)
                          .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                        .Count();
                }
              else  if(CustomerName!="" &&CustomerName!=null)
                {
                    customers = db.Customers.Where(c => c.Name.Contains(CustomerName))
                        .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    superSetCount = db.Customers.Where(c => c.Name.Contains(CustomerName))
                          .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList().Count();
                }
                else
                {
                    customers = db.Customers
                        .OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    superSetCount = db.Customers.Count();
                }
                resultAsPagedList = new StaticPagedList<Customer>(customers, pageNumber, pageSize, superSetCount);
                return PartialView("_CustomerDataTable", resultAsPagedList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.AreaCodeId = new SelectList(db.AreaCodes, "Id", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhoneNumber,Address,AreaCodeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaCodeId = new SelectList(db.AreaCodes, "Id", "Name", customer.AreaCodeId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaCodeId = new SelectList(db.AreaCodes, "Id", "Name", customer.AreaCodeId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Address,AreaCodeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaCodeId = new SelectList(db.AreaCodes, "Id", "Name", customer.AreaCodeId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
