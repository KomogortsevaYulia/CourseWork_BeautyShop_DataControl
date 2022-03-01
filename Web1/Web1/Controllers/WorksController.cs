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
    public class WorksController : Controller
    {
        private BeautyShopEntities1 db = new BeautyShopEntities1();

        // GET: Works
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewBag.TimeSortParm = sortOrder == "time" ? "time_desc" : "time";
            ViewBag.ClientsSortParm = sortOrder == "clients" ? "clients_desc" : "clients";
            ViewBag.IncomeSortParm = sortOrder == "income" ? "income_desc" : "income";
            ViewBag.EmployeeSortParm = sortOrder == "employee" ? "employee_desc" : "employee";
            ViewBag.ServicesSortParm = sortOrder == "services" ? "services_desc" : "services";

            var work = from s in db.Work
                         select s;
            switch (sortOrder)
            {
                case "Date_desc":
                    work = work.OrderByDescending(s => s.Date);
                    break;
                case "time":
                    work = work.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    work = work.OrderByDescending(s => s.Time);
                    break;
                case "income":
                    work = work.OrderBy(s => s.Income);
                    break;
                case "income_desc":
                    work = work.OrderByDescending(s => s.Income);
                    break;
                case "clients":
                    work = work.OrderBy(s => s.Id_Clients);
                    break;
                case "clients_desc":
                    work = work.OrderByDescending(s => s.Id_Clients);
                    break;
                case "employee":
                    work = work.OrderBy(s => s.Id_Employee);
                    break;
                case "employee_desc":
                    work = work.OrderByDescending(s => s.Id_Employee);
                    break;
                case "services":
                    work = work.OrderBy(s => s.Id_Services);
                    break;
                case "services_desc":
                    work = work.OrderByDescending(s => s.Id_Services);
                    break;
                default:  // Name ascending 
                    work = work.OrderBy(s => s.Date);
                    break;
            }
            int pageSize =  10;
            int pageNumber = (page ?? 1);
            return View(work.ToPagedList(pageNumber, pageSize));
        }

        
        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        public ActionResult Create()
        {
            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO");
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat");
            return View();
        }

        // POST: Works/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Services,Id_Employee,Date,Time,Income,Comment")] Work work)
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
                        work.Id_Clients = c.Id;
                        db.Work.Add(work);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else {
                    ModelState.AddModelError("", "Клиента не существует");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Работа уже существует.");
            }


            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO", work.Id_Employee);
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat", work.Id_Services);
            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO", work.Id_Employee);
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat", work.Id_Services);
            return View(work);
        }

        // POST: Works/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Services,Id_Employee,Id_Clients,Date,Time,Income,Comment")] Work work)
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
                        work.Id_Clients = c.Id;
                        db.Entry(work).State = EntityState.Modified;
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
                ModelState.AddModelError("", "Работа уже существует.");
            }

            ViewBag.Id_Employee = new SelectList(db.Employee, "Id", "FIO", work.Id_Employee);
            ViewBag.Id_Services = new SelectList(db.Services, "Id", "Concat", work.Id_Services);
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Work.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Work.Find(id);
            db.Work.Remove(work);
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
