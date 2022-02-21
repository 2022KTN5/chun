using found.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace found.Controllers
{
    public class HomeController : Controller
    {
        MyDatabaseEntities dc = new MyDatabaseEntities();
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            //var cases=dc.Tables.
            //return View();
            var ccase = from p in (new MyDatabaseEntities()).Tables select p;
            return View(ccase);
        }
        public ActionResult money5()
        {
            var ccase = from p in (new MyDatabaseEntities()).Solutions select p;
            return View(ccase);
        }
        public ActionResult money5List()
        {
            var ccase = from p in (new MyDatabaseEntities()).Solutions select p;
            return View(ccase);
        }
        public ActionResult mailthefounder()
        {
            return View();
        }
        public ActionResult supportMoney5List()
        {
            var ccase = from p in (new MyDatabaseEntities()).Solutions select p;
            return View(ccase);
        }
        public ActionResult supportMoney5()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            //MyDatabaseEntities db = new MyDatabaseEntities();
            var sol = dc.Solutions.FirstOrDefault(p=>p.sId==id);
            if (sol == null)
                return RedirectToAction("supportMoney5List");
            return View(sol);
        }
        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult QA()
        {
            return View();
        }
        public ActionResult caseOne()
        {
            //using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //{ 
            //ViewBag.total =dc.Tables.where(a=>a.fId==1)
            //}

            int fiid = 1;
            var tot = dc.Tables.Where(m => m.fId == fiid).FirstOrDefault();
            ViewBag.tot = tot.accMoney;
            
            //fId = 1;
            //var tot = dc.Tables.Where(m => m.fId == fiid).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult caseOne(Table tableacc)
        {
            //using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //{ 
            //ViewBag.total =dc.Tables.where(a=>a.fId==1)
            //}

            //int fId = 1;
            //var tot = dc.Tables.Where(m => m.fId == 1).FirstOrDefault();
            //tot.accMoney +=1688;
            //ViewBag.tot = tot.accMoney;

            var totalacc = dc.Tables.Where(m => m.fId == 1).FirstOrDefault();
            //totalacc.accMoney += tableacc.accMoney;
            totalacc.accMoney += 1688;
            ViewBag.total = totalacc.accMoney;
            dc.SaveChanges();
            return RedirectToAction("caseOne");
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
        public ActionResult List()
        {
            //List<Table> list = null;
            //var dbacc =[];
            //dbacc=from p in (new MyDatabaseEntities()).Tables select p.accMoney;
            //ViewBag.acc = dbacc;

            var ccase = from p in (new MyDatabaseEntities()).Tables select p;
            
            
                return View(ccase);
        }
        public ActionResult popular()
        {
            var ccase = from p in (new MyDatabaseEntities()).Tables orderby p.accMoney/p.targetMoney descending select p;
            return View(ccase);
        }
        public ActionResult latest()
        {
            var ccase = from p in (new MyDatabaseEntities()).Tables orderby p.startTime descending select p;
            return View(ccase);
        }
    }
}