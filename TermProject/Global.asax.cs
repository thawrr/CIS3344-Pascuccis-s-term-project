using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            Account objAccount = (Account)Session["Account"];
            Storage objStorage = (Storage)Session["Storage"];

            Byte[] byteAccount = SerializeAccount(objAccount);
            Byte[] byteStorage = SerializeStorage(objStorage);

            Console.WriteLine(pxy.UpdateAccount(byteAccount, byteStorage, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword));
        }

        public Byte[] SerializeAccount(Account objAccount)
        {
            // Serialize the Account object
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, objAccount);
            byteArray = memStream.ToArray();

            return byteArray;
        }

        public Byte[] SerializeStorage(Storage objStorage)
        {
            // Serialize the Account object
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, objStorage);
            byteArray = memStream.ToArray();

            return byteArray;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}