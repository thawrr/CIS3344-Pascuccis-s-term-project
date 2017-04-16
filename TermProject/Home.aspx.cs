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
        Validation v = new Validation();

        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;//necessary for asp validators... or else

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

                txtName.Text = ((Account)Session["Account"]).UserFullName;
                txtEmail.Text = ((Account)Session["Account"]).UserEmail;
                txtCapacity.Text = ((Account)Session["Account"]).StorageCapacity.ToString();
                txtPW.Text = ((Account)Session["Account"]).UserPassword;
                txtUsedSC.Text = ((Account)Session["Account"]).StorageUsed.ToString();
                
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

        private void BindData()
        {

        }

        protected void gvUserFiles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Set the edit index.
            gvUserFiles.EditIndex = e.NewEditIndex;
            //Bind data to the GridView control.
            //BindData();
        }

        protected void gvUserFiles_SelectedIndexChanged(object sender, EventArgs e)
        {// Get the currently selected row using the SelectedRow property.
         
        }

        protected void gvUserFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {// RowEditing event handler that fires when the CommandField Edit button is clicked.

        }

        protected void gvUserFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        { // RowUpdating event handler that fires when the CommandField Update button is clicked.

        }

        protected void gvUserFiles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }



        //NEXT GRIDVIEW -- TRANSACTION
        protected void gvTransactions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvTransactions_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvTransactions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvTransactions_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        //submit button 
        protected void btnAccount_Click(object sender, EventArgs e)
        {
            if (v.IsText(txtName) && v.IsValidEmail(txtEmail) && v.IsInt(txtCapacity) && v.IsInt(txtCapacity) == false)
            {
                lblAccountError.Text = "Input error in one of the input boxes";

                txtName.Text = ((Account)Session["Account"]).UserFullName;
                txtEmail.Text = ((Account)Session["Account"]).UserEmail;
                txtCapacity.Text = ((Account)Session["Account"]).StorageCapacity.ToString();
                txtPW.Text = ((Account)Session["Account"]).UserPassword;
                txtUsedSC.Text = ((Account)Session["Account"]).StorageUsed.ToString();
            }
            else
            {
                CloudSvc.CloudService pxy = new CloudSvc.CloudService();
                Account a = new Account();
                int userID = ((Account)Session["Account"]).UserID;
                string tempName = txtName.Text;
                string tempEmail = txtEmail.Text;
                int capTemp = Convert.ToInt32(txtCapacity.Text);
                string tempPW = txtPW.Text;


                DataSet newAccountInfo = new DataSet();
                newAccountInfo = pxy.AccountUpdate(userID, tempName, tempEmail, capTemp, tempPW);

                if (newAccountInfo.Tables[0].Rows[0][0] == DBNull.Value)
                    lblTest.Text = "could not update Account :(";
                else
                {
                    objAccount.UserID = Convert.ToInt32(newAccountInfo.Tables[0].Rows[0]["UserID"]);
                    objAccount.UserEmail = newAccountInfo.Tables[0].Rows[0]["LoginID"].ToString();
                    objAccount.UserPassword = newAccountInfo.Tables[0].Rows[0]["HashedPassword"].ToString();
                    objAccount.UserFullName = newAccountInfo.Tables[0].Rows[0]["Name"].ToString();
                    objAccount.StorageCapacity = Convert.ToInt32(newAccountInfo.Tables[0].Rows[0]["StorageCapacity"]);

                    txtName.Text = objAccount.UserFullName;
                    txtEmail.Text = objAccount.UserEmail;
                    txtCapacity.Text = objAccount.StorageCapacity.ToString();
                    txtPW.Text = objAccount.UserPassword;
                    txtUsedSC.Text = objAccount.StorageCapacity.ToString();

                }
            }
        }
    }//end class1
}//end name space 