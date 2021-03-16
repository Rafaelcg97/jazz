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
using LiveCharts;
using LiveCharts.Wpf;

namespace Production_control_1._0
{
    public partial class imprimir_rebalance : Page
    {
        #region clases_especiales
        private PrintDocument printDoc = new PrintDocument();
        #endregion

        #region clases_para_la_grafica
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        #endregion

        #region datos_iniciales
        public imprimir_rebalance()
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

            cargar_datos_grafica();
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

        private void disminuir_copias_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(copias.Text) > 1)
            {
                copias.Text = (Convert.ToInt32(copias.Text) - 1).ToString();
            }

        }

        #endregion

        #region calculos_generales
        private void cargar_datos_generales()
        {
            modulo.Content = impresion_global.modulo;
            impresion.Content = DateTime.Now.ToString("yyyy-MM-dd");
            creacion.Content = impresion_global.fecha;
            operarios.Content = impresion_global.operarios;
            sam.Content = impresion_global.sam;
            tipo.Content = impresion_global.tipo;
            estilo.Content = impresion_global.estilo + ", " + impresion_global.temporada;
            eficiencia.Content = impresion_global.eficiencia;

        }
        private void cargar_datos_grafica()
        {
            // se cargan los datos iniciales para la graf
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Carga",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.Green,
                },
                new LineSeries
                {
                    Title="",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = Brushes.Transparent,
                    PointGeometry= System.Windows.Media.Geometry.Empty
                },
                 new LineSeries
                {
                    Title="",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Blue,
                    Fill = Brushes.Transparent,
                    PointGeometry= System.Windows.Media.Geometry.Empty
                },
                 new LineSeries
                {
                    Title="Eficiencia",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.DarkGoldenrod,
                    Fill = Brushes.Transparent,
                    PointGeometry= DefaultGeometries.Circle,
                }
            };
            Formatter = value => value.ToString("N");
            DataContext = this;

            grafico.AxisX.Clear();
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();
            SeriesCollection[3].Values.Clear();
            grafico.AxisX.Add(new Axis() { Labels = clases_globales.impresion_rebalance.lista_de_operarios_rebalance.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 }, });

            foreach (double item in clases_globales.impresion_rebalance.lista_de_cargas_rebalance)
            {
                SeriesCollection[0].Values.Add(item);
                SeriesCollection[1].Values.Add(1d);
                SeriesCollection[2].Values.Add(0.9d);
            }

            foreach (double item2 in clases_globales.impresion_rebalance.lista_de_eficiencias_rebalance)
            {
                SeriesCollection[3].Values.Add(item2);
            }


        }

        #endregion
    }
}
