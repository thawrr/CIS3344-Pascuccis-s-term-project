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
        GMethods objGM = new GMethods();
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
                string email = ((Account)Session["Account"]).UserEmail;
                objAccount.UserID = ((Account)Session["Account"]).UserID;
                objAccount.UserEmail = ((Account)Session["Account"]).UserEmail;
                objAccount.UserPassword = ((Account)Session["Account"]).UserPassword;

                lblStatus.Text = "Welcome! Your email is: " + email + ". Your UserID is " + objAccount.UserID;
            }
            else
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                FillControls();
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

                        string fileType = objGM.GetFileType(fileExtension);

                        bool result = pxy.AddFile(input, userID, fileName, fileType, fileSize, objAccount.UserEmail, objAccount.UserPassword);

                        if (result == true)
                            lblTest.Text = userID + ", " + fileName + ", " + fileType + ", " + fileExtension + " was uploaded";
                        else
                            lblTest.Text = "Could not upload file to DataBase";
                    }
                }
                else
                {
                    lblFileError.Text = "You have reached your maximum storage quota";
                }
            }
            FillControls();
        }

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
            int fileID = Convert.ToInt32(gvFiles.Rows[e.RowIndex].Cells[2].Text);

            bool isDelete = pxy.DeleteFile(fileID, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword);

            if (isDelete)
                lblDeleteStatus.Text = "File was deleted";
            else
                lblDeleteStatus.Text = "An error occured and the file was not deleted";

            FillControls();
        }

        public void FillControls()
        {
            DataSet dsFiles = pxy.GetFilesByUserID(objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword);
            DataTable dtFiles = dsFiles.Tables[0];

            if (dsFiles.Tables[0].Rows.Count != 0)
            {
                dtFiles.Columns.Add("ImageURL", typeof(string));

                // Determine which icon to show based on the file type
                for (int i = 0; i < dtFiles.Rows.Count; i++)
                {
                    string fileType = dtFiles.Rows[i]["FileType"].ToString();
                    string url = objGM.GetImageURL(fileType);
                    dtFiles.Rows[i]["ImageURL"] = url;
                }

                gvFiles.Visible = true;
                gvFiles.DataSource = dtFiles;
                gvFiles.DataBind();

                ddlFiles.DataSource = dtFiles;
                ddlFiles.DataBind();

                lblStorageInfo.Text = "You have used " + objAccount.StorageUsed + " bytes of your allotted " + objAccount.StorageCapacity + " bytes.";
            }
            else
            {
                gvFiles.Visible = false;
                lblDeleteStatus.Text = "No files were found";
                lblStorageInfo.Text = "";
            }

            DataSet dsUsers = pxy.GetAllCloudUsers(objAccount.UserEmail, objAccount.UserPassword);
            ddlUser.DataSource = dsUsers;
            ddlUser.DataBind();

            ddlUserTrans.DataSource = dsUsers;
            ddlUserTrans.DataBind();

            DataSet dlRoles = pxy.GetAllRoles(objAccount.UserEmail, objAccount.UserPassword);
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

                        string fileType = objGM.GetFileType(fileExtension);

                        int roleID = Convert.ToInt32(ddlRole.SelectedValue);

                        bool result = pxy.UpdateFile(fileID, input, userID, fileName, fileType, fileSize, roleID, objAccount.UserEmail, objAccount.UserPassword);

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
            DataSet dsUser = pxy.GetUserByID(Convert.ToInt32(ddlUser.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

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
                FillControls();
            }
            else
            {
                lblUpdateStatus.Text = "Account was not updated.";
            }
        }

        protected void btnSelectUserTrans_Click(object sender, EventArgs e)
        {
            DataSet dsTrans = pxy.GetAllTrans(Convert.ToInt32(ddlUserTrans.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            if (dsTrans.Tables[0].Rows.Count != 0)
            {
                gvTransactions.DataSource = dsTrans;
                gvTransactions.DataBind();
                gvTransactions.Visible = true;
            }
            else
            {
                lblTransactionStatus.Text = "No transactions were found";
                gvTransactions.Visible = false;
            }
        }
    }
}