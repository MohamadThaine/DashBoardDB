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
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace DashBoardDB
{
    /// <summary>
    /// Interaction logic for EmailReportWindow.xaml
    /// </summary>
    public partial class EmailReportWindow : Window
    {
        public EmailReportWindow()
        {
            InitializeComponent();
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
            string To = EmailTextBox.Text;
            string From = "hamod.khled12345@gmail.com";
            string subject = "DashBoard Email!";
            string body = "Here is the data you asked For!";
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
                Body = body
            })
                try
                {
                    client.Send(mailMessage);
                    MessageBox.Show("Email Has been sent!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erros happend and email has not been sent \n Error Code is: " + ex.HResult);
                }
                
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
