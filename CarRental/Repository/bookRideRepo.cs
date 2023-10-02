using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;


namespace CarRental.Repository
{
    public class bookRideRepo
    {
        private SqlConnection connections;

        /// <summary>
        /// This method Establish the Database connection 
        /// </summary>
        private void connection()
        {
            string constructor = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            connections = new SqlConnection(constructor);
        }

        /// <summary>
        /// This method is used to book the car by the customer
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="User_id"></param>
        /// <param name="Vehicle_id"></param>
        /// <returns></returns>
        public bool InsertRide(BookRide bookRide, int User_id, int Vehicle_id)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPI_InsertRide", connections))
                   {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@vehicleID_FK", Vehicle_id);
                    command.Parameters.AddWithValue("@customerId_Fk", User_id);
                    command.Parameters.AddWithValue("@PaymentMethod", bookRide.PaymentMethod);
                    command.Parameters.AddWithValue("@fare", bookRide.Fare);
                    command.Parameters.AddWithValue("@pickDate", bookRide.PickDate);
                    command.Parameters.AddWithValue("@dropDate", bookRide.DropDate);
                    command.Parameters.AddWithValue("@ApprovalStatus", bookRide.ApprovalStatus);

                    connections.Open();
                    int i = command.ExecuteNonQuery();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
               return false;
            }
            finally {
                connections.Close();
           }
        }

        /// <summary>
        /// This method is used to accept the booked car from the admin side
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        public bool AcceptBooking(int rideId)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPU_AcceptBooking", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RideId", rideId);

                    connections.Open();
                    int i = cmd.ExecuteNonQuery();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                throw ex;
            }
            finally
            {
                connections.Close();

            }
        }

        /// <summary>
        /// This method is used to Reject the Booked Car by the admin
        /// </summary>
        /// <param name="rideId"></param>
        /// <returns></returns>
        public bool RejectBooking(int rideId)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPD_CancelBooking", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RideId", rideId);

                    connections.Open();
                    int i = command.ExecuteNonQuery();

                    return i >= 1;
                }   
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                connections.Close();
            }
        }   
     /// <summary>
     /// This method is used to set the vehicle status not available to available
     /// </summary>
     /// <param name="rideId"></param>
     /// <returns></returns>     
        public bool vehicleSubmitted(int rideId)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPI_vehicleSubmitted", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RideId", rideId);

                    connections.Open();
                    int i = cmd.ExecuteNonQuery();
                    
                    return i >= 1;
                }   
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally {
                connections.Close();

            }
        }

        public List<Customer> GetBookedCustomerDetails()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPS_GetBookedCustomerDetails", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    connections.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Customer customer = new Customer
                        {
                            customerId = Convert.ToInt32(rdr["customerId"]),
                            firstName = Convert.ToString(rdr["firstName"]),
                            lastName = Convert.ToString(rdr["lastName"]),
                            dateOfBirth = Convert.ToDateTime(rdr["dateOfBirth"]),
                            gender = Convert.ToString(rdr["gender"]),
                            phoneNumber = Convert.ToString(rdr["phoneNumber"]),
                            emailAddress = Convert.ToString(rdr["emailAddress"]),
                            Address = Convert.ToString(rdr["address"]),
                            state = Convert.ToString(rdr["state"]),
                            city = Convert.ToString(rdr["city"]),
                            username = Convert.ToString(rdr["username"]),
                            password = Convert.ToString(rdr["password"]),
                            rideId = Convert.ToInt32(rdr["rideId"]),
                            vehicleID_FK = Convert.ToInt32(rdr["vehicleID_FK"]),
                            fare = Convert.ToDecimal(rdr["fare"]),
                            PaymentMethod = Convert.ToString(rdr["PaymentMethod"]),
                            pickDate = Convert.ToDateTime(rdr["pickDate"]),
                            dropDate = Convert.ToDateTime(rdr["dropDate"]),
                            ApprovalStatus = Convert.ToString(rdr["ApprovalStatus"])
                        };
                        customers.Add(customer);
                    }
                    connections.Close();
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);       
            }
            finally { 
            
                    connections.Close();         
            }
            return customers;
        }

        public bool GetVehicleAndPaymentDetails(BookRide obj, int User_id, int Vehicle_id)
        {
            try
            { 
        connection();
                using (SqlCommand cmd = new SqlCommand("SPS_GetVehicleAndPaymentDetails", connections))
                {
                    int customerId = (int)HttpContext.Current.Session["customerId"];
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vehicleID_FK", Vehicle_id);
                    cmd.Parameters.AddWithValue("@customerId_Fk", User_id);
                    cmd.Parameters.AddWithValue("@PaymentMethod", obj.PaymentMethod);
                    cmd.Parameters.AddWithValue("@fare", obj.Fare);
                    cmd.Parameters.AddWithValue("@pickDate", obj.PickDate);
                    cmd.Parameters.AddWithValue("@dropDate", obj.DropDate);
                    cmd.Parameters.AddWithValue("@ApprovalStatus", obj.ApprovalStatus);
                    cmd.Parameters.AddWithValue("@VehicleID", obj.VehicleId);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@LicensePlate", obj.LicensePlate);
                    cmd.Parameters.AddWithValue("@VehiclePrice", obj.VehiclePrice);
                    cmd.Parameters.AddWithValue("@FuelType", obj.FuelType);
                    cmd.Parameters.AddWithValue("@VehicleType", obj.VehicleType);
                    cmd.Parameters.AddWithValue("@VehiclePrice", obj.VehiclePrice);
                    cmd.Parameters.AddWithValue("@VehicleImage", obj.VehicleImage);
                    cmd.Parameters.AddWithValue("@VehicleStatus", obj.VehicleStatus);
                    connections.Open();
                    int i = cmd.ExecuteNonQuery();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                connections.Close();

            }
        }

        public List<ApprovalHistoryModel> GetApprovalHistory()
        {
            List<ApprovalHistoryModel> approvals = new List<ApprovalHistoryModel>();

            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPS_GetApprovalHistory", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    connections.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        ApprovalHistoryModel approvalHistory = new ApprovalHistoryModel
                        {
                            RideId = Convert.ToInt32(rdr["RideId"]),
                            customerId_FK = Convert.ToInt32(rdr["customerId_FK"]),
                            vehicleID_FK = Convert.ToInt32(rdr["vehicleID_FK"]),
                            CustomerFirstName = Convert.ToString(rdr["CustomerFirstName"]),
                            CustomerLastName = Convert.ToString(rdr["CustomerLastName"]),
                            Fare = Convert.ToDecimal(rdr["Fare"]),
                            PaymentMethod = Convert.ToString(rdr["PaymentMethod"]),
                            PickDate = Convert.ToDateTime(rdr["pickDate"]),
                            DropDate = Convert.ToDateTime(rdr["dropDate"]),
                            ApprovalStatus = Convert.ToString(rdr["ApprovalStatus"]),
                            VehicleBrand = Convert.ToString(rdr["VehicleBrand"]),
                            VehicleLicensePlate = Convert.ToString(rdr["VehicleLicensePlate"]),
                            VehicleImage = rdr["vehicleImage"] as byte[]
                        };

                        approvals.Add(approvalHistory);
                    }

                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
            }
            finally {
                connections.Close();
            }
            return approvals;
        }

        public List<Customer> GetBookedCustomerDetailsById( )
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPS_Mybooking", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    int id = (int)HttpContext.Current.Session["customerId"];
                    cmd.Parameters.AddWithValue("@customerId", id); 

                    connections.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Customer customer = new Customer
                        {
                            customerId = Convert.ToInt32(rdr["customerId"]),
                            firstName = Convert.ToString(rdr["firstName"]),
                            lastName = Convert.ToString(rdr["lastName"]),
                            dateOfBirth = Convert.ToDateTime(rdr["dateOfBirth"]),
                            gender = Convert.ToString(rdr["gender"]),
                            phoneNumber = Convert.ToString(rdr["phoneNumber"]),
                            emailAddress = Convert.ToString(rdr["emailAddress"]),
                            Address = Convert.ToString(rdr["address"]),
                            state = Convert.ToString(rdr["state"]),
                            city = Convert.ToString(rdr["city"]),
                            rideId = Convert.ToInt32(rdr["rideId"]),
                            vehicleID_FK = Convert.ToInt32(rdr["vehicleID_FK"]),
                            fare = Convert.ToDecimal(rdr["fare"]),
                            PaymentMethod = Convert.ToString(rdr["PaymentMethod"]),
                            pickDate = Convert.ToDateTime(rdr["pickDate"]),
                            dropDate = Convert.ToDateTime(rdr["dropDate"]),
                            ApprovalStatus = Convert.ToString(rdr["ApprovalStatus"])
                        };

                        customers.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                
            }
            finally
            {
                connections.Close();

            }

            return customers;
        }


        public bool ClearBookRide()
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPD_ClearBookRide", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                  

                    connections.Open();
                    int i = command.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Errorlog errorlog = new Errorlog(); 
                    errorlog.LogError(ex);
                    return false;
                }
                finally
                {
                    connections.Close();
                }
            }
        }


    }
}