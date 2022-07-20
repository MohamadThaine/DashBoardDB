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
        public ManageDB()
        {
            ConnectionToDB();
        }
        MySqlConnection connection;
        public MySqlConnection ConnectionToDB()
        {
            connection = new MySqlConnection(@"user id=root;password=123321Aa.;server=localhost;database=market;persistsecurityinfo=True");
            return connection;
        }
        public bool insertProduct(String ProductName , String ProductTypeName , String CompanyName , int Quantity , Double ProductOGprice , Double ProductPfprice , DateTime EXPdate)
        {
            MySqlCommand cmd;
            int ProductTypeID = 0 , CompanyID = 0;
            String SQLstatment;
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (MySqlException MYSQLEX)
                {
                    return false;
                }
            }
                
            SQLstatment = "SELECT idCompanies FROM companies WHERE CompanyName LIKE '%" + CompanyName + "%'";
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                var CompanyDontExistChecker = cmd.ExecuteScalar();
                if (CompanyDontExistChecker is not DBNull)
                    CompanyID = Convert.ToInt32(CompanyDontExistChecker);
                else
                    return false;
            }
            SQLstatment = "SELECT idProductTypes FROM producttypes WHERE ProductTypesName LIKE '%" + ProductTypeName + "%'";
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                var TypeDontExistChecker = cmd.ExecuteScalar();
                if (TypeDontExistChecker is not DBNull)
                    ProductTypeID = Convert.ToInt32(TypeDontExistChecker);
                else
                    return false;
            }
            SQLstatment = "INSERT INTO products (`ProdcutName`, `ProductsTypeID`, `CompanyID`, `Quantity`, `ProductsOGprice`, `ProductProfitPrice`, `ExpDate`)" +
                " VALUES (@ProductNameP , @ProductTypeIDP , @CompanyIDP , @QuantityP , @OGpriceP , @ProfitPriceP , @ExpDateP)";//P in the end means parameter
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                cmd.Parameters.AddWithValue("@ProductNameP", ProductName);
                cmd.Parameters.AddWithValue("@ProductTypeIDP", ProductTypeID);
                cmd.Parameters.AddWithValue("@CompanyIDP", CompanyID);
                cmd.Parameters.AddWithValue("@QuantityP", Quantity);
                cmd.Parameters.AddWithValue("@OGpriceP", ProductOGprice);
                cmd.Parameters.AddWithValue("@ProfitPriceP", ProductPfprice);
                cmd.Parameters.AddWithValue("@ExpDateP", EXPdate);
                cmd.ExecuteNonQuery();
               
            }
            return true;
        }
        public List<String> GetAllTypes()
        {
            List<String> TypesName = new List<String>();
            MySqlCommand cmd;
            MySqlDataReader reader;
            String SQLstatment = "SELECT ProductTypesName FROM producttypes";
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (MySqlException MYSQLEX)
                {
                    //
                }
            }
            using (cmd = new MySqlCommand(SQLstatment , connection))
            {
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    TypesName.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return TypesName;
        }
        public List<String> GetAllCompanies()
        {
            List<String> CompanyName = new List<String>();
            MySqlCommand cmd;
            MySqlDataReader reader;
            String SQLstatment = "SELECT CompanyName FROM companies";
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (MySqlException MYSQLEX)
                {
                    //
                }
            }
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CompanyName.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return CompanyName;
        }
    }
}
