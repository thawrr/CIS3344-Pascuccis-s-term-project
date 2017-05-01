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
    public partial class CustomControlTechSupport : System.Web.UI.UserControl
    {
        Account objAccount = new Account();
        CloudSvc.CloudService pxy = new CloudSvc.CloudService();

        protected void Page_Load(object sender, EventArgs e)
        {
            objAccount = (Account)Session["Account"];
            objAccount.UserEmail = ((Account)Session["Account"]).UserEmail;
            objAccount.UserPassword = ((Account)Session["Account"]).UserPassword;

            if (!IsPostBack)
                FillControls();
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
                lblQAStatus.Text = "No questions have been asked";
            }
        }

        protected void timerQA_Tick(object sender, EventArgs e)
        {
            FillControls();
        }
    }
}