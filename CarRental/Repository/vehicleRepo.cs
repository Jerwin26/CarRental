using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CarRental.Repository
{
    public class vehicleRepo
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
        /// This method is used to getvehicledetails
        /// </summary>
        /// <returns></returns>

        public List<Vehicle> GetVehicleDetails()
        {
            connection();
            List<Vehicle> GetVehicleDetails = new List<Vehicle>();
            SqlCommand command = new SqlCommand("SPS_ViewVehicles", connections);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connections.Open();
            dataAdapter.Fill(dataTable);
            connections.Close();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                GetVehicleDetails.Add(
                    new Vehicle
                    {
                        vehicleID = Convert.ToInt32(dataRow["vehicleID"]),
                        brand = Convert.ToString(dataRow["brand"]),
                        licensePlate = Convert.ToString(dataRow["licensePlate"]),
                        fuelType = Convert.ToString(dataRow["fuelType"]),
                        vehicleType = Convert.ToString(dataRow["vehicleType"]),
                        vehiclePrice = Convert.ToDecimal(dataRow["vehiclePrice"]),
                        vehicleStatus = Convert.ToString(dataRow["vehicleStatus"]),
                        vehicleImage = dataRow["vehicleImage"] as byte[]
                    }
                );
            }

            return GetVehicleDetails;
        }

        /// <summary>
        /// This method is used to update the vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool UpdateVehicle(Vehicle vehicle, HttpPostedFileBase image)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPU_UpdateVehicle", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vehicleID", vehicle.vehicleID);
                command.Parameters.AddWithValue("@brand", vehicle.brand);
                command.Parameters.AddWithValue("@licensePlate", vehicle.licensePlate);
                command.Parameters.AddWithValue("@fuelType", vehicle.fuelType);
                command.Parameters.AddWithValue("@vehicleType", vehicle.vehicleType);
                command.Parameters.AddWithValue("@vehiclePrice", vehicle.vehiclePrice);
                command.Parameters.AddWithValue("@vehicleStatus", vehicle.vehicleStatus);
                byte[] vehiclePictureBytes = ConvertToBytes(image);
                command.Parameters.AddWithValue("@vehicleImage", vehiclePictureBytes);

                connections.Open();
                int i = command.ExecuteNonQuery();
                connections.Close();
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

        /// <summary>
        /// This method is used to Insert the vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool InsertVehicle(Vehicle vehicle, HttpPostedFileBase image)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPI_InsertVehicle", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@brand", vehicle.brand);
                    command.Parameters.AddWithValue("@licensePlate", vehicle.licensePlate);
                    command.Parameters.AddWithValue("@fuelType", vehicle.fuelType);
                    command.Parameters.AddWithValue("@vehicleType", vehicle.vehicleType);
                    command.Parameters.AddWithValue("@vehiclePrice", vehicle.vehiclePrice);
                    //command.Parameters.AddWithValue("@vehicleStatus", vehicle.vehicleStatus);
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
        /// This method is used to delete the vehicle
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        public bool DeleteVehicle(int vehicleID)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPD_DeleteVehicle", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@vehicleID", vehicleID);

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
      /// This method is used to reject the host vehicle 
      /// </summary>
      /// <param name="Id"></param>
      /// <returns></returns>      
        public bool RejectHostVehicle(int Id)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPU_RejectHostVehicleRequestByAdmin", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@VehicleID", Id);

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
/// This method is used to Get all the available vehicles
/// </summary>
/// <param name="pickDate"></param>
/// <param name="dropDate"></param>
/// <returns></returns>

        public List<Vehicle> GetAvailableVehicles(DateTime pickDate, DateTime dropDate)
        {
            connection();
            List<Vehicle> availableVehicles = new List<Vehicle>();

            using (SqlCommand command = new SqlCommand("SPS_GetAvailableVehicles", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PickDate", pickDate);
                command.Parameters.AddWithValue("@DropDate", dropDate);

                try
                {
                    connections.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        availableVehicles.Add(new Vehicle
                        {
                            vehicleID = Convert.ToInt32(reader["vehicleID"]),
                            brand = Convert.ToString(reader["brand"]),
                            licensePlate = Convert.ToString(reader["licensePlate"]),
                            fuelType = Convert.ToString(reader["fuelType"]),
                            vehicleType = Convert.ToString(reader["vehicleType"]),
                            vehiclePrice = Convert.ToDecimal(reader["vehiclePrice"]),
                            vehicleImage = reader["vehicleImage"] as byte[]
                        });
                    }

                    reader.Close();
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
            }

            return availableVehicles;
        }
        /// <summary>
        /// This method is used to insert the host vehicles
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool InsertVehiclebyhost(Vehicle vehicle, HttpPostedFileBase image)
        {
            connection();
            using (SqlCommand command = new SqlCommand("SPI_Insertvehiclebyhost", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    //command.Parameters.AddWithValue("@customerID_fk", vehicle.customerId);
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
        /// This method is used to gethostVehiclesapproval
        /// </summary>
        /// <returns></returns>
        public List<HostVehicleApprovalModel> GetHostVehiclesApprovalAdmin()
        {
            connection();
            List<HostVehicleApprovalModel> approvedHostedVehicles = new List<HostVehicleApprovalModel>();

            using (SqlCommand command = new SqlCommand("SPS_GetHostVehiclesApproval", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connections.Open();
                dataAdapter.Fill(dataTable);
                connections.Close();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    approvedHostedVehicles.Add(
                        new HostVehicleApprovalModel
                        {
                            CustomerID = (int)dataRow["customerId"],
                            VehicleID = (int)dataRow["VehicleID"],
                            Brand = dataRow["brand"].ToString(),
                            VehicleImage = dataRow["vehicleImage"] as byte[],
                            FuelType = dataRow["fuelType"].ToString(),
                            LicensePlate = dataRow["licensePlate"].ToString(),
                            HostingVehicleApprovalStatus = dataRow["HostingVehicleApprovalStatus"].ToString(),
                            VehiclePrice = (decimal)dataRow["vehiclePrice"],
                            VehicleStatus = dataRow["vehicleStatus"].ToString(),
                            FirstName = dataRow["firstName"].ToString(),
                            LastName = dataRow["lastName"].ToString()
                        }
                    );
                }
            }

            return approvedHostedVehicles;
        }

        /// <summary>
        /// This method is used to get the hostvehiclesstatus
        /// </summary>
        /// <returns></returns>
        public List<HostVehicleApprovalModel> GetHostVehiclesStatus()
        {
            connection();
            List<HostVehicleApprovalModel> HostVehicleStatus = new List<HostVehicleApprovalModel>();           
            using (SqlCommand command = new SqlCommand("SPS_GetHostVehiclesApproval", connections))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                connections.Open();
                dataAdapter.Fill(dataTable);
                connections.Close();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    HostVehicleStatus.Add(
                        new HostVehicleApprovalModel
                        {
                            VehicleID = (int)dataRow["vehicleID"],
                            Brand = dataRow["brand"].ToString(),
                            VehicleImage = dataRow["vehicleImage"] as byte[],
                            FuelType = dataRow["fuelType"].ToString(),
                            LicensePlate = dataRow["licensePlate"].ToString(),
                            HostingVehicleApprovalStatus = (string)dataRow["HostingVehicleApprovalStatus"],
                            VehiclePrice = (decimal)dataRow["vehiclePrice"],
                            VehicleStatus = dataRow["vehicleStatus"].ToString(),
                            CustomerID = (int)dataRow["customerId"],
                            FirstName = dataRow["firstName"].ToString(),
                            LastName = dataRow["lastName"].ToString()
                        }
                    );
                }
            }

            return HostVehicleStatus;
        }

        /// <summary>
        /// This method is used to get the Host vehicle request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AcceptHostVehicleRequest(int id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SPU_AcceptHostVehicleRequestByAdmin", connections))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vehicleID", id);

                    connections.Open();
                    int i = cmd.ExecuteNonQuery();
                    connections.Close();

                    return i >= 1;
                }
            }
            catch (Exception ex)
            {
                Errorlog error = new Errorlog();
                error.LogError(ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<HostVehicleApprovalModel> GetHostVehiclesStatusById()
        {
            connection();
            List<HostVehicleApprovalModel> HostVehicleStatus = new List<HostVehicleApprovalModel>();

            using (SqlCommand command = new SqlCommand("[SPS_HostStatusById]", connections))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (HttpContext.Current.Session["customerId"] != null)
                {
                    int customerId = (int)HttpContext.Current.Session["customerId"];

                    command.Parameters.Add(new SqlParameter("@customer_ID", SqlDbType.Int));
                    command.Parameters["@customer_ID"].Value = customerId;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    connections.Open();
                    dataAdapter.Fill(dataTable);
                    connections.Close();

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        HostVehicleStatus.Add(
                            new HostVehicleApprovalModel
                            {
                                VehicleID = (int)dataRow["vehicleID"],
                                Brand = dataRow["brand"].ToString(),
                                VehicleImage = dataRow["vehicleImage"] as byte[],
                                FuelType = dataRow["fuelType"].ToString(),
                                LicensePlate = dataRow["licensePlate"].ToString(),
                                HostingVehicleApprovalStatus = (string)dataRow["HostingVehicleApprovalStatus"],
                                VehiclePrice = (decimal)dataRow["vehiclePrice"],
                                VehicleStatus = dataRow["vehicleStatus"].ToString(),
                                CustomerID = (int)dataRow["customerId"],
                                FirstName = dataRow["firstName"].ToString(),
                                LastName = dataRow["lastName"].ToString()
                            }
                        );
                    }
                }
                else
                {
                    
                }
            }

            return HostVehicleStatus;
        }
    }
}