using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web1.Models;

namespace Web1.Views
{
    public class RecordsController : Controller
    {
        private BeautyShopEntities1 db = new BeautyShopEntities1();

        // GET: Records
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewBag.TimeSortParm = sortOrder == "time" ? "time_desc" : "time";
            ViewBag.ClientsSortParm = sortOrder == "clients" ? "clients_desc" : "clients";
            ViewBag.EmployeeSortParm = sortOrder == "employee" ? "employee_desc" : "employee";
            ViewBag.ServicesSortParm = sortOrder == "services" ? "services_desc" : "services";
            ViewBag.DaySortParm = sortOrder == "day" ? "day_no" : "day";

            var record = from s in db.Record
                           select s;
            switch (sortOrder)
            {
                case "Date_desc":
                    record = record.OrderByDescending(s => s.Date);
                    break;
                case "time":
                    record = record.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    record = record.OrderByDescending(s => s.Time);
                    break;
                case "clients":
                    record = record.OrderBy(s => s.Id_Clients);
                    break;
                case "clients_desc":
                    record = record.OrderByDescending(s => s.Id_Clients);
                    break;
                case "employee":
                    record = record.OrderBy(s => s.Id_Employee);
                    break;
                case "employee_desc":
                    record = record.OrderByDescending(s => s.Id_Employee);
                    break;
                case "services":
                    record = record.OrderBy(s => s.Id_Services);
                    break;
                case "services_desc":
                    record = record.OrderByDescending(s => s.Id_Services);
                    break;
                default: 
                    record = record.OrderBy(s => s.Date);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(record.ToPagedList(pageNumber, pageSize));
        }

        // GET: Records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: Records/Create
        public ActionResult Create()
        {
            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO");
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat");
            return View();
        }

        // POST: Records/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Services,Id_Employee,Id_Clients,Date,Time,Comment")] Record record)
        {
            var clients = from s in db.Clients select s;
            var Name = Request["Clients.Name"].ToString();
            var Surname = Request["Clients.Surname"].ToString();
            var Middle = Request["Clients.Middle"].ToString();
            var c = clients.Where(s => s.Name.Contains(Name)
                    & s.Surname.Contains(Surname)
                    & s.Middle.Contains(Middle)).FirstOrDefault();
            try
            {
                if (c != null)
                {
                    if (ModelState.IsValid)
                    {
                        record.Id_Clients = c.Id;
                        db.Record.Add(record);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Клиента не существует");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Запись уже существует.");
            }

            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO", record.Id_Employee);
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat", record.Id_Services);
            return View(record);
        }

        // GET: Records/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO");
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat");
            return View(record);
        }

        // POST: Records/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Services,Id_Employee,Id_Clients,Date,Time,Comment")] Record record)
        {
            var clients = from s in db.Clients select s;
            var Name = Request["Clients.Name"].ToString();
            var Surname = Request["Clients.Surname"].ToString();
            var Middle = Request["Clients.Middle"].ToString();
            var c = clients.Where(s => s.Name.Contains(Name)
                    & s.Surname.Contains(Surname)
                    & s.Middle.Contains(Middle)).FirstOrDefault();
            try
            {
                if (c != null)
                {
                    if (ModelState.IsValid)
                    {
                        record.Id_Clients = c.Id;
                        db.Entry(record).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Клиента не существует");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Запись уже существует.");
            }

            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO", record.Id_Employee);
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat", record.Id_Services);
            return View(record);
        }

        // GET: Records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Record.Find(id);
            db.Record.Remove(record);
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
