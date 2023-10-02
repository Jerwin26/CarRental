using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class bookRideController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult carBooking(int User_id, int Vehicle_id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult carBooking(BookRide obj, int User_id, int Vehicle_id)
        {
            bookRideRepo bookRide = new bookRideRepo();
            bookRide.GetVehicleAndPaymentDetails(obj, User_id, Vehicle_id);
            return View();
        }

        /// <summary>
        /// This method is used to  pay and book the car by the customer
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="Vehicle_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult book(int User_id, int Vehicle_id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult book(BookRide obj, int User_id, int Vehicle_id)
        {
            bookRideRepo bookRide = new bookRideRepo();
            bookRide.InsertRide(obj, User_id, Vehicle_id);
            return View();
        }
        /// <summary>
        /// This method is used to accept the car booking
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>

        public ActionResult AcceptBooking(int rideId)
        {
            bookRideRepo bookRide = new bookRideRepo();
            bool isAccepted = bookRide.AcceptBooking(rideId);

            if (isAccepted)
            {

                ViewBag.Message = "Booking accepted successfully!";
                return RedirectToAction("ViewBookedCustomers");
            }
            else
            {

                ViewBag.Message = "Booking acceptance failed!";
            }

            return View();
        }

        /// <summary>
        /// This method is used to reject the car booking from the cusotmer
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        public ActionResult RejectBooking(int rideId)
        {
            bookRideRepo bookRide = new bookRideRepo();
            bool isRejected = bookRide.RejectBooking(rideId);

            if (isRejected)
            {

                ViewBag.Message = "Booking rejected successfully!";
                return RedirectToAction("ViewBookedCustomers");
            }
            else
            {

                ViewBag.Message = "Booking rejection failed!";
                
            }

            return View();
        } 
        /// <summary>
        /// This method is used to set the vehicle status to available 
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        public ActionResult rentVehicleSubmitted(int rideId)
        {
            bookRideRepo bookRide = new bookRideRepo();
            bool isVehicleSubmitted = bookRide.vehicleSubmitted(rideId);

            if (isVehicleSubmitted)
            {

                ViewBag.Message = "Booking rejected successfully!";
                return RedirectToAction("ViewBookedCustomers");
            }
            else
            {

                ViewBag.Message = "Booking rejection failed!";
                
            }

            return View();
        }
        /// <summary>
        /// This method is used to view the bookedCustomers
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewBookedCustomers()
        {
            bookRideRepo bookRideRepo = new bookRideRepo();
            var bookedCustomers = bookRideRepo.GetBookedCustomerDetails();

            return View(bookedCustomers);


        }
        /// <summary>
        /// This method is used to see the bookings of customer
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBookings( )
        {
            bookRideRepo bookRideRepo = new bookRideRepo();
            var bookedCustomers = bookRideRepo.GetBookedCustomerDetailsById();
            return View(bookedCustomers);
        }

        /// <summary>
        /// To view the history
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovalHistory()
        {
            bookRideRepo bookRideRepo = new bookRideRepo();
            var approvalHistory = bookRideRepo.GetApprovalHistory();

            return View(approvalHistory);

        }

/// <summary>
/// This method is used to clear the Bookride 
/// </summary>
/// <returns></returns>
        public ActionResult ClearBookRide()
        {
            try
            {
                bookRideRepo repo = new bookRideRepo();
                if (repo.ClearBookRide())
                {
                    ViewBag.alert = "Sucess";
                }
                return RedirectToAction("ViewBookedCustomers");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error Occured while deleting the record: " + ex.Message;
                return RedirectToAction("ViewBookedCustomers");
            }
        }
    }
}