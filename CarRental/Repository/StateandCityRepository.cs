using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CarRental.Repository
{
    public class StateandCityRepository
    {
        private SqlConnection connections;

        /// <summary>
        /// This method establish the database connection 
        /// </summary>
        private void connection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                connections = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        /// <summary>
        /// This method is used to show the statelist
        /// </summary>
        /// <returns></returns>
        public List<StateandCityModel> statelist()
        {
            List<StateandCityModel> statelist = new List<StateandCityModel>();
            DataTable datatable = new DataTable(); 

            try
            {
                connection();

                using (SqlCommand com = new SqlCommand("SPS_States", connections))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(com);

                    connections.Open();
                    da.Fill(datatable);
                }

                foreach (DataRow dr in datatable.Rows) 
                {
                    statelist.Add(
                        new StateandCityModel
                        {
                            stateid = Convert.ToInt32(dr["stateId"]),
                            statename = Convert.ToString(dr["stateName"])
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
            }
            finally
            {
                if (connections != null && connections.State != ConnectionState.Closed)
                {
                    connections.Close();
                }
            }

            return statelist;
        }

        /// <summary>
        /// This method is used to show the citylist
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<StateandCityModel> Citylist(int sid)
        {
            List<StateandCityModel> Citylist = new List<StateandCityModel>();

            try
            {
                connection();
                DataTable dt = new DataTable(); 

                using (SqlCommand com = new SqlCommand("SPS_Cities", connections))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@stateId", sid);
                    SqlDataAdapter da = new SqlDataAdapter(com);

                    connections.Open();
                    da.Fill(dt);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    Citylist.Add(
                        new StateandCityModel
                        {
                            cityid = Convert.ToInt32(dr["cityId"]),
                            cityname = Convert.ToString(dr["cityName"]),
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
               
            }
            finally
            {
                if (connections != null && connections.State != ConnectionState.Closed)
                {
                    connections.Close();
                }
            }

            return Citylist;
        }
    }
}
