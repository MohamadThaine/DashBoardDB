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

namespace DashBoardDB
{
    [ObservableObject]
    public partial class MainWindow : Window
    {
        double appVersion = 1.0;
        public MainWindow()
        {
            
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
            //label1.Content = DateTime.Today.ToString("M/d/yyyy");
            //DataContext = new Viewmodelx();
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
