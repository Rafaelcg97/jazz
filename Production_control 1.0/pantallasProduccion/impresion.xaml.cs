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
using System.Configuration;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Printing;
using System.Drawing.Printing;
using NHibernate.Impl;
using Production_control_1._0.clases;
using System.Management;

namespace Production_control_1._0
{
    public partial class impresion : Page
    {
        #region clases_especiales
        private PrintDocument printDoc = new PrintDocument();
        #endregion
        #region datos_iniciales
        public impresion(List<elementoListBox> listaOperariosRecibidos, List<maquina> resumenMaquinas, clases.balance general)
        {
            InitializeComponent();
            #region datosMaquinaa
            foreach (object elemento in prueba.Children)
            {
                if(elemento.GetType() == typeof(Label))
                {
                    Label label = ((Label)elemento);
                    foreach(elementoListBox item in listaOperariosRecibidos)
                    {
                        #region determinarColor
                        Brush color = Brushes.White;
                        switch (item.colorAjuste)
                        {
                            case "rojo":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF8080");
                                break;
                            case "azul":
                                color =(SolidColorBrush)new BrushConverter().ConvertFromString("#FF89BFF5");
                                break;
                            case "verde":
                                color =(SolidColorBrush)new BrushConverter().ConvertFromString("#FF7FE483");
                                break;
                            case "amarillo":
                                color = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFC662");
                                break;
                            default:
                                color = Brushes.White;
                                break;
                        }
                        #endregion
                        if (label.Name.ToString() == "mp" + item.correlativoOperacion)
                        {
                            label.Content = item.ajusteMaquina;
                            label.Background = color;
                        }
                        if (label.Name.ToString() == "o" + item.correlativoOperacion)
                        {
                            label.Content = item.nombreOperario;
                            label.Background = color;
                        }
                    }
                }
                if(elemento.GetType() == typeof(ListBox))
                {
                    ListBox listBox = ((ListBox)elemento);
                    foreach (elementoListBox item in listaOperariosRecibidos)
                    {
                        #region determinarColor
                        Brush color = Brushes.White;
                        switch (item.colorAjuste)
                        {
                            case "rojo":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF8080");
                                break;
                            case "azul":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF89BFF5");
                                break;
                            case "verde":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7FE483");
                                break;
                            case "amarillo":
                                color = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                color = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFC662");
                                break;
                            default:
                                color = Brushes.White;
                                break;
                        }
                        #endregion
                        if (listBox.Name.ToString() == "op" + item.correlativoOperacion)
                        {
                            listBox.Items.Add(item.tituloOperacion);
                            listBox.Background = color;
                        }
                        else
                        {

                        }
                    }
                }
            }
            #endregion
            #region datosGenerales
            resumen_maquinas.ItemsSource = resumenMaquinas;
            lote.Content = general.lote;
            sam.Content = general.sam;
            sobrecarga.Text = general.sobre;
            subutilizado.Text = general.sub;
            estilo.Content = general.nombre;
            temporada.Content = general.temporada;
            modulo.Content = general.modulo;
            operarios.Content = general.operarios;
            ingeniero.Content = general.ingeniero;
            textBoxFechaModificado.Text = general.fechaModificacion.ToString("yyyy-MM-dd");
            #endregion
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
            tamano_impresion.SelectedItem="Tabloide";

            //numero de copias
            copias.Text = "1";
            #endregion
            cargar_image();
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
                tmp.FontSize = e.NewSize.Height * 0.25 / tmp.FontFamily.LineSpacing;
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
        #region accionesBarraDeTitulo
        private void ButtonSalir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonMaximizar(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            };

        }
        private void ButtonMinimizar(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }
        #endregion
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
                    PrintDialog dialog = new PrintDialog();
                    dialog.PrintTicket.PageOrientation = orientacion;
                    dialog.PrintTicket.PageBorderless = PageBorderless.None;
                    dialog.PrintTicket.PageMediaSize = tamano;
                    dialog.PrintTicket.CopyCount = Convert.ToInt32(copias.Text);
                    dialog.PrintVisual(area_de_impresion, "LayOut");
                    MessageBox.Show("Enviado a Impresora");
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
                    PrintDialog dialog = new PrintDialog();
                    dialog.PrintTicket.PageOrientation = orientacion;
                    dialog.PrintTicket.PageBorderless = PageBorderless.None;
                    dialog.PrintTicket.PageMediaSize = tamano;
                    dialog.PrintTicket.CopyCount = Convert.ToInt32(copias.Text);
                    dialog.PrintQueue = new PrintQueue(new PrintServer(), impresora.SelectedItem.ToString());
                    dialog.PrintVisual(area_de_impresion, "LayOut");
                    MessageBox.Show("Enviado a Impresora");
                }
                catch
                {
                    MessageBox.Show("No se reconoce la impresora o el número de copias es invalido");
                }
            }
            //try
            //{
            //    this.IsEnabled = false;
            //    PrintDialog printDialog = new PrintDialog();
            //    if (printDialog.ShowDialog() == true)
            //    {
            //        printDialog.PrintVisual(area_de_impresion, "layOut");
            //    }
            //}
            //finally
            //{
            //    this.IsEnabled = true;
            //}
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
        #region calculos_generales
        private void cargar_image()
        {
            // se carga la imagen desde archivo
            try
            {
                Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + temporada.Content.ToString() + "/" + estilo.Content.ToString() + ".jpg");
                imagen.Source = new BitmapImage(fileUri);
            }

            // si no encuentra la imagen del estilo carga la imagen inicial
            catch
            {
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imagen.Source = new BitmapImage(fileUri);
            }
        }

        #endregion
    }
}
