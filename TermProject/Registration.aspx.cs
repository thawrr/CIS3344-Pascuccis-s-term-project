using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GlobalMethods;

namespace TermProject
{
    public partial class Registration : System.Web.UI.Page
    {
        Account objAccount = new Account();
        CloudSvc.CloudService pxy = new CloudSvc.CloudService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CheckSession()==false)
                lblStatus.Text = "Hello. Please create an account.";
            else
                Response.Redirect("Login.aspx");
        }

        // If there is no active session, redirect to the login page
        public bool CheckSession()
        {
            if (Session["Login"] == null)
                return false;
            else
                return true;
        }

        public void StartSession()
        {
            //Make sure there is an object saved for this user
            if (Session["Account"] == null)
                Session["Account"] = new Account();

           // objAccount.UserEmail = txtEmail.Text;

            ((Account)Session["Account"]).UserEmail = objAccount.UserEmail;
            ((Account)Session["Account"]).UserID = objAccount.UserID;
            ((Account)Session["Account"]).UserPassword = objAccount.UserPassword;

            Session["Account"] = objAccount;
        }

        // Write cookie if the login information is to be saved
        public void WriteLoginCookie()
        {
            // Write email, password, and access date to cookie
            HttpCookie myCookie = new HttpCookie("LoginCredentials_Cookie");
           // myCookie.Values["Email"] = txtEmail.Text;
            Response.Cookies.Add(myCookie);
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            // Assuming fields are validated
            bool isAdded = pxy.AddUser(txtFullName.Text, txtEmail.Text, txtPassword.Text);

            if (isAdded)
            {
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";

                lblAddStatus.Text = "New user has been added.";

                Response.Redirect("Home.aspx");
            }
            else
            {
                lblAddStatus.Text = "An error occured. User has not been added.";
            }
        }

    }//end class
}//end nameSpace