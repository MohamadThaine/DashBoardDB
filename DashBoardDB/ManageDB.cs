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
            MySqlConnection connection = new MySqlConnection(@"user id=root;password=123321Aa.;server=localhost;database=market;persistsecurityinfo=True");
            return connection;
        }
    }
}
