using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalMethods
{
    [Serializable]
    public class Account
    {
        private int ID;
        private String LoginID;
        private String Password;
        private String Name;
        private String Role;
        private String Email;

        public int UserID
        {
            get { return ID; }
            set { ID = value; }
        }

        public String UserEmail
        {
            get { return Email; }
            set { Email = value; }
        }

        public String UserLoginID
        {
            get { return LoginID; }
            set { LoginID = value; }
        }

        public String UserPassword
        {
            get { return Password; }
            set { Password = value; }
        }

        public String UserFullName
        {
            get { return Name; }
            set { Name = value; }
        }

        public String UserRole
        {
            get { return Role; }
            set { Role = value; }
        }
    }
}
