using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Printing;
using System.Drawing.Printing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for imprimir_balance.xaml
    /// </summary>
    public partial class imprimir_balance : Page
    {
        #region clases_especiales
        private PrintDocument printDoc = new PrintDocument();
        #endregion

        #region datos_iniciales
        public imprimir_balance()
        {
            InitializeComponent();

            //agregar la lista de impresoras instaladas
            string impresora_instalada;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                impresora_instalada = PrinterSettings.InstalledPrinters[i];
                impresora.Items.Add(impresora_instalada);
                if (printDoc.PrinterSettings.IsDefaultPrinter)
                {
                    impresora.SelectedItem = printDoc.PrinterSettings.PrinterName;
                }
            }

            //agregar orientaciones de paginas
            orientacion_impresion.Items.Add("Horizontal");
            orientacion_impresion.Items.Add("Vertical");

            //agregar tamaños de paginas
            tamano_impresion.Items.Add("Tabloide");
            tamano_impresion.Items.Add("Carta");
            tamano_impresion.Items.Add("Oficio");

            //establecer orientacion predeterminada
            orientacion_impresion.SelectedItem = "Horizontal";

            //establecer tamaño predeterminado
            tamano_impresion.SelectedItem = "Tabloide";

            //numero de copias
            copias.Text = "1";

            cargar_datos_generales();
        }
        #endregion

        #region tamanos_de_letra_/_tipo_de_texto

        private void letra_pequena(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.9 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_pequena_2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_pequena_3(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.7 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_list_box(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.2 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion

        #region control_general_Del_programa

        private void salir_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        #endregion

        #region formulario_impresion
        private void confirmar_impresion_Click(object sender, RoutedEventArgs e)
        {
            //varibales para hacer el switch de los tamaños de pagina y orientaciones de pagina
            string tamano_pagina = tamano_impresion.SelectedItem.ToString();
            string orientacion_pagina = tamano_impresion.SelectedItem.ToString();
            PageMediaSize tamano;
            PageOrientation orientacion;


            //configurar el valor del tamño de pagina
            switch (tamano_pagina)
            {
                case "Carta":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaLetter);
                    break;
                case "Oficio":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaLegal);
                    break;
                case "Tabloide":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaTabloid);
                    break;
                default:
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaTabloid);
                    break;
            };

            //configurar el valor de orientacion de pagina
            switch (orientacion_pagina)
            {
                case "Horizontal":
                    orientacion = PageOrientation.Landscape;
                    break;
                case "Vertical":
                    orientacion = PageOrientation.Portrait;
                    break;
                default:
                    orientacion = PageOrientation.Landscape;
                    break;
            };

            //establecer las propiedades de impresion

            try
            {
                PrintDialog dialog = new PrintDialog();
                dialog.PrintTicket.PageOrientation = orientacion;
                dialog.PrintTicket.PageBorderless = PageBorderless.None;
                dialog.PrintTicket.PageMediaSize = tamano;
                dialog.PrintTicket.CopyCount = Convert.ToInt32(copias.Text);
                dialog.PrintQueue = new PrintQueue(new PrintServer(), impresora.SelectedItem.ToString());
                dialog.PrintVisual(area_de_impresion, "LayOut");
            }
            catch
            {
                MessageBox.Show("No se reconoce la impresora o el número de copias es invalido");
            }
        }

        private void aumentar_copias_Click(object sender, RoutedEventArgs e)
        {
            copias.Text = (Convert.ToInt32(copias.Text) + 1).ToString();
        }

        #endregion

        private void cargar_datos_generales()
        {
            modulo.Content = impresion_global.modulo;
            impresion.Content = DateTime.Now.ToString("yyyy-MM-dd");
            creacion.Content = impresion_global.fecha;
            
        }
    }
}
