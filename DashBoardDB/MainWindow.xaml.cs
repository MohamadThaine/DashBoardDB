using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Net.Mail;
using System.Net;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DashBoardDB
{
    [ObservableObject]
    public partial class MainWindow : Window
    {
        Double appVersion = 0.2;
        Double LastVersion = 0;
        MySqlConnection connection = null;
        Double[] ArrayCounter = new Double[5];
        string DBError = "";
        public MainWindow()
        {
            ManageDB DBmanager = new();
            connection = DBmanager.ConnectionToDB();
            if (connection != null)
            {
                try
                {
                    connection.Open();
                }catch(MySqlException MySQLEX)
                {
                    DBError = "There was an error Connecting to the DB \n Error Code is: " + MySQLEX.HResult;
                    return;
                }
                ArrayCounter = GetProductNumber();
                LastVersion = GetLastVersion();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DBError != "")
            {
                MessageBox.Show(DBError, "Error with the DataBase");
                Application.Current.Shutdown();
            }
            PrepareCountersText();
        }
        private Double GetLastVersion()
        {
            Double LastVersion = 0;
            String GetLastVersionFromDB = "SELECT VersionNum FROM aboutapp ORDER BY idAboutApp DESC LIMIT 1";
            MySqlCommand cmd;
            using (cmd = new MySqlCommand(GetLastVersionFromDB, connection))
            {
                LastVersion = Convert.ToDouble(cmd.ExecuteScalar());
            }
            return LastVersion;
        }
        private Double[] GetProductNumber()
        {
            Double[] CountersArray = new Double[5];
            MySqlCommand cmd;
            String GetCounterOfProducts = "SELECT COUNT(*) FROM products";
            String GetCounterOfCompanies = "SELECT COUNT(*) FROM companies";
            String GetCounterOf7DaysProfit = "SELECT SUM(ProfitFromOrder) FROM orders WHERE DATE(OrderDate) > CURRENT_DATE() - interval 7 day";
            String GetTodayOrdersCounter = "SELECT COUNT(*) FROM orders WHERE DATE(OrderDate) = CURRENT_DATE()";
            String GetTodayProfit = "SELECT SUM(ProfitFromOrder) FROM orders WHERE DATE(OrderDate) = CURRENT_DATE() ";

            using (cmd = new MySqlCommand(GetCounterOfProducts, connection))
            {
                CountersArray[0] = Convert.ToInt32(cmd.ExecuteScalar());
            }
            using (cmd = new MySqlCommand(GetCounterOfCompanies, connection))
            {
                CountersArray[1] = Convert.ToInt32(cmd.ExecuteScalar());
            }
            using (cmd = new MySqlCommand(GetCounterOf7DaysProfit, connection))
            {
                var NoPrfitChecker = cmd.ExecuteScalar();
                if(NoPrfitChecker is not DBNull)
                {
                    CountersArray[2] = Convert.ToDouble(NoPrfitChecker);
                }
            }
            using (cmd = new MySqlCommand(GetTodayOrdersCounter, connection))
            {
                var NoOrdersTodayChecker = cmd.ExecuteScalar();
                if (NoOrdersTodayChecker is not DBNull)
                {
                    CountersArray[3] = Convert.ToInt32(NoOrdersTodayChecker);
                }
            }
            using (cmd = new MySqlCommand(GetTodayProfit, connection))
            {
                var NoProfitTodayChecker = cmd.ExecuteScalar();
                if (NoProfitTodayChecker is not DBNull)
                {
                    CountersArray[4] = Convert.ToDouble(NoProfitTodayChecker);
                }
            }
            return CountersArray;
        }
        private void PrepareCountersText()
        {
            TotalProducts.Text = ArrayCounter[0].ToString();
            TotalCompanies.Text = ArrayCounter[1].ToString();
            TotalProfits.Text = ArrayCounter[2].ToString() + "$";
            OrdersToday.Text = ArrayCounter[3].ToString();
            ProfitToday.Text = ArrayCounter[4].ToString() + "$";
        }
        private void Email_Click(object sender, RoutedEventArgs e)
        {
            EmailReportWindow emailReportWindow = new();
            emailReportWindow.Show();
        }
        private void Pdf_Click(object sender, RoutedEventArgs e)
        {
            PdfReportWindow pdfReportWindow = new();
            pdfReportWindow.Show();
        }
        private void AddRemovebt(object sender, RoutedEventArgs e)
        {
            AddRemoveEdIt addRemoveEdIt = new();
            addRemoveEdIt.Show();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure?", "Exit Confirmation", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void MenuClick(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }
        private void CheckForUpdateClick(object sender, RoutedEventArgs e)
        {
            if(appVersion == LastVersion)
            {
                MessageBox.Show("No update Found!");
            }
            else
            {
                MessageBox.Show("Update Found , Starting Download...");
            }
        }
        private void AboutUsClick(object sender, RoutedEventArgs e)
        {
            string AboutUs = "App Was Created by Mohamad thaine to practice XAML and C# \n \n \n Copyrights©MohamadThaine 2022";
            MessageBox.Show(AboutUs);
        }
    }
}
