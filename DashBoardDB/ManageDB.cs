﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DashBoardDB
{
    class ManageDB
    {
        MySqlConnection connection;
        MySqlCommand cmd;
        public ManageDB()
        {
            ConnectionToDB();
            try
            {
                connection.Open();
            }
            catch (MySqlException MYSQLEX)
            {
                //
            }
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
            SQLstatment = "SELECT idCompanies FROM companies WHERE CompanyName = '" + CompanyName + "'";
            using (cmd = new MySqlCommand(SQLstatment, connection))
            {
                var CompanyDontExistChecker = cmd.ExecuteScalar();
                if (CompanyDontExistChecker is not DBNull)
                    CompanyID = Convert.ToInt32(CompanyDontExistChecker);
                else
                    return false;
            }
            SQLstatment = "SELECT idProductTypes FROM producttypes WHERE ProductTypesName = '" + ProductTypeName + "'";
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
        public bool InsertOrder(List<String> ProductNames, List<int> ProductQuanties, List<Double> ProductsProfitPriceForEachItem, Double TotalProfitPrice, DateTime OrderDate)
        {
            List<Double> ProductsOGPriceForEachItem = new List<double>();
            List<int> ProductIDS = new List<int>();
            int OrderID = 0;
            int ProductCount = ProductNames.Count;
            Double EachProductTotalOGprice, EachProductTotalProfitPrice, TotalProductOGprice = 0, ProfitFromOrder;
            String SQLstatemnt = "INSERT INTO orders (`TotalPrice`, `ProfitFromOrder`, `OrderDate`) VALUES(@TotalPriceP, '0', @DateOfTheOrderP)";
            int RowAffected = 0;
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                cmd.Parameters.AddWithValue("@TotalPriceP", TotalProfitPrice);
                cmd.Parameters.AddWithValue("@DateOfTheOrderP", OrderDate);
                RowAffected = cmd.ExecuteNonQuery();
            }
            if (RowAffected == 0)
                return false;
            foreach (String ProductName in ProductNames)
            {
                ProductsOGPriceForEachItem.Add(GetProductInfo("ProductsOGprice", ProductName));
                ProductIDS.Add(Convert.ToInt32(GetProductInfo("idProducts", ProductName)));
            }
            for (int i = 0; i < ProductCount; i++)
            {
                SQLstatemnt = "UPDATE `products` SET Quantity = Quantity - " + ProductQuanties[i] + " WHERE (`idProducts` = '" + ProductIDS[i] + "')";
                using (cmd = new MySqlCommand(SQLstatemnt, connection))
                    cmd.ExecuteNonQuery();
            }
            SQLstatemnt = "SELECT idOrders FROM orders ORDER BY idOrders DESC LIMIT 1";
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                OrderID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            for (int i = 0; i < ProductCount; i++)
            {
                EachProductTotalOGprice = ProductsOGPriceForEachItem[i] * ProductQuanties[i];
                TotalProductOGprice += EachProductTotalOGprice;
                EachProductTotalProfitPrice = ProductsProfitPriceForEachItem[i] * ProductQuanties[i];
                SQLstatemnt = "INSERT INTO orderproducts (`ProductID`, `OrderID`, `ProfitPrice`, `OGprice`, `Quantity`) " +
                    "VALUES (@ProductIDP, @OrderIDP, @ProfitPriceP, @OGpriceP, @QuantityP)";
                using (cmd = new MySqlCommand(SQLstatemnt, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductIDP", ProductIDS[i]);
                    cmd.Parameters.AddWithValue("@OrderIDP", OrderID);
                    cmd.Parameters.AddWithValue("@ProfitPriceP", EachProductTotalProfitPrice);
                    cmd.Parameters.AddWithValue("@OGpriceP", EachProductTotalOGprice);
                    cmd.Parameters.AddWithValue("@QuantityP", ProductQuanties[i]);
                    RowAffected = cmd.ExecuteNonQuery();
                }
            }
            if (RowAffected == 0)
                return false;
            ProfitFromOrder = TotalProfitPrice - TotalProductOGprice;
            SQLstatemnt = "UPDATE `orders` SET `ProfitFromOrder` = '" + ProfitFromOrder +
                "' WHERE (`idOrders` = '" + OrderID + "')";
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
                RowAffected = cmd.ExecuteNonQuery();
            if (RowAffected == 0)
                return false;
            return true;
        }
        public bool DeleteRecord(String TableName, String ColumnName, String ColumValue)
        {
            String SQLstatemnt = "";
            int DeleteChecker = 0;
            if (TableName == "orders")
            {
                {
                    SQLstatemnt = "DELETE FROM `orderproducts` WHERE (`OrderID` = '" + ColumValue + "')";
                    using (cmd = new MySqlCommand(SQLstatemnt, connection))
                        cmd.ExecuteNonQuery();
                }
            }
            if (TableName != "orders")
                SQLstatemnt = "DELETE FROM " + TableName + " WHERE " + ColumnName + " = '%" + ColumValue + "%'";
            else
                SQLstatemnt = "DELETE FROM " + TableName + " WHERE (`" + ColumnName + "` = '" + ColumValue + "')";
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
        public Double GetProductPriceWithQuantity(String ProductName, int Quantity)
        {
            Double TotalPrice = 0;// = ProfitPrice*Quantity
            String SQLstatemnt = "SELECT ProductProfitPrice FROM `products` WHERE ProductName = '" + ProductName + "'";
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                var ProductNotExist = cmd.ExecuteScalar();
                if (ProductNotExist is not DBNull)
                    TotalPrice = Convert.ToDouble(ProductNotExist);
            }
            TotalPrice *= Quantity;
            return TotalPrice;
        }
        public Double GetProductInfo(String ColomnName, String ProductName)//Work For everything expect date for now
        {
            Double Info = 0;
            String SQLstatemnt = "SELECT " + ColomnName + " FROM `products` WHERE ProductName = '" + ProductName + "'";
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                var ProductNotExist = cmd.ExecuteScalar();
                if (ProductNotExist is not DBNull)
                    Info = Convert.ToDouble(ProductNotExist);
            }
            return Info;
        }

        public void UpdateEachTypeProfit()
        {
            List<Double> TotalProfit = new List<Double>();
            List<int> TypeID = new List<int>();
            String SQLstatemnt = "SELECT  p.ProductsTypeID, SUM(op.ProfitPrice - op.OGprice) " +
                "FROM `products` AS p JOIN `orderproducts` AS op ON p.idProducts = op.ProductID GROUP BY p.ProductsTypeID";
            MySqlDataReader reader;
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TypeID.Add(reader.GetInt32(0));
                    TotalProfit.Add(reader.GetDouble(1));
                }
                reader.Close();
            }
            for (int i = 0; i < TotalProfit.Count; i++)
            {
                SQLstatemnt = " UPDATE `producttypes` SET TypeProfit = " + TotalProfit[i] + " WHERE (`idProductTypes` = '" + TypeID[i] + "')";
                using (cmd = new MySqlCommand(SQLstatemnt, connection))
                    cmd.ExecuteNonQuery();
            }
        }
        public void GetEachTypeProfitWithDate(List<String> TypesName, List<Double> TotalProfit, int Date)
        {
            String SQLstatemnt;
            List<int> OrdersID = OrdersID = GetOrdersWithDate(Date);
            MySqlDataReader reader;
            int CurrentTypeID = -1;//-1 because there is no -1 id
            Double SumProfit = 0;
            bool FirstTimeFlag = false;
            foreach (int ID in OrdersID)
            {
                SQLstatemnt = "SELECT pt.ProductTypesName , pt.idProductTypes, SUM(op.ProfitPrice - op.OGprice) FROM `orderproducts` AS op " +
                    "INNER JOIN `products` AS p ON p.idProducts = op.ProductID " +
                    "INNER JOIN `producttypes` as pt ON pt.idProductTypes  = p.ProductsTypeID WHERE OrderID = " + ID + " GROUP BY pt.idProductTypes ";
                using (cmd = new MySqlCommand(SQLstatemnt, connection))
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (FirstTimeFlag == false)
                        {
                            TypesName.Add(reader.GetString(0));
                            CurrentTypeID = reader.GetInt32(1);
                            FirstTimeFlag = true;
                        }
                        if (reader.GetInt32(1) != CurrentTypeID)
                        {
                            TypesName.Add(reader.GetString(0));
                            CurrentTypeID = reader.GetInt32(1);
                            TotalProfit.Add(SumProfit);
                            SumProfit = reader.GetDouble(2);
                        }
                        else
                        {
                            SumProfit += reader.GetDouble(2);
                        }
                        if (ID == OrdersID.Last())
                            TotalProfit.Add(SumProfit);

                    }
                    reader.Close();
                }
            }
            for (int i = 0; i < TotalProfit.Count; i++)
                for (int j = i + 1; i < TotalProfit.Count; j++)
                {
                    if (j > TotalProfit.Count - 1 || i > TotalProfit.Count - 1)
                        break;
                    if (TypesName[i] == TypesName[j])
                    {
                        TotalProfit[i] += TotalProfit[j];
                        TotalProfit.RemoveAt(j);
                        TypesName.RemoveAt(j);
                        j--;
                    }
                }
        }
        public List<int> GetOrdersWithDate(int Date)
        {
            List<int> OrdersIDs = new List<int>();
            String SQLstatemnt = "";
            if (Date == 0)
                SQLstatemnt = "SELECT idOrders FROM orders";
            else
                SQLstatemnt = "SELECT idOrders FROM orders WHERE DATE(OrderDate) > CURRENT_DATE() - interval " + Date + " day";
            MySqlDataReader reader;
            using (cmd = new MySqlCommand(SQLstatemnt, connection))
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    OrdersIDs.Add(reader.GetInt32(0));
                reader.Close();
            }
            return OrdersIDs;
        }
        public void GetProductsSales(List<String> ProductsNames, List<int> ProductsSales, int ProductsNum, int Date)
        {
            List<int> OrdersIDs = null;
            String SqlStatemnt = "";
            bool DateEnabled = false;
            if (ProductsNum == 0 && Date == 0)
                SqlStatemnt = "SELECT p.ProductName, SUM(op.Quantity) FROM orderproducts AS op join products AS p " +
                "ON p.idProducts = op.ProductID GROUP BY p.ProductName order by SUM(op.Quantity) DESC";
            else if (ProductsNum != 0 && Date == 0)
                SqlStatemnt = "SELECT p.ProductName, SUM(op.Quantity) FROM orderproducts AS op join products AS p " +
                "ON p.idProducts = op.ProductID GROUP BY p.ProductName order by SUM(op.Quantity) DESC LIMIT " + ProductsNum;
            else if (ProductsNum == 0 || Date != 0)
            {
                DateEnabled = true;
                OrdersIDs = GetOrdersWithDate(Date);
            }
            MySqlDataReader reader;
            if (DateEnabled == false)
            {
                using (cmd = new MySqlCommand(SqlStatemnt, connection))
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductsNames.Add(reader.GetString(0));
                        ProductsSales.Add(reader.GetInt32(1));
                    }
                    reader.Close();
                }
            }
            else
            {
                foreach (int ID in OrdersIDs)
                {
                    SqlStatemnt = "SELECT p.ProductName, SUM(op.Quantity) FROM orderproducts AS op join products AS p " +
                    "ON p.idProducts = op.ProductID WHERE op.OrderID = " + ID + " GROUP BY p.ProductName order by SUM(op.Quantity) DESC ";
                    using (cmd = new MySqlCommand(SqlStatemnt, connection))
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ProductsNames.Add(reader.GetString(0));
                            ProductsSales.Add(reader.GetInt32(1));
                        }
                        reader.Close();
                    }
                }
                for (int i = 0; i < ProductsSales.Count; i++)
                    for (int j = i + 1; i < ProductsSales.Count; j++)
                    {
                        if (j > ProductsSales.Count - 1 || i > ProductsSales.Count - 1)
                            break;
                        if (ProductsNames[i] == ProductsNames[j])
                        {
                            ProductsSales[i] += ProductsSales[j];
                            ProductsSales.RemoveAt(j);
                            ProductsNames.RemoveAt(j);
                            j--;
                        }
                    }
            }
        }
        public void GetProfit(List<DateTime> Dates, List<Double> Profit, int Date)
        {
            String SqlStatemnt = "SELECT Date(OrderDate), SUM(ProfitFromOrder) FROM orders " +
                "WHERE DATE(OrderDate) > CURRENT_DATE() - interval " + Date + " day GROUP BY OrderDate ORDER BY OrderDate DESC";
            MySqlDataReader reader;
            using (cmd = new MySqlCommand(SqlStatemnt, connection))
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Dates.Add(reader.GetDateTime(0));
                    Profit.Add(reader.GetDouble(1));
                }
                reader.Close();
            }
        }
        public void CloseConnetion()
        {
            connection.Close();
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                CloseConnetion();
            }
        }
    }
}
