using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class AdminTools : System.Web.UI.Page
    {
        Account objAccount = new Account();
        CloudSvc.CloudService pxy = new CloudSvc.CloudService();
        GMethods objGM = new GMethods();
        DataSet userCloud = new DataSet("Account");

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
                objAccount.UserRole = ((Account)Session["Account"]).UserRole;

                if (objAccount.UserRole == "Cloud Admin" || objAccount.UserRole == "Super Admin")
                {
                    // If Super Admin is visiting, then show respective tools
                    if (objAccount.UserRole == "Super Admin")
                    {
                        pnlSuperAdminTools.Visible = true;
                    }

                    lblStatus.Text = "Welcome! Your email is: " + email + ". Your UserID is " + objAccount.UserID;
                }
                else
                {
                    Response.Redirect("UserPage.aspx");
                }
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

        protected void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            // Check all controls for valid input
            bool isActive = chkActive.Checked;
            if (CheckUpdateInput(txtUpdateEmail.Text, txtUpdateName.Text, txtUpdatePassword.Text))
            {
                bool isUpdated = pxy.AccountUpdate(Convert.ToInt32(lblUserID.Text), txtUpdateName.Text, txtUpdateEmail.Text, Convert.ToInt32(txtUpdateCapacity.Text), txtUpdatePassword.Text, Convert.ToInt32(ddlRole.SelectedValue), isActive, objAccount.UserEmail, objAccount.UserPassword);

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
            else
                lblUpdateStatus.Text = "Check your input. Ensure your entered name is not blank, email is valid, and password meets the criteria.";
        }


        // Validate registration inputs
        private bool CheckInput(string email, string name, string password)
        {
            bool isValidEmail = false;
            bool isValidName = false;
            bool isValidPassword = false;

            if (name != "")
            {
                isValidName = true;
            }
            if (pxy.CheckEmail(email))
            {
                if (email.Contains('@'))
                {
                    isValidEmail = true;
                }
            }
            if (password.Length >= 6)
            {
                if (password.Contains('!') || password.Contains('@') || password.Contains('#') || password.Contains('$') || password.Contains('%') || password.Contains('&'))
                {
                    isValidPassword = true;
                }
            }

            if (isValidName && isValidEmail && isValidPassword)
            {
                return true;
            }
            else
                return false;
        }

        // Validate registration inputs
        private bool CheckUpdateInput(string email, string name, string password)
        {
            bool isValidEmail = false;
            bool isValidName = false;
            bool isValidPassword = false;

            if (name != "")
            {
                isValidName = true;
            }
            if (email.Contains('@'))
            {
                isValidEmail = true;
            }
            if (password.Length >= 6)
            {
                if (password.Contains('!') || password.Contains('@') || password.Contains('#') || password.Contains('$') || password.Contains('%') || password.Contains('&'))
                {
                    isValidPassword = true;
                }
            }

            if (isValidName && isValidEmail && isValidPassword)
            {
                return true;
            }
            else
                return false;
        }

        protected void btnSelectUpdateUser_Click(object sender, EventArgs e)
        {
            // Fill the controls with the selected user's information
            DataSet dsUser = pxy.GetUserByID(Convert.ToInt32(ddlUser.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            lblUserID.Text = dsUser.Tables[0].Rows[0]["UserID"].ToString();
            txtUpdateName.Text = dsUser.Tables[0].Rows[0]["Name"].ToString();
            txtUpdateEmail.Text = dsUser.Tables[0].Rows[0]["LoginID"].ToString();
            txtUpdatePassword.Text = objGM.DecryptPassword(dsUser.Tables[0].Rows[0]["HashedPassword"].ToString());
            txtUpdateCapacity.Text = dsUser.Tables[0].Rows[0]["StorageCapacity"].ToString();
            ddlRole.SelectedValue = dsUser.Tables[0].Rows[0]["RoleID"].ToString();

            if (Convert.ToBoolean(dsUser.Tables[0].Rows[0]["isActive"]))
            {
                chkActive.Checked = true;
            }
            else
                chkActive.Checked = false;

            tblUpdateUser.Visible = true;
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {

            if (CheckInput(txtEmail.Text, txtFullName.Text, txtPassword.Text))
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
            else
            {
                lblAddStatus.Text = "Check your input. Ensure your entered name is not blank, email is valid, and password meets the criteria.";
            }
        }

        protected void btnSelectUserTrans_Click(object sender, EventArgs e)
        {
            // Fill control with transaction data
            DataSet dsTrans = pxy.GetAllTransByID(Convert.ToInt32(ddlUserTrans.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            if (dsTrans.Tables[0].Rows.Count != 0)
            {
                gvTransactions.DataSource = dsTrans;
                gvTransactions.DataBind();
                gvTransactions.Visible = true;

                lblTransactionStatus.Text = "";
            }
            else
            {
                lblTransactionStatus.Text = "No transactions were found";
                gvTransactions.Visible = false;
            }
        }

        // View transactions per the selected interval
        protected void btnSelectTransByDate_Click(object sender, EventArgs e)
        {
            DataSet dsTrans = pxy.GetAllTransByDate(Convert.ToInt32(ddlInterval.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            if (dsTrans.Tables[0].Rows.Count != 0)
            {
                gvTransactions.DataSource = dsTrans;
                gvTransactions.DataBind();
                gvTransactions.Visible = true;

                lblTransactionStatus.Text = "";
            }
            else
            {
                lblTransactionStatus.Text = "No transactions were found";
                gvTransactions.Visible = false;
            }
        }

        protected void btnSelectAdminTrans_Click(object sender, EventArgs e)
        {
            // Fill control with transaction data
            DataSet dsTrans = pxy.GetAllTransByID(Convert.ToInt32(ddlAdmins.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            if (dsTrans.Tables[0].Rows.Count != 0)
            {
                gvAdminTransactions.DataSource = dsTrans;
                gvAdminTransactions.DataBind();
                gvAdminTransactions.Visible = true;

                lblSuperTransactionStatus.Text = "";
            }
            else
            {
                lblSuperTransactionStatus.Text = "No transactions were found";
                gvAdminTransactions.Visible = false;
            }
        }

        protected void btnAdminTransByDate_Click(object sender, EventArgs e)
        {
            // Fill control with transaction data
            DataSet dsTrans = pxy.GetAllAdminTransByDate(Convert.ToInt32(ddlSuperInterval.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);

            if (dsTrans.Tables[0].Rows.Count != 0)
            {
                gvAdminTransactions.DataSource = dsTrans;
                gvAdminTransactions.DataBind();
                gvAdminTransactions.Visible = true;

                lblSuperTransactionStatus.Text = "";
            }
            else
            {
                lblSuperTransactionStatus.Text = "No transactions were found";
                gvAdminTransactions.Visible = false;
            }

        }

        // Fill page controls with updated information
        public void FillControls()
        {
            DataSet dsAllUsersAdmins = pxy.GetAllUsersAdmins(objAccount.UserEmail, objAccount.UserPassword);
            ddlUser.DataSource = dsAllUsersAdmins;
            ddlUser.DataBind();

            ddlCloudUsers.DataSource = dsAllUsersAdmins;
            ddlCloudUsers.DataBind();

            DataSet dsUsers = pxy.GetAllCloudUsers(objAccount.UserEmail, objAccount.UserPassword);
            ddlUserTrans.DataSource = dsUsers;
            ddlUserTrans.DataBind();

            DataSet dsAdmins = pxy.GetAllCloudAdmins(objAccount.UserEmail, objAccount.UserPassword);
            ddlAdmins.DataSource = dsAdmins;
            ddlAdmins.DataBind();

            DataSet dsRoles = pxy.GetAllRoles(objAccount.UserEmail, objAccount.UserPassword);
            ddlRole.DataSource = dsRoles;
            ddlRole.DataBind();

            // Bind interval drop down lists with intervals
            DataSet dsIntervals = pxy.GetAllIntervals(objAccount.UserEmail, objAccount.UserPassword);
            ddlInterval.DataSource = dsIntervals;
            ddlInterval.DataBind();

            ddlSuperInterval.DataSource = dsIntervals;
            ddlSuperInterval.DataBind();
        }

        protected void ddlCloudUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUserFileStatus.Text = "";

            DataSet dsFiles = pxy.GetFilesByUserID(Convert.ToInt32(ddlCloudUsers.SelectedValue), objAccount.UserEmail, objAccount.UserPassword);
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
            }
            else
            {
                gvUserCloud.Visible = false;
                lblUserFileStatus.Text = "No files were found";
            }
        }

        // Timer event for the live transaction panel
        protected void TimerTransaction_Tick(object sender, EventArgs e)
        {
            DataSet dsUsers = pxy.GetAllCloudTrans(objAccount.UserEmail, objAccount.UserPassword);
            gvAllCloudUserTrans.DataSource = dsUsers;
            gvAllCloudUserTrans.DataBind();
        }
    }
}