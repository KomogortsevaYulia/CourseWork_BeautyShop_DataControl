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
    public class ClientsController : Controller
    {
        private BeautyShopEntities1 db = new BeautyShopEntities1();

        // GET: Clients
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SurameSortParm = String.IsNullOrEmpty(sortOrder) ? "Surname_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.MiddleSortParm = sortOrder == "middle" ? "middle_desc" : "middle";
            ViewBag.PhoneSortParm = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.BirhdateSortParm = sortOrder == "birhdate" ? "birhdate_desc" : "birhdate";
            ViewBag.PostSortParm = sortOrder == "email" ? "email_desc" : "email";
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


            var clients = from s in db.Clients
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.Name.Contains(searchString)
                || s.Surname.Contains(searchString)
                || s.Middle.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Surname_desc":
                    clients = clients.OrderByDescending(s => s.Surname);
                    break;
                case "name":
                    clients = clients.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.Name);
                    break;
                case "middle":
                    clients = clients.OrderBy(s => s.Middle);
                    break;
                case "middle_desc":
                    clients = clients.OrderByDescending(s => s.Middle);
                    break;
                case "phone":
                    clients = clients.OrderBy(s => s.Phone);
                    break;
                case "phone_desc":
                    clients = clients.OrderByDescending(s => s.Phone);
                    break;
                case "email":
                    clients = clients.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    clients = clients.OrderByDescending(s => s.Email);
                    break;
                case "comment":
                    clients = clients.OrderBy(s => s.Comment);
                    break;
                case "comment_desc":
                    clients = clients.OrderByDescending(s => s.Comment);
                    break;
                case "birhdate":
                    clients = clients.OrderBy(s => s.Birhdate);
                    break;
                case "birhdate_desc":
                    clients = clients.OrderByDescending(s => s.Birhdate);
                    break;
                default:  // Name ascending 
                    clients = clients.OrderBy(s => s.Surname);
                    break;
            }
            int pageSize =10;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber, pageSize));
        }
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Surname,Name,Middle,Birhdate,Email,Phone,Comment")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var e = db.Clients.Where(s => s.Phone.Equals(((decimal)clients.Phone)));
                    if (e.Any())
                    {
                        ModelState.AddModelError("", "Клиент с таким номером телефона уже существует.");
                    }
                    else
                    {
                        db.Clients.Add(clients);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Клиент уже существует.");
                }
            }

            return View(clients);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // POST: Clients/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Surname,Name,Middle,Birhdate,Email,Phone,Comment")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(clients).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Клиент уже существует.");
                }
            }
            return View(clients);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clients clients = db.Clients.Find(id);
            db.Clients.Remove(clients);
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
