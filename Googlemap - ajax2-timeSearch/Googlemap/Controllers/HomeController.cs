using Googlemap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Googlemap.Controllers
{
    
    public class HomeController : Controller
    {
        mappEntities1 contextMAP = new mappEntities1();
        public ActionResult Index()
        {
            ViewBag.lat = 22.628026;
            ViewBag.lng = 120.293009;
            IEnumerable<SelectListItem> objMemberSelectListItem =
                (from p in contextMAP.theMap
                 where p.tele != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.tele });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");
            //if(){ }
            return View();
        }
        [HttpPost]
        //public ActionResult Index(DateTime dt)
        //{
        //    if (dt == null)
        //    {
        //        IEnumerable<SelectListItem> objMemberSelectListItem =
        //        (from p in contextMAP.theMap
        //         where p.tele != null
        //         select p).ToList()
        //         .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.tele });
        //        ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");
        //        return View();
        //    }
        //    else
        //    {
        //        ViewBag.start = dt;
        //        var datepicker = contextMAP.theMap.Where(x => x.dateFrom <= dt).ToList();
        //        return View(datepicker);
        //    }
        //}
        public ActionResult Index(DateTime dt)
        {
            IEnumerable <SelectListItem> objMemberSelectListItem =
                (from p in contextMAP.theMap
                 where p.tele != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.tele });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");
            if (dt == null)
            {
                return View();
            }
            else
            {
                //    theMap betweendate = contextMAP.theMap.FirstOrDefault(p => p.dateFrom == dt);
                //ViewBag.a = betweendate;
                //    return View(betweendate);
                ViewBag.lat = contextMAP.theMap.FirstOrDefault(p => p.dateFrom == dt).lat;
                ViewBag.lng = contextMAP.theMap.FirstOrDefault(p => p.dateFrom == dt).lng;
                return View();
            }
        }
        public ActionResult List()
        {
            //IEnumerable<theMap> themap = null;
            //DateTime keyword = Request.Form["txtKeyword"];
            //if (string.IsNullOrEmpty(keyword))
            //    themap = from p in (new mappEntities1()).theMap select p;
            //else
            //    themap = from p in (new mappEntities1()).theMap where p.dateFrom == keyword select p;
            //return View(themap);
           



            ViewBag.lat = 22.628026;
            ViewBag.lng = 120.293009;
            IEnumerable<SelectListItem> objMemberSelectListItem =
                (from p in contextMAP.theMap
                 where p.tele != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.tele });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");
            
            var selected = from p in (new mappEntities1()).theMap select p;
            return View(selected);//if(){ }
            //return View();
        }
        [HttpPost]
        public ActionResult List(DateTime? txtKeyword)
        {
            IEnumerable<SelectListItem> objMemberSelectListItem =
                (from p in contextMAP.theMap
                 where p.tele != null
                 select p).ToList()
                 .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.tele });
            ViewBag.SelectListItem = new SelectList(objMemberSelectListItem, "Value", "Text", "");

            IEnumerable<theMap> themap = 
                (from p in contextMAP.theMap
                select p).ToList();
            
            if (txtKeyword == null)
            {
                themap = from p in contextMAP.theMap select p;
                return View(themap);
            }
            else {
                themap = from p in contextMAP.theMap 
                         where p.dateFrom <= txtKeyword 
                         && p.dateEnd>=txtKeyword
                         select p;
                return View(themap);
            }
        }
        public ActionResult Edit(int id)
        {
            theMap mapEdit = contextMAP.theMap.FirstOrDefault(p=>p.Id==id);
            ViewBag.xx = mapEdit.lat;
            ViewBag.yy = mapEdit.lng;
            if (mapEdit == null)
                return RedirectToAction("List");
            return View(mapEdit);
        }
        //[HttpPost]
        //public ActionResult List(DateTime dt)
        //{
        //    var selected = from p in (new mappEntities1()).theMap select p;
        //    if (dt == null)
        //    {
        //        return View(selected);
        //    }
        //    else {
        //        ViewBag.lat = contextMAP.theMap.FirstOrDefault(p => p.dateFrom == dt).lat;
        //        ViewBag.lng = contextMAP.theMap.FirstOrDefault(p => p.dateFrom == dt).lng;

        //        return View();
        //    }
        //}
        public ActionResult queryByDate(DateTime dt)
        {
            mappEntities1 db = new mappEntities1();
            theMap betweendate = db.theMap.FirstOrDefault(p=>p.dateFrom<=dt);
            return View(betweendate);
        }
        public JsonResult GetAllLocation()
        {
            var data = contextMAP.theMap.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLocation(DateTime? txtKeyword)
        {
            var data = (from p in contextMAP.theMap select p).ToList();
            if (txtKeyword == null)
            {
                data = (from p in contextMAP.theMap select p).ToList();
                //return View(themap);
            }
            else
            {
                data = (from p in contextMAP.theMap
                         where p.dateFrom <= txtKeyword
                         && p.dateEnd >= txtKeyword
                         select p).ToList();
                //return View(themap);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}