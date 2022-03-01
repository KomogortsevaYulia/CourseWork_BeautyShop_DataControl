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

namespace Web1.Controllers
{
    public class EmployeesController : Controller
    {
        private BeautyShopEntities1 db = new BeautyShopEntities1();
        // GET: Employees
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SurameSortParm = String.IsNullOrEmpty(sortOrder) ? "Surname_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.MiddleSortParm = sortOrder == "middle" ? "middle_desc" : "middle";
            ViewBag.PhoneSortParm = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.BirhdateSortParm = sortOrder == "birhdate" ? "birhdate_desc" : "birhdate";
            ViewBag.PostSortParm = sortOrder == "post" ? "post_desc" : "post";
            ViewBag.CommentSortParm = sortOrder == "comment" ? "comment_desc" : "comment";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var employee = from s in db.Employee
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {

                employee = employee.Where(s => s.Name.Contains(searchString)
                || s.Surname.Contains(searchString)
                || s.Middle.Contains(searchString)
                || s.Post.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Surname_desc":
                    employee = employee.OrderByDescending(s => s.Surname);
                    break;
                case "name":
                    employee = employee.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    employee = employee.OrderByDescending(s => s.Name);
                    break;
                case "middle":
                    employee = employee.OrderBy(s => s.Middle);
                    break;
                case "middle_desc":
                    employee = employee.OrderByDescending(s => s.Middle);
                    break;
                case "phone":
                    employee = employee.OrderBy(s => s.Phone);
                    break;
                case "phone_desc":
                    employee = employee.OrderByDescending(s => s.Phone);
                    break;
                case "post":
                    employee = employee.OrderBy(s => s.Post);
                    break;
                case "post_desc":
                    employee = employee.OrderByDescending(s => s.Post);
                    break;
                case "comment":
                    employee = employee.OrderBy(s => s.Comment);
                    break;
                case "comment_desc":
                    employee = employee.OrderByDescending(s => s.Comment);
                    break;
                case "birhdate":
                    employee = employee.OrderBy(s => s.Birhdate);
                    break;
                case "birhdate_desc":
                    employee = employee.OrderByDescending(s => s.Birhdate);
                    break;
                default:  // Name ascending 
                    employee = employee.OrderBy(s => s.Surname);
                    break;
            }
            int pageSize =  10;
            int pageNumber = (page ?? 1);
            return View(employee.ToPagedList(pageNumber, pageSize));
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Surname,Name,Middle,Phone,Birhdate,Post,Comment")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var e=db.Employee.Where(s => s.Phone.Equals(((decimal)employee.Phone)));
                    if (e.Any())
                    {
                        ModelState.AddModelError("", "Сотрудник с таким номером телефона уже существует.");
                    }
                    else {
                        db.Employee.Add(employee);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Сотрудник уже существует.");
                }
                
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Surname,Name,Middle,Phone,Birhdate,Post,Comment")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Сотрудник уже существует.");
                }
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
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
