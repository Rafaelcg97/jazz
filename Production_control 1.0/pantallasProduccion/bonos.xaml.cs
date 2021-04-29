using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
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
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasProduccion
{
    public partial class bonos : Page
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        public bonos()
        {
            InitializeComponent();
        }
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PagePrincipal());

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

        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewBomoPorOperariio.Items.Clear();
            DateTime v2 = (DateTime)calendarFecha.SelectedDate;
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(v2, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = v2.Year;
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            //llenar lista de modulos
            //consultar
            sql = "select*from asistencia_3 where semana_b='" + semana + "' and year_b='" + anio + "'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewBomoPorOperariio.Items.Add(new bonoPorOperario {modulo=dr["modulo_b"].ToString(), codigo=Convert.ToInt32(dr["codigo_b"]), nombre=dr["nombre_b"].ToString(), bonoBruto= Convert.ToDouble(dr["bono_total"] is DBNull ? 0 : dr["bono_total"]).ToString("C"), eficienciaG= Convert.ToDouble(dr["efi_$"] is DBNull ? 0 : dr["efi_$"]).ToString("C"), aqlGP= Convert.ToDouble(dr["ganado_aql"] is DBNull ? 0 : dr["ganado_aql"]).ToString("P"), aqlGD= Convert.ToDouble(dr["aql_$"] is DBNull ? 0 : dr["aql_$"]).ToString("C"), aqlIP= Convert.ToDouble(dr["ganado_aqli"] is DBNull ? 0 : dr["ganado_aqli"]).ToString("P"), aqlID= Convert.ToDouble(dr["aqli_$"] is DBNull ? 0 : dr["aqli_$"]).ToString("C"), bpD= Convert.ToDouble(dr["bp_$"] is DBNull ? 0 : dr["bp_$"]).ToString("C"), bpP= Convert.ToDouble(dr["ganado_bp"] is DBNull ? 0 : dr["ganado_bp"]).ToString("P"), asistenciaD= Convert.ToDouble(dr["faltas_$"] is DBNull ? 0 : dr["faltas_$"]).ToString("C"), asistenciaP= Convert.ToDouble(dr["ganado_faltas"] is DBNull ? 0 : dr["ganado_faltas"]).ToString("P"), conductaP= Convert.ToDouble(dr["ganado_amt"] is DBNull ? 0 : dr["ganado_amt"]).ToString("P"), conductaD= Convert.ToDouble(dr["amt_$"] is DBNull ? 0 : dr["amt_$"]).ToString("C"), bonoNeto= Convert.ToDouble(dr["total"] is DBNull ? 0 : dr["total"]).ToString("C") });
            }
            dr.Close();
            cnProduccion.Close();
        }
    }
}
