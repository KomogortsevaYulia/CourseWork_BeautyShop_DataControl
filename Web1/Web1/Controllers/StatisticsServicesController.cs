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
   
    public class StatisticsServicesController : Controller
    { 
        private BeautyShopEntities1 db = new BeautyShopEntities1();

        public ViewResult Index()
        {
            return View();
        }

        // GET: Statistics
        public ActionResult Details(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            } 
            String d = null;
            try {
                Convert.ToInt32(searchString);
                d = searchString;
            }
            catch {
                ModelState.AddModelError("", "Вы ввели не число");
                TempData["msg"] = "1";
                return View();
            } 
            ViewBag.CurrentFilter = searchString;
            if ((String.IsNullOrEmpty(searchString) && page == 1 )|| (Convert.ToInt32(searchString) > 2050) || (Convert.ToInt32(searchString) <1900))
            {
                TempData["msg"] = "1";
                return View();
            }
            else
            {
                TempData["msg"] = "";
            }
           
            
            object[] parameters = {
                    new SqlParameter("@date1",SqlDbType.VarChar) {Value=d}                
                };
            db.Database.CommandTimeout = 360;
            IEnumerable<SP1_Result> query = db.Database.SqlQuery<SP1_Result>("SP1 @date1", parameters).ToList();
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