using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC_CRUD.DAL;
using WebApplicationMVC_CRUD.Helpers;
using WebApplicationMVC_CRUD.Models;

namespace WebApplicationMVC_CRUD.Controllers
{
    public class PhoneController : Controller
    {
        private PfonesModel db = new PfonesModel();

        // GET: /Phone/
        public ActionResult Index(PhoneSearchCondition values)
        {
            var viewModelData = new PagedList<Phones>(values);

            var query = db.Phones
                .WhereIf(!String.IsNullOrEmpty(values.filter), x => (x.Model.Contains(values.filter)) || x.Manufacturers.Name.Contains(values.filter))
                .WhereIf(values.ManufacturerId > 0, x => x.ManufacturerId == values.ManufacturerId);

            viewModelData= query.ToPagedList(values, x=>x);

            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;

           // records.SearchConditions.A

            return View(viewModelData);
        }

        public ActionResult SelectManufacturer()
        {
            var items = db.Manufacturers.ToList();
            ViewBag.Manufacturer = items;
            return View();
        }

        // GET: /Phone/Details/5
        public ActionResult Details(int id = 0)
        {
            Phones phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }

            //
            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;

            return PartialView("Details", phone);
        }


        // GET: /Phone/Create
        [HttpGet]
        public ActionResult Create()
        {
            var phone = new Phones();

            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;

            return PartialView("Create", phone);
        }


        // POST: /Phone/Create
        [HttpPost]
        public JsonResult Create(Phones phone)
        {
            if (ModelState.IsValid)
            {
                db.Phones.Add(phone);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(phone, JsonRequestBehavior.AllowGet);
        }

        // GET: /Phone/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }

            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;

            return PartialView("Edit", phone);
        }


        // POST: /Phone/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Phones phone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phone).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }

            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;

            return PartialView("Edit", phone);
        }


        // GET: /Phone/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Phones phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }


            var manu = db.Manufacturers.ToList();
            ViewBag.ManufacturersList = manu;
            return PartialView("Delete", phone);
        }


        //
        // POST: /Phone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var phone = db.Phones.Find(id);
            db.Phones.Remove(phone);
            db.SaveChanges();
            return Json(new { success = true });
        }


    }

}