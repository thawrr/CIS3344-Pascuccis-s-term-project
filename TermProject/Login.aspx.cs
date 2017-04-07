using System;
using System.Collections.Generic;
using System.Linq;
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
            //necessary for asp validators... or else
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            // If cookies is found then populate email textbox and prompt user for password
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