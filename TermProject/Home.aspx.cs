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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CheckSession())
            {
                lblStatus.Text = "Welcome! Your email is: " + Session["Login"].ToString();
            }
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
                    a.UserID = Convert.ToInt32(Session["Account"]);//current user id

                    lblTest.Text = a.UserID + ", " + fileName + ", " + fileExtension+" was uploaded";
                }
            }
        }
    }//end class
}//end name space 