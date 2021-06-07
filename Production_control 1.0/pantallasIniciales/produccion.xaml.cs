using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class produccion : UserControl
    {
        #region clases_especiales
        public class elemento_grafica
        {
            public string modulo { get; set; }
            public string coordinador { get; set; }
            public double eficiencia { get; set; }
            public int piezas { get; set; }
            public int h1 { get; set; }
            public int h2 { get; set; }
            public int h3 { get; set; }
            public int h4 { get; set; }
            public int h5 { get; set; }
            public int h6 { get; set; }
            public int h7 { get; set; }
            public int h8 { get; set; }
            public int h9 { get; set; }
            public int h10 { get; set; }
            public int h11 { get; set; }
            public int h12 { get; set; }
        }
        #endregion
        #region clases_especiales_para_la_grafica
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<int, string> Formatter { get; set; }
        public Func<double, string> Formatter2 { get; set; }
        #endregion
        #region datos_iniciales
        public produccion()
        {
            InitializeComponent();
            #region datosIncialesDeGrafica
            // se cargan los datos iniciales para la grafica
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Producción",
                    Values = new ChartValues<int> {0},
                    Fill = System.Windows.Media.Brushes.DarkGreen,
                    DataLabels=true,
                },
                new LineSeries
                {
                    Title="Eficiencia",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = Brushes.Transparent,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15,
                    ScalesYAt=1,
                    DataLabels=true,
                },
            };
            Formatter = value => value.ToString("N");
            Formatter2 = value => value.ToString("P");
            DataContext = this;
            #endregion
            radioButtomDiurno.IsChecked = true;
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto
        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void soloNumerosDecimales(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Decimal) || (e.Key == Key.Tab))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void letra_pop_cerrar(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        #region calculos_generals
        private DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            //dependencia hacia la pagina
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

        #endregion
        #region botonesDeAccesoAreas
        private void buttonReporteProduccion_Click(object sender, RoutedEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new pantallasProduccion.reporteProduccion();
        }
        private void buttonConsultaBono_Click(object sender, RoutedEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new pantallasProduccion.bonos();
        }
        private void buttonEditarProduccion_Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasProduccion.validacionUsuarioProduccion("registros"));
        }
        private void buttonAsistencia_Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasProduccion.validacionUsuarioProduccion("asistencia"));
        }
        private void buttonConsultaSam_Click(object sender, RoutedEventArgs e)
        {
            estadoPlantaProduccion estadoPlantaProduccion = new estadoPlantaProduccion();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new sam();
        }
        private void buttonAbrirBalance_Click(object sender, RoutedEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new abrir();
        }
        private void buttonLotes_Click(object sender, RoutedEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new pantallasProduccion.cuadrarLotes();
        }
        private void buttonBuenasPracticas_Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasProduccion.resultadosBuenasPracticas());
        }
        #endregion
        #region actualizarGrafica
        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            string turno = "Diurno";
            if (radioButtomDiurno.IsChecked == true)
            {
                turno = "Diurno";
            }
            else if (radioButtomNocturno.IsChecked == true)
            {
                turno = "Nocturno";
            }
            if (radioButtomExtra.IsChecked == true)
            {
                turno = "Extra";
            }
            actualizarGrafica(turno, fecha);

        }
        private void radioButtomDiurno_Checked(object sender, RoutedEventArgs e)
        {
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            actualizarGrafica("Diurno", fecha);
        }
        private void radioButtomNocturno_Checked(object sender, RoutedEventArgs e)
        {
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            actualizarGrafica("Nocturno", fecha);

        }
        private void radioButtomExtra_Checked(object sender, RoutedEventArgs e)
        {
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            actualizarGrafica("Extra", fecha);

        }
        private void actualizarGrafica(string turno, string fecha)
        {
            //se crea una lista de strings para las etiquetas del eje horizontal (los nombres de los operarios) solo se agregan los que ya han sido asignados
            List<string> modulos = new List<string>();
            int totalPiezas = 0;
            double trabajado = 0;
            double disponible = 0;
            List<elemento_grafica> modulosProduccionEficiencia = new List<elemento_grafica>();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modart, coordinador, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12, totalDePiezas, minutosTrabajados, minutosDisponibles, eficiencia from vistaKPI where fecha='" + fecha + "' and turno='" + turno + "' order by coordinador";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulos.Add(dr["modart"].ToString());
                totalPiezas = totalPiezas + Convert.ToInt32(dr["totalDePiezas"] is DBNull? 0: dr["totalDePiezas"]);
                trabajado = trabajado + Convert.ToDouble(dr["minutosTrabajados"] is DBNull? 0: dr["minutosTrabajados"]);
                disponible = disponible + Convert.ToDouble(dr["minutosDisponibles"] is DBNull? 0: dr["minutosDisponibles"]);
                modulosProduccionEficiencia.Add(new elemento_grafica { modulo = dr["modart"].ToString(), eficiencia = Convert.ToDouble(dr["eficiencia"]), piezas = Convert.ToInt32(dr["totalDePiezas"]), coordinador = dr["coordinador"].ToString(), h1 = Convert.ToInt32(dr["H1"]), h2 = Convert.ToInt32(dr["H2"]), h3 = Convert.ToInt32(dr["H3"]), h4 = Convert.ToInt32(dr["H4"]), h5 = Convert.ToInt32(dr["H5"]), h6 = Convert.ToInt32(dr["H6"]), h7 = Convert.ToInt32(dr["H7"]), h8 = Convert.ToInt32(dr["H8"]), h9 = Convert.ToInt32(dr["H9"]), h10 = Convert.ToInt32(dr["H10"]), h11 = Convert.ToInt32(dr["H11"]), h12 = Convert.ToInt32(dr["H12"]) });
            };
            dr.Close();
            cn.Close();
            //se limpian los datos cargados anteriormente para poder volver a cargar
            grafico.AxisX.Clear();
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            grafico.AxisX.Add(new Axis() { Labels = modulos.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 }, });
            foreach (elemento_grafica item in modulosProduccionEficiencia)
            {
                SeriesCollection[0].Values.Add(item.piezas);
                SeriesCollection[1].Values.Add(item.eficiencia);
            };
            listViewProduccionHora.ItemsSource = modulosProduccionEficiencia;
            labelTotalPiezas.Content = totalPiezas;
            labelTotalEficiencia.Content = (trabajado / disponible).ToString("P");

        }
        #endregion
    }
}
