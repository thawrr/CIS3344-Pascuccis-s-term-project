using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using GlobalMethods;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TermProject
{
    public partial class UserPage : System.Web.UI.Page
    {
        Account objAccount = new Account();
        CloudSvc.CloudService pxy = new CloudSvc.CloudService();
        GMethods objGM = new GMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
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
        }

        // If there is no active session, redirect to the login page
        public bool CheckSession()
        {
            if (Session["Account"] == null)
                return false;
            else
                return true;
        }

        public void FillControls()
        {
            gvViewVersions.Visible = false;

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

                gvUserCloud.Visible = true;
                gvUserCloud.DataSource = dtFiles;
                gvUserCloud.DataBind();

                ddlFiles.DataSource = dtFiles;
                ddlFiles.DataBind();

                lblStatus.Text += "\nYou have used " + objAccount.StorageUsed + " bytes of your allotted " + objAccount.StorageCapacity + " bytes.";
            }
            else
            {
                lblStatus.Text = "No files were found";
            }

            DataSet dsStorageOptions = pxy.GetStorageOptions(objAccount.UserEmail, objAccount.UserPassword);
            ddlPlanOptions.DataSource = dsStorageOptions;
            ddlPlanOptions.DataBind();
        }

        protected void gvUserCloud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If multiple ButtonField column fields are used, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "gvCommandDownload")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                //get the fileID of the selected row to Download
                // cell in the GridView control.
                GridViewRow selectedRow = gvUserCloud.Rows[index];

                int fileID = int.Parse(selectedRow.Cells[0].Text);//got userID for selected file
                int userID = int.Parse(selectedRow.Cells[1].Text);//got fileID for that file
                string fileName = selectedRow.Cells[2].Text;
                string ContentType = selectedRow.Cells[3].Text;

                string extension = "";
                switch (ContentType)
                {
                    case "Text":
                        extension = ".txt";
                        break;

                    case "Portable Network Graphics":
                        extension = ".png";
                        break;

                    case "Graphics Interchange Format":
                        extension = ".gif";
                        break;

                    case "Joint Photographic Experts Group":
                        extension = ".jpeg";
                        break;

                    case "Windows Word Document":
                        extension = ".docx";
                        break;

                    case "Batch File":
                        extension = ".bat";
                        break;

                    default:
                        extension = ".txt";
                        break;
                }

                //string customValue = "attachment;fileID="+ fileID.ToString();
                //select file from DB
                DataSet returnedFile = new DataSet();
                returnedFile = pxy.GetOneFile(fileID, userID, objAccount.UserEmail, objAccount.UserPassword);
                //returns file data of choosen file


                if (returnedFile.Tables[0].Rows.Count == 0)
                {
                    lblFileStatus.Text = "Something went wrong at download";
                }
                else
                {
                    byte[] bytes = new byte[1];
                    bytes = (byte[])returnedFile.Tables[0].Rows[0][0];

                    // Give content type to convert byte array to original file
                    Response.AddHeader("content-disposition", "attachment;filename= " + fileName + extension);
                    // Re-create the file from the byte array
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
                Response.Write(fileName);

                FillControls();
            }
            else if (e.CommandName == "gvCommandViewVersions")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Get the fileID to query to get the versions
                GridViewRow selectedRow = gvUserCloud.Rows[index];

                int fileID = int.Parse(selectedRow.Cells[0].Text);

                DataSet dsFileVersions = pxy.GetFileVersions(fileID, objAccount.UserEmail, objAccount.UserPassword);

                if (dsFileVersions.Tables[0].Rows.Count != 0)
                {
                    gvViewVersions.DataSource = dsFileVersions;
                    gvViewVersions.DataBind();
                    gvViewVersions.Visible = true;
                }
                else
                {
                    gvViewVersions.Visible = false;
                    // Should only show for old files initially uploaded
                    lblViewVersionStatus.Text = "No versions of this file were found.";
                }
            }
            else if (e.CommandName == "gvCommandDelete")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Get the fileID to query to get the versions
                GridViewRow selectedRow = gvUserCloud.Rows[index];

                int fileID = int.Parse(selectedRow.Cells[0].Text);

                if (pxy.DeleteFile(fileID, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword))
                {
                    lblFileStatus.Text = "File has been deleted.";
                    FillControls();
                }
                else
                    lblFileStatus.Text = "Unable to restore file. Please try again.";

            }
            else
                lblFileStatus.Text = "Something went wrong at gridview Command";

        }

        protected void ddlPlanOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPlanOptions.Enabled = false;
            tblTransaction.Visible = true;

            float price = pxy.GetStoragePrice(Convert.ToInt32(ddlPlanOptions.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            lblAmountDue.Text = "Your card will be charged: " + price.ToString("C");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tblTransaction.Visible = false;
            ddlPlanOptions.Enabled = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidatePayment())
            {
                float price = pxy.GetStoragePrice(Convert.ToInt32(ddlPlanOptions.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);


                if (pxy.UpgradePlan(txtCC.Text, Convert.ToInt32(txtCCV.Text), price, objAccount.UserID, Convert.ToInt32(ddlPlanOptions.SelectedValue), objAccount.UserEmail, objAccount.UserPassword))
                {
                    lblPaymentStatus.Text = "Plan has been updgraded";
                }
                else
                {
                    lblPaymentStatus.Text = "An error has occured and the plan has not been upgraded";
                }
            }
        }

        // Validate form data
        public bool ValidatePayment()
        {
            if (txtCC.Text.Length != 16)
            {
                lblPaymentStatus.Text = "Be sure the length of the credit card is exactly 16 digits.";
                return false;
            }
            else if (txtCCV.Text.Length != 3)
            {
                lblPaymentStatus.Text = "The security code must be exactly 3 digits.";
                return false;
            }
            return true;
        }

        protected void gvViewVersions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "gvCommandRestoreVersion")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Get the fileID and masterFileID
                GridViewRow selectedRow = gvViewVersions.Rows[index];

                int fileID = int.Parse(selectedRow.Cells[0].Text);
                int masterFileID = int.Parse(selectedRow.Cells[1].Text);
                
                if (pxy.RestoreOldVersion(fileID, masterFileID, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword))
                {
                    lblViewVersionStatus.Text = "File has been restored.";
                    gvViewVersions.Visible = false;
                    FillControls();
                }
                else
                    lblViewVersionStatus.Text = "Unable to restore file. Please try again.";
            }
        }

        protected void gvDeletedFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "gvCommandRestoreVersion")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Get the fileID to query to get the versions
                GridViewRow selectedRow = gvDeletedFiles.Rows[index];

                int fileID = int.Parse(selectedRow.Cells[0].Text);

                if (pxy.RestoreDeletedFile(fileID, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword))
                {
                    lblViewDeletedStatus.Text = "File has been restored.";
                    ViewDeletedFiles();
                    FillControls();
                }
                else
                    lblViewDeletedStatus.Text = "Unable to restore file. Please try again.";

            }
        }

        protected void btnViewDeletedFiles_Click(object sender, EventArgs e)
        {
            lblViewDeletedStatus.Text = "";
            ViewDeletedFiles();
        }

        private void ViewDeletedFiles()
        {
            lblViewVersionStatus.Text = "";

            DataSet dsDeletedFiles = pxy.GetDeletedFiles(objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword);

            if (dsDeletedFiles.Tables[0].Rows.Count != 0)
            {
                gvDeletedFiles.DataSource = dsDeletedFiles;
                gvDeletedFiles.DataBind();
                gvDeletedFiles.Visible = true;
            }
            else
            {
                gvDeletedFiles.Visible = false;
                lblViewDeletedStatus.Text = "No deleted files were found.";
            }
        }

        protected void btnUpdateFile_Click(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }
    }//end class
}//end nameSpace