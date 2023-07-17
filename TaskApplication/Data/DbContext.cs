using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskApplication.Models;

namespace TaskApplication.Data
{
    public class DbContext
    {

        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cons"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        public List<User> GetUsers()
        {
            //cmd = new SqlCommand("SELECT U.*, C.cityName, CT.countryName FROM TblUsers U JOIN TblCity C ON U.userCity = C.cityId JOIN TblCountry CT ON C.countryId = CT.countryId", con);
            cmd = new SqlCommand("SELECT U.*, C.cityName, CT.countryName FROM TblUsers U JOIN TblCity C ON U.userCity = C.cityId JOIN TblCountry CT ON C.countryId = CT.countryId", con);
            // cmd = new SqlCommand("select * from TblUsers", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            List<User> list = new List<User>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new User{
                     userId = Convert.ToInt32(dr["userId"]),
                     fname = dr["fName"].ToString(),
                     lname = dr["lName"].ToString(),
                     phoneNo = dr["phoneNo"].ToString(),
                     emailNo = dr["emailNo"].ToString(),
                     //userCity = Convert.ToInt32(dr["userCity"]),
                     userCity = dr["cityName"].ToString(),
                     userImg = dr["userImg"].ToString(),
                     userCV = dr["userCV"].ToString(),
                     password = dr["password"].ToString(),
                     dob = Convert.ToDateTime(dr["dob"])
                });
                
            }
            return list;
        }



        public bool InsertUser(User user, HttpPostedFileBase imageFile, HttpPostedFileBase cvFile)
        {
            string query = "INSERT INTO [dbo].[TblUsers] (fName, lName, phoneNo, emailNo, userCity, userImg, userCV, password, dob) VALUES (@fName, @lName, @phoneNo, @emailNo, @userCity, @userImg, @userCV, @password, @dob)";
            cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@fName", user.fname);
            cmd.Parameters.AddWithValue("@lName", user.lname);
            cmd.Parameters.AddWithValue("@phoneNo", user.phoneNo);
            cmd.Parameters.AddWithValue("@emailNo", user.emailNo);
            cmd.Parameters.AddWithValue("@userCity", user.userCity);

            // Upload and save the image file
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string imageName = Path.GetFileName(imageFile.FileName);
                string imagePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Image/"), imageName);
                imageFile.SaveAs(imagePath);

                cmd.Parameters.AddWithValue("@userImg", "~/Image/" + imageName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@userImg", DBNull.Value);
            }

            // Upload and save the CV (PDF) file
            if (cvFile != null && cvFile.ContentLength > 0)
            {
                string cvName = Path.GetFileName(cvFile.FileName);
                string cvPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/CV/"), cvName);
                cvFile.SaveAs(cvPath);

                cmd.Parameters.AddWithValue("@userCV", "~/CV/" + cvName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@userCV", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@password", user.password);
            cmd.Parameters.AddWithValue("@dob", user.dob);

            con.Open();
            int r = cmd.ExecuteNonQuery();
            con.Close();

            return r > 0;
        }


        public bool UpdateUser(User user, HttpPostedFileBase imageFile, HttpPostedFileBase cvFile)
        {
            string query = "UPDATE [dbo].[TblUsers] SET fName = @fName, lName = @lName, phoneNo = @phoneNo, emailNo = @emailNo, " +
                           "userCity = @userCity, userImg = @userImg, userCV = @userCV, password = @password, dob = @dob " +
                           "WHERE userId = @userId";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cons"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@fName", user.fname);
                    cmd.Parameters.AddWithValue("@lName", user.lname);
                    cmd.Parameters.AddWithValue("@phoneNo", user.phoneNo);
                    cmd.Parameters.AddWithValue("@emailNo", user.emailNo);
                    cmd.Parameters.AddWithValue("@userCity", user.userCity);

                    // Upload and save the image file if provided
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        string imageName = Path.GetFileName(imageFile.FileName);
                        string imagePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Image/"), imageName);
                        imageFile.SaveAs(imagePath);

                        cmd.Parameters.AddWithValue("@userImg", "~/Image/" + imageName);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@userImg", user.userImg);
                    }

                    // Upload and save the CV (PDF) file if provided
                    if (cvFile != null && cvFile.ContentLength > 0)
                    {
                        string cvName = Path.GetFileName(cvFile.FileName);
                        string cvPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/CV/"), cvName);
                        cvFile.SaveAs(cvPath);

                        cmd.Parameters.AddWithValue("@userCV", "~/CV/" + cvName);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@userCV", user.userCV);
                    }

                    cmd.Parameters.AddWithValue("@password", user.password);
                    cmd.Parameters.AddWithValue("@dob", user.dob);
                    cmd.Parameters.AddWithValue("@userId", user.userId);

                    con.Open();
                    int r = cmd.ExecuteNonQuery();

                    return r > 0;
                }
            }
        }
    }
}