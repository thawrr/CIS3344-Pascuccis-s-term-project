using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class Home : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CheckSession())
                lblStatus.Text = "Welcome! Your email is: " + Session["Login"].ToString();
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
    }
}