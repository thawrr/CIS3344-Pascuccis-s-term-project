using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GlobalMethods;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Data.SqlClient;
using Utilities;

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
                objAccount.UserID = ((Account)Session["Account"]).UserID;//get user ID from session object
                lblStatus.Text = "Welcome! Your email is: " + email + ". Your UserID is " + objAccount.UserID;//put it on the page
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
            CloudSvc.CloudService pxy = new CloudSvc.CloudService();
            Account a = new Account();

            lblFileError.Visible = false;//hide error

            if (FileUpload1.HasFile == false)//check for file
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
                    string fileType;


                    switch (fileExtension)
                    {
                        case ".txt":
                            fileType = "Text";
                            break;

                        case ".png":
                            fileType = "Portable Network Graphics";
                            break;

                        case ".gif":
                            fileType = "Graphics Interchange Format";
                            break;

                        case ".jpg":
                            fileType = "Joint Photographic Experts Group";
                            break;

                        case ".jpeg":
                            fileType = "Joint Photographic Experts Group";
                            break;

                        case ".docx":
                            fileType = "Windows Word Document";
                            break;

                        case ".bat":
                            fileType = "Batch File";
                            break;

                        default:
                            fileType = "unknown to app";
                            break;
                    }

                    bool result = pxy.AddFile(input, userID, fileName, fileType, fileSize);

                    if (result == true)
                        lblTest.Text = userID + ", " + fileName + ", " + fileType + ", " + fileExtension + " was uploaded";
                    else
                        lblTest.Text = "could not upload file to DataBase";
                }//end else
            }
        }//end btnClick

        // Deserialize the binary data to reconstruct the Account object
        public Account DeserializeAccount(Byte[] byteArray)
        {
            BinaryFormatter deSerializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(byteArray);
            memStream.Position = 0;
            objAccount = (Account)deSerializer.Deserialize(memStream);

            return objAccount;
        }
    }//end class
}//end name space 