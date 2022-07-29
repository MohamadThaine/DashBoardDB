using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;

namespace DashBoardDB
{
    /// <summary>
    /// Interaction logic for EmailReportWindow.xaml
    /// </summary>
    public partial class EmailReportWindow : Window
    {
        int Date = 0;
        ManageDB manageDB;
        public EmailReportWindow()
        {
            InitializeComponent();
            manageDB = new();
        }
        private bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        private bool DataChecked()
        {
            if (ProfitData.IsChecked == true || SalesData.IsChecked == true || ProductTypeData.IsChecked == true)
                return true;
            return false;
        }
        private int DataAskedDate()
        {
            int Date = 0;
            if (GetDataFromDate.SelectedIndex == 0)
                Date = 1;
            else if (GetDataFromDate.SelectedIndex == 1)
                Date = 7;
            else if (GetDataFromDate.SelectedIndex == 2)
                Date = 30;
            else if (GetDataFromDate.SelectedIndex == 3)
                Date = 365;
            return Date;
        }
        private void SendEmailClick(object sender, RoutedEventArgs e)
        {
            if (!DataChecked())
            {
                MessageBox.Show("Please Check the data you want to recive email report with");
                return;
            }
            if (!IsValidEmailAddress(EmailTextBox.Text))
            {
                MessageBox.Show("Email is not a valid email address");
                return;
            }
            Date = DataAskedDate();
            string To = EmailTextBox.Text;
            string From = "hamod.khled12345@gmail.com";
            string subject = "DashBoard Email!";
            string body = Create_Body();
            var client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(From, "zvttwdcdafswugfs")//zvttwdcdafswugfs
            };
            using (var mailMessage = new MailMessage(From, To)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    client.Send(mailMessage);
                    MessageBox.Show("Email Has been sent!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erros happend and email has not been sent \n Error Code is: " + ex.HResult);
                }
        }
        private String Create_Body()
        {
            List<Double> ProfitData = new List<Double>();
            List<int> SalesData = new List<int>();
            List<String> NameOfData = new List<string>();
            List<DateTime> DateOfData = new List<DateTime>();
            String Body = "<h1 style=\"color: #5e9ca0; text-align: center;\">Here is your data report:</h1> ";
            if (ProductTypeData.IsChecked == true)
            {
                Body += "<br> <p style=\"text-align: center;\"><span style=\"font-size:x-large; \">Type Sales:</span></p> <br> <table style=\"margin-left:auto; margin-right:auto;\" border =\"4\";\"> <tbody>" +
                    " <tr>  <td> Type Name </td> <td> Type Profit </td>  </tr>";
                manageDB.GetEachTypeProfitWithDate(NameOfData, ProfitData, Date);
                for (int i = 0; i < ProfitData.Count; i++)
                    Body += " <tr>  <td> " + NameOfData[i] + " </td> <td>" + ProfitData[i] + " </td>  </tr>";
                Body += "</tbody> </table>";
            }
            if (this.SalesData.IsChecked == true)
            {
                NameOfData.Clear();
                Body += "<br> <p style=\"text-align: center;\"><span style=\"font-size:x-large; \">Products Sales:</span></p> <br> <table style=\"margin-left:auto; margin-right:auto;\" border =\"4\";\"> <tbody>" +
                    " <tr>  <td> Product Name </td> <td> Quantity Sold </td>  </tr>";
                manageDB.GetProductsSales(NameOfData, SalesData, 0, Date);
                for (int i = 0; i < SalesData.Count; i++)
                    Body += " <tr>  <td> " + NameOfData[i] + " </td> <td>" + SalesData[i] + " </td>  </tr>";
                Body += "</tbody> </table>";
            }
            if (this.ProfitData.IsChecked == true)
            {
                ProfitData.Clear();
                Body += "<br> <p style=\"text-align: center;\"><span style=\"font-size:x-large; \">Profit Each Day:</span></p> <br> <table style=\"margin-left:auto; margin-right:auto;\" border =\"4\";\"> <tbody>" +
                    " <tr>  <td> Date </td> <td> Profit </td>  </tr>";
                manageDB.GetProfit(DateOfData, ProfitData, Date);
                for (int i = 0; i < DateOfData.Count; i++)
                    Body += " <tr>  <td> " + DateOfData[i].ToString("yyyy/MM/dd") + " </td> <td>" + ProfitData[i] + " </td>  </tr>";
                Body += "</tbody> </table>";
            }
            return Body;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            manageDB.CloseConnetion();
            Close();
        }
    }
}
