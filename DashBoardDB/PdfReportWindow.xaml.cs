using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DashBoardDB
{
    /// <summary>
    /// Interaction logic for PdfReportWindow.xaml
    /// </summary>
    public partial class PdfReportWindow : Window
    {
        ManageDB manageDB;
        int Date;
        public PdfReportWindow()
        {
            InitializeComponent();
            manageDB = new();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            manageDB.CloseConnetion();
            Close();
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
        private bool DataChecked()
        {
            if (ProfitData.IsChecked == true || SalesData.IsChecked == true || ProductTypeData.IsChecked == true)
                return true;
            return false;
        }
        private bool GeneratePDF(String Location, int Date)
        {
            try
            {
                List<Double> ProfitData = new List<Double>();
                List<int> SalesData = new List<int>();
                List<String> NameOfData = new List<string>();
                List<DateTime> DateOfData = new List<DateTime>();
                FileStream fileStream = new FileStream(Location, FileMode.Create);
                Document Pdf = new Document(PageSize.A4, 10, 10, 10, 10);
                if (A4.IsChecked == true)
                {
                    Pdf.SetPageSize(PageSize.A4);
                }
                else if (A5.IsChecked == true)
                {
                    Pdf.SetPageSize(PageSize.A5);
                }
                else
                    Pdf.SetPageSize(PageSize.A6);
                PdfWriter pdfWriter = PdfWriter.GetInstance(Pdf, fileStream);
                Pdf.Open();
                var FontColor = new BaseColor(94, 156, 160);
                var Font = FontFactory.GetFont("fonts/#Dosis", 28, FontColor);
                Paragraph DataReportPara = new Paragraph("Here is your data report:\n\n\n", Font);
                DataReportPara.Alignment = Element.ALIGN_CENTER;
                Pdf.Add(DataReportPara);
                if (ProductTypeData.IsChecked == true)
                {
                    Paragraph TypeProfitPara = new Paragraph("Type Profit Table:\n\n");
                    TypeProfitPara.Alignment = Element.ALIGN_CENTER;
                    Pdf.Add(TypeProfitPara);
                    PdfPTable TypeProfitTable = new PdfPTable(2);
                    TypeProfitTable.AddCell("Type Name");
                    TypeProfitTable.AddCell("Type Profit");
                    manageDB.GetEachTypeProfitWithDate(NameOfData, ProfitData, Date);
                    for (int i = 0; i < ProfitData.Count; i++)
                    {
                        TypeProfitTable.AddCell(NameOfData[i]);
                        TypeProfitTable.AddCell(ProfitData[i].ToString());
                    }
                    Pdf.Add(TypeProfitTable);
                }
                if (this.SalesData.IsChecked == true)
                {
                    NameOfData.Clear();
                    Paragraph SalesDataPara = new Paragraph("Product Sold Table:\n\n");
                    SalesDataPara.Alignment = Element.ALIGN_CENTER;
                    Pdf.Add(SalesDataPara);
                    PdfPTable SalesDataTable = new PdfPTable(2);
                    SalesDataTable.AddCell("Product Name");
                    SalesDataTable.AddCell("Quantity Sold");
                    manageDB.GetProductsSales(NameOfData, SalesData, 0, Date);
                    for (int i = 0; i < SalesData.Count; i++)
                    {
                        SalesDataTable.AddCell(NameOfData[i]);
                        SalesDataTable.AddCell(SalesData[i].ToString());
                    }
                    Pdf.Add(SalesDataTable);
                }
                if (this.ProfitData.IsChecked == true)
                {
                    ProfitData.Clear();
                    Paragraph ProfitEachDay = new Paragraph("Profit By Day Table:\n\n");
                    ProfitEachDay.Alignment = Element.ALIGN_CENTER;
                    Pdf.Add(ProfitEachDay);
                    PdfPTable ProfitEachDayTable = new PdfPTable(2);
                    ProfitEachDayTable.AddCell("Date");
                    ProfitEachDayTable.AddCell("Profit");
                    manageDB.GetProfit(DateOfData, ProfitData, Date);
                    for (int i = 0; i < DateOfData.Count; i++)
                    {
                        ProfitEachDayTable.AddCell(DateOfData[i].ToString("yyyy/MM/dd"));
                        ProfitEachDayTable.AddCell(ProfitData[i].ToString());
                    }
                    Pdf.Add(ProfitEachDayTable);
                }
                Pdf.Close();
                pdfWriter.Close();
                fileStream.Close();
                return true;
            }
            catch (PdfException PDFex)
            {
                return false;
            }
        }
        private void CreatePDF_Click(object sender, RoutedEventArgs e)
        {
            if (!DataChecked())
            {
                MessageBox.Show("Please Check the data you want to create pdf report with");
                return;
            }
            SaveFileDialog savePDF = new SaveFileDialog();
            savePDF.Filter = "PDF file (*.pdf)|*.pdf";
            if (savePDF.ShowDialog() == true)
            {
                String Location = savePDF.FileName;
                String Extention = Location.Substring(Location.Length - 4);
                if (!string.Equals(Extention, ".pdf", StringComparison.OrdinalIgnoreCase))
                    Location += ".pdf";
                Date = DataAskedDate();
                bool PdfCreated = GeneratePDF(Location, Date);
                if (PdfCreated)
                {
                    MessageBoxResult ButtonClicked = MessageBox.Show("Pdf has been created , do you want to open it?",
                       "report creating has been complated", MessageBoxButton.YesNo);
                    if (ButtonClicked == MessageBoxResult.Yes)
                    {
                        Process PdfOpener = new Process();
                        PdfOpener.StartInfo = new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = Location,
                        };
                        PdfOpener.Start();
                    }
                }
                else
                    MessageBox.Show("There was an error while creating pdf file(hint:maybe user permission " +
                        "dont allow creating files in this location!");

            }
        }
    }
}
