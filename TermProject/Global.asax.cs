using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace TermProject
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            CloudSvc.CloudService pxy = new CloudSvc.CloudService();

            // Update the DB with serialized Account object
            Account objAccount = (Account)Session["Account"];

            DataSet objDS = new DataSet();
            //objDS.Tables.Add(dt);

            objDS = pxy.GetUserByLoginIDandPass(objAccount.UserLoginID, objAccount.UserPassword);

            String strPlaceHolder = pxy.UpdateAccount(objDS, objAccount.UserID);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}