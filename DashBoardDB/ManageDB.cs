using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DashBoardDB
{
    class ManageDB
    {
        MySqlConnection connection;
        MySqlCommand cmd;
        public ManageDB()
        {
            ConnectionToDB();
        }
        public MySqlConnection ConnectionToDB()
        {
            connection = new MySqlConnection(@"user id=root;password=123321Aa.;server=localhost;database=market;persistsecurityinfo=True");
            return connection;
        }
        public bool insertProduct(String ProductName, String ProductTypeName, String CompanyName, int Quantity, Double ProductOGprice, Double ProductPfprice, DateTime EXPdate)
        {
            int ProductTypeID = 0, CompanyID = 0;
            String SQLstatment;
            int RowAffected = 0;
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
            SQLstatment = "INSERT INTO products (`ProductName`, `ProductsTypeID`, `CompanyID`, `Quantity`, `ProductsOGprice`, `ProductProfitPrice`, `ExpDate`)" +
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
                RowAffected = cmd.ExecuteNonQuery();

            }
            if (RowAffected == 0)
                return false;
            return true;
        }
        public bool InsertCompany(String CompanyName, String CompanyEmail, Double CompanyNum)
        {
            String SQLstatment = "INSERT INTO companies (`CompanyName`, `PhoneNumber`, `Email`) " +
                "VALUES (@CompanyNameP,@CompanyNum, @CompanyEmailP)";
            int RowAffected = 0;
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                cmd.Parameters.AddWithValue("@CompanyNameP", CompanyName);
                cmd.Parameters.AddWithValue("@CompanyNum", CompanyNum);
                cmd.Parameters.AddWithValue("@CompanyEmailP", CompanyEmail);
                RowAffected = cmd.ExecuteNonQuery();
            }
            if (RowAffected == 0)
                return false;
            return true;
        }
        public bool DeleteRecord(String TableName, String ColumnName, String ColumValue)
        {
            String SQLstatemnt = "";
            if (TableName != "orders")
                SQLstatemnt = "DELETE FROM " + TableName + " WHERE " + ColumnName + " LIKE '%" + ColumValue + "%'";
            else
                SQLstatemnt = "DELETE FROM " + TableName + " WHERE (`" + ColumnName + "` = '" + ColumValue + "')";
            int DeleteChecker = 0;
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                DeleteChecker = cmd.ExecuteNonQuery();//Return Number of row affected , if DeleteChecker == 0 then it failed , if 1 then it worked
            }
            if (DeleteChecker == 0)
                return false;
            return true;
        }
        public List<String> GetAllTypes()
        {
            List<String> TypesName = new List<String>();
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
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
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
