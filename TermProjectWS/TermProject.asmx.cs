﻿using GlobalMethods;
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
    /// Summary description for TermProject
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TermProject : System.Web.Services.WebService
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

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
        public String UpdateAccount(DataSet objDS, int UserID)
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


        [WebMethod]
        public Byte[] SerializeData(DataSet objDS)
        {
            // Create an Account object to store in database
            Account objAccount = new Account();
            objAccount.UserID = Convert.ToInt32(objDS.Tables[0].Rows[0]["UserID"]);
            objAccount.UserLoginID = objDS.Tables[0].Rows[0]["LoginID"].ToString();
            objAccount.UserPassword = objDS.Tables[0].Rows[0]["HashedPassword"].ToString();
            objAccount.UserFullName = objDS.Tables[0].Rows[0]["Name"].ToString();
            objAccount.UserRole = objDS.Tables[0].Rows[0]["RoleDescription"].ToString();

            // Serialize the Account object
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, objAccount);
            byteArray = memStream.ToArray();

            return byteArray;
        }
    }
}
