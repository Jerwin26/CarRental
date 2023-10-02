using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class AdminController : Controller
    {

        /// <summary>
        /// Retrieves a list of administrators from the AdminRepo and renders a view with the list of administrators.
        /// </summary>
        /// <returns>Returns an ActionResult containing the list of administrators to be displayed in a view.</returns>
        public ActionResult GetAdmins()
        {
            AdminRepo adminRepo = new AdminRepo();
            List<Customer> admins = adminRepo.GetAdmins(); 
            return View(admins);
        }


        /// <summary>
        /// Displays the admin index page, restricting access to authorized users only.
        /// </summary>
        /// <returns>Returns an ActionResult representing the admin index view.</returns>
        [Authorize]
        public ActionResult adminIndex()
        {
            return View();
        }

        /// <summary>
        /// Retrieves a list of cities based on the specified state ID and returns them as JSON data.
        /// </summary>
        /// <param name="stateid">The ID of the state for which to retrieve the list of cities.</param>
        /// <returns>Returns a JsonResult containing a list of SelectListItem objects representing cities.</returns>
        public ActionResult Cities(int stateid)
        {
            StateandCityRepository stateandCityRepository = new StateandCityRepository();

            List<StateandCityModel> cities = stateandCityRepository.Citylist(stateid);
            var cityList = cities.Select(city => new SelectListItem
            {
                Value = city.cityid.ToString(),
                Text = city.cityname
            }).ToList();

            return Json(cityList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Displays the page for adding a new admin. Requires authorization.
        /// </summary>
        /// <returns>Returns an ActionResult representing the addAdmin view with a list of states for selection.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult addAdmin()
        {
            StateandCityRepository stateandCityRepository = new StateandCityRepository();

            List<StateandCityModel> states = stateandCityRepository.statelist();

            ViewBag.States = new SelectList(states, "stateid", "statename");
            return View();
        }
        /// <summary>
        /// Handles the POST request for adding a new admin.
        /// </summary>
        /// <param name="customer">The Customer object representing the new admin to be added.</param>
        /// <returns>Returns a RedirectToAction result, redirecting to the adminIndex action.</returns>
        [HttpPost]
        public ActionResult addAdmin(Customer customer)
        {
            AdminRepo adminRepo = new AdminRepo();
            StateandCityRepository cityRepository = new StateandCityRepository();
            adminRepo.addAdmin(customer);
            List<StateandCityModel> states = cityRepository.statelist();
            ViewBag.States = new SelectList(states, "stateid", "statename");
            return RedirectToAction("adminIndex");

        }

        /// <summary>
        /// This method is used to update the admin details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
      
       [HttpGet]
        [Authorize]
        public ActionResult updateAdmin(int id)
        {
            AdminRepo edit = new AdminRepo();
            return View(edit.GetAdmins().Find(obj => obj.customerId == id));
        }

        [HttpPost]
        public ActionResult updateAdmin(Customer admin)
        {
            try
            {
                AdminRepo edit = new AdminRepo();
                edit.UpdateAdmin(admin);
                return RedirectToAction("adminIndex");
            }
            catch
            {
                return View();
            }

        }

        /// <summary>
        /// This method is to get the new host request
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult hostRequests()
        {
            try
            {
                HostRepo hostRepo = new HostRepo();
                List<NewHostModel> hosts = hostRepo.GetHostRequestNewHost();

                return View(hosts);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("An error occurred: " + ex.Message);
                return View("Error");
            }
        }


        /// <summary>
        /// This method is used to accept the host from the  admin 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Authorize]
        public ActionResult Updatetohostbyadmin(int id)
        {
            try
            {
                AdminRepo repo = new AdminRepo();
                if (repo.UpdateHostByAdmin(id))
                {
                    TempData["SuccessMessage"] = "Host updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update host.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the host: " + ex.Message;
            }

            return RedirectToAction("hostRequests");



        }

        /// <summary>
        /// This method is used to reject the host request from the admin 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="del"></param>
        /// <returns></returns>

        public ActionResult RejectHostRequest(int id, AdminRepo del)
        {
            try
            {
                AdminRepo repo = new AdminRepo();
                if (repo.RejectNewHost(id))
                {
                    ViewBag.alert = "sucess";
                }
                return RedirectToAction("hostRequests");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("hostRequests");

            }
        }
    }
}
