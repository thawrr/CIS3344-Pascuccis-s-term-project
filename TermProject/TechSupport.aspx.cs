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
    public partial class TechSupport : System.Web.UI.Page
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

                // If an admin is visiting, then show respective panel
                if (objAccount.UserRole == "Cloud Admin" || objAccount.UserRole == "Super Admin")
                {
                    pnlAnswer.Visible = true;
                }
                else
                {
                    pnlAsk.Visible = true;
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

        public void FillControls()
        {
            DataSet dsFiles = pxy.GetAllQuestionsAndAnswers(objAccount.UserEmail, objAccount.UserPassword);

            if (dsFiles.Tables[0].Rows.Count != 0)
            {
                gvViewQuestions.DataSource = dsFiles;
                gvViewQuestions.DataBind();

            }
            else
            {
                lblStatus.Text = "No questions have been asked";
            }

            DataSet dsUnansweredQuestions = pxy.GetAllUnansweredQuestions(objAccount.UserEmail, objAccount.UserPassword);

            if (dsUnansweredQuestions.Tables[0].Rows.Count != 0)
            {
                ddlUnansweredQuestions.DataSource = dsUnansweredQuestions;
                ddlUnansweredQuestions.DataBind();
            }
            else
            {
                ddlUnansweredQuestions.Visible = false;
                txtAnswer.Visible = false;
                btnSubmitAnswer.Visible = false;
                lblAnswerStatus.Text = "There are no unanswered questions at this time. Check back later to answer a question.";
            }

        }

        protected void btnSubmitQuestion_Click(object sender, EventArgs e)
        {
            lblAskStatus.Text = "";

            if (txtQuestion.Text != "" && txtQuestion.Text.Length >= 20)
            {
                if (pxy.AskQuestion(objAccount.UserID, txtQuestion.Text, objAccount.UserEmail, objAccount.UserPassword))
                {
                    lblAskStatus.Text = "Question posted! Admin will answer your question shortly.";
                    FillControls();
                }
                else
                {
                    lblAskStatus.Text = "An error has occured. Your question has not been posted. Try again or consult an admin.";
                }
            }
            else
            {
                lblAskStatus.Text = "Please enter at least 20 characters!";
            }
        }

        protected void btnSubmitAnswer_Click(object sender, EventArgs e)
        {
            lblAnswerStatus.Text = "";

            if (txtAnswer.Text != "" && txtAnswer.Text.Length >= 20)
            {
                if (pxy.AnswerQuestion(objAccount.UserID, Convert.ToInt32(ddlUnansweredQuestions.SelectedValue), txtAnswer.Text, objAccount.UserEmail, objAccount.UserPassword))
                {
                    txtAnswer.Text = "";
                    FillControls();
                }
                else
                {
                    lblAnswerStatus.Text = "An error has occured. Your question has not been posted. Try again or consult an admin.";
                }
            }
            else
            {
                lblAnswerStatus.Text = "Please enter at least 20 characters!";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string password = txtUpdatePassword.Text;

            if (password.Length >= 6)
            {
                if (password.Contains('!') || password.Contains('@') || password.Contains('#') || password.Contains('$') || password.Contains('%') || password.Contains('&'))
                {
                    if (pxy.UpdatePassword(password, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword))
                    {
                        txtUpdatePassword.Text = "";
                        lblUpdateStatus.Text = "Password has been updated.";
                        objAccount.UserPassword = password;
                    }
                    else
                        lblUpdateStatus.Text = "An error occured and the password has not been updated.";
                }
            }
            else
                lblUpdateStatus.Text = "Invalid password format.";
        }
    }
}