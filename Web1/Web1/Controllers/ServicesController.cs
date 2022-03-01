using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web1.Models;

namespace Web1.Controllers
{
    public class ServicesController : Controller
    {
        private BeautyShopEntities1 db = new BeautyShopEntities1();
        // GET: Services

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.CommentSortParm = sortOrder == "comment" ? "comment_desc" : "comment";
            if (searchString != null)
            { page = 1; }
            else
            { searchString = currentFilter; }

            ViewBag.CurrentFilter = searchString;


            var services = from s in db.Services
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.Name.Contains(searchString)
                || s.Comment.Contains(searchString) || s.Price.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    services = services.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    services = services.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    services = services.OrderByDescending(s => s.Price);
                    break;
                case "comment":
                    services = services.OrderBy(s => s.Comment);
                    break;
                case "comment_desc":
                    services = services.OrderByDescending(s => s.Comment);
                    break;
                default:  // Name ascending 
                    services = services.OrderBy(s => s.Name);
                    break;
            }
            int pageSize =  10;
            int pageNumber = (page ?? 1);
            return View(services.ToPagedList(pageNumber, pageSize));
        }
            // GET: Services/Details/5
        public ActionResult Details(string sortOrder, string currentFilter, string searchString, int? id, int? page, int? SizePage)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Comment")] Services services)
        {
            if (ModelState.IsValid)
            {
                try {
                    db.Services.Add(services);
                    db.SaveChanges();
                    return RedirectToAction("Index"); 
                } catch {
                    ModelState.AddModelError("", "Услуга уже существует.");
                }
                
            }
            return View(services);
        }


        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // POST: Services/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Comment")] Services services)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(services).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Такая услуга уже существует.");
                }
            }
            return View(services);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Services services = db.Services.Find(id);
            db.Services.Remove(services);
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
