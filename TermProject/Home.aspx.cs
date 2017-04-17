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
        CloudSvc.CloudService pxy = new CloudSvc.CloudService();


        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            //Check if it exists and if logged in
            if (CheckSession())
            {
                //Make sure there is an object saved for this user
                if (Session["Account"] == null)
                    Session["Account"] = new Account();

                objAccount = (Account)Session["Account"];

                //Access your properties here
                string email = ((Account)Session["Account"]).UserEmail;//get email
                objAccount.UserID = ((Account)Session["Account"]).UserID;//get user ID from session object
                lblStatus.Text = "Welcome! Your email is: " + email + ". Your UserID is " + objAccount.UserID;//put it on the page

                FillControls();
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


                if (objAccount.StorageUsed + fileSize < objAccount.StorageCapacity)
                {
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
                        string fileName = FileUpload1.PostedFile.FileName;
                        string fileExtension = Path.GetExtension(fileName);
                        int userID = ((Account)Session["Account"]).UserID;
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
                                fileType = "Unknown to app";
                                break;
                        }

                        bool result = pxy.AddFile(input, userID, fileName, fileType, fileSize);

                        if (result == true)
                            lblTest.Text = userID + ", " + fileName + ", " + fileType + ", " + fileExtension + " was uploaded";
                        else
                            lblTest.Text = "Could not upload file to DataBase";
                    }//end else
                }
                else
                {
                    lblFileError.Text = "You have reached your maximum storage quota";
                }
            }
            FillControls();
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

        protected void gvFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the fileID
            int fileID = Convert.ToInt32(gvFiles.Rows[e.RowIndex].Cells[1].Text);
            bool isDelete = pxy.DeleteFile(fileID, objAccount.UserID);

            if (isDelete)
                lblDeleteStatus.Text = "File was deleted";
            else
                lblDeleteStatus.Text = "An error occured and the file was not deleted";

            FillControls();
        }

        public void FillControls()
        {
            DataSet dsFiles = pxy.GetFilesByUserID(objAccount.UserID);

            if (dsFiles.Tables[0].Rows.Count != 0)
            {
                gvFiles.Visible = true;
                gvFiles.DataSource = dsFiles;
                gvFiles.DataBind();

                ddlFiles.DataSource = dsFiles;
                ddlFiles.DataBind();
            }
            else
            {
                gvFiles.Visible = false;
                lblDeleteStatus.Text = "No files were found";
            }

            DataSet dsUsers = pxy.GetAllCloudUsers();
            ddlUser.DataSource = dsUsers;
            ddlUser.DataBind();

            DataSet dlRoles = pxy.GetAllRoles();
            ddlRole.DataSource = dlRoles;
            ddlRole.DataBind();
        }

        protected void btnUpdateFile_Click(object sender, EventArgs e)
        {
            int fileID = Convert.ToInt32(ddlFiles.SelectedValue);

            lblFileError.Visible = false;//hide error

            if (FileUploadUpdate.HasFile == false)//check for file
            {
                lblFileError.Visible = true;//show error
                lblFileError.Text = "No file was uploaded";
            }
            else
            {
                // Get the size in bytes of the file to upload.
                int fileSize = FileUploadUpdate.PostedFile.ContentLength;

                if (objAccount.StorageUsed + fileSize < objAccount.StorageCapacity)
                {
                    // Create a byte array to hold the contents of the file.
                    byte[] input = new byte[fileSize - 1];
                    input = FileUploadUpdate.FileBytes;//file data

                    if (fileSize > 510000)//limit size
                    {
                        lblFileError.Visible = true;//show error
                        lblFileError.Text = "File is too big.";
                    }
                    else
                    {
                        string fileName = FileUploadUpdate.PostedFile.FileName;
                        string fileExtension = Path.GetExtension(fileName);
                        int userID = ((Account)Session["Account"]).UserID;
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
                                fileType = "Unknown to app";
                                break;
                        }

                        bool result = pxy.UpdateFile(fileID, input, userID, fileName, fileType, fileSize);

                        if (result == true)
                            lblTest.Text = userID + ", " + fileName + ", " + fileType + ", " + fileExtension + " was uploaded";
                        else
                            lblTest.Text = "Could not upload file to DataBase";
                    }//end else
                }
                else
                {
                    lblFileError.Text = "You have reached your maximum storage quota";
                }
            }
            FillControls();
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

                FillControls();
            }
            else
            {
                lblAddStatus.Text = "An error occured. User has not been added.";
            }
        }

        protected void btnSelectUpdateUser_Click(object sender, EventArgs e)
        {
            DataSet dsUser = pxy.GetUserByID(Convert.ToInt32(ddlUser.SelectedValue));

            lblUserID.Text = dsUser.Tables[0].Rows[0]["UserID"].ToString();
            txtUpdateName.Text = dsUser.Tables[0].Rows[0]["Name"].ToString();
            txtUpdateEmail.Text = dsUser.Tables[0].Rows[0]["LoginID"].ToString();
            txtUpdatePassword.Text = dsUser.Tables[0].Rows[0]["HashedPassword"].ToString();
            txtUpdateCapacity.Text = dsUser.Tables[0].Rows[0]["StorageCapacity"].ToString();
            ddlRole.SelectedValue = dsUser.Tables[0].Rows[0]["RoleID"].ToString();

            tblUpdateUser.Visible = true;

        }

        protected void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            bool isUpdated = pxy.AccountUpdate(Convert.ToInt32(lblUserID.Text), txtUpdateName.Text, txtUpdateEmail.Text, Convert.ToInt32(txtUpdateCapacity.Text), txtUpdatePassword.Text);

            if (isUpdated)
            {
                tblUpdateUser.Visible = false;
                lblUpdateStatus.Text = "Account has been updated.";
            }
            else
            {
                lblUpdateStatus.Text = "Account was not updated.";
            }
        }
    }//end class
}//end name space 