using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DashBoardDB
{
    public partial class AddRemoveEdIt : Window
    {
        string[] AddType = new string[] { "Choose what you like to add", "Product", "Company", "Missing Order" };
        string[] RemoveType = new string[] { "Choose what you like to remove", "Product", "Company", "Order" };
        Boolean addflag = false, Checker; //Checker used to check if the item has been added/removed or not
        Visibility Show = Visibility.Visible;
        Visibility Hide = Visibility.Hidden;
        bool emailvalition;
        ManageDB managaDB = new();
        List<String> TypesList = new List<String>();
        List<String> CompaniesList = new List<String>();
        List<String> MissingOrderNames = new List<String>();
        List<int> MissingOrderQuanties = new List<int>();
        List<Double> MissingOrderPfPriceForEachProduct = new List<Double>();
        String MissingOrderProductName = "";
        Double MissingOrderTotalPrice = 0;
        private readonly MainWindow LiveUpdate;
        TextBox[] TextBoxes;
        public AddRemoveEdIt(MainWindow LiveUpdates)
        {
            LiveUpdate = LiveUpdates;
            InitializeComponent();
            TextBoxes = new TextBox[7] { productnamebox, productojbricebox, productpfbricebox, quantitybox, OrderProductNameBox, OrderProductQuantityBox, PhoneNumbox };
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            managaDB.CloseConnetion();
            Close();
        }
        private void ClearTextBoxes()
        {
            foreach (TextBox Text in TextBoxes)
            {
                Text.Clear();
            }
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
            productnameblock.Visibility = Hide;
            productnamebox.Visibility = Hide;
            ProductTypeBlock.Visibility = Hide;
            TypeBox.Visibility = Hide;
            productogpriceblock.Visibility = Hide;
            productojbricebox.Visibility = Hide;
            productpfpriceblock.Visibility = Hide;
            productpfbricebox.Visibility = Hide;
            quantityblock.Visibility = Hide;
            quantitybox.Visibility = Hide;
            companyblock.Visibility = Hide;
            CompanyBox.Visibility = Hide;
            expdateblock.Visibility = Hide;
            expdatepciker.Visibility = Hide;
            OrderProductNameBlock.Visibility = Hide;
            OrderProductNameBox.Visibility = Hide;
            OrderProductQuantityBlock.Visibility = Hide;
            OrderProductQuantityBox.Visibility = Hide;
            ScrollBar.Visibility = Hide;
            AddProductToOrderBlock.Visibility = Hide;
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
                quantityblock.Text = "Quantity:";
                companyblock.Visibility = Show;
                CompanyBox.Visibility = Show;
                expdateblock.Visibility = Show;
                expdateblock.Text = "Exp date:";
                expdatepciker.Visibility = Show;
                expdatepciker.Visibility = Show;
                PhoneNumber.Visibility = Hide;
                PhoneNumbox.Visibility = Hide;
                Confirm.Visibility = Show;
            }
            else
            {
                productnameblock.Visibility = Hide;
                productnamebox.Visibility = Hide;
                ProductTypeBlock.Visibility = Hide;
                TypeBox.Visibility = Hide;
                productogpriceblock.Visibility = Hide;
                productojbricebox.Visibility = Hide;
                productpfpriceblock.Visibility = Hide;
                productpfbricebox.Visibility = Hide;
                quantityblock.Visibility = Hide;
                quantitybox.Visibility = Hide;
                companyblock.Visibility = Hide;
                CompanyBox.Visibility = Hide;
                expdateblock.Visibility = Hide;
                expdatepciker.Visibility = Hide;
            }

        }
        private void PrepareCompany()
        {
            if (addflag != true)
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
                productnameblock.Visibility = Hide;
                productnamebox.Visibility = Hide;
                productpfpriceblock.Visibility = Hide;
                productpfbricebox.Visibility = Hide;
                PhoneNumber.Visibility = Hide;
                PhoneNumbox.Visibility = Hide;
                Confirm.Visibility = Hide;
            }
        }
        private void PrepareMissingOrder()
        {
            if (addflag != true)
                return;
            if (EditType.SelectedIndex == 3)
            {
                MissingOrderGrid.IsEnabled = true;
                OrderProductNameBlock.IsEnabled = true;
                OrderProductNameBox.IsEnabled = true;
                OrderProductQuantityBlock.IsEnabled = true;
                OrderProductQuantityBox.IsEnabled = true;
                OrderProductNameBlock.Visibility = Show;
                OrderProductNameBox.Visibility = Show;
                OrderProductQuantityBlock.Visibility = Show;
                OrderProductQuantityBox.Visibility = Show;
                ScrollBar.Visibility = Show;
                ScrollBar.IsEnabled = true;
                AddProductToOrderBlock.Visibility = Show;
                ProductNameAndQuantityBlock.Visibility = Show;
                quantityblock.Text = "Price:";
                quantityblock.Visibility = Show;
                quantitybox.Visibility = Show;
                quantitybox.Text = "";
                quantitybox.IsReadOnly = true;
                expdateblock.Text = "Date:";
                expdateblock.Visibility = Show;
                expdatepciker.Visibility = Show;
                Confirm.Visibility = Show;
            }
            else
            {
                MissingOrderGrid.IsEnabled = false;
                OrderProductNameBlock.IsEnabled = false;
                OrderProductNameBox.IsEnabled = false;
                OrderProductQuantityBlock.IsEnabled = false;
                OrderProductQuantityBox.IsEnabled = false;
                OrderProductNameBlock.Visibility = Hide;
                OrderProductNameBox.Visibility = Hide;
                OrderProductQuantityBlock.Visibility = Hide;
                OrderProductQuantityBox.Visibility = Hide;
                AddProductToOrderBlock.Visibility = Hide;
                ProductNameAndQuantityBlock.Visibility = Hide;
                ScrollBar.Visibility = Hide;
                ScrollBar.IsEnabled = false;
                quantitybox.IsReadOnly = false;
                expdateblock.Visibility = Hide;
                expdatepciker.Visibility = Hide;
            }
        }
        private void PrepareRemoveProdect()
        {
            if (addflag != false)
                return;
            if (EditType.SelectedIndex == 1)
            {
                hideboxes();
                productpfpriceblock.Text = "        Name:";
                productpfpriceblock.Margin = new Thickness(2, 4, 318, 18);
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
                productpfpriceblock.Margin = new Thickness(2, 4, 318, 18);
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
            ClearTextBoxes();
            if (addflag == true)
            {
                if (EditType.SelectedIndex == 1)
                {
                    PrepareMissingOrder();
                    PrepareCompany();
                    PrepareAddProduct();

                }
                else if (EditType.SelectedIndex == 2)
                {
                    PrepareMissingOrder();
                    PrepareAddProduct();
                    PrepareCompany();
                }
                else if (EditType.SelectedIndex == 3)
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
                if (EditType.SelectedIndex == 1)
                {
                    PrepareRemoveCompany();
                    PrepareRemoveOrder();
                    PrepareRemoveProdect();
                }
                else if (EditType.SelectedIndex == 2)
                {
                    PrepareRemoveProdect();
                    PrepareRemoveOrder();
                    PrepareRemoveCompany();
                }
                else if (EditType.SelectedIndex == 3)
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
            if (addflag == true && EditType.SelectedIndex == 1)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            else if (addflag == false && EditType.SelectedIndex == 3)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }
        private void missingorderquantitybox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
        }
        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                if (OrderProductNameBox.Text == "")
                {
                    MessageBox.Show("Please Enter The Product Name!");
                    return;
                }
                int MissingOrderProductQuantity = 1;
                Double MissingOrderTotalPriceForLastProduct = 0;
                if (OrderProductNameBox.Text.Length > 5)
                {
                    MissingOrderProductName = OrderProductNameBox.Text.Substring(0, 5);
                    MissingOrderProductName += "..";
                }
                else
                    MissingOrderProductName = OrderProductNameBox.Text;
                if (OrderProductQuantityBox.Text != "")
                {
                    try
                    {
                        MissingOrderProductQuantity = Convert.ToInt32(OrderProductQuantityBox.Text);
                    }
                    catch (OverflowException TooLargeNumber)
                    {
                        MessageBox.Show("Qunatity number is too large! Where would you fit all of that\n Error code is " + TooLargeNumber.HResult);
                        return;
                    }
                }
                MissingOrderTotalPriceForLastProduct = managaDB.GetProductPriceWithQuantity(OrderProductNameBox.Text, MissingOrderProductQuantity);
                if (MissingOrderTotalPriceForLastProduct == 0)
                {
                    MessageBox.Show("Product Deos Not exist in DB");
                    return;
                }
                MissingOrderNames.Add(OrderProductNameBox.Text);
                MissingOrderQuanties.Add(MissingOrderProductQuantity);
                MissingOrderPfPriceForEachProduct.Add(MissingOrderTotalPriceForLastProduct / MissingOrderProductQuantity);
                ProductNameAndQuantityBlock.Text += Environment.NewLine + "      " + MissingOrderProductName + "                                                  " + MissingOrderProductQuantity.ToString();
                OrderProductNameBox.Text = "";
                OrderProductQuantityBox.Text = "";
                MissingOrderTotalPrice += MissingOrderTotalPriceForLastProduct;
                quantitybox.Text = MissingOrderTotalPrice.ToString();
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (addflag == true)
            {
                if (EditType.SelectedIndex == 1)
                {
                    try
                    {
                        Convert.ToInt32(quantitybox.Text);
                        Convert.ToDouble(productojbricebox.Text);
                        Convert.ToDouble(productpfbricebox.Text);
                    }
                    catch (OverflowException TooLargeNumber)
                    {
                        MessageBox.Show("One of the numbers is too large!\n Error code is " + TooLargeNumber.HResult);
                        return;
                    }
                    if (expdatepciker.SelectedDate.Value == null || expdatepciker.SelectedDate.Value < DateTime.Today)
                    {
                        MessageBox.Show("Please pick a valid exp date!");
                        return;
                    }
                    if (productnamebox.Text == "" || productpfbricebox.Text == "" || TypeBox.SelectedItem.ToString() == "" ||
                        CompanyBox.SelectedItem == null || quantitybox.Text == "" || productojbricebox.Text == "")
                    {
                        MessageBox.Show("Dont leave any TextBox empty!");
                        return;
                    }
                    Checker = managaDB.insertProduct(productnamebox.Text, TypeBox.SelectedItem.ToString(), CompanyBox.SelectedItem.ToString(),
                        Convert.ToInt32(quantitybox.Text), Convert.ToDouble(productojbricebox.Text), Convert.ToDouble(productpfbricebox.Text), expdatepciker.SelectedDate.Value);
                    if (Checker == true)
                    {
                        MessageBox.Show("Product has been added to the DB");
                        LiveUpdate.PrepareMainWindowData();
                    }
                    else
                        MessageBox.Show("There was an error adding the product to the DB");
                }
                else if (EditType.SelectedIndex == 2)
                {
                    if (productnamebox.Text == "" || productpfbricebox.Text == "" || PhoneNumbox.Text == "")
                    {
                        MessageBox.Show("Dont leave any TextBox empty!");
                        return;
                    }
                    Checker = managaDB.InsertCompany(productnamebox.Text, productpfbricebox.Text, Convert.ToDouble(PhoneNumbox.Text));
                    if (Checker == true)
                    {
                        MessageBox.Show("Company has been added to the DB");
                        LiveUpdate.PrepareMainWindowData();
                    }
                    else
                        MessageBox.Show("There was an error adding the company to the DB (hint:maybe the name is wrong)");
                }
                else if (EditType.SelectedIndex == 3)
                {
                    if (quantitybox.Text == "")//if quantityBox is empty then they didnt any product to the list yet so check it alone is enough
                    {
                        MessageBox.Show("Add product to the list first!");
                        return;
                    }
                    else if (expdatepciker.SelectedDate.Value == null)
                    {
                        MessageBox.Show("Pick a date First!");
                        return;
                    }
                    else if (expdatepciker.SelectedDate.Value.Date > DateTime.Today)
                    {
                        MessageBox.Show("Date cant be after now!");//bad wording but idk how to say it!
                        return;
                    }
                    Checker = managaDB.InsertOrder(MissingOrderNames, MissingOrderQuanties, MissingOrderPfPriceForEachProduct, MissingOrderTotalPrice, expdatepciker.SelectedDate.Value);
                    if (Checker == true)
                    {
                        MessageBox.Show("Order has been added to the DB");
                        MissingOrderNames.Clear();
                        MissingOrderQuanties.Clear();
                        MissingOrderPfPriceForEachProduct.Clear();
                        MissingOrderTotalPrice = 0;
                        quantitybox.Text = "";
                        ProductNameAndQuantityBlock.Text = "     Product:                                            Quantity:   ";
                        expdatepciker.SelectedDate = null;
                        LiveUpdate.PrepareMainWindowData();
                        LiveUpdate.PreparePieChartWithDate(this, e);
                    }

                    else
                        MessageBox.Show("There was an error while adding the order to the DB");
                }
            }
            else
            {
                if (EditType.SelectedIndex == 1)
                {
                    if (productpfbricebox.Text == "")
                    {
                        MessageBox.Show("Dont leave the TextBox empty!");
                        return;
                    }
                    Checker = managaDB.DeleteRecord("products", "ProductName", productpfbricebox.Text);
                    if (Checker == true)
                    {
                        MessageBox.Show("Product has been removed from the DB");
                        LiveUpdate.PrepareMainWindowData();
                    }
                    else
                        MessageBox.Show("There was an error removeing the product from the DB");
                }
                else if (EditType.SelectedIndex == 2)
                {
                    if (productpfbricebox.Text == "")
                    {
                        MessageBox.Show("Dont leave the TextBox empty!");
                        return;
                    }
                    Checker = managaDB.DeleteRecord("companies", "CompanyName", productpfbricebox.Text);
                    if (Checker == true)
                    {
                        MessageBox.Show("Company has been removed from the DB");
                        LiveUpdate.PrepareMainWindowData();
                    }
                    else
                        MessageBox.Show("There was an error removeing the company from the DB (hint:maybe the name is wrong)");
                }
                else if (EditType.SelectedIndex == 3)
                {
                    if (productpfbricebox.Text == "")
                    {
                        MessageBox.Show("Dont leave the TextBox empty!");
                        return;
                    }
                    Checker = managaDB.DeleteRecord("orders", "idOrders", productpfbricebox.Text);
                    if (Checker == true)
                    {
                        MessageBox.Show("The order has been removed from the DB");
                        LiveUpdate.PrepareMainWindowData();
                        LiveUpdate.PreparePieChartWithDate(this, e);
                    }
                    else
                        MessageBox.Show("There was an error removeing the order from the DB (hint:maybe the id is wrong)");
                }
            }
        }
    }
}
