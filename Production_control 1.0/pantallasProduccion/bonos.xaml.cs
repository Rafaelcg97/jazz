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
            //definir variables de la consulta
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
            string modulo = "0";
            if (Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd") == "0001-01-01")
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = DateTime.Now.Year;
            }
            else
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(Convert.ToDateTime(calendarFecha.SelectedDate), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = Convert.ToDateTime(calendarFecha.SelectedDate).Year;

            }
            if (comboBoxModulo.SelectedIndex == -1)
            {
                modulo = "0";
            }
            else
            {
                modulo = comboBoxModulo.SelectedItem.ToString();
            }
            // revisar que bono se va a consultar
            if (tabControlListBono.SelectedIndex == 0)
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
                    sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' and (modart='" + comboBoxModulo.SelectedItem.ToString() + "-1' or modart='" + comboBoxModulo.SelectedItem.ToString() + "-2') order by turno";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewBomoPorModulo.Items.Add(new bonoPorModulo { turno = dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
                }
                dr.Close();
                cnProduccion.Close();
            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                listViewBomoPorOperariio.Items.Clear();
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                if (modulo == "0")
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' order by asignado";
                }
                else
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' and asignado='" + comboBoxModulo.SelectedItem.ToString() + "' order by codigo";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["semana"]) == semana)
                    {
                        listViewBomoPorOperariio.Items.Add(new bonoPorOperario { modulo = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C") });
                    }

                }
                dr.Close();
                cnProduccion.Close();

            }

        }
        private void tabControlListBono_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //definir variables de la consulta
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
            string modulo = "0";
            if (Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd") == "0001-01-01")
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = DateTime.Now.Year;
            }
            else
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(Convert.ToDateTime(calendarFecha.SelectedDate), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = Convert.ToDateTime(calendarFecha.SelectedDate).Year;

            }
            if (comboBoxModulo.SelectedIndex == -1)
            {
                modulo = "0";
            }
            else
            {
                modulo = comboBoxModulo.SelectedItem.ToString();
            }
            // revisar que bono se va a consultar
            if (tabControlListBono.SelectedIndex == 0)
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
                    sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' and (modart='" + comboBoxModulo.SelectedItem.ToString() + "-1' or modart='" + comboBoxModulo.SelectedItem.ToString() + "-2') order by turno";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewBomoPorModulo.Items.Add(new bonoPorModulo { turno = dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
                }
                dr.Close();
                cnProduccion.Close();
            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                listViewBomoPorOperariio.Items.Clear();
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                if (modulo == "0")
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' order by asignado";
                }
                else
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' and asignado='" + comboBoxModulo.SelectedItem.ToString() + "' order by codigo";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["semana"]) == semana)
                    {
                        listViewBomoPorOperariio.Items.Add(new bonoPorOperario { modulo = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C") });
                    }
                    
                }
                dr.Close();
                cnProduccion.Close();

            }
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //definir variables de la consulta
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
            string modulo = "0";
            if (Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd") == "0001-01-01")
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = DateTime.Now.Year;
            }
            else
            {
                semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(Convert.ToDateTime(calendarFecha.SelectedDate), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                anio = Convert.ToDateTime(calendarFecha.SelectedDate).Year;

            }
            if (comboBoxModulo.SelectedIndex == -1)
            {
                modulo = "0";
            }
            else
            {
                modulo = comboBoxModulo.SelectedItem.ToString();
            }
            // revisar que bono se va a consultar
            if (tabControlListBono.SelectedIndex == 0)
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
                    sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' and (modart='" + comboBoxModulo.SelectedItem.ToString() + "-1' or modart='" + comboBoxModulo.SelectedItem.ToString() + "-2') order by turno";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewBomoPorModulo.Items.Add(new bonoPorModulo { turno = dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
                }
                dr.Close();
                cnProduccion.Close();
            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                listViewBomoPorOperariio.Items.Clear();
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                if (modulo == "0")
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' order by asignado";
                }
                else
                {
                    sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' and asignado='" + comboBoxModulo.SelectedItem.ToString() + "' order by codigo";
                }
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["semana"]) == semana)
                    {
                        listViewBomoPorOperariio.Items.Add(new bonoPorOperario { modulo = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C") });
                    }

                }
                dr.Close();
                cnProduccion.Close();

            }
        }
    }
}
