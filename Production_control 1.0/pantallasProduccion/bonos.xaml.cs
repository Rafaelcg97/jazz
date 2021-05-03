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
        #region datosIniciales
        public bonos()
        {
            InitializeComponent();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            sql = "select modulo from modulosProduccion where coordinadorNombre<>'-'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());                
            }
            dr.Close();
            cnProduccion.Close();
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
            consultarBonoPorModoulo(anio, semana, "0");
        }
        #endregion
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
            string modulo = "0";
            if (comboBoxModulo.SelectedIndex == -1)
            {
                modulo = "0";
            }
            else
            {
                modulo = comboBoxModulo.SelectedItem.ToString();
            }

            consultarBonoPorModoulo(anio, semana, modulo);
            consultarBonoPorOperario(anio, semana, modulo);
            consultarHorasPorOperario(anio, semana, modulo);
            MessageBox.Show("Datos Cargados");
        }
        #region calculosGenerales
        private void consultarBonoPorModoulo(int anio, int semana, string modulo)
        {
            listViewBomoPorModulo.Items.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            if (modulo == "0")
            {
                sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' order by turno";
            }
            else
            {
                sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' and modart like'" + comboBoxModulo.SelectedItem.ToString()+"%' order by turno"; 
            }
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewBomoPorModulo.Items.Add(new bonoPorModulo { turno=dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
            }
            dr.Close();
            cnProduccion.Close();
        }
        private void consultarBonoPorOperario(int anio, int semana, string modulo)
        {
            listViewBomoPorOperariio.Items.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            if (modulo == "0")
            {
                sql = "select*from asistencia_3 where semana_b='" + semana + "' and year_b='" + anio + "' order by modulo_b";
            }
            else
            {
                sql = "select*from asistencia_3 where semana_b='" + semana + "' and year_b='" + anio + "' and modulo_b='"+comboBoxModulo.SelectedItem.ToString()+"' order by modulo_b";
            }
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewBomoPorOperariio.Items.Add(new bonoPorOperario { modulo = dr["modulo_b"].ToString(), codigo = Convert.ToInt32(dr["codigo_b"]), nombre = dr["nombre_b"].ToString(), bonoBruto = Convert.ToDouble(dr["bono_total"] is DBNull ? 0 : dr["bono_total"]).ToString("C"), eficienciaG = Convert.ToDouble(dr["efi_$"] is DBNull ? 0 : dr["efi_$"]).ToString("C"), aqlGP = Convert.ToDouble(dr["ganado_aql"] is DBNull ? 0 : dr["ganado_aql"]).ToString("P"), aqlGD = Convert.ToDouble(dr["aql_$"] is DBNull ? 0 : dr["aql_$"]).ToString("C"), aqlIP = Convert.ToDouble(dr["ganado_aqli"] is DBNull ? 0 : dr["ganado_aqli"]).ToString("P"), aqlID = Convert.ToDouble(dr["aqli_$"] is DBNull ? 0 : dr["aqli_$"]).ToString("C"), bpD = Convert.ToDouble(dr["bp_$"] is DBNull ? 0 : dr["bp_$"]).ToString("C"), bpP = Convert.ToDouble(dr["ganado_bp"] is DBNull ? 0 : dr["ganado_bp"]).ToString("P"), asistenciaD = Convert.ToDouble(dr["faltas_$"] is DBNull ? 0 : dr["faltas_$"]).ToString("C"), asistenciaP = Convert.ToDouble(dr["ganado_faltas"] is DBNull ? 0 : dr["ganado_faltas"]).ToString("P"), conductaP = Convert.ToDouble(dr["ganado_amt"] is DBNull ? 0 : dr["ganado_amt"]).ToString("P"), conductaD = Convert.ToDouble(dr["amt_$"] is DBNull ? 0 : dr["amt_$"]).ToString("C"), bonoNeto = Convert.ToDouble(dr["total"] is DBNull ? 0 : dr["total"]).ToString("C") });
            }
            dr.Close();
            cnProduccion.Close();
        }
        private void consultarHorasPorOperario(int anio, int semana, string modulo)
        {
            listViewAsitenciaPorColaborador.Items.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            if (modulo == "0")
            {
                sql = "SELECT [semana],[asignado],[codigo],[nombre],[bonoBrutoLunes],[horasLunes],[bonoBrutoMartes],[horasMartes],[bonoBrutoMiercoles],[horasMiercoles],[bonoBrutoJueves],[horasJueves] ,[bonoBrutoViernes],[horasViernes],[bonoBrutoSabado],[horasSabado],[bonoBruto],[horas] FROM [produccion].[dbo].[bonoPorDiaOperario] where semana='"+semana+"' and anio='"+anio+"' order by asignado";
            }
            else
            {
                sql = "SELECT [semana],[asignado],[codigo],[nombre],[bonoBrutoLunes],[horasLunes],[bonoBrutoMartes],[horasMartes],[bonoBrutoMiercoles],[horasMiercoles],[bonoBrutoJueves],[horasJueves] ,[bonoBrutoViernes],[horasViernes],[bonoBrutoSabado],[horasSabado],[bonoBruto],[horas] FROM [produccion].[dbo].[bonoPorDiaOperario] where semana='"+semana+"' and anio='"+anio+"'and asignado='"+comboBoxModulo.SelectedItem.ToString()+"' order by asignado";
            }
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewAsitenciaPorColaborador.Items.Add(new horasPorOperario {semana=Convert.ToInt32(dr["semana"]), modulo=dr["asignado"].ToString(), codigo=Convert.ToInt32(dr["codigo"]), nombre=dr["nombre"].ToString(), bonoBrutoLunes=Convert.ToDouble(dr["bonoBrutoLunes"]).ToString("C"), bonoBrutoMartes = Convert.ToDouble(dr["bonoBrutoMartes"]).ToString("C"), bonoBrutoMiercoles = Convert.ToDouble(dr["bonoBrutoMiercoles"]).ToString("C"), bonoBrutoJueves = Convert.ToDouble(dr["bonoBrutoJueves"]).ToString("C"), bonoBrutoViernes = Convert.ToDouble(dr["bonoBrutoViernes"]).ToString("C"), bonoBrutoSabado = Convert.ToDouble(dr["bonoBrutoSabado"]).ToString("C"),  horasLunes=Convert.ToInt64(dr["horasLunes"]), horasMartes = Convert.ToInt64(dr["horasMartes"]), horasMiercoles= Convert.ToInt64(dr["horasMiercoles"]), horasJueves= Convert.ToInt64(dr["horasJueves"]), horasViernes= Convert.ToInt64(dr["horasViernes"]), horasSabado= Convert.ToInt64(dr["horasSabado"]), horas= Convert.ToInt64(dr["horas"]), bono=Convert.ToDouble(dr["bonoBruto"]).ToString("C") }); 
            }
            dr.Close();
            cnProduccion.Close();
        }
        #endregion
    }
}
