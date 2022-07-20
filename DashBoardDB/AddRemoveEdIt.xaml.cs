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
using System.Text.RegularExpressions;

namespace DashBoardDB
{
    /// <summary>
    /// Interaction logic for AddRemoveEdIt.xaml
    /// </summary>
    public partial class AddRemoveEdIt : Window
    {
        string[] AddType = new string[] { "Choose what you like to add", "Product", "Company", "Missing Order" };
        string[] RemoveType = new string[] { "Choose what you like to remove", "Product", "Company", "Order" };
        Boolean addflag = false , AddChecker; //Add Checked used to check if the item has been added or not
         Visibility Show = Visibility.Visible;
        Visibility hide = Visibility.Hidden;
        bool emailvalition;
        ManageDB managaDB = new();
        List<String> TypesList = new List<String>();
        List<String> CompaniesList = new List<String>();
        public AddRemoveEdIt()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            addflag = true;
            EditType.Visibility = Visibility.Visible;
            Confirm.Visibility = Visibility.Hidden;
            if (EditType.Items.Count != 0)
                EditType.Items.Clear();
            foreach (string item in AddType)
                EditType.Items.Add(item);
            EditType.SelectedIndex = EditType.Items.IndexOf(AddType[0]);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            addflag = false;
            EditType.Visibility = Visibility.Visible;
            Confirm.Visibility = Visibility.Hidden;
            if (EditType.Items.Count != 0)
                EditType.Items.Clear();
            foreach (string item in RemoveType)
                EditType.Items.Add(item);
            EditType.SelectedIndex = EditType.Items.IndexOf(RemoveType[0]);
            PrepareAddProduct();
            PrepareCompany();
            PrepareMissingOrder();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hidden();
            TypesList = managaDB.GetAllTypes();
            CompaniesList = managaDB.GetAllCompanies();
            foreach (String TypeName in TypesList)
                TypeBox.Items.Add(TypeName);
            foreach (String CompanyName in CompaniesList)
                CompanyBox.Items.Add(CompanyName);
        }
        private void hidden()
        {
            EditType.Visibility = Visibility.Hidden;
            Confirm.Visibility = Visibility.Hidden;
            hideboxes();
        }
        private void hideboxes()
        {
            productnameblock.Visibility = hide;
            productnamebox.Visibility = hide;
            ProductTypeBlock.Visibility = hide;
            TypeBox.Visibility = hide;
            productogpriceblock.Visibility = hide;
            productojbricebox.Visibility = hide;
            productpfpriceblock.Visibility = hide;
            productpfbricebox.Visibility = hide;
            quantityblock.Visibility = hide;
            quantitybox.Visibility = hide;
            companyblock.Visibility = hide;
            CompanyBox.Visibility = hide;
            expdateblock.Visibility = hide;
            expdatepciker.Visibility = hide;
        }
        private void PrepareAddProduct()
        {
            if (addflag != true)
            {
                return;
            }
            if (EditType.SelectedIndex == 1)
            {
                productnameblock.Visibility = Show;
                productnamebox.Visibility = Show;
                productnameblock.Text = "Name:";
                productnameblock.Margin = new Thickness(26, 2, 290, 20);
                ProductTypeBlock.Visibility = Show;
                TypeBox.Visibility = Show;
                productogpriceblock.Visibility = Show;
                productojbricebox.Visibility = Show;
                productpfpriceblock.Visibility = Show;
                productpfpriceblock.Text = "Profit Price:";
                productpfpriceblock.Margin = new Thickness(2, 4, 318, 18);
                productpfbricebox.Visibility = Show;
                quantityblock.Visibility = Show;
                quantitybox.Visibility = Show;
                companyblock.Visibility = Show;
                CompanyBox.Visibility = Show;
                expdateblock.Visibility = Show;
                expdatepciker.Visibility = Show;
                expdatepciker.Visibility = Show;
                PhoneNumber.Visibility = hide;
                PhoneNumbox.Visibility = hide;
                Confirm.Visibility = Show;
            }
            else
            {
                productnameblock.Visibility = hide;
                productnamebox.Visibility = hide;
                ProductTypeBlock.Visibility = hide;
                TypeBox.Visibility = hide;
                productogpriceblock.Visibility = hide;
                productojbricebox.Visibility = hide;
                productpfpriceblock.Visibility = hide;
                productpfbricebox.Visibility = hide;
                quantityblock.Visibility = hide;
                quantitybox.Visibility = hide;
                companyblock.Visibility = hide;
                CompanyBox.Visibility = hide;
                expdateblock.Visibility = hide;
                expdatepciker.Visibility = hide;
            }
            
        }
        private void PrepareCompany()
        {
            if(addflag != true)
                return;
            if (EditType.SelectedIndex == 2)
            {
                productnameblock.Visibility = Show;
                productnamebox.Visibility = Show;
                productpfpriceblock.Text = "Email:";
                productpfpriceblock.Margin = new Thickness(32, 4, 288, 18);
                productpfpriceblock.Visibility = Show;
                productpfbricebox.Visibility = Show;
                PhoneNumber.Visibility = Show;
                PhoneNumbox.Visibility = Show;
                Confirm.Visibility = Show;
            }
            else
            {
                productnameblock.Visibility = hide;
                productnamebox.Visibility = hide;
                productpfpriceblock.Text = "Email:";
                productpfpriceblock.Margin = new Thickness(32, 4, 288, 18);
                productpfpriceblock.Visibility = hide;
                productpfbricebox.Visibility = hide;
                PhoneNumber.Visibility = hide;
                PhoneNumbox.Visibility = hide;
                Confirm.Visibility = hide;
            }
        }
        private void PrepareMissingOrder()
        {
            if (addflag != true)
                return;
            if (EditType.SelectedIndex == 3)
            {
                productnameblock.Margin = new Thickness(15, 2, 337, 20);
                productnameblock.Text = "Products:";
                productnameblock.Visibility = Show;
                productnamebox.Visibility = Show;
                ScrollBar.Visibility = Show;
                ScrollBar.IsEnabled = true;
                AddProductToOrderBlock.Visibility = Show;
                quantityblock.Text = "Price";
                quantityblock.Visibility = Show;
                quantitybox.Visibility = Show;
                quantitybox.IsReadOnly = true;
                Confirm.Visibility = Show;
            }
            else
            {
                AddProductToOrderBlock.Visibility = hide;
                ScrollBar.Visibility = hide;
                ScrollBar.IsEnabled = false;
                quantitybox.IsReadOnly = false;

            }
        }
        private void PrepareRemoveProdect()
        {
            if (addflag != false)
                return;
            if(EditType.SelectedIndex == 1)
            {
                hideboxes();
                productpfpriceblock.Text = "        Name:";
                productpfpriceblock.Visibility = Show;
                productpfbricebox.Visibility = Show;
                Confirm.Visibility = Show;
            }
        }
        private void PrepareRemoveCompany()
        {
            if (addflag != false)
            {
                return;
            }
            if (EditType.SelectedIndex == 2)
            {
                hideboxes();
                productpfpriceblock.Text = "        Name:";
                productpfpriceblock.Visibility = Show;
                productpfbricebox.Visibility = Show;
                Confirm.Visibility = Show;
            }
        }
        private void PrepareRemoveOrder()
        {
            if (addflag != false)
            {
                return;
            }
            if (EditType.SelectedIndex == 3)
            {
                hideboxes();
                productpfpriceblock.Text = "    Order ID:";
                productpfpriceblock.Visibility = Show;
                productpfbricebox.Visibility = Show;
                Confirm.Visibility = Show;
            }
        }
        private void EditType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addflag == true)
            {
                if (EditType.SelectedIndex == 1)
                {
                    PrepareMissingOrder();
                    PrepareCompany();
                    PrepareAddProduct();

                }
                else if(EditType.SelectedIndex == 2)
                {
                    PrepareMissingOrder();
                    PrepareAddProduct();
                    PrepareCompany();
                }
                else if(EditType.SelectedIndex == 3)
                {
                    PrepareAddProduct();
                    PrepareCompany();
                    PrepareMissingOrder();
                }
                else
                    hideboxes();
            }
            else
            {
                if(EditType.SelectedIndex == 1)
                {
                    PrepareRemoveCompany();
                    PrepareRemoveOrder();
                    PrepareRemoveProdect();
                }
                else if(EditType.SelectedIndex == 2)
                {
                    PrepareRemoveProdect();
                    PrepareRemoveOrder();
                    PrepareRemoveCompany();
                }
                else if(EditType.SelectedIndex == 3)
                {
                    PrepareRemoveProdect();
                    PrepareRemoveCompany();
                    PrepareRemoveOrder();
                }
                else
                    hideboxes();
            }     
        }
        private void quantitybox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (productpfpriceblock.Text == "Profit Price:")
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
           
        }

        private void productpfbricebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (productpfpriceblock.Text == "Email:")
                emailvalition = IsValidEmailAddress(productojbricebox.Text);


        }
        public bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        private void PhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if(PhoneNumbox.Text.Length >= 10)
            {
                MessageBox.Show("Phone Number cant be more than 10 numbers!");
            }
        }
        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if(addflag == true)
            {
                if(EditType.SelectedIndex == 3)
                {
                    if (e.Key == Key.Return)
                    {
                        string ProductName = productnamebox.Text;
                        AddProductToOrderBlock.Text += Environment.NewLine + productnamebox.Text;
                    }
                }
            }   
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (addflag == true)
                if (EditType.SelectedIndex == 1)
                {
                    AddChecker = managaDB.insertProduct(productnamebox.Text, TypeBox.SelectedItem.ToString(), CompanyBox.SelectedItem.ToString(),
                        Convert.ToInt32(quantitybox.Text), Convert.ToDouble(productojbricebox.Text), Convert.ToDouble(productpfbricebox.Text) , expdatepciker.SelectedDate.Value);
                    if (AddChecker == true)
                        MessageBox.Show("Product has been added to the DB");
                    else
                        MessageBox.Show("There was an error adding the product to the DB");
                }
                    


        }
    }       
}
