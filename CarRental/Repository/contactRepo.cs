using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CarRental.Repository
{
    public class contactRepo
    {
        private SqlConnection con;
        private void connection()
        {
            string connection = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            con = new SqlConnection(connection);
        }
/// <summary>
/// This method is used to view the enquires by admin
/// </summary>
/// <returns></returns>
        public List<Contact>ViewEnquires()
        {
            connection();
            List<Contact> contactDetailsList = new List<Contact>();
            SqlCommand com = new SqlCommand("SPS_GetAllContactUsDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);

            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                contactDetailsList.Add(
                    new Contact
                    {
                        fullName = Convert.ToString(dr["fullName"]),
                        phoneNumber = Convert.ToString(dr["phoneNumber"]),
                        email = Convert.ToString(dr["email"]),
                        message = Convert.ToString(dr["message"])
                        
                    }
                );
            }

            return contactDetailsList;
        }
/// <summary>
/// This method is used to insert enquires
/// </summary>
/// <param name="contact"></param>
/// <returns></returns>
        public bool insertContact(Contact contact)
        {
            connection();
            using (SqlCommand cmd = new SqlCommand("SPI_InsertContactUs", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fullName", contact.fullName);
                cmd.Parameters.AddWithValue("@phoneNumber", contact.phoneNumber);
                cmd.Parameters.AddWithValue("@message", contact.message);
                cmd.Parameters.AddWithValue("@email", contact.email);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }
/// <summary>
/// This method is used to delete the enquires
/// </summary>
/// <returns></returns>
        public bool DeleteContact()
        {
            connection();
            using (SqlCommand cmd = new SqlCommand("SPD_Contactus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
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





    }
}