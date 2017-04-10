﻿using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using GlobalMethods;

namespace TermProject
{
    public partial class LogInPage : System.Web.UI.Page
    {
        GMethods g = new GMethods();//object of methods class, mostly calls stored procedures


        protected void Page_Load(object sender, EventArgs e)
        {
            // If cookie is found then populate email textbox and prompt user for password
            if (!IsPostBack && Request.Cookies["LoginCredentials_Cookie"] != null)
            {
                HttpCookie cookie = Request.Cookies["LoginCredentials_Cookie"];
                txtEmail.Text = cookie.Values["Email"].ToString();
                chkRem.Checked = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateInput() && CheckCredentials())
            {
                lblStatus.Text = "";

                // Write the cookie if requested
                if (chkRem.Checked == true)
                    WriteLoginCookie();

                StartSession();
                Response.Redirect("Home.aspx");
            }
            else
            {
                Session["Login"] = null;
                lblStatus.Text = "Invalid email or password. Please try again";
            }
        }

        // Check if input is not empty and valid
        public bool ValidateInput()
        {
            if (txtEmail.Text == "" || txtEmail.Text.Contains('@') == false)
                return false;
            else if (txtPassword.Text == "")
                return false;

            return true;
        }

        // Check with database to see if credentials are correct
        public bool CheckCredentials()
        {
            DataSet objDS = new DataSet();

            // Still need to create database tables (tblUser && tblRole)
            TermProjectSvc.TermProject pxy = new TermProjectSvc.TermProject();
            try
            {
                objDS = pxy.GetUserByLoginIDandPass(txtEmail.Text, txtPassword.Text);

                // Check if returned DataSet is empty
                if (objDS.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    // If serialized column is empty, then store info, else load into Account object
                    if (objDS.Tables[0].Rows[0]["Account"] == DBNull.Value)
                    {
                        int UserID = Convert.ToInt32(objDS.Tables[0].Rows[0]["UserID"]);

                        // Serialize data for database input and display status
                        lblStatus.Text = pxy.UpdateAccount(objDS, UserID);
                    }
                    else
                    {
                        Byte[] byteArray = Encoding.ASCII.GetBytes(objDS.Tables[0].Rows[0]["Account"].ToString());
                        //Account objAccount = DeserializeAccount(byteArray);
                        //Account objAccount = objGM.DeserializeAccount(byteArray);
                    }

                    // User entered correct login information
                    return true;
                }
            }
            catch (Exception e)
            {
                //lblStatus.Text = "An unexpected error has occured.";
                lblStatus.Text = e.ToString();
                return false;
            }

            DataSet ds = g.GetUserNames();//ds has everything
            //userID, FK_RoleID=null, email, password, FN, LN

            DataTable dt = new DataTable();
            int rowNum = 0;// row number
            string columnEmail = "Email";  // database table column name
            string columnPW = "Password";  // database table column name

            dt = ds.Tables["Users"];

            foreach (DataRow dr in dt.Rows)//reading the dataSet
            {
                columnEmail = dt.Rows[rowNum][columnEmail].ToString();//specific cell value 
                columnPW = dt.Rows[rowNum][columnPW].ToString();
                rowNum++;
            }

            if (String.Equals(txtEmail.Text, columnEmail) == true && String.Equals(txtPassword.Text, columnPW) == true)
                return true;//all good
            else
                return false;//one or both failed
        }

        // Write cookie if the login information is to be saved
        public void WriteLoginCookie()
        {
            // Write email, password, and access date to cookie
            HttpCookie myCookie = new HttpCookie("LoginCredentials_Cookie");
            myCookie.Values["Email"] = txtEmail.Text;
            Response.Cookies.Add(myCookie);
        }

        public void StartSession()
        {
            Session["Login"] = txtEmail.Text;
        }

        // Deserialize the binary data to reconstruct the Account object
        // Not working. Getting error:
        //'System.Runtime.Serialization.SerializationException' in mscorlib.dll ("End of Stream encountered before parsing was completed.")
        public Account DeserializeAccount(Byte[] byteArray)
        {
            BinaryFormatter deSerializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(byteArray);
            memStream.Position = 0;
            Account objAccount = (Account)deSerializer.Deserialize(memStream);

            return objAccount;
        }
    }
}