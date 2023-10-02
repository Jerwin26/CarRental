using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace CarRental.Repository
{
    public class AdminRepo
    {
        public Encrypt encrypt = new Encrypt();

        private SqlConnection connections;
        /// <summary>
        /// This method establishes a database connection.
        /// </summary>
        private void connection()
        {
            string constructor = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            connections = new SqlConnection(constructor);
        }

        /// <summary>
        ///  This method is used to create a new admin
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool addAdmin(Customer customer)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPI_AddAdmin", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
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
                    command.Parameters.AddWithValue("@password", customer.password);
                    connections.Open();
                    int i = command.ExecuteNonQuery();
                    connections.Close();

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
                if (connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }
        /// <summary>
        /// This method is used to retrieve all the admin from the database 
        /// </summary>
        /// <returns></returns>

        public List<Customer> GetAdmins()
        {
            List<Customer> userDetailsList = new List<Customer>();
            try
            {
                connection(); 
                SqlCommand command = new SqlCommand("SPS_GetAdminDetails", connections);
                command.CommandType = CommandType.StoredProcedure;
                connections.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    userDetailsList.Add(
                        new Customer
                        {
                            customerId = Convert.ToInt32(dataRow["customerId"]),
                            firstName = Convert.ToString(dataRow["firstName"]),
                            lastName = Convert.ToString(dataRow["lastName"]),
                            dateOfBirth = Convert.ToDateTime(dataRow["dateOfBirth"]),
                            gender = Convert.ToString(dataRow["gender"]),
                            phoneNumber = Convert.ToString(dataRow["phoneNumber"]),
                            emailAddress = Convert.ToString(dataRow["emailAddress"]),
                            Address = Convert.ToString(dataRow["address"]),
                            state = Convert.ToString(dataRow["state"]),
                            city = Convert.ToString(dataRow["city"]),
                            username = Convert.ToString(dataRow["username"]),
                            password = Convert.ToString(dataRow["password"]),
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError( ex );
                throw;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }

            return userDetailsList;
        }
        
        /// <summary>
        /// This method is used to update the details of the admin
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public bool UpdateAdmin(Customer customer)
        {
            try
            {
                connection(); 

                using (SqlCommand command = new SqlCommand("SPU_UpdateAdmin", connections))
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
                    command.Parameters.AddWithValue("@password", customer.password);

                    connections.Open();
                    int i = command.ExecuteNonQuery();
                    connections.Close();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                throw;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }


        /// <summary>
        /// This method is used to delete the admin 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAdmin(int id)
        {
            try
            {
                connection();

                using (SqlCommand command = new SqlCommand("SPD_DeleteAdmin", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@customerId", id);

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
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }

        /// <summary>
        /// This method is used to update the customer to host
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateHostByAdmin(int id)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPU_UpdateToHost", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@customerId", id);
                    connections.Open();
                    int rowsAffected = command.ExecuteNonQuery();

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

        /// <summary>
        /// This method is use to reject the New Host request from the unknown
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RejectNewHost(int id)
        {
            try
            {
                connection(); 
                using (SqlCommand command = new SqlCommand("SPD_DeleteHost", connections)) 
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@customerId", id);
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
            }
            catch (Exception ex)
            {
               Errorlog errorlog= new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }

        /// <summary>
        /// This method is used to Encrypt the Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}