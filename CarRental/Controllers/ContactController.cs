using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{

    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewEnquiries()
        {
            contactRepo contactRepo = new contactRepo();    
            List<Contact> contacts = contactRepo.ViewEnquires();
            return View(contacts);
        }
        /// <summary>
        /// This method is used to insert the contact
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult InsertContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertContact(Contact contact)
        {
            contactRepo contactRepo = new contactRepo();

            contactRepo.insertContact(contact);

            return RedirectToAction("InsertContact");


        }

        /// <summary>
        /// This method is used to clear all the Enquires
        /// </summary>
        /// <param name="del"></param>
        /// <returns></returns>

        public ActionResult DeleteContact( contactRepo del)
        {
            try
            {
                contactRepo contactRepo = new contactRepo();
                if (contactRepo.DeleteContact())
                {
                    ViewBag.alert = "sucess";
                }
                return RedirectToAction("ViewEnquiries");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("ViewEnquiries");

            }
        }

    }
}