using Auth.Models;
using Auth.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Controllers
{
    public class firstController : Controller
    {
        // GET: first
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            //string keyword = Request.Form["txtKeyword"];
            List<Class1> list = null;
            //if (string.IsNullOrEmpty(keyword))
            list = (new Class1_factory()).queryAll();
            //else
            //    list = (new Class1_factory()).queryByKeyword(keyword);
            return View(list);
        }
        public ActionResult displayCharityMemberById(string id)
        {
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=shopping;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "select * from Charity_member1 where cId=" + id, con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Class1 x = new Class1();
                    x.cId = (int)reader["cId"];
                    x.c_name = reader["c_name"].ToString();
                    x.c_address = reader["c_address"].ToString();
                    x.c_account = reader["c_account"].ToString();
                    x.c_password = reader["c_password"].ToString();
                    x.c_phone = reader["c_phone"].ToString();
                    x.c_email = reader["c_email"].ToString();
                    x.c_pname = reader["c_pname"].ToString();
                    ViewBag.KK = x;
                }
                con.Close();
            }
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save()
        {
            Class1 x = new Class1();
            x.c_name = Request.Form["txtc_name"];
            x.c_address = Request.Form["txtc_address"];
            x.c_account = Request.Form["txtc_account"];
            x.c_password = Request.Form["txtc_password"];
            x.c_phone = Request.Form["txtc_phone"];
            x.c_email = Request.Form["txtc_email"];
            x.c_pname = Request.Form["txtc_pname"];

            (new Class1_factory()).create(x);
            return RedirectToAction("List");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                (new Class1_factory()).delete((int)id);
            }
            return RedirectToAction("List");
        }
        public ActionResult Edit(int? id)
        {
            Class1 x = null;
            if (id != null)
                x = (new Class1_factory()).queryBycid((int)id);
            return View(x);
        }
        [HttpPost]
        public ActionResult Edit(Class1 x)
        {
            //CCustomer x = new CCustomer();
            //x.fName = Request.Form["txtName"];
            //x.fPhone = Request.Form["txtPhone"];
            //x.fEmail = Request.Form["txtEmail"];
            //x.fAddress = Request.Form["txtAddress"];
            //x.fPassword = Request.Form["txtPassword"];

            (new Class1_factory()).update(x);
            return RedirectToAction("List");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LloginViewModel p)
        {
            Class1 cust = (new Class1_factory()).queryLogIn(p.txtAccount);
            if (cust != null)
            {
                if (cust.c_password.Equals(p.txtPassword))
                {
                    Session[CDictionary.SK_LOGINED_USER] = cust;
                    return RedirectToAction("Home");
                }
            }
            return View();
        }
        //public ActionResult RePassword()
        //{
        //Class1 reP = new Class1();
        //reP.c_email = Request.Form["RetxtEmail"];

        //if (reP != null)
        //return View(); 
        //else
        //  return RedirectToAction("Login");
        //}
        //[HttpPost]
        //public ActionResult RePassword()
        //{
        //  Class1 reP = new Class1();
        //reP.c_email = Request.Form["RetxtEmail"];

        //if (reP != null)
        //{ return View(); }
        //else
        //  return RedirectToAction("Login");
        // }
        public ActionResult RePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RePassword(string id)
        {
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=shopping;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "select * from Charity_member1 where cId=" + id, con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Class1 x = new Class1();
                    Request.Form["RetxtEmail"] = reader["c_email"].ToString();
                    ViewBag.KK = x;
                }
                con.Close(); return View();
            }
            else
                return RedirectToAction("Login");
        }
        public ActionResult Home()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
                return RedirectToAction("Login");
            return View();
        }
        
    }
}