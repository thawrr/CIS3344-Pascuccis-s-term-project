using GlobalMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class CustomControlNavBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["Account"] = null;
            Session["Storage"] = null;
            Response.Redirect("Login.aspx");
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            CloudSvc.CloudService pxy = new CloudSvc.CloudService();
            Account objAccount = (Account)Session["Account"];
            Storage objStorage = (Storage)Session["Storage"];
            
            Byte[] byteAccount = SerializeAccount(objAccount);
            Byte[] byteStorage = SerializeStorage(objStorage);

            lblSyncStatus.Text = pxy.UpdateAccount(byteAccount, byteStorage, objAccount.UserID, objAccount.UserEmail, objAccount.UserPassword);
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
    }
}