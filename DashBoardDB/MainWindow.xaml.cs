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
        double appVersion = 1.0;
        MySqlConnection connection = null;
        Double[] ArrayCounter = new Double[3];
        public MainWindow()
        {
            ManageDB DBmanager = new();
            connection = DBmanager.ConnectionToDB();
            connection.Open();
            ArrayCounter = GetProductNumber();
        }
        private Double[] GetProductNumber()
        {
            Double[] CountersArray = new Double[3];
            MySqlCommand cmd;
            String GetCounterOfProducts = "SELECT COUNT(*) FROM products";
            String GetCounterOfCompanies = "SELECT COUNT(*) FROM companies";
            String GetCounterOf7DaysProfit = "SELECT SUM(ProfitFromOrder) FROM orders WHERE OrderDate > now() - interval 7 day";
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
                var NoProdfitChecker = cmd.ExecuteScalar();
                if(NoProdfitChecker is not DBNull)
                {
                    CountersArray[2] = Convert.ToDouble(NoProdfitChecker);
                }
            }
            return CountersArray;
        }
        private void PrepareCountersText()
        {
            TotalProducts.Text = ArrayCounter[0].ToString();
            TotalCompanies.Text = ArrayCounter[1].ToString();
            TotalProfits.Text = ArrayCounter[2].ToString();
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
            MessageBoxResult confirm = System.Windows.MessageBox.Show("Are you sure?", "Exit Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareCountersText();
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
            if(appVersion == 1.0)//Get Last Update Version from database
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
