using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GlobalMethods;

namespace TermProject
{
    public partial class Home : System.Web.UI.Page
    {
        Account objAccount = new Account();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Check if it exists and if logged in
            if (CheckSession())
            {
                //Make sure there is an object saved for this user
                if (Session["Account"] == null)
                    Session["Account"] = new Account();

                //Access your properties here
                string email = ((Account)Session["Account"]).UserEmail;//get email
                lblStatus.Text = "Welcome! Your email is: " + email;//put it on the page
            }
            else
                Response.Redirect("Login.aspx");
        }//end Page_Load

        // If there is no active session, redirect to the login page
        public bool CheckSession()
        {
            if (Session["Account"] == null)
                return false;
            else
                return true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            TermProjectSvc.TermProject pxy = new TermProjectSvc.TermProject();
            Account a = new Account();

            lblFileError.Visible = false;//hide error

            if (FileUpload1.HasFile==false)//check for file
            {
                lblFileError.Visible = true;//show error
                lblFileError.Text = "No file was uploaded";
            }
            else
            {
                // Get the size in bytes of the file to upload.
                int fileSize = FileUpload1.PostedFile.ContentLength;

                // Create a byte array to hold the contents of the file.
                byte[] input = new byte[fileSize - 1];
                input = FileUpload1.FileBytes;//file data

                if (fileSize > 510000)//limit size
                {
                    lblFileError.Visible = true;//show error
                    lblFileError.Text = "File is too big.";
                }
                else
                {
                    string fileName = FileUpload1.PostedFile.FileName;//SomeFile.txt
                    string fileExtension = Path.GetExtension(fileName);//.txt
                    int userID = ((Account)Session["Account"]).UserID;//When retrieving an object from session state,  
                                                   //cast it to the appropriate type.

                    lblTest.Text = userID + ", " + fileName + ", " + fileExtension+" was uploaded";
                }
            }
        }
    }//end class
}//end name space 