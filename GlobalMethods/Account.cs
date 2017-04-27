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
        private int Capacity;
        private int Used;

        private String FName;
        private String FType;
        private int FSize;
        private float FVersion;


        public int UserID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int StorageCapacity
        {
            get { return Capacity; }
            set { Capacity = value; }
        }

        public int StorageUsed
        {
            get { return Used; }
            set { Used = value; }
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

        public String FileName
        {
            get { return FName; }
            set { FName = value; }
        }

        public String FileType
        {
            get { return FType; }
            set { FType = value; }
        }

        public int FileSize
        {
            get { return FSize; }
            set { FSize = value; }
        }

        public float FileVersion
        {
            get { return FVersion; }
            set { FVersion = value; }
        }
    }
}
