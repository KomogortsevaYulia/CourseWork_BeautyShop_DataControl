using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web1.Models;

namespace Web1.Controllers
{
   
    public class StatisticsEmployeesController : Controller
    { 
        private BeautyShopEntities1 db = new BeautyShopEntities1();

        public ViewResult Index()
        {
            return View();
        }

        
        // GET: Statistics
        public ActionResult Details(string currentFilter, string searchString ,int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            if ((String.IsNullOrEmpty(searchString) && page == 1))
            {
                TempData["msg"] = "1";
                return View();
            }
            else if (!String.IsNullOrEmpty(searchString) && searchString.Length > 1) {
                ModelState.AddModelError("", "Вы ввели не букву");
                TempData["msg"] = "1";
                return View();
            }
            else
            {
                TempData["msg"] = "";
            }
            string d1 = searchString;

            object[] parameters = {
                new SqlParameter("@date",SqlDbType.VarChar) {Value=d1}
            };
            db.Database.CommandTimeout = 360;
            IEnumerable<SP2_Result> query = db.Database.SqlQuery<SP2_Result>("SP2 @date", parameters).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
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