using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DashBoardDB
{
    class ManageDB
    {
        public MySqlConnection ConnectionToDB()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(@"user id=root;password=123321Aa.;server=localhost;database=market;persistsecurityinfo=True");
            }
            catch (MySqlException DBExc)
            {
                MessageBox.Show("Error Connection to DB \n Error Code: " + DBExc.HResult);
            }
            return connection;
        }
    }
}
