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
    public class HostRepo
    {
        private SqlConnection connections;

        /// <summary>
        /// This method is used to establish the database connection
        /// </summary>
        private void connection()
        {
            string constructor = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            connections = new SqlConnection(constructor);
        }
        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            if (image != null)
            {
                byte[] imageBytes = new byte[image.ContentLength];
                image.InputStream.Read(imageBytes, 0, image.ContentLength);
                return imageBytes;
            }
            return null;
        }

        /// <summary>
        /// This method is used to insert the new host by admin
        /// </summary>
        /// <param name="hostmodel"></param>
        /// <returns></returns>
        public bool Insertnewhost(HostModel hostmodel)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPI_AddHost", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstName", hostmodel.firstName);
                    cmd.Parameters.AddWithValue("@lastName", hostmodel.lastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", hostmodel.dateOfBirth);
                    cmd.Parameters.AddWithValue("@gender", hostmodel.gender);
                    cmd.Parameters.AddWithValue("@phoneNumber", hostmodel.phoneNumber);
                    cmd.Parameters.AddWithValue("@emailAddress", hostmodel.emailAddress);
                    cmd.Parameters.AddWithValue("@address", hostmodel.Address);
                    cmd.Parameters.AddWithValue("@state", hostmodel.state);
                    cmd.Parameters.AddWithValue("@city", hostmodel.city);
                    cmd.Parameters.AddWithValue("@username", hostmodel.username);
                    cmd.Parameters.AddWithValue("@password", (hostmodel.password));


                    connections.Open();
                    int i = cmd.ExecuteNonQuery();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog error = new Errorlog();
                error.LogError(ex);
                return false;
            }
            finally
            {
                connections.Close();

            }
        }
        /// <summary>
        /// This method is used to insert vehicle by host
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="image"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool InsertVehiclebyhost(Vehicle vehicle, HttpPostedFileBase image, int id)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPI_Insertvehiclebyhost", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@customerID_fk",id);
                    command.Parameters.AddWithValue("@brand", vehicle.brand);
                    command.Parameters.AddWithValue("@licensePlate", vehicle.licensePlate);
                    command.Parameters.AddWithValue("@fuelType", vehicle.fuelType);
                    command.Parameters.AddWithValue("@vehicleType", vehicle.vehicleType);
                    command.Parameters.AddWithValue("@vehiclePrice", vehicle.vehiclePrice);
                    if (image != null && image.ContentLength > 0)
                    {
                        byte[] vehiclePictureBytes = ConvertToBytes(image);
                        command.Parameters.AddWithValue("@vehicleImage", vehiclePictureBytes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@vehicleImage", DBNull.Value);
                    }


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

        /// <summary>
        /// This method is used to get host details
        /// </summary>
        /// <returns></returns>
        public List<HostModel> GetHostDetails()
        {
            connection();
            List<HostModel> userDetailsList = new List<HostModel>();
            SqlCommand com = new SqlCommand("SPS_ViewHostDetails", connections);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataTable dt = new DataTable();

            connections.Open();
            da.Fill(dt);
            connections.Close();

            foreach (DataRow dr in dt.Rows)
            {
                userDetailsList.Add(
                    new HostModel
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
/// <summary>
/// This method is used to update the host details
/// </summary>
/// <param name="host"></param>
/// <returns></returns>
        public bool Updatehost(HostModel host)
        {
            try
            {
                connection();
                using (SqlCommand command = new SqlCommand("SPU_UpdateHost", connections))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@customerId", host.customerId);
                    command.Parameters.AddWithValue("@firstName", host.firstName);
                    command.Parameters.AddWithValue("@lastName", host.lastName);
                    command.Parameters.AddWithValue("@dateOfBirth", host.dateOfBirth);
                    command.Parameters.AddWithValue("@gender", host.gender);
                    command.Parameters.AddWithValue("@phoneNumber", host.phoneNumber);
                    command.Parameters.AddWithValue("@emailAddress", host.emailAddress);
                    command.Parameters.AddWithValue("@address", host.Address);
                    command.Parameters.AddWithValue("@state", host.state);
                    command.Parameters.AddWithValue("@city", host.city);
                    command.Parameters.AddWithValue("@username", host.username);

                    command.Parameters.AddWithValue("@password",host.password);

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
                if (connections != null && connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }

/// <summary>
/// This method is used to delete the host
/// </summary>
/// <param name="id"></param>
/// <returns></returns>

        public bool DeleteHost(int id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPD_Deletehost", connections))
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
                Errorlog errorlog= new Errorlog();
                errorlog.LogError(ex);
                return false;
                
            }
            finally
            {
                if (connections != null && connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }


/// <summary>
/// This method is used to promote host to customer
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public bool PromoteHostToCustomerByAdmin(int id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PromoteHostToCustomer", connections))
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
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                if (connections != null && connections.State == ConnectionState.Open)
                {
                    connections.Close();
                }
            }
        }

/// <summary>
/// This method is used to get the users request
/// </summary>
/// <param name="hostmodel"></param>
/// <returns></returns>

        public bool UnknownHostRequests(NewHostModel hostmodel)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPI_AddUnknownHost", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstName", hostmodel.firstName);
                    cmd.Parameters.AddWithValue("@lastName", hostmodel.lastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", hostmodel.dateOfBirth);
                    cmd.Parameters.AddWithValue("@gender", hostmodel.gender);
                    cmd.Parameters.AddWithValue("@phoneNumber", hostmodel.phoneNumber);
                    cmd.Parameters.AddWithValue("@emailAddress", hostmodel.emailAddress);
                    cmd.Parameters.AddWithValue("@address", hostmodel.Address);
                    cmd.Parameters.AddWithValue("@state", hostmodel.state);
                    cmd.Parameters.AddWithValue("@city", hostmodel.city);
                    cmd.Parameters.AddWithValue("@username", hostmodel.username);
                    cmd.Parameters.AddWithValue("@password", hostmodel.password);

                    connections.Open();
                    int i = cmd.ExecuteNonQuery();
                    

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog  errorlog = new Errorlog();
                errorlog.LogError(ex);
                return false;
            }
            finally
            {
                connections.Close();
            }
        }
/// <summary>
/// This method is used to get the new host request
/// </summary>
/// <returns></returns>
        public List<NewHostModel> GetHostRequestNewHost()
        {
            connection();
            List<NewHostModel> HostRequesList = new List<NewHostModel>();
            SqlCommand com = new SqlCommand("SPS_GetHostRequest", connections);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataTable dt = new DataTable();

            connections.Open();
            da.Fill(dt);
            connections.Close();

            foreach (DataRow dr in dt.Rows)
            {
                HostRequesList.Add(
                    new NewHostModel
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

            return HostRequesList;
        }


        public string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }




    }

}
