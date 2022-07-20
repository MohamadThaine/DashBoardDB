using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace DashBoardDB
{
    /// <summary>
    /// Interaction logic for PdfReportWindow.xaml
    /// </summary>
    public partial class PdfReportWindow : Window
    {
        public PdfReportWindow()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private bool DataChecked()
        {
            if (ProfitData.IsChecked == true || SalesData.IsChecked == true || ProductTypeData.IsChecked == true)
                return true;
            return false;
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
                Chunk chunk = new Chunk("This is from chunk. ");
                Pdf.Add(chunk);
                Phrase phrase = new Phrase("This is from Phrase.");
                Pdf.Add(phrase);
                iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph("This is from paragraph.");
                Pdf.Add(para);
                Pdf.Close();
                pdfWriter.Close();
                fileStream.Close();
            }
        }
    }
}
