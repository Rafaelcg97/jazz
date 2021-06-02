using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using Microsoft.Win32;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasProduccion
{
    public partial class bonos : Page
    {
        List<bonoPorOperario> bonoPorOperario = new List<bonoPorOperario>();
        List<bonoPorModulo> bonoPorModulo= new List<bonoPorModulo>();
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
            this.NavigationService.GoBack();
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
        #region consultaDeRegistros
        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //definir variables de la consulta
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
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
            // revisar que bono se va a consultar
            if (tabControlListBono.SelectedIndex == 0)
            {
                bonoPorModulo.Clear();
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' order by turno";
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    bonoPorModulo.Add(new bonoPorModulo { turno = dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
                }
                dr.Close();
                cnProduccion.Close();
                listViewBomoPorModulo.Items.Clear();
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBomoPorModulo.Items.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if (item.modart == comboBoxModulo.SelectedItem.ToString() + "-1" || item.modart == comboBoxModulo.SelectedItem.ToString() + "-2")
                        {
                            listViewBomoPorModulo.Items.Add(item);
                        }
                    }

                }

            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                bonoPorOperario.Clear();
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' order by asignado";
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["semana"]) == semana)
                    {
                        bonoPorOperario.Add(new bonoPorOperario { asignado = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C"), tiempoLunes = Convert.ToDouble(dr["tiempoLunes"]), tiempoMartes = Convert.ToDouble(dr["tiempoMartes"]), tiempoMiercoles = Convert.ToDouble(dr["tiempoMiercoles"]), tiempoJueves = Convert.ToDouble(dr["tiempoJueves"]), tiempoViernes = Convert.ToDouble(dr["tiempoViernes"]), tiempoSabado = Convert.ToDouble(dr["tiempoSabado"]), bp = Convert.ToDouble(dr["bp"]).ToString("P"), aqlG = Convert.ToDouble(dr["aqlG"]).ToString("P"), aqlI = Convert.ToDouble(dr["aqlI"]).ToString("P"), inasistencias = Convert.ToDouble(dr["inasistencias"]).ToString("P"), amonestaciones = Convert.ToDouble(dr["amonestaciones"]).ToString("P"), bonoNeto = Convert.ToDouble(dr["bonoNeto"]).ToString("C") });
                    }
                }
                dr.Close();
                cnProduccion.Close();
                listViewBomoPorOperariio.Items.Clear();
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        listViewBomoPorOperariio.Items.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        if (item.asignado == comboBoxModulo.SelectedItem.ToString())
                        {
                            listViewBomoPorOperariio.Items.Add(item);
                        }
                    }
                }

            }
        }
        private void tabControlListBono_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //definir variables de la consulta
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
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
            // revisar que bono se va a consultar
            if (tabControlListBono.SelectedIndex == 0)
            {
                bonoPorModulo.Clear();
                textBoxBuscarOperario.IsEnabled = false;
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                sql = "SELECT [turno], [modart],[piezasLunes],[eficienciaLunes],[bonoLunes],[piezasMartes],[eficienciaMartes],[bonoMartes],[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles],[piezasJueves],[eficienciaJueves],[bonoJueves],[piezasViernes],[eficienciaViernes],[bonoViernes],[piezasSabado],[eficienciaSabado],[bonoSabado],[totalDePiezas],[bono] FROM [produccion].[dbo].[bonoPorDiaSemana] where anio='" + anio + "' and semana='" + semana + "' order by turno";
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    bonoPorModulo.Add(new bonoPorModulo { turno = dr["turno"].ToString(), modart = dr["modart"].ToString(), piezasLunes = Convert.ToInt32(dr["piezasLunes"]), piezasMartes = Convert.ToInt32(dr["piezasmartes"]), piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"]), piezasJueves = Convert.ToInt32(dr["piezasJueves"]), piezasViernes = Convert.ToInt32(dr["piezasViernes"]), piezasSabado = Convert.ToInt32(dr["piezasSabado"]), bonoLunes = Convert.ToDouble(dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"]).ToString("C"), eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"]).ToString("P"), eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"]).ToString("P"), eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"]).ToString("P"), eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"]).ToString("P"), eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"]).ToString("P"), eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"]).ToString("P"), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]), bono = Convert.ToDouble(dr["bono"]).ToString("C") });
                }
                dr.Close();
                cnProduccion.Close();
                listViewBomoPorModulo.Items.Clear();
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBomoPorModulo.Items.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if(item.modart==comboBoxModulo.SelectedItem.ToString()+"-1"|| item.modart == comboBoxModulo.SelectedItem.ToString() + "-2")
                        {
                            listViewBomoPorModulo.Items.Add(item);
                        }
                    }

                }

            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                bonoPorOperario.Clear();
                textBoxBuscarOperario.IsEnabled = true;
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                sql = "select anio, semana, asignado, codigo, nombre, bonoLunes, bonoMartes, bonoMiercoles, bonoJueves, bonoViernes, bonoSabado, tiempoLunes, tiempoMartes, tiempoMiercoles, tiempoJueves, tiempoViernes, tiempoSabado, BonoBruto, bp, aqlG, aqlI, inasistencias, amonestaciones, bonoNeto from bonoPorOperario where semana>='" + semana + "' and anio='" + anio + "' order by asignado";
                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["semana"]) == semana)
                    {
                        bonoPorOperario.Add(new bonoPorOperario { asignado = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C"), tiempoLunes = Convert.ToDouble(dr["tiempoLunes"]), tiempoMartes = Convert.ToDouble(dr["tiempoMartes"]), tiempoMiercoles = Convert.ToDouble(dr["tiempoMiercoles"]), tiempoJueves = Convert.ToDouble(dr["tiempoJueves"]), tiempoViernes = Convert.ToDouble(dr["tiempoViernes"]), tiempoSabado = Convert.ToDouble(dr["tiempoSabado"]), bp = Convert.ToDouble(dr["bp"]).ToString("P"), aqlG = Convert.ToDouble(dr["aqlG"]).ToString("P"), aqlI = Convert.ToDouble(dr["aqlI"]).ToString("P"), inasistencias = Convert.ToDouble(dr["inasistencias"]).ToString("P"), amonestaciones = Convert.ToDouble(dr["amonestaciones"]).ToString("P"), bonoNeto = Convert.ToDouble(dr["bonoNeto"]).ToString("C") });
                    }
                }
                dr.Close();
                cnProduccion.Close();
                listViewBomoPorOperariio.Items.Clear();
                if (comboBoxModulo.SelectedIndex== -1)
                {
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        listViewBomoPorOperariio.Items.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        if (item.asignado == comboBoxModulo.SelectedItem.ToString())
                        {
                            listViewBomoPorOperariio.Items.Add(item);
                        }
                    }
                }

            }
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlListBono.SelectedIndex == 0)
            {
                if (comboBoxModulo.SelectedIndex > -1)
                {
                    listViewBomoPorModulo.Items.Clear();
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if (item.modart == comboBoxModulo.SelectedItem.ToString()+"-1"|| item.modart == comboBoxModulo.SelectedItem.ToString() + "-2")

                        {
                            listViewBomoPorModulo.Items.Add(item);
                        }
                    }
                }

                else if (comboBoxModulo.SelectedIndex == -1)
                {
                    listViewBomoPorModulo.Items.Clear();
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBomoPorModulo.Items.Add(item);
                    }
                }

            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                if (comboBoxModulo.SelectedIndex > -1)
                {
                    listViewBomoPorOperariio.Items.Clear();
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        if (item.asignado == comboBoxModulo.SelectedItem.ToString())

                        {
                            listViewBomoPorOperariio.Items.Add(item);
                        }
                    }
                }

                else if (comboBoxModulo.SelectedIndex == -1)
                {
                    listViewBomoPorOperariio.Items.Clear();
                    foreach (bonoPorOperario item in bonoPorOperario)
                    {
                        listViewBomoPorOperariio.Items.Add(item);
                    }
                }
            }
        }
        private void buttonLimipiar_Click(object sender, RoutedEventArgs e)
        {
            comboBoxModulo.SelectedIndex = -1;
        }
        private void textBoxBuscarOperario_TextChanged(object sender, TextChangedEventArgs e)
        {
            //buscar por codigo
            if (checkBoxOpcionDeBusqueda.IsChecked == true)
            {
                //si no hay modulo seleccionado
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    if (String.IsNullOrEmpty(textBoxBuscarOperario.Text.Trim()) == false)
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.codigo.ToString().StartsWith(textBoxBuscarOperario.Text.Trim()))
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }
                    else if (textBoxBuscarOperario.Text.Trim() == "")
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            listViewBomoPorOperariio.Items.Add(item);
                        }
                    }
                }
                //si hay modulo seleccionado
                else
                {
                    if (String.IsNullOrEmpty(textBoxBuscarOperario.Text.Trim()) == false)
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.codigo.ToString().StartsWith(textBoxBuscarOperario.Text.Trim()) && item.asignado==comboBoxModulo.SelectedItem.ToString())
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }
                    else if (textBoxBuscarOperario.Text.Trim() == "")
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.asignado == comboBoxModulo.SelectedItem.ToString())
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }

                }
            }
            //buscar por nombre de persona
            else if (checkBoxOpcionDeBusqueda.IsChecked == false)
            {
                //si no hay modulo seleccionado
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    if (String.IsNullOrEmpty(textBoxBuscarOperario.Text.Trim()) == false)
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.nombre.StartsWith(textBoxBuscarOperario.Text.Trim()))
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }
                    else if (textBoxBuscarOperario.Text.Trim() == "")
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            listViewBomoPorOperariio.Items.Add(item);
                        }
                    }
                }
                //si hay modulo seleccionado
                else
                {
                    if (String.IsNullOrEmpty(textBoxBuscarOperario.Text.Trim()) == false)
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.nombre.StartsWith(textBoxBuscarOperario.Text.Trim()) && item.asignado == comboBoxModulo.SelectedItem.ToString())
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }
                    else if (textBoxBuscarOperario.Text.Trim() == "")
                    {
                        listViewBomoPorOperariio.Items.Clear();
                        foreach (bonoPorOperario item in bonoPorOperario)
                        {
                            if (item.asignado == comboBoxModulo.SelectedItem.ToString())
                            {
                                listViewBomoPorOperariio.Items.Add(item);
                            }
                        }
                    }

                }

            }

        }
        private void buttonDescargar_Click(object sender, RoutedEventArgs e)
        {
            if (tabControlListBono.SelectedIndex == 0)
            {
                StringBuilder buffer = new StringBuilder();
                #region encabezados
                buffer.Append("TURNO");
                buffer.Append(",");
                buffer.Append("MODULO");
                buffer.Append(",");
                buffer.Append("PIEZAS LU");
                buffer.Append(",");
                buffer.Append("% LU");
                buffer.Append(",");
                buffer.Append("$ LU");
                buffer.Append(",");
                buffer.Append("PIEZAS MA");
                buffer.Append(",");
                buffer.Append("% MA");
                buffer.Append(",");
                buffer.Append("$ MA");
                buffer.Append(",");
                buffer.Append("PIEZAS MI");
                buffer.Append(",");
                buffer.Append("% MI");
                buffer.Append(",");
                buffer.Append("$ MI");
                buffer.Append(",");
                buffer.Append("PIEZAS JU");
                buffer.Append(",");
                buffer.Append("% JU");
                buffer.Append(",");
                buffer.Append("$ JU");
                buffer.Append(",");
                buffer.Append("PIEZAS VI");
                buffer.Append(",");
                buffer.Append("% VI");
                buffer.Append(",");
                buffer.Append("$ VI");
                buffer.Append(",");
                buffer.Append("PIEZAS SA");
                buffer.Append(",");
                buffer.Append("% SA");
                buffer.Append(",");
                buffer.Append("$ SA");
                buffer.Append(",");
                buffer.Append("TOTAL PIEZAS");
                buffer.Append(",");
                buffer.Append("TOTAL BONO");
                buffer.Append("\n");
                #endregion
                foreach (bonoPorModulo item in listViewBomoPorModulo.Items)
                {
                    buffer.Append(item.turno);
                    buffer.Append(",");
                    buffer.Append(item.modart);
                    buffer.Append(",");
                    buffer.Append(item.piezasLunes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaLunes);
                    buffer.Append(",");
                    buffer.Append(item.bonoLunes);
                    buffer.Append(",");
                    buffer.Append(item.piezasMartes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaMartes);
                    buffer.Append(",");
                    buffer.Append(item.bonoMartes);
                    buffer.Append(",");
                    buffer.Append(item.piezasMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.bonoMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.piezasJueves);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaJueves);
                    buffer.Append(",");
                    buffer.Append(item.bonoJueves);
                    buffer.Append(",");
                    buffer.Append(item.piezasViernes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaViernes);
                    buffer.Append(",");
                    buffer.Append(item.bonoViernes);
                    buffer.Append(",");
                    buffer.Append(item.piezasSabado);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaSabado);
                    buffer.Append(",");
                    buffer.Append(item.bonoSabado);
                    buffer.Append(",");
                    buffer.Append(item.totalDePiezas);
                    buffer.Append(",");
                    buffer.Append(item.bono);
                    buffer.Append("\n");
                }
                String result = buffer.ToString();
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CSV (*.csv)|*.csv";
                    string fileName = "";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        fileName = saveFileDialog.FileName;
                        StreamWriter sw = new StreamWriter(fileName);
                        sw.WriteLine(result);
                        sw.Close();
                    }

                    Process.Start(fileName);
                }
                catch (Exception ex)
                { }

            }
            else if (tabControlListBono.SelectedIndex == 1)
            {
                StringBuilder buffer = new StringBuilder();
                #region encabezados
                buffer.Append("MODULO");
                buffer.Append(",");
                buffer.Append("CODIGO");
                buffer.Append(",");
                buffer.Append("NOMBRE");
                buffer.Append(",");
                buffer.Append("LU H");
                buffer.Append(",");
                buffer.Append("LU $");
                buffer.Append(",");
                buffer.Append("MA H");
                buffer.Append(",");
                buffer.Append("MA $");
                buffer.Append(",");
                buffer.Append("MI H");
                buffer.Append(",");
                buffer.Append("MI $");
                buffer.Append(",");
                buffer.Append("JU H");
                buffer.Append(",");
                buffer.Append("JU $");
                buffer.Append(",");
                buffer.Append("VI H");
                buffer.Append(",");
                buffer.Append("VI $");
                buffer.Append(",");
                buffer.Append("SA H");
                buffer.Append(",");
                buffer.Append("SA $");
                buffer.Append(",");
                buffer.Append("BONO BRUTO");
                buffer.Append(",");
                buffer.Append("BP");
                buffer.Append(",");
                buffer.Append("AQL G");
                buffer.Append(",");
                buffer.Append("AQL I");
                buffer.Append(",");
                buffer.Append("INASISTENCIA");
                buffer.Append(",");
                buffer.Append("AMONESTACIONES");
                buffer.Append(",");
                buffer.Append("BONO NETO");
                buffer.Append("\n");
                #endregion
                foreach (bonoPorOperario item in listViewBomoPorOperariio.Items)
                {
                    buffer.Append(item.asignado);
                    buffer.Append(",");
                    buffer.Append(item.codigo);
                    buffer.Append(",");
                    buffer.Append(item.nombre);
                    buffer.Append(",");
                    buffer.Append(item.tiempoLunes);
                    buffer.Append(",");
                    buffer.Append(item.bonoLunes);
                    buffer.Append(",");
                    buffer.Append(item.tiempoMartes);
                    buffer.Append(",");
                    buffer.Append(item.bonoMartes);
                    buffer.Append(",");
                    buffer.Append(item.tiempoMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.bonoMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.tiempoJueves);
                    buffer.Append(",");
                    buffer.Append(item.bonoJueves);
                    buffer.Append(",");
                    buffer.Append(item.tiempoViernes);
                    buffer.Append(",");
                    buffer.Append(item.bonoViernes);
                    buffer.Append(",");
                    buffer.Append(item.tiempoSabado);
                    buffer.Append(",");
                    buffer.Append(item.bonoSabado);
                    buffer.Append(",");
                    buffer.Append(item.bonoBruto);
                    buffer.Append(",");
                    buffer.Append(item.bp);
                    buffer.Append(",");
                    buffer.Append(item.aqlG);
                    buffer.Append(",");
                    buffer.Append(item.aqlI);
                    buffer.Append(",");
                    buffer.Append(item.inasistencias);
                    buffer.Append(",");
                    buffer.Append(item.amonestaciones);
                    buffer.Append(",");
                    buffer.Append(item.bonoNeto);
                    buffer.Append("\n");
                }
                String result = buffer.ToString();
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CSV (*.csv)|*.csv";
                    string fileName = "";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        fileName = saveFileDialog.FileName;
                        StreamWriter sw = new StreamWriter(fileName);
                        sw.WriteLine(result);
                        sw.Close();
                    }
                    Process.Start(fileName);
                }
                catch (Exception ex)
                { }
            }
        }
        #endregion
    }
}
