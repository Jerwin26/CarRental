using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CarRental.Repository
{
    public class CustomerRepo
    {
        private SqlConnection connections;
        private void connection()
        {
            string constructor = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            connections = new SqlConnection(constructor);
        }

/// <summary>
/// This method is used to for customer insert
/// </summary>
/// <param name="obj"></param>
/// <returns></returns>
        public bool InsertNewCustomer(Customer obj)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPI_InsertCustomer" /*"SPI_InsertCustomerwithlocation"*/, connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstName", obj.firstName);
                    cmd.Parameters.AddWithValue("@lastName", obj.lastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", obj.dateOfBirth);
                    cmd.Parameters.AddWithValue("@gender", obj.gender);
                    cmd.Parameters.AddWithValue("@phoneNumber", obj.phoneNumber);
                    cmd.Parameters.AddWithValue("@emailAddress", obj.emailAddress);
                    cmd.Parameters.AddWithValue("@address", obj.Address);
                    cmd.Parameters.AddWithValue("@stateName", obj.state);
                    cmd.Parameters.AddWithValue("@cityName", obj.city);
                    cmd.Parameters.AddWithValue("@username", obj.username);
                    cmd.Parameters.AddWithValue("@password", (obj.password));
                    
                    connections.Open();
                    int i = cmd.ExecuteNonQuery();
                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
               Errorlog var = new Errorlog();
                var.LogError(ex);
                return false;
            }
            finally
            {
                connections.Close(); 
            }
        }


/// <summary>
/// This method is used to viewCustomers
/// </summary>
/// <returns></returns>
        public List<Customer> ViewCustomers()
        {
            try
            {
                connection();
                List<Customer> userDetailsList = new List<Customer>();
                SqlCommand com = new SqlCommand("SPS_viewallcustomers", connections);
                com.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                connections.Open();
                da.Fill(dt);
               

                foreach (DataRow dr in dt.Rows)
                {
                    userDetailsList.Add(
                        new Customer
                        {
                            customerId = Convert.ToInt32(dr["customerId"]),
                            firstName = Convert.ToString(dr["firstName"]),
                            lastName = Convert.ToString(dr["lastName"]),
                            dateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]),
                            gender = Convert.ToString(dr["gender"]),
                            phoneNumber = Convert.ToString(dr["phoneNumber"]),
                            emailAddress = Convert.ToString(dr["emailAddress"]),
                            Address = Convert.ToString(dr["address"]),
                            state = Convert.ToString(dr["state"]),
                            city = Convert.ToString(dr["city"]),
                            username = Convert.ToString(dr["username"]),
                            password = Convert.ToString(dr["password"]),
                        }
                    );
                }

                return userDetailsList;
            }
            catch (Exception ex)
            {
                Errorlog ErrorLogger    = new Errorlog();
                ErrorLogger.LogError(ex);
                return new List<Customer>(); 
            }

            finally
            {
                connections.Close();
            }
        }
/// <summary>
/// This method is used to update the customer profile
/// </summary>
/// <param name="customer"></param>
/// <returns></returns>
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPU_UpdateCustomer", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@customerId", customer.customerId);
                    command.Parameters.AddWithValue("@firstName", customer.firstName);
                    command.Parameters.AddWithValue("@lastName", customer.lastName);
                    command.Parameters.AddWithValue("@dateOfBirth", customer.dateOfBirth);
                    command.Parameters.AddWithValue("@gender", customer.gender);
                    command.Parameters.AddWithValue("@phoneNumber", customer.phoneNumber);
                    command.Parameters.AddWithValue("@emailAddress", customer.emailAddress);
                    command.Parameters.AddWithValue("@address", customer.Address);
                    command.Parameters.AddWithValue("@state", customer.state);
                    command.Parameters.AddWithValue("@city", customer.city);
                    command.Parameters.AddWithValue("@username", customer.username);
                    command.Parameters.AddWithValue("@password", ( customer.password));

                    connections.Open();
                    int i = command.ExecuteNonQuery();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog ErrorLogger = new Errorlog();
                ErrorLogger.LogError(ex);
                return false; 
            }
            finally
            {
                connections.Close();

            }
        }
/// <summary>
/// This method is used to delete the customer by the admin
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public bool DeleteCustomer(int id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPD_DeleteCustomer", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerId", id);

                    connections.Open();
                    int i = cmd.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlog ErrorLogger = new Errorlog();
                ErrorLogger.LogError(ex);
                return false; 
            }
            finally {
                connections.Close();
            
            }
           
        }
/// <summary>
/// This moethod is used to see the customer profile
/// </summary>
/// <returns></returns>
        public List<Customer> CustomerProfile()
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                connection();

                int customerId = (int)HttpContext.Current.Session["customerId"];
                List<Customer> userDetailsList = new List<Customer>();
                SqlCommand com = new SqlCommand("SPS_GetCustomerDetail", connections);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@customerId", customerId);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                connections.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    userDetailsList.Add(
                        new Customer
                        {
                            customerId = Convert.ToInt32(dr["customerId"]),
                            firstName = Convert.ToString(dr["firstName"]),
                            lastName = Convert.ToString(dr["lastName"]),
                            dateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]),
                            gender = Convert.ToString(dr["gender"]),
                            phoneNumber = Convert.ToString(dr["phoneNumber"]),
                            emailAddress = Convert.ToString(dr["emailAddress"]),
                            Address = Convert.ToString(dr["address"]),
                            state = Convert.ToString(dr["state"]),
                            city = Convert.ToString(dr["city"]),
                            username = Convert.ToString(dr["username"]),
                            password = Convert.ToString(dr["password"]),
                        }
                    );
                }

                return userDetailsList;
            }
            catch (Exception ex)
            {
                Errorlog ErrorLogger = new Errorlog();
                ErrorLogger.LogError(ex);
                return new List<Customer>();
            }
            finally
            {
                connections.Close();

            }
        }
/// <summary>
/// This method is used to view the customer
/// </summary>
/// <returns></returns>
        public List<Customer> ViewCustomer()
        {
            try
            {
                connection();

                List<Customer> userDetailsList = new List<Customer>();
                SqlCommand com = new SqlCommand("SPS_viewallcustomers", connections);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                connections.Open();
                da.Fill(dt);
             

                foreach (DataRow dr in dt.Rows)
                {
                    userDetailsList.Add(
                        new Customer
                        {
                            customerId = Convert.ToInt32(dr["customerId"]),
                            firstName = Convert.ToString(dr["firstName"]),
                            lastName = Convert.ToString(dr["lastName"]),
                            dateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]),
                            gender = Convert.ToString(dr["gender"]),
                            phoneNumber = Convert.ToString(dr["phoneNumber"]),
                            emailAddress = Convert.ToString(dr["emailAddress"]),
                            Address = Convert.ToString(dr["address"]),
                            state = Convert.ToString(dr["state"]),
                            city = Convert.ToString(dr["city"]),
                            username = Convert.ToString(dr["username"]),
                            password = Convert.ToString(dr["password"]),
                        }
                    );
                }

                return userDetailsList;
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return new List<Customer>(); 
            }
            finally
            {
                connections.Close();
            }
        }
        /// <summary>
        /// This method is used to promote the customer  to host by the admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PromoteCustomerToHostByAdmin(int id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PromoteCustomerToHost", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerId", id);
                    connections.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connections.Close();
                    return rowsAffected >= 0;
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

        public string DecryptString(string encrString)
        {
            byte[] bytes = Convert.FromBase64String(encrString);
            string decrypted = Encoding.ASCII.GetString(bytes);
            return decrypted;
        }


        public string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }




    }
}