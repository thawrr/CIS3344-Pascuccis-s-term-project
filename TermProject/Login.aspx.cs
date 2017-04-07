using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class LogInPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //necessary for asp validators... or else
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

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
                lblStatus.Text = "Invalid email or password. Please try again";
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
            // Still need to create database tables (tblUser && tblRole)
            //TermProjectSvc.TermProject pxy = new TermProjectSvc.TermProject();
            //try
            //{
            //    DataSet objDS = pxy.GetUserByLoginIDandPass(txtEmail.Text, txtPassword.Text);

            //    // Check if returned DataSet is empty
            //    if (objDS.Tables[0].Rows.Count == 0)
            //        return false;
            //}
            //catch (Exception)
            //{
            //    lblStatus.Text = "An unexpected error has occured.";
            //    throw;
            //}

            
            // User entered correct login information
            return true;
        }

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
    }
}