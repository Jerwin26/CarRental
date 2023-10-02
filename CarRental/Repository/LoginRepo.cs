using System;
using CarRental.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Repository
{
    public class LoginRepo
    {
        private SqlConnection connections;

        public Encrypt Encrypt = new Encrypt();

        /// <summary>
        /// This method is used to establish the database connection
        /// </summary>
        private void connection()
        {
            string constructor = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            connections = new SqlConnection(constructor);
        }

        /// <summary>
        /// This method is used to login the users based on there role
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <param name="customerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Login(Login login, out string role, out int customerId, out string username)
        {
            connection();
            try
            {
                SqlCommand command = new SqlCommand("spLogin", connections);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", login.username);
                command.Parameters.AddWithValue("@password", login.password);
                connections.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customerId = reader.GetInt32(reader.GetOrdinal("customerId"));
                        role = reader["role"].ToString();
                        username = reader["username"].ToString();
                        HttpContext.Current.Session["customerId"] = customerId;
                        HttpContext.Current.Session["username"] = username;
                        return true;
                    }
                    else
                    {
                        role = null;
                        customerId = 0;
                        username = null;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                
                Errorlog errorlog = new Errorlog();
                errorlog.LogError(ex);
                role = null;
                username = null;
                customerId = 0;
                return false; 
            }
            finally
            {
                
                    connections.Close();               
            }      
        
        }

        public static string EncryptPassword(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}
