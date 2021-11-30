using System;
using System.Windows;
using System.Windows.Controls;
using System.Printing;
using System.Drawing.Printing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Configuration;

namespace JazzCCO._0
{
    public partial class imprimir_balance : Page
    {
        #region strings
        private PrintDocument printDoc = new PrintDocument();
        Double tkt_ = 0;
        #endregion
        #region datos_iniciales
        public imprimir_balance(string tipoBalance, clases.balance datosBalanceRecibidos, FrameworkElement visual)
        {
            InitializeComponent();
            RenderTargetBitmap bitmap = new RenderTargetBitmap(((int)visual.ActualWidth), ((int)visual.ActualHeight), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            imagen.Source = bitmap;
            tkt_ = Math.Round(3600 / (Convert.ToDouble(datosBalanceRecibidos.corrida) / Convert.ToDouble(datosBalanceRecibidos.horas)), 2);
            int categoriaDeSam = 0;
            double eficienciaDouble = 0;
            if (datosBalanceRecibidos.eficiencia.Length >= 2)
            {
                eficienciaDouble = Math.Round(Convert.ToDouble(datosBalanceRecibidos.eficiencia.Substring(0, datosBalanceRecibidos.eficiencia.Length - 1)) / 100, 2);
            }
            #region formularioImprimir
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
            #endregion
            #region establecerCategoriaSam
            if (datosBalanceRecibidos.sam <= 10)
            {
                categoriaDeSam = 1;
            }
            else if(datosBalanceRecibidos.sam >10 && datosBalanceRecibidos.sam <= 13.5)
            {
                categoriaDeSam = 2;
            }
            else if (datosBalanceRecibidos.sam > 13.5 && datosBalanceRecibidos.sam <= 16.5)
            {
                categoriaDeSam = 3;
            }
            else if (datosBalanceRecibidos.sam > 16.5 && datosBalanceRecibidos.sam <=20)
            {
                categoriaDeSam = 4;
            }
            else if (datosBalanceRecibidos.sam > 20 && datosBalanceRecibidos.sam <=25)
            {
                categoriaDeSam = 5;
            }
            else if (datosBalanceRecibidos.sam > 25)
            {
                categoriaDeSam = 6;
            }
            #endregion
            #region consultarBonos
            SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql; //Consulta que se hace en sql
            SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
            SqlDataReader dr; //leer los resultados del comando sql
            sql = "select turno, bono  from bono_t where eficiencia='" +eficienciaDouble + "' and categoria='" + categoriaDeSam +"' order by turno";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            string cadenaBono = "";
            while (dr.Read())
            {
                cadenaBono = cadenaBono+ "$ "+ dr["bono"].ToString() + " - " ;
            };
            dr.Close();
            cnProduccion.Close();
            bonoPorTurno.Content = cadenaBono;
            #endregion
            #region datosEncabezado
            creacion.Content = datosBalanceRecibidos.fechaCreacion.ToString("yyyy-MM-dd");
            impresion.Content = DateTime.Now.ToString("yyyy-MM-dd");
            tipo.Content = tipoBalance;
            estilo.Content = datosBalanceRecibidos.nombre;
            sam.Content = datosBalanceRecibidos.sam;
            operarios.Content = datosBalanceRecibidos.operarios;
            eficiencia.Content = datosBalanceRecibidos.eficiencia;
            modulo.Content = datosBalanceRecibidos.modulo;
            tkt.Content =tkt_;
            piezasPorHora.Content =Math.Round((Convert.ToDouble(datosBalanceRecibidos.corrida) / Convert.ToDouble(datosBalanceRecibidos.horas)),0);
            #endregion
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

            if (checkBoxImpresora.IsChecked == true)
            {
                try
                {
                    Print(area_de_impresion);
                }
                catch
                {
                    MessageBox.Show("No se reconoce la impresora o el número de copias es invalido");
                }
            }
            else if (checkBoxImpresora.IsChecked == false)
            {
                try
                {
                    Print(area_de_impresion,1);
                }
                catch
                {
                    MessageBox.Show("No se reconoce la impresora o el número de copias es invalido");
                }
            }
        }
        private void aumentar_copias_Click(object sender, RoutedEventArgs e)
        {
            copias.Text = (Convert.ToInt32(copias.Text) + 1).ToString();
        }
        private void disminuir_copias_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(copias.Text) > 1)
            {
                copias.Text = (Convert.ToInt32(copias.Text) - 1).ToString();
            }
        }
        private void checkBoxImpresora_Checked(object sender, RoutedEventArgs e)
        {
            impresora.IsEnabled = false;
        }
        private void checkBoxImpresora_Unchecked(object sender, RoutedEventArgs e)
        {
            impresora.IsEnabled = true;
        }
        #endregion
        private void Print(Visual v, int impresoraSeleccionada=0)
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

            System.Windows.FrameworkElement e = v as System.Windows.FrameworkElement;
            if (e == null)
                return;

            PrintDialog pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = orientacion;
            pd.PrintTicket.PageBorderless = PageBorderless.None;
            pd.PrintTicket.PageMediaSize = tamano;
            pd.PrintTicket.CopyCount = Convert.ToInt32(copias.Text);
            if (impresoraSeleccionada != 0)
            {
                pd.PrintQueue = new PrintQueue(new PrintServer(), impresora.SelectedItem.ToString());
            }


            //store original scale
            Transform originalScale = e.LayoutTransform;
            //get selected printer capabilities
            System.Printing.PrintCapabilities capabilities = pd.PrintQueue.GetPrintCapabilities(pd.PrintTicket);

            //get scale of the print wrt to screen of WPF visual
            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / e.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                           e.ActualHeight);

            //Transform the Visual to scale
            e.LayoutTransform = new ScaleTransform(scale, scale);

            //get the size of the printer page
            System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            //update the layout of the visual to the printer page size.
            e.Measure(sz);
            e.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            //now print the visual to printer to fit on the one page.
            pd.PrintVisual(v, "Balance " + estilo.Content.ToString());

            //apply the original transform.
            e.LayoutTransform = originalScale;

            MessageBox.Show("¡Listo!");

        }
    }
}
