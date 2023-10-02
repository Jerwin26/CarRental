using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarRental.Repository;
using CarRental.Models;
using System.Web;

namespace CarRental.Controllers
{
    public class VehicleController : Controller 
    {
        private vehicleRepo vehicleRepo; 

        public VehicleController()
        {
            vehicleRepo = new vehicleRepo(); 
        }
/// <summary>
/// To get the vehicle details
/// </summary>
/// <returns></returns>
        public ActionResult GetVehicleDetails()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            return View(vehicleRepo.GetVehicleDetails());

        } 
  /// <summary>
  /// This method is used to manage the vehicle details
  /// </summary>
  /// <returns></returns>  
        public ActionResult manageVehicleDetails()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            return View(vehicleRepo.GetVehicleDetails());
        }
/// <summary>
/// This method is used to convert the image into binary
/// </summary>
/// <param name="vehicleImage"></param>
/// <returns></returns>
        private bool IsImage(HttpPostedFileBase vehicleImage)
        {
            if (vehicleImage != null && vehicleImage.ContentLength > 0)
            {
                string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif" };
                string contentType = vehicleImage.ContentType;

                return allowedImageTypes.Contains(contentType);
            }
            return false;
        }

/// <summary>
/// This  method is used to add the vehicle
/// </summary>
/// <returns></returns>
        [HttpGet]
        public ActionResult AddVehicle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVehicle(Vehicle vehicle, HttpPostedFileBase image)
        {
            vehicleRepo usersRepo = new vehicleRepo();
            vehicleRepo.InsertVehicle(vehicle, image);
            return View();
        }

        /// <summary>
        /// Deletes a vehicle record with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        public ActionResult DeleteVehicle(int id, vehicleRepo delete)
        {
            try
            {
                vehicleRepo repo = new vehicleRepo();
                if (repo.DeleteVehicle(id))
                {
                    ViewBag.alert = "Sucess";
                }

                return RedirectToAction("manageVehicleDetails");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;

                return RedirectToAction("manageVehicleDetails");


            }
        }
        /// <summary>
        /// Handles the rejection of a host vehicle request.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RejectHostVehicleRequest(int id)
        {
            vehicleRepo HostVehicleRequest = new vehicleRepo();
            bool isAccepted = HostVehicleRequest.RejectHostVehicle(id);

            if (isAccepted)
            {

                ViewBag.Message = "Booking accepted successfully!";
                return RedirectToAction("HostVehiclesApprovalAdmin");
            }
            else
            {

                ViewBag.Message = "Booking acceptance failed!";
            }

            return View();
        }
        /// <summary>
        /// Retrieve the vehicle details with the specified ID and pass it to the view.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditVehicle(int Id)
        {
            vehicleRepo edit = new vehicleRepo();
            return View(edit.GetVehicleDetails().Find(obj => obj.vehicleID == Id));
        }

        /// <summary>
        /// Update the vehicle details and optionally the image.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditVehicle(Vehicle obj, HttpPostedFileBase image)
        {
            try
            {
                vehicleRepo edit = new vehicleRepo();
                edit.UpdateVehicle(obj, image);
                return RedirectToAction("manageVehicleDetails");

            }
            catch
            {
                return View();
            }

        }
/// <summary>
/// This method is used to get the availabkeVehicls
/// </summary>
/// <returns></returns>
        public ActionResult AvailableVehicles()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            DateTime pickDate = DateTime.Now;
            DateTime dropDate = DateTime.Now; 

            var availableVehicles = vehicleRepo.GetAvailableVehicles(pickDate, dropDate);

            return View(availableVehicles);
        }

        //public ActionResult ApprovedHostedVehicles()
        //{
        //    vehicleRepo vehicleRepo = new vehicleRepo();
        //    List<Vehicle> approvedHostedVehicles = vehicleRepo.GetApprovedHostedVehiclesByAdmin();

        //    return View(approvedHostedVehicles);
        //}

        //public ActionResult ApprovedHostedVehicles()
        //{
        //    vehicleRepo vehicleRepo = new vehicleRepo();
        //    return View(vehicleRepo.GetApprovedHostedVehiclesByAdmin());

        //}


/// <summary>
/// This method is used to accept the hostVehiclesapproval
/// </summary>
/// <returns></returns>
        public ActionResult HostVehiclesApprovalAdmin()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            var approvedHostedVehicles = vehicleRepo.GetHostVehiclesApprovalAdmin();
            return View(approvedHostedVehicles);
        }  
   /// <summary>
   /// This method is used to hostVehiclestatus
   /// </summary>
   /// <returns></returns>   
        public ActionResult HostVehiclesStatus()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            var HostedVehicleStatus = vehicleRepo.GetHostVehiclesStatus();
            return View(HostedVehicleStatus);
        }
/// <summary>
/// This method is used to addVehiclebyhost
/// </summary>
/// <returns></returns>
        [HttpGet]
        public ActionResult AddVehicleByHost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVehicleByHost(Vehicle vehicle, HttpPostedFileBase image)
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            vehicleRepo.InsertVehiclebyhost(vehicle, image);
            return View();
        }
/// <summary>
/// This method is used to accept the hostVehiclerequest
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public ActionResult AcceptHostVehicleRequest(int id)
        {
            vehicleRepo HostVehicleRequest = new vehicleRepo();
            bool isAccepted = HostVehicleRequest.AcceptHostVehicleRequest(id);

            if (isAccepted)
            {

                ViewBag.Message = "Booking accepted successfully!";
                return RedirectToAction("HostVehiclesApprovalAdmin");
            }
            else
            {

                ViewBag.Message = "Booking acceptance failed!";
            }

            return View();
        }

        /// <summary>
        /// This method is used to see the host status
        /// </summary>
        /// <returns></returns>
        public ActionResult HostStatusById()
        {
            vehicleRepo vehicleRepo = new vehicleRepo();
            var HostedVehicleStatus = vehicleRepo.GetHostVehiclesStatusById();
            return View(HostedVehicleStatus);
        }


    }
}
