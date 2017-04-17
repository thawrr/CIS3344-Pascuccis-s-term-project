using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace GlobalMethods
{
    public class Validation
    {
        public bool IsPresentAndText(TextBox textBox)
        {
            if (textBox.Text == "" && Regex.IsMatch(textBox.Text, @"^[a-zA-Z]+$") == false)
            {
                //MessageBox.Show(name + " is a required field.", "Entry Error");
                return false;
            }
            else
                return true;
        }

        public bool IsText(TextBox textBox)
        {
            if (Regex.IsMatch(textBox.Text, @"^[a-zA-Z]+$") == false)
            {
                //MessageBox.Show(name + " is a required field.", "Entry Error");
                return false;
            }
            else
                return true;
        }

        public bool IsInt(TextBox textBox)
        {
            int value;
            if (int.TryParse(textBox.Text, out value) == true)
            {
                return true;
            }
            else
                return false;
        }

        public bool IsDecimal(TextBox textBox)
        {
            decimal num = 0m;
            if (Decimal.TryParse(textBox.Text, out num) == true)
            {

                return true;
            }
            else
            {
                //MessageBox.Show(name + " must be a decimal value.", "Entry Error");
                return false;
            }
        }

        public bool IsValidEmail(TextBox textBox)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(textBox.Text);
                return addr.Address == textBox.Text;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckForData(DataSet ds)
        {
            bool dataFound = false;
            if (ds != null && ds.Tables.Count > 0)
            {
                // Check the row count for each table to see if any one table has data.
                foreach (System.Data.DataTable table in ds.Tables)
                {
                    if (table.Rows.Count > 0)
                    {
                        //we have data
                        dataFound = true;
                        break;
                    }
                }
            }
            if (dataFound)
            {
                Console.WriteLine("YES we have data");
                return true;
            }
            else
            {
                Console.WriteLine("we have NO NO NO NO NO data");
                return false;
            }
        }
    }
}
