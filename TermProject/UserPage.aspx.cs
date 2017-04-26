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
            DataSet dsFiles = pxy.GetFilesByUserID(objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword);

            if (dsFiles.Tables[0].Rows.Count != 0)
            {
                gvUserCloud.DataSource = dsFiles;
                gvUserCloud.DataBind();
                
            }
            else
            {
                lblStatus.Text = "No files were found";
            }
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
                
                int userID = int.Parse(selectedRow.Cells[0].Text);//got userID for selected file
                int fileID = int.Parse(selectedRow.Cells[1].Text);//got fileID for that file
                string ContentType = selectedRow.Cells[2].Text;
                string fileName = selectedRow.Cells[3].Text;

                //string customValue = "attachment;fileID="+ fileID.ToString();
                //select file from DB
                byte[] bytes = pxy.GetOneFile(fileID, userID, objAccount.UserEmail, objAccount.UserPassword);
                //returns file data of choosen file


                if (bytes == null)
                {
                    lblStatus2.Text = "Something went wrong at download";
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename= " + fileName);
                    Response.ContentType = "application/octectstream";
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
                Response.Write(fileName);

                FillControls();
            }
            else
                lblStatus2.Text = "Something went wrong at gridview Command";
        }
    }//end class
}//end nameSpace