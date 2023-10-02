using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class HostController : Controller
    {

/// <summary>
/// This method is to add the host by admin
/// </summary>
/// <returns></returns>
        [HttpGet]
        public ActionResult addHosts()
        {
            StateandCityRepository stateandCityRepository = new StateandCityRepository();

            List<StateandCityModel> states = stateandCityRepository.statelist();

            ViewBag.States = new SelectList(states, "stateid", "statename");

            return View();
        }

       

        [HttpPost]
        public ActionResult addHosts(HostModel hostmodel)
        {
            StateandCityRepository cityRepository = new StateandCityRepository();
            HostRepo customerRepo = new HostRepo();
            List<StateandCityModel> states = cityRepository.statelist();

            ViewBag.States = new SelectList(states, "stateid", "statename");
            customerRepo.Insertnewhost(hostmodel);
            TempData["SuccessMessage"] = "Successfully registered!";
            return RedirectToAction("HostIndex");

        }

/// <summary>
/// This method is used to add the vehicle by the host
/// </summary>
/// <param name="customerId"></param>
/// <returns></returns>
        [HttpGet]
        public ActionResult AddVehicleByHost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVehicleByHost(Vehicle vehicle, HttpPostedFileBase image,int id)
        {
            HostRepo HostRepo = new HostRepo();
            HostRepo.InsertVehiclebyhost(vehicle, image,id);
            return View();
        }

/// <summary>
/// This Method is used to get the new hos request
/// </summary>
/// <returns></returns>
        [HttpGet]
        public ActionResult NewHostRequests()
        {
            StateandCityRepository stateandCityRepository = new StateandCityRepository();

            List<StateandCityModel> states = stateandCityRepository.statelist();

            ViewBag.States = new SelectList(states, "stateid", "statename");

            ViewBag.Cities = new SelectList(new List<SelectListItem>(), "Value", "Text");

            return View();
        }



        [HttpPost]
        public ActionResult NewHostRequests(NewHostModel hostmodel)
        {
            StateandCityRepository CityRepository = new StateandCityRepository();
            HostRepo hostRepo = new HostRepo();
            List<StateandCityModel> states = CityRepository.statelist();
            ViewBag.States = new SelectList(states, "stateid", "statename");
            hostRepo.UnknownHostRequests(hostmodel);
            TempData["SuccessMessage"] = "Successfully registered!";
            return RedirectToAction("NewHostRequests", new { success = true });

        }

        // GET: Host
        [Authorize]
        public ActionResult HostIndex()
        {
            return View();
        }

    /// <summary>
    /// This method is used to View the page for host polices
    /// </summary>
    /// <returns></returns>
        public ActionResult Hostspolices()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ViewHostDetails()
        {
            HostRepo hostRepo = new HostRepo();
            List<HostModel> hosts = hostRepo.GetHostDetails();

            return View(hosts);
        }
        [Authorize]
        public ActionResult ViewHost()
        {
            HostRepo hostRepo = new HostRepo();
            List<HostModel> hosts = hostRepo.GetHostDetails();

            return View(hosts);
        }


        // GET: Host/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Host/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Host/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// This method is used to Edit the host profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Edithost(int id)
        {
            HostRepo edit = new HostRepo();
            return View(edit.GetHostDetails().Find(obj => obj.customerId == id));
        }

        [HttpPost]
        public ActionResult Edithost(HostModel obj)
        {
            try
            {
                HostRepo edit = new HostRepo();
                edit.Updatehost(obj);
                return RedirectToAction("HostIndex");
            }
            catch
            {
                return View();
            }

        }

/// <summary>
/// This method is used to delete the host by the admin
/// </summary>
/// <param name="id"></param>
/// <param name="del"></param>
/// <returns></returns>
        public ActionResult Deletehost(int id, HostRepo del)
        {               
            try
            {
                HostRepo repo = new HostRepo();
                if (repo.DeleteHost(id))
                {
                    ViewBag.alert = "sucess";
                }
                return RedirectToAction("ViewHostDetails");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("ViewHostDetails");

            }
        } 
        
       

        // GET: Host/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Host/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

/// <summary>
/// This method is used to promote host to customer by admin    
/// </summary>
/// <param name="id"></param>
/// <param name="del"></param>
/// <returns></returns>
        public ActionResult PromoteHostToCustomerByAdmin(int id, HostRepo del)
        {
            try
            {
                HostRepo hostRepo = new HostRepo();
                if (hostRepo.PromoteHostToCustomerByAdmin(id))
                {
                    ViewBag.alert = "sucess";
                }
                return RedirectToAction("ViewHostDetails");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("ViewHostDetails");

            }
        }
    }
}
