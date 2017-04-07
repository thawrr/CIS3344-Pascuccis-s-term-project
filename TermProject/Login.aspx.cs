using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using GlobalMethods;
using System.Data;

namespace TermProject
{
    public partial class LogInPage : System.Web.UI.Page
    {
        Gmethods g = new Gmethods();//object of methods class, mostly calls stored procedures

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
            if (ValidateInput() && FoundUser())
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
            else if (txtPassword.Text == "" || Regex.IsMatch(txtPassword.Text, @"^[a-zA-Z]+[^a-zA-Z]+[^0-9]+$") == false) 
                // test for empty, then start with character, 
                //has strings, has ints, and Match zero or one occurrence of the dollar sign
                return false;

            return true;
        }

        // Check with database to see if credentials are correct
        public bool FoundUser()
        {
            DataSet ds = g.GetUserNames();

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