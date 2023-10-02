using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarRental.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string role;
                    int id;
                    string username;
                    LoginRepo loginRepo = new LoginRepo();
                    loginRepo.Login(login, out role, out id, out username);
                    
                        if (role == "0")
                        {
                            FormsAuthentication.SetAuthCookie(username, false);
                            return RedirectToAction("CustomerIndex", "Customer", new { id = id });
                        }
                        else if (role == "1")
                        {
                            FormsAuthentication.SetAuthCookie(username, false);
                            return RedirectToAction("adminIndex", "Admin", new { id = id });
                        }
                        else if (role == "2")
                        {
                            FormsAuthentication.SetAuthCookie(username, false);
                            return RedirectToAction("HostIndex", "Host", new { id = id });
                        }
                        else
                        {
                            ViewBag.Message = "Invalid username and password";
                        }
                    }
                return View(login);
            }
            catch (Exception ex)
            {             
                return View(login);
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}