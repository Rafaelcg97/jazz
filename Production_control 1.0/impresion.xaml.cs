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
using System.Printing;
using System.Drawing.Printing;

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for impresion.xaml
    /// </summary>
    public partial class impresion : Page
    {
        private PrintDocument printDoc = new PrintDocument();
        public impresion()
        {
            InitializeComponent();

            string pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                impresora.Items.Add(pkInstalledPrinters);
            }




        }

        private void confirmar_impresion_Click(object sender, RoutedEventArgs e)
        {
            
            PageMediaSize pageMediaSize = new PageMediaSize(PageMediaSizeName.NorthAmericaTabloid);
            PrintDialog dialog = new PrintDialog();
            dialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
            dialog.PrintTicket.PageBorderless = PageBorderless.None;
            dialog.PrintTicket.PageMediaSize = pageMediaSize;
            printDoc.DefaultPageSettings.PrinterSettings.PrinterName = impresora.SelectedItem.ToString();
            dialog.PrintVisual(area_de_impresion, "LayOut");

            // Imprimir la pantalla


            //if (dialog.ShowDialog() == true)
            //{
            //dialog.PrintVisual(this, "Impresión");
            //}


        }


    }
}
