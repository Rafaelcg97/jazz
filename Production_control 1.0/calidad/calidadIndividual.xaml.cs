using Production_control_1._0.clases;
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

namespace Production_control_1._0.calidad
{
    public partial class calidadIndividual : Page
    {
        #region varibalesConexion
        public SqlConnection cnCalidad = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_calidad"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        List<registroCalidad> registrosCalidad = new List<registroCalidad>();
        #endregion
        public calidadIndividual()
        {
            InitializeComponent();
            consultarDatos();
            foreach(registroCalidad item in registrosCalidad)
            {
                listViewAqlOperarios.Items.Add(item);
            }
        }
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            // navegarInicioKanBan
            PagePrincipal pagePrincipal = new PagePrincipal();
            Grid gridInicio = (Grid)pagePrincipal.Content;
            foreach (object objeto in gridInicio.Children)
            {
                if (objeto.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)objeto;
                    if (grid.Name == "gridListaAreas")
                    {
                        foreach (object objeto2 in grid.Children)
                        {
                            if (objeto2.GetType() == typeof(ListView))
                            {
                                ListView listviewMenu = (ListView)objeto2;
                                listviewMenu.SelectedIndex = 6;
                            }
                        }
                    }
                }
            }
            NavigationService.Navigate(pagePrincipal);
        }
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
        #region tamanoLetra/solonumerosenTextBox
        private void letraAjustable1(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable2(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable3(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        private void consultarDatos()
        {
            registrosCalidad.Clear();

            //int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear((DateTime)calendarCalidad.SelectedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //int anio = ((DateTime)calendarCalidad.SelectedDate).Year;
            string sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo";
            SqlCommand cm = new SqlCommand(sql, cnCalidad);
            cnCalidad.Open();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                registrosCalidad.Add(
                    new registroCalidad
                    {
                        fechaDt = Convert.ToDateTime(dr["fecha_p1"]),
                        fecha = Convert.ToDateTime(dr["fecha_p1"]).ToString("yyyy-MM-dd"),
                        codigo =dr["operario_p1"].ToString(),
                        nombre=dr["nombre"].ToString(),
                        muestraP=Convert.ToInt32(dr["piezas_p1"] is DBNull ? 0 : dr["piezas_p1"]),
                        rechazosP= Convert.ToInt32(dr["rechazos_p1"] is DBNull ? 0 : dr["rechazos_p1"]),
                        aqlP= Convert.ToDouble(dr["aql_f1"] is DBNull ? 0 : dr["aql_f1"])
                    });
            }
            cnCalidad.Close();
        }
    }
}
