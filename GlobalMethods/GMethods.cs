using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace GlobalMethods
{
    public class GMethods
    {
        public DataSet GetUserNames()//get everything from Users Table
        {
            DBConnect objDB = new DBConnect();
            SqlCommand objCommand = new SqlCommand();

            objCommand.Parameters.Clear();//use this before executing a stored procedure
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetAllUsers"; // name of the Stored Procedure

            objDB.GetConnection();
            DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);
            objDB.CloseConnection();
            
            return ds;
        }

    }//end class
}
