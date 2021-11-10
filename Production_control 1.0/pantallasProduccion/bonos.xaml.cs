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
        List<bonoPorModulo> listViewBonoPorModulo = new List<bonoPorModulo>();
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
                textBoxBuscarOperario.IsEnabled = false;
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                sql = "SELECT [turno], [modart]," +
                    "[piezasLunes],[eficienciaLunes],[bonoLunes], [samLunes], [operariosLunes], [minutosTrabajadosLunes],[minutosDisponiblesLunes], " +
                    "[piezasMartes],[eficienciaMartes],[bonoMartes], [samMartes], [operariosMartes], [minutosTrabajadosMartes], [minutosDisponiblesMartes], " +
                    "[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles], [samMiercoles], [operariosMiercoles], [minutosTrabajadosMiercoles], [minutosDisponiblesMiercoles], " +
                    "[piezasJueves],[eficienciaJueves],[bonoJueves],[samJueves], [operariosJueves], [minutosTrabajadosJueves], [minutosDisponiblesJueves], " +
                    "[piezasViernes],[eficienciaViernes],[bonoViernes], [samViernes], [operariosViernes], [minutosTrabajadosViernes], [minutosDisponiblesViernes], " +
                    "[piezasSabado], [samSabado], [operariosSabado], [eficienciaSabado],[bonoSabado], [minutosTrabajadosSabado], [minutosDisponiblesSabado]," +
                    "[totalDePiezas], [samTotal], [eficienciaTotal], [operarios], [bono], [minutosTrabajados], [minutosDisponibles] FROM [produccion].[dbo].[bonoPorDiaSemana] " +
                    "where anio='" + anio + "' and semana='" + semana + "' order by turno";

                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    bonoPorModulo.Add(new bonoPorModulo { 
                        turno = dr["turno"].ToString(), 
                        modart = dr["modart"].ToString(),
                        piezasLunes = Convert.ToInt32(dr["piezasLunes"] is DBNull ? 0 : dr["piezasLunes"]), 
                        piezasMartes = Convert.ToInt32(dr["piezasmartes"] is DBNull ? 0 : dr["piezasMartes"]), 
                        piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"] is DBNull ? 0 : dr["piezasMiercoles"]),
                        piezasJueves = Convert.ToInt32(dr["piezasJueves"] is DBNull ? 0 : dr["piezasJueves"]), 
                        piezasViernes = Convert.ToInt32(dr["piezasViernes"] is DBNull ? 0 : dr["piezasViernes"]), 
                        piezasSabado = Convert.ToInt32(dr["piezasSabado"] is DBNull ? 0 : dr["piezasSabado"]), 
                        bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"), 
                        bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"), 
                        bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"), 
                        bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"), 
                        bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"), 
                        bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"),
                        bonoLunesD = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]),
                        bonoMartesD = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]),
                        bonoMiercolesD = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]),
                        bonoJuevesD = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]),
                        bonoViernesD = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]),
                        bonoSabadoD = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]),
                        eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"] is DBNull ? 0 : dr["eficienciaLunes"]).ToString("P"), 
                        eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"] is DBNull ? 0 : dr["eficienciaMartes"]).ToString("P"), 
                        eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"] is DBNull ? 0 : dr["eficienciaMiercoles"]).ToString("P"), 
                        eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"] is DBNull ? 0 : dr["eficienciaJueves"]).ToString("P"), 
                        eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"] is DBNull ? 0 : dr["eficienciaViernes"]).ToString("P"), 
                        eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"] is DBNull ? 0 : dr["eficienciaSabado"]).ToString("P"), 
                        operariosLunes= Convert.ToInt32(dr["operariosLunes"] is DBNull ? 0 : dr["operariosLunes"]),
                        operariosMartes = Convert.ToInt32(dr["operariosMartes"] is DBNull ? 0 : dr["operariosMartes"]),
                        operariosMiercoles = Convert.ToInt32(dr["operariosMiercoles"] is DBNull ? 0 : dr["operariosMiercoles"]),
                        operariosJueves = Convert.ToInt32(dr["operariosJueves"] is DBNull ? 0 : dr["operariosJueves"]),
                        operariosViernes = Convert.ToInt32(dr["operariosViernes"] is DBNull ? 0 : dr["operariosViernes"]),
                        operariosSabado = Convert.ToInt32(dr["operariosSabado"] is DBNull ? 0 : dr["operariosSabado"]),
                        samLunes =dr["samLunes"].ToString(), 
                        samMartes = dr["samMartes"].ToString(), 
                        samMiercoles = dr["samMiercoles"].ToString(), 
                        samJueves = dr["samJueves"].ToString(), 
                        samViernes= dr["samViernes"].ToString(), 
                        samSabado= dr["samSabado"].ToString(),
                        mtLunes = Convert.ToDouble(dr["minutosTrabajadosLunes"]),
                        mtMartes = Convert.ToDouble(dr["minutosTrabajadosMartes"]),
                        mtMiercoles = Convert.ToDouble(dr["minutosTrabajadosMiercoles"]),
                        mtJueves = Convert.ToDouble(dr["minutosTrabajadosJueves"]),
                        mtViernes = Convert.ToDouble(dr["minutosTrabajadosViernes"]),
                        mtSabado = Convert.ToDouble(dr["minutosTrabajadosSabado"]),
                        mdLunes = Convert.ToDouble(dr["minutosDisponiblesLunes"]),
                        mdMartes = Convert.ToDouble(dr["minutosDisponiblesMartes"]),
                        mdMiercoles = Convert.ToDouble(dr["minutosDisponiblesMiercoles"]),
                        mdJueves = Convert.ToDouble(dr["minutosDisponiblesJueves"]),
                        mdViernes = Convert.ToDouble(dr["minutosDisponiblesViernes"]),
                        mdSabado = Convert.ToDouble(dr["minutosDisponiblesSabado"]),
                        totalDePiezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]), 
                        samTotal=dr["samTotal"].ToString(), 
                        eficienciaTotal=Convert.ToDouble(dr["eficienciaTotal"] is DBNull? 0: dr["eficienciaTotal"]).ToString("P"), 
                        bono = Convert.ToDouble(dr["bono"] is DBNull ? 0 : dr["bono"]).ToString("C"),
                        bonoD = Convert.ToDouble(dr["bono"] is DBNull ? 0 : dr["bono"]),
                        operarios = Convert.ToInt32(dr["operarios"] is DBNull ? 0 : dr["operarios"]),
                        mt = Convert.ToDouble(dr["minutosTrabajados"]),
                        md=Convert.ToDouble(dr["minutosDisponibles"])});
                }
                dr.Close();
                cnProduccion.Close();
                listViewBonoPorModulo.Clear();
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBonoPorModulo.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if (item.modart == comboBoxModulo.SelectedItem.ToString() + "-1" || item.modart == comboBoxModulo.SelectedItem.ToString() + "-2")
                        {
                            listViewBonoPorModulo.Add(item);
                        }
                    }
                }
                GridBono.Children.Clear();
                GridBono.Children.Add(new bonoPorModuloSemana(listViewBonoPorModulo));
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
                        bonoPorOperario.Add(new bonoPorOperario { asignado = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"] is DBNull ? 0 : dr["codigo"]), nombre = dr["nombre"].ToString(), bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C"), tiempoLunes = Convert.ToDouble(dr["tiempoLunes"] is DBNull ? 0 : dr["tiempoLunes"]), tiempoMartes = Convert.ToDouble(dr["tiempoMartes"] is DBNull ? 0 : dr["tiempoMartes"]), tiempoMiercoles = Convert.ToDouble(dr["tiempoMiercoles"] is DBNull ? 0 : dr["tiempoMiercoles"]), tiempoJueves = Convert.ToDouble(dr["tiempoJueves"] is DBNull ? 0 : dr["tiempoJueves"]), tiempoViernes = Convert.ToDouble(dr["tiempoViernes"] is DBNull ? 0 : dr["tiempoViernes"]), tiempoSabado = Convert.ToDouble(dr["tiempoSabado"] is DBNull ? 0 : dr["tiempoSabado"]), bp = Convert.ToDouble(dr["bp"] is DBNull ? 0 : dr["bp"]).ToString("P"), aqlG = Convert.ToDouble(dr["aqlG"] is DBNull ? 0 : dr["aqlG"]).ToString("P"), aqlI = Convert.ToDouble(dr["aqlI"] is DBNull ? 0 : dr["aqlI"]).ToString("P"), inasistencias = Convert.ToDouble(dr["inasistencias"] is DBNull ? 0 : dr["inasistencias"]).ToString("P"), amonestaciones = Convert.ToDouble(dr["amonestaciones"] is DBNull ? 0 : dr["amonestaciones"]).ToString("P"), bonoNeto = Convert.ToDouble(dr["bonoNeto"] is DBNull ? 0 : dr["bonoNeto"]).ToString("C") });
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
                sql = "SELECT [turno], [modart]," +
                    "[piezasLunes],[eficienciaLunes],[bonoLunes], [samLunes], [operariosLunes], [minutosTrabajadosLunes],[minutosDisponiblesLunes], " +
                    "[piezasMartes],[eficienciaMartes],[bonoMartes], [samMartes], [operariosMartes], [minutosTrabajadosMartes], [minutosDisponiblesMartes], " +
                    "[piezasMiercoles],[eficienciaMiercoles],[bonoMiercoles], [samMiercoles], [operariosMiercoles], [minutosTrabajadosMiercoles], [minutosDisponiblesMiercoles], " +
                    "[piezasJueves],[eficienciaJueves],[bonoJueves],[samJueves], [operariosJueves], [minutosTrabajadosJueves], [minutosDisponiblesJueves], " +
                    "[piezasViernes],[eficienciaViernes],[bonoViernes], [samViernes], [operariosViernes], [minutosTrabajadosViernes], [minutosDisponiblesViernes], " +
                    "[piezasSabado], [samSabado], [operariosSabado], [eficienciaSabado],[bonoSabado], [minutosTrabajadosSabado], [minutosDisponiblesSabado]," +
                    "[totalDePiezas], [samTotal], [eficienciaTotal], [operarios], [bono], [minutosTrabajados], [minutosDisponibles] FROM [produccion].[dbo].[bonoPorDiaSemana] " +
                    "where anio='" + anio + "' and semana='" + semana + "' order by turno";

                cnProduccion.Open();
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    bonoPorModulo.Add(new bonoPorModulo
                    {
                        turno = dr["turno"].ToString(),
                        modart = dr["modart"].ToString(),
                        piezasLunes = Convert.ToInt32(dr["piezasLunes"] is DBNull ? 0 : dr["piezasLunes"]),
                        piezasMartes = Convert.ToInt32(dr["piezasmartes"] is DBNull ? 0 : dr["piezasMartes"]),
                        piezasMiercoles = Convert.ToInt32(dr["piezasMiercoles"] is DBNull ? 0 : dr["piezasMiercoles"]),
                        piezasJueves = Convert.ToInt32(dr["piezasJueves"] is DBNull ? 0 : dr["piezasJueves"]),
                        piezasViernes = Convert.ToInt32(dr["piezasViernes"] is DBNull ? 0 : dr["piezasViernes"]),
                        piezasSabado = Convert.ToInt32(dr["piezasSabado"] is DBNull ? 0 : dr["piezasSabado"]),
                        bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"),
                        bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"),
                        bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"),
                        bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"),
                        bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"),
                        bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"),
                        bonoLunesD = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]),
                        bonoMartesD = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]),
                        bonoMiercolesD = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]),
                        bonoJuevesD = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]),
                        bonoViernesD = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]),
                        bonoSabadoD = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]),
                        eficienciaLunes = Convert.ToDouble(dr["eficienciaLunes"] is DBNull ? 0 : dr["eficienciaLunes"]).ToString("P"),
                        eficienciaMartes = Convert.ToDouble(dr["eficienciaMartes"] is DBNull ? 0 : dr["eficienciaMartes"]).ToString("P"),
                        eficienciaMiercoles = Convert.ToDouble(dr["eficienciaMiercoles"] is DBNull ? 0 : dr["eficienciaMiercoles"]).ToString("P"),
                        eficienciaJueves = Convert.ToDouble(dr["eficienciaJueves"] is DBNull ? 0 : dr["eficienciaJueves"]).ToString("P"),
                        eficienciaViernes = Convert.ToDouble(dr["eficienciaViernes"] is DBNull ? 0 : dr["eficienciaViernes"]).ToString("P"),
                        eficienciaSabado = Convert.ToDouble(dr["eficienciaSabado"] is DBNull ? 0 : dr["eficienciaSabado"]).ToString("P"),
                        operariosLunes = Convert.ToInt32(dr["operariosLunes"] is DBNull ? 0 : dr["operariosLunes"]),
                        operariosMartes = Convert.ToInt32(dr["operariosMartes"] is DBNull ? 0 : dr["operariosMartes"]),
                        operariosMiercoles = Convert.ToInt32(dr["operariosMiercoles"] is DBNull ? 0 : dr["operariosMiercoles"]),
                        operariosJueves = Convert.ToInt32(dr["operariosJueves"] is DBNull ? 0 : dr["operariosJueves"]),
                        operariosViernes = Convert.ToInt32(dr["operariosViernes"] is DBNull ? 0 : dr["operariosViernes"]),
                        operariosSabado = Convert.ToInt32(dr["operariosSabado"] is DBNull ? 0 : dr["operariosSabado"]),
                        samLunes = dr["samLunes"].ToString(),
                        samMartes = dr["samMartes"].ToString(),
                        samMiercoles = dr["samMiercoles"].ToString(),
                        samJueves = dr["samJueves"].ToString(),
                        samViernes = dr["samViernes"].ToString(),
                        samSabado = dr["samSabado"].ToString(),
                        mtLunes = Convert.ToDouble(dr["minutosTrabajadosLunes"]),
                        mtMartes = Convert.ToDouble(dr["minutosTrabajadosMartes"]),
                        mtMiercoles = Convert.ToDouble(dr["minutosTrabajadosMiercoles"]),
                        mtJueves = Convert.ToDouble(dr["minutosTrabajadosJueves"]),
                        mtViernes = Convert.ToDouble(dr["minutosTrabajadosViernes"]),
                        mtSabado = Convert.ToDouble(dr["minutosTrabajadosSabado"]),
                        mdLunes = Convert.ToDouble(dr["minutosDisponiblesLunes"]),
                        mdMartes = Convert.ToDouble(dr["minutosDisponiblesMartes"]),
                        mdMiercoles = Convert.ToDouble(dr["minutosDisponiblesMiercoles"]),
                        mdJueves = Convert.ToDouble(dr["minutosDisponiblesJueves"]),
                        mdViernes = Convert.ToDouble(dr["minutosDisponiblesViernes"]),
                        mdSabado = Convert.ToDouble(dr["minutosDisponiblesSabado"]),
                        totalDePiezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]),
                        samTotal = dr["samTotal"].ToString(),
                        eficienciaTotal = Convert.ToDouble(dr["eficienciaTotal"] is DBNull ? 0 : dr["eficienciaTotal"]).ToString("P"),
                        bono = Convert.ToDouble(dr["bono"] is DBNull ? 0 : dr["bono"]).ToString("C"),
                        bonoD = Convert.ToDouble(dr["bono"] is DBNull ? 0 : dr["bono"]),
                        operarios = Convert.ToInt32(dr["operarios"] is DBNull ? 0 : dr["operarios"]),
                        mt = Convert.ToDouble(dr["minutosTrabajados"]),
                        md = Convert.ToDouble(dr["minutosDisponibles"])
                    });
                }
                dr.Close();
                cnProduccion.Close();
                listViewBonoPorModulo.Clear();
                if (comboBoxModulo.SelectedIndex == -1)
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBonoPorModulo.Add(item);
                    }
                }
                else
                {
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if (item.modart == comboBoxModulo.SelectedItem.ToString() + "-1" || item.modart == comboBoxModulo.SelectedItem.ToString() + "-2" || item.modart == comboBoxModulo.SelectedItem.ToString() + "-3")
                        {
                            listViewBonoPorModulo.Add(item);
                        }
                    }
                }
                GridBono.Children.Clear();
                GridBono.Children.Add(new bonoPorModuloSemana(listViewBonoPorModulo));
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
                        bonoPorOperario.Add(new bonoPorOperario { asignado = dr["asignado"].ToString(), codigo = Convert.ToInt32(dr["codigo"] is DBNull? 0: dr["codigo"]), nombre = dr["nombre"].ToString(), bonoLunes = Convert.ToDouble(dr["bonoLunes"] is DBNull ? 0 : dr["bonoLunes"]).ToString("C"), bonoMartes = Convert.ToDouble(dr["bonoMartes"] is DBNull ? 0 : dr["bonoMartes"]).ToString("C"), bonoMiercoles = Convert.ToDouble(dr["bonoMiercoles"] is DBNull ? 0 : dr["bonoMiercoles"]).ToString("C"), bonoJueves = Convert.ToDouble(dr["bonoJueves"] is DBNull ? 0 : dr["bonoJueves"]).ToString("C"), bonoViernes = Convert.ToDouble(dr["bonoViernes"] is DBNull ? 0 : dr["bonoViernes"]).ToString("C"), bonoSabado = Convert.ToDouble(dr["bonoSabado"] is DBNull ? 0 : dr["bonoSabado"]).ToString("C"), bonoBruto = Convert.ToDouble(dr["bonoBruto"] is DBNull ? 0 : dr["bonoBruto"]).ToString("C"), tiempoLunes = Convert.ToDouble(dr["tiempoLunes"] is DBNull ? 0 : dr["tiempoLunes"]), tiempoMartes = Convert.ToDouble(dr["tiempoMartes"] is DBNull ? 0 : dr["tiempoMartes"]), tiempoMiercoles = Convert.ToDouble(dr["tiempoMiercoles"] is DBNull ? 0 : dr["tiempoMiercoles"]), tiempoJueves = Convert.ToDouble(dr["tiempoJueves"] is DBNull ? 0 : dr["tiempoJueves"]), tiempoViernes = Convert.ToDouble(dr["tiempoViernes"] is DBNull ? 0 : dr["tiempoViernes"]), tiempoSabado = Convert.ToDouble(dr["tiempoSabado"] is DBNull ? 0 : dr["tiempoSabado"]), bp = Convert.ToDouble(dr["bp"] is DBNull ? 0 : dr["bp"]).ToString("P"), aqlG = Convert.ToDouble(dr["aqlG"] is DBNull ? 0 : dr["aqlG"]).ToString("P"), aqlI = Convert.ToDouble(dr["aqlI"] is DBNull ? 0 : dr["aqlI"]).ToString("P"), inasistencias = Convert.ToDouble(dr["inasistencias"] is DBNull ? 0 : dr["inasistencias"]).ToString("P"), amonestaciones = Convert.ToDouble(dr["amonestaciones"] is DBNull ? 0 : dr["amonestaciones"]).ToString("P"), bonoNeto = Convert.ToDouble(dr["bonoNeto"] is DBNull ? 0 : dr["bonoNeto"]).ToString("C") });
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
                    listViewBonoPorModulo.Clear();
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        if (item.modart == comboBoxModulo.SelectedItem.ToString()+"-1"|| item.modart == comboBoxModulo.SelectedItem.ToString() + "-2")

                        {
                            listViewBonoPorModulo.Add(item);
                        }
                    }

                    GridBono.Children.Clear();
                    GridBono.Children.Add(new bonoPorModuloSemana(listViewBonoPorModulo));
                }

                else if (comboBoxModulo.SelectedIndex == -1)
                {
                    listViewBonoPorModulo.Clear();
                    foreach (bonoPorModulo item in bonoPorModulo)
                    {
                        listViewBonoPorModulo.Add(item);
                    }
                    GridBono.Children.Clear();
                    GridBono.Children.Add(new bonoPorModuloSemana(listViewBonoPorModulo));
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
                buffer.Append("SAM LU");
                buffer.Append(",");
                buffer.Append("% LU");
                buffer.Append(",");
                buffer.Append("$ LU");
                buffer.Append(",");
                buffer.Append("OPERARIOS LU");
                buffer.Append(",");
                buffer.Append("PIEZAS MA");
                buffer.Append(",");
                buffer.Append("SAM MA");
                buffer.Append(",");
                buffer.Append("% MA");
                buffer.Append(",");
                buffer.Append("$ MA");
                buffer.Append(",");
                buffer.Append("OPERARIOS MA");
                buffer.Append(",");
                buffer.Append("PIEZAS MI");
                buffer.Append(",");
                buffer.Append("SAM MI");
                buffer.Append(",");
                buffer.Append("% MI");
                buffer.Append(",");
                buffer.Append("$ MI");
                buffer.Append(",");
                buffer.Append("OPERARIOS MI");
                buffer.Append(",");
                buffer.Append("PIEZAS JU");
                buffer.Append(",");
                buffer.Append("SAM JU");
                buffer.Append(",");
                buffer.Append("% JU");
                buffer.Append(",");
                buffer.Append("$ JU");
                buffer.Append(",");
                buffer.Append("OPERARIOS JU");
                buffer.Append(",");
                buffer.Append("PIEZAS VI");
                buffer.Append(",");
                buffer.Append("SAM VI");
                buffer.Append(",");
                buffer.Append("% VI");
                buffer.Append(",");
                buffer.Append("$ VI");
                buffer.Append(",");
                buffer.Append("OPERARIOS VI");
                buffer.Append(",");
                buffer.Append("PIEZAS SA");
                buffer.Append(",");
                buffer.Append("SAM SA");
                buffer.Append(",");
                buffer.Append("% SA");
                buffer.Append(",");
                buffer.Append("$ SA");
                buffer.Append(",");
                buffer.Append("OPERARIOS SA");
                buffer.Append(",");
                buffer.Append("TOTAL PIEZAS");
                buffer.Append(",");
                buffer.Append("SAM TOTAL");
                buffer.Append(",");
                buffer.Append("EFICIENCIA TOTAL");
                buffer.Append(",");
                buffer.Append("TOTAL BONO");
                buffer.Append(",");
                buffer.Append("OPERARIOS");
                buffer.Append("\n");
                #endregion
                foreach (bonoPorModulo item in listViewBonoPorModulo)
                {
                    buffer.Append(item.turno);
                    buffer.Append(",");
                    buffer.Append(item.modart);
                    buffer.Append(",");
                    buffer.Append(item.piezasLunes);
                    buffer.Append(",");
                    buffer.Append(item.samLunes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaLunes);
                    buffer.Append(",");
                    buffer.Append(item.bonoLunes);
                    buffer.Append(",");
                    buffer.Append(item.operariosLunes);
                    buffer.Append(",");
                    buffer.Append(item.piezasMartes);
                    buffer.Append(",");
                    buffer.Append(item.samMartes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaMartes);
                    buffer.Append(",");
                    buffer.Append(item.bonoMartes);
                    buffer.Append(",");
                    buffer.Append(item.operariosMartes);
                    buffer.Append(",");
                    buffer.Append(item.piezasMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.samMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.bonoMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.operariosMiercoles);
                    buffer.Append(",");
                    buffer.Append(item.piezasJueves);
                    buffer.Append(",");
                    buffer.Append(item.samJueves);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaJueves);
                    buffer.Append(",");
                    buffer.Append(item.bonoJueves);
                    buffer.Append(",");
                    buffer.Append(item.operariosJueves);
                    buffer.Append(",");
                    buffer.Append(item.piezasViernes);
                    buffer.Append(",");
                    buffer.Append(item.samViernes);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaViernes);
                    buffer.Append(",");
                    buffer.Append(item.bonoViernes);
                    buffer.Append(",");
                    buffer.Append(item.operariosViernes);
                    buffer.Append(",");
                    buffer.Append(item.piezasSabado);
                    buffer.Append(",");
                    buffer.Append(item.samSabado);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaSabado);
                    buffer.Append(",");
                    buffer.Append(item.bonoSabado);
                    buffer.Append(",");
                    buffer.Append(item.operariosSabado);
                    buffer.Append(",");
                    buffer.Append(item.totalDePiezas);
                    buffer.Append(",");
                    buffer.Append(item.samTotal);
                    buffer.Append(",");
                    buffer.Append(item.eficienciaTotal);
                    buffer.Append(",");
                    buffer.Append(item.bono);
                    buffer.Append(",");
                    buffer.Append(item.operarios);
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
