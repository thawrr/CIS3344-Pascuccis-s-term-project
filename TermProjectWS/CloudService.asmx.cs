using GlobalMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public bool AuthenticateMethod(string LoginID, string Password)
        {

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
        // 	SELECT * FROM tblUser u JOIN tblRole r ON r.RoleID = u.RoleID= UPPER(@LoginID) AND Password = @Password
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

            inputParameter = new SqlParameter("@Password", Password);
            inputParameter.Direction = ParameterDirection.Input;
            inputParameter.SqlDbType = SqlDbType.VarChar;
            inputParameter.Size = 100;
            objCommand.Parameters.Add(inputParameter);

            DataSet dsUser = objDB.GetDataSetUsingCmdObj(objCommand);

            return dsUser;
        }

        [WebMethod]
        public String UpdateAccount(DataSet objDS, int UserID, string LoginID, string Password)
        {
            if (AuthenticateMethod(LoginID, Password) == true)
            {
                String strStatus;
                //DataSet objDS = GetUserByLoginIDandPass(objAccount.UserLoginID, objAccount.UserPassword);

                Byte[] byteArray = SerializeData(objDS);

                // Update the account to store the serialized object (binary data) in the database
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "StoreAccount";
                objCommand.Parameters.Clear();

                objCommand.Parameters.AddWithValue("@UserID", UserID);
                objCommand.Parameters.AddWithValue("@Account", byteArray);

                int returnValue = objDB.DoUpdateUsingCmdObj(objCommand);

                // Check to see whether the update was successful
                if (returnValue > 0)
                    strStatus = "The account was successfully stored for this user.";
                else
                    strStatus = "A problem occured in storing the account info.";

                return strStatus;
            }
            else
            {
                String strStatus = "unauthorized";

                return strStatus;
            }

        }

        public Byte[] SerializeData(DataSet objDS)
        {
            // Create an Account object to store in database
            Account objAccount = new Account();
            objAccount.UserID = Convert.ToInt32(objDS.Tables[0].Rows[0]["UserID"]);
            objAccount.UserLoginID = objDS.Tables[0].Rows[0]["LoginID"].ToString();
            objAccount.UserPassword = objDS.Tables[0].Rows[0]["HashedPassword"].ToString();
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
        public bool UpdateFile(int fileID, Byte[] input, int userID, string fileName, string fileType, int fileSize, int roleID, string LoginID, string Password)
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

                inputParameter = new SqlParameter("@roleID", roleID);
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
        public bool AccountUpdate(int userID, string name, string email, int sc, string pw, int roleID, bool isActive)
        {
            if (AuthenticateMethod(email, pw) == true)
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

                inputParameter = new SqlParameter("@pw", pw);
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

            inputParameter = new SqlParameter("@password", pw);
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
    }
}
