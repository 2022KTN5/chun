using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using REpassword.Models;

namespace REpassword.Controllers
{
    public class UserController : Controller
    {
        // Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        // Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User user) 
        {
            bool Status = false;
            string Message = "";
            //
            //Model Validation
            if (ModelState.IsValid)
            {
                #region //Email is already Exist
                var isExist =IsEmailExist(user.EmailID);
                if (isExist) {
                    ModelState.AddModelError("EmailExist","Email already exist");
                    return View(user);
                }
                #endregion

                #region//Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region//Password Hashing
                user.Password = Crypto.Hash(user.ConfirmPassword);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.IsEmailVerified = false;
                #region//Save data to Database
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    Message = "OK"+user.EmailID;
                    Status = true;
                }
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }

            ViewBag.Message=Message;
            ViewBag.Status = Status;
            return View(user);
        }
        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.Users.Where(a=>a.ActivationCode==new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = true;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login,string ReturnUrl="") 
        {
            string message = "";
            using (MyDatabaseEntities dc=new MyDatabaseEntities()) 
            {
                var v = dc.Users.Where(a=>a.EmailID==login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password),v.Password)==0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index","Home");
                        }
                    }
                    else
                    {
                        message = "Invalid";
                    }
                }
                else
                {
                    message = "Invalid";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","User");
        }

        [NonAction]
        public bool IsEmailExist(string emailID) 
        {
            using (MyDatabaseEntities dc=new MyDatabaseEntities()) 
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]//"miniwei1011@gmail.com","Passw0rd-iii"
        public void SendVerificationLinkEmail(string emailID,string activationCode,string emailFor= "VerifyAccount")
        {
            var verifyUrl = "/User/"+emailFor+"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("miniwei1011@gmail.com","WEIWEI");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Passw0rd-iii";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "OK";
                body = "<br/><br/>aa" + "<br/><br/><a href='" + link + "'>" + link + "</a>";
            }
            else if (emailFor=="ResetPassword") 
            {
                subject = "reset Password";
                body = "<br/><br/>reset Password" + "<br/><br/><a href="+link+">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            { 
            Host="smtp.gmail.com",
            Port=587,
            EnableSsl=true,
            DeliveryMethod=SmtpDeliveryMethod.Network,
            UseDefaultCredentials=false,
            Credentials=new NetworkCredential(fromEmail.Address,fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link
            //Send Email
            string message = "";
            bool status = false;

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var account = dc.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send Email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailID,resetCode,"ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "has been sent";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
                return View();
        }
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model) 
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc=new MyDatabaseEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password update successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
    
}