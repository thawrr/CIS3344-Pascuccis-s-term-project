using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using Utilities;

namespace TermProjectWS
{
    /// <summary>
    /// Summary description for CloudService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CloudService : System.Web.Services.WebService
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        [WebMethod]
        private bool AuthenticateMethod(string LoginID, string Password)
        {
            // Encrypt password to match with database
            Password = EncryptPass(Password);

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetUserByLoginIDandPass";
            objCommand.Parameters.Clear();

            SqlParameter inputParameter = new SqlParameter("@LoginID", LoginID);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 100;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@Password", Password);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 100;
            objCommand.Parameters.Add(inputParameter);

            DataSet dsUser = objDB.GetDataSetUsingCmdObj(objCommand);

            if (dsUser.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        // Method is used to check for valid login credentials
        // SELECT * FROM tblUser u JOIN tblRole r ON r.RoleID = u.RoleID= UPPER(@LoginID) AND Password = @Password
        [WebMethod]
        public DataSet GetUserByLoginIDandPass(string LoginID, string Password)
        {
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetUserByLoginIDandPass";
            objCommand.Parameters.Clear();

            SqlParameter inputParameter = new SqlParameter("@LoginID", LoginID);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 100;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@Password", EncryptPass(Password));
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 100;
            objCommand.Parameters.Add(inputParameter);

            DataSet dsUser = objDB.GetDataSetUsingCmdObj(objCommand);

            return dsUser;
        }

        [WebMethod]
        public String UpdateAccount(Byte[] byteAccount, Byte[] byteStorage, int UserID, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                String strStatus;

                // Update the account to store the serialized object (binary data) in the database
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "[SyncAccountStorage]";
                objCommand.Parameters.Clear();

                objCommand.Parameters.AddWithValue("@UserID", UserID);
                objCommand.Parameters.AddWithValue("@Account", byteAccount);
                objCommand.Parameters.AddWithValue("@Storage", byteStorage);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                // Check to see whether the update was successful
                if (returnValue > 0)
                    strStatus = "Success";
                else
                    strStatus = "Fail";

                return strStatus;
            }
            else
            {
                String strStatus = "Unauthorized";

                return strStatus;
            }
        }

        public Byte[] SerializeData(DataSet objDS)
        {
            // Create an Account object to store in database
            Account objAccount = new Account();
            objAccount.UserID = Convert.ToInt32(objDS.Tables[0].Rows[0]["UserID"]);
            objAccount.UserLoginID = objDS.Tables[0].Rows[0]["LoginID"].ToString();
            objAccount.UserPassword = EncryptPass(objDS.Tables[0].Rows[0]["HashedPassword"].ToString());
            objAccount.UserFullName = objDS.Tables[0].Rows[0]["Name"].ToString();
            objAccount.UserRole = objDS.Tables[0].Rows[0]["RoleDescription"].ToString();
            objAccount.StorageCapacity = Convert.ToInt32(objDS.Tables[0].Rows[0]["StorageCapacity"]);
            objAccount.StorageUsed = Convert.ToInt32(objDS.Tables[0].Rows[0]["StorageUsed"]);
            objAccount.Active = Convert.ToBoolean(objDS.Tables[0].Rows[0]["isActive"]);

            // Serialize the Account object
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, objAccount);
            byteArray = memStream.ToArray();

            return byteArray;
        }

        [WebMethod]
        public bool AddFile(Byte[] input, int userID, string fileName, string fileType, int fileSize, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                // Serialize the input file (input)
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                Byte[] byteArray;
                serializer.Serialize(memStream, input);
                byteArray = memStream.ToArray();

                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "AddFile";
                objCommand.Parameters.Clear();

                objCommand.Parameters.AddWithValue("@inputData", input);

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileName", fileName);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileType", fileType);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileSize", fileSize);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }

        }

        [WebMethod]
        public bool UpdateFile(int fileID, Byte[] input, int userID, string fileName, string fileType, int fileSize, string LoginID, string Password)
        {
            bool result = false;
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                // Serialize the input file (input)
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                Byte[] byteArray;
                serializer.Serialize(memStream, input);
                byteArray = memStream.ToArray();

                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "UpdateFile";
                objCommand.Parameters.Clear();

                objCommand.Parameters.AddWithValue("@inputData", input);

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileName", fileName);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileType", fileType);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileSize", fileSize);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@fileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    result = true;
                    return result;
                }
                else
                    return result;
            }
            else
                return result;

        }

        [WebMethod]
        public DataSet GetFilesByUserID(int userID, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetFilesByUserID";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty dataSet
            }

        }

        [WebMethod]
        public bool DeleteFile(int fileID, int userID, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "DeleteFile";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@fileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public bool AccountUpdate(int userID, string name, string email, int sc, string newPassword, int roleID, bool isActive, string userEmail, string userPassword)
        {
            if (AuthenticateMethod(userEmail, userPassword) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "UpdateAccount";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@name", name);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@email", email);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@sc", sc);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@pw", EncryptPass(newPassword));
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@roleID", roleID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                int active;
                if (isActive)
                    active = 1;
                else
                    active = 0;

                inputParameter = new SqlParameter("@isActive", active);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 8000;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public bool AddUser(string fullName, string email, string pw)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddUser";
            objCommand.Parameters.Clear();

            SqlParameter inputParameter = new SqlParameter("@fullName", fullName);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 500;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@email", email);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 500;
            objCommand.Parameters.Add(inputParameter);

            inputParameter = new SqlParameter("@password", EncryptPass(pw));
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 500;
            objCommand.Parameters.Add(inputParameter);

            int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

            if (returnValue > 0)
                return true;
            else
                return false;

        }

        [WebMethod]
        public bool CheckEmail(string email)
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.Parameters.Clear();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "CheckEmail";
            objCommand.Parameters.Clear();

            SqlParameter inputParameter = new SqlParameter("@email", email);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 500;
            objCommand.Parameters.Add(inputParameter);

            DataSet objDS = objDB.GetDataSetUsingCmdObj(objCommand);

            if (objDS.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }

        [WebMethod]
        public DataSet GetAllCloudUsers(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllCloudUsers";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetAllUsersAdmins(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllUsersAdmins";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetAllCloudAdmins(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllCloudAdmins";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetAllRoles(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllRoles";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }

        }

        [WebMethod]
        public DataSet GetAllIntervals(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllIntervals";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetUserByID(int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetUserByID";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetAllTransByID(int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllTransByID";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;//empty
            }
        }

        [WebMethod]
        public DataSet GetAllTransByDate(int interval, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllTransByDate";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@interval", interval);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;
            }
        }

        [WebMethod]
        public DataSet GetAllAdminTransByDate(int interval, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllAdminTransByDate";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@interval", interval);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 100;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet dsFiles = new DataSet();
                return dsFiles;
            }
        }

        [WebMethod]
        public DataSet GetOneFile(int fileID, int userID, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetOneFileByID";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@fileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;//return empty dataset

            }
        }

        [WebMethod]
        public DataSet GetAllQuestionsAndAnswers(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllQAs";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;//return empty dataset

            }
        }

        [WebMethod]
        public DataSet GetAllUnansweredQuestions(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetAllUnansweredQuestions";
                objCommand.Parameters.Clear();

                DataSet dsFiles = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsFiles;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;//return empty dataset
            }
        }

        [WebMethod]
        public bool AskQuestion(int userID, string qa, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "AskQuestion";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@question", qa);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public bool AnswerQuestion(int userID, int supportID, string answer, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "AnswerQuestion";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@supportID", supportID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@answer", answer);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public int AddQuestion(int userID, int roleID, string qa, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "AddQA";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@roleID", roleID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@question", qa);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int result = objDB.DoUpdateUsingCmdObj(objCommand);

                return result;
            }
            else
                return -1;//failed logIn
        }

        [WebMethod]
        public DataSet GetStorageOptions(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetStorageOptions";
                objCommand.Parameters.Clear();

                DataSet dsPlans = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsPlans;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;
            }
        }

        [WebMethod]
        public float GetStoragePrice(int optionID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetStoragePrice";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@optionID", optionID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsPrice = objDB.GetDataSetUsingCmdObj(objCommand);

                float price = float.Parse(dsPrice.Tables[0].Rows[0]["Price"].ToString());

                return price;
            }
            else
            {
                return 0;
            }
        }

        [WebMethod]
        public bool UpgradePlan(string creditcard, int ccv, float price, int userID, int storageID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "UpgradePlan";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@creditcard", creditcard);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@ccv", ccv);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@amount", price);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Float;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@storageID", storageID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public DataSet GetFileVersions(int fileID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetFileVersions";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@masterFileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsPlans = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsPlans;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;
            }
        }

        [WebMethod]
        public DataSet GetDeletedFiles(int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetDeletedFiles";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsPlans = objDB.GetDataSetUsingCmdObj(objCommand);

                return dsPlans;
            }
            else
            {
                DataSet empty = new DataSet();
                return empty;
            }
        }

        [WebMethod]
        public bool RestoreDeletedFile(int fileID, int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "RestoreDeletedFile";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@fileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public bool RestoreOldVersion(int storageID, int masterFileID, int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "RestoreOldVersion";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@storageID", storageID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@masterFileID", masterFileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public bool ClearStorage(int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "ClearStorage";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public int GetUserStorageCapacity(int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetUserStorageCapacity";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsPlans = objDB.GetDataSetUsingCmdObj(objCommand);

                int capacity = Convert.ToInt32(dsPlans.Tables[0].Rows[0]["StorageCapacity"]);

                return capacity;
            }
            else
            {
                return -1;
            }
        }

        [WebMethod]
        public bool UpdatePassword(string newPassword, int userID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "UpdatePassword";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@userID", userID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                inputParameter = new SqlParameter("@newPassword", EncryptPass(newPassword));
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.VarChar;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                if (returnValue != -1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public int GetFileSize(int fileID, string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetFileSize";
                objCommand.Parameters.Clear();

                SqlParameter inputParameter = new SqlParameter("@fileID", fileID);
                inputParameter.Direction = ParameterDirection.Input;
                inputParameter.SqlDbType = SqlDbType.Int;
                inputParameter.Size = 500;
                objCommand.Parameters.Add(inputParameter);

                DataSet dsPlans = objDB.GetDataSetUsingCmdObj(objCommand);

                int fileSize = Convert.ToInt32(dsPlans.Tables[0].Rows[0]["fileSize"]);
                return fileSize;
            }
            else
            {
                return -1;
            }
        }

        [WebMethod]
        public DataSet GetAllCloudTrans(string email, string password)
        {
            if (AuthenticateMethod(email, password) == true)
            {
                DBConnect objDB = new DBConnect();
                SqlCommand objCommand = new SqlCommand();

                objCommand.Parameters.Clear();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetTopTenTrans";
                objCommand.Parameters.Clear();

                DataSet userData = objDB.GetDataSetUsingCmdObj(objCommand);
                return userData;
            }
            else
            {
                DataSet userData = new DataSet();
                return userData;//empty dataSet
            }
        }

        [WebMethod]
        public string EncryptPass(string plainTextPassword)
        {
            Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
            Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };
            string encryptedPassword;

            UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
            Byte[] textBytes;                               // stores the plain text data as bytes

            // Perform Encryption
            //-------------------
            // Convert a string to a byte array, which will be used in the encryption process.
            textBytes = encoder.GetBytes(plainTextPassword);

            // Create an instances of the encryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the encrypted data temporarily, and
            // a crypto stream that performs the encryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            // Use the crypto stream to perform the encryption on the plain text byte array.
            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();

            // Retrieve the encrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            // Close all the streams.
            myEncryptionStream.Close();
            myMemoryStream.Close();

            // Convert the bytes to a string and display it.
            encryptedPassword = Convert.ToBase64String(encryptedBytes);

            return encryptedPassword;
        }
    }
}
