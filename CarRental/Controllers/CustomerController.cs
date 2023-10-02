using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using CarRental.Repository; 
namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepo customerRepo;
        string message = "access denied";
       
        [Authorize]
        public ActionResult CustomerIndex()
        {
            return View();
        }
        public CustomerController()
        {
            customerRepo = new CustomerRepo();
        }

        public ActionResult ViewCustomers()
        {
            List<Customer> customers = customerRepo.ViewCustomers();
            return View(customers);
        }

        /// <summary>
        /// This method is used to get the view page of customer registration form
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult InsertCustomer()
        {
            StateandCityRepository stateandCityRepository = new StateandCityRepository();

            List<StateandCityModel> states = stateandCityRepository.statelist();

            ViewBag.States = new SelectList(states, "stateid", "statename");

            return View();


        }
        /// <summary>
        /// This message is to View the cities
        /// </summary>
        /// <param name="stateid"></param>
        /// <returns></returns>
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
        /// This method is used to register as customer 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertCustomer(Customer obj)
        {
            CustomerRepo customerRepo = new CustomerRepo();           
            StateandCityRepository cityRepository = new StateandCityRepository();
            customerRepo.InsertNewCustomer(obj);
             List<StateandCityModel> states = cityRepository.statelist();
            ViewBag.States = new SelectList(states, "stateid", "statename");
            TempData["SuccessMessage"] = "Successfully registered!";
            return RedirectToAction("Login","Login");
        }

        /// <summary>
        /// This method is used to Edit the customerProfile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            CustomerRepo edit = new CustomerRepo();
            return View(edit.ViewCustomers().Find(obj => obj.customerId == id));
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer obj)
        {
            try
            {
                CustomerRepo edit = new CustomerRepo();
                edit.UpdateCustomer(obj);
                return RedirectToAction("CustomerIndex");
            }
            catch
            {
                return View();
            }

        }
/// <summary>
/// This method is used to delete the customer
/// </summary>
/// <param name="id"></param>
/// <param name="del"></param>
/// <returns></returns>
        public ActionResult DeleteCustomer(int id, CustomerRepo del)
        {
            try
            {
                CustomerRepo repo = new CustomerRepo();
                if (repo.DeleteCustomer(id))
                {
                    ViewBag.alert = "sucess";
                }
                return RedirectToAction("ViewCustomer");
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("ViewCustomer");

            }
        }
/// <summary>
/// This method is used to view the customer
/// </summary>
/// <returns></returns>
        public ActionResult ViewCustomer()
        {
            List<Customer> customers = customerRepo.ViewCustomer();
            return View(customers);
        }
        
        public ActionResult CustomerProfile()
        {
            List<Customer> customers = customerRepo.CustomerProfile();
            return View(customers);
        }
/// <summary>
/// This method is used to promote the customer to host by admin
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public ActionResult PromoteCustomerToHostByAdmin(int id)
        {
            try
            {
                CustomerRepo repo = new CustomerRepo();
                if (repo.PromoteCustomerToHostByAdmin(id))
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

            return RedirectToAction("ViewCustomer");
        }

    }
}

