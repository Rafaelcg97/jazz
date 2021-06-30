using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class cuadrarLotes : Page
    {
        #region stringsGenerales
        List<horaProduccion> listaTodosLotes = new List<horaProduccion>();
        List<horaProduccion> lotesPorModulo = new List<horaProduccion>();
        List<loteConfirma> lotesConfirma = new List<loteConfirma>();
        SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        public cuadrarLotes()
        {
            InitializeComponent();
            string sql = "select lote, xxs, xs, s, m, l, xl, xxl, xxxl, totalDePiezas, make, totalDePiezas-make as diferencia from lotesAgrupados";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                listaTodosLotes.Add(new horaProduccion { lote = dr["lote"].ToString(), xxs = Convert.ToInt32(dr["xxs"] is DBNull ? 0 :dr["xxs"]), xs = Convert.ToInt32(dr["xs"] is DBNull ? 0 : dr["xs"]), s = Convert.ToInt32(dr["s"] is DBNull ? 0 :dr["s"]), m = Convert.ToInt32(dr["m"] is DBNull ? 0 : dr["m"]), l = Convert.ToInt32(dr["l"] is DBNull ? 0 : dr["l"]), xl = Convert.ToInt32(dr["xl"] is DBNull ? 0 :dr["xl"]), xxl = Convert.ToInt32(dr["xxl"] is DBNull ? 0 : dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"] is DBNull ? 0 : dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]), make= Convert.ToInt32(dr["make"] is DBNull ? 0 : dr["make"]), diferencia= Convert.ToInt32(dr["diferencia"] is DBNull ? 0 : dr["diferencia"]) });
            };
            //se termina la conexion a la base
            dr.Close();

            sql = "select modulo from modulosProduccion";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
                comboBoxModuloConfirm.Items.Add(dr["modulo"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();

            sql = "select ano from semana_de_produccion";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            List<int> anios = new List<int>();
            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                anios.Add(Convert.ToInt32(dr["ano"]));
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
            foreach(horaProduccion item in listaTodosLotes)
            {
                listViewLotesLista.Items.Add(item);
            }
            IEnumerable<int> aniosUnicos = anios.Distinct();
            foreach(int item in aniosUnicos)
            {
                comboBoxAnio.Items.Add(item);
            }
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
        #region controlesDeConsulta
        private void textBoxLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewLotesLista.Items.Clear();
            listViewLotesDetalles.Items.Clear();
            if (String.IsNullOrEmpty(textBoxLote.Text.Trim()) == false)
            {
                foreach (horaProduccion item in listaTodosLotes)
                {
                    if (item.lote.StartsWith(textBoxLote.Text.Trim()))
                    {
                        listViewLotesLista.Items.Add(item);
                    }
                }
            }
            else if (textBoxLote.Text.Trim() == "")
            {
                foreach (horaProduccion item in listaTodosLotes)
                {
                    listViewLotesLista.Items.Add(item);
                }
            }
        }
        private void listViewLotesLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewLotesLista.SelectedIndex > -1)
            {
                string loteSeleccionado = ((horaProduccion)listViewLotesLista.SelectedItem).lote;
                listViewLotesDetalles.Items.Clear();
                string sql = "select fecha, modulo, [2XS] as xxs, xs, s, m, l, xl, [2XL] as xxl, [3XL] as xxxl, totalDePiezas, Coordinador from horahora where lote='" + loteSeleccionado + "'";
                cnProduccion.Open();
                SqlCommand cm = new SqlCommand(sql, cnProduccion);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewLotesDetalles.Items.Add(new horaProduccion { fecha= Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), modulo=dr["modulo"].ToString(), xxs = Convert.ToInt32(dr["xxs"] is DBNull ? 0 : dr["xxs"]), xs = Convert.ToInt32(dr["xs"] is DBNull ? 0 : dr["xs"]), s = Convert.ToInt32(dr["s"] is DBNull ? 0 : dr["s"]), m = Convert.ToInt32(dr["m"] is DBNull ? 0 : dr["m"]), l = Convert.ToInt32(dr["l"] is DBNull ? 0 : dr["l"]), xl = Convert.ToInt32(dr["xl"] is DBNull ? 0 : dr["xl"]), xxl = Convert.ToInt32(dr["xxl"] is DBNull ? 0 : dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"] is DBNull ? 0 : dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]), coordinadorNombre=dr["Coordinador"].ToString() });
                };
                //se termina la conexion a la base
                dr.Close();
                cnProduccion.Close();
            }
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxLotePorModulo.Clear();
            listViewLotesPorModulo.Items.Clear();
            lotesPorModulo.Clear();
            string sql = "SELECT Modulo, Lote, SUM([2XS]) AS xxs, SUM(XS) AS xs, SUM(S) AS s, SUM(M) AS m, SUM(L) AS l, SUM(XL) AS xl, SUM([2XL]) AS xxl, SUM([3XL]) AS xxxl, SUM(TotalDePiezas) AS totalDePiezas FROM dbo.horahora where Modulo='"+comboBoxModulo.SelectedItem.ToString()+"' GROUP BY Lote, Modulo";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                lotesPorModulo.Add(new horaProduccion { modulo=dr["modulo"].ToString(), lote = dr["lote"].ToString(), xxs = Convert.ToInt32(dr["xxs"] is DBNull ? 0 : dr["xxs"]), xs = Convert.ToInt32(dr["xs"] is DBNull ? 0 : dr["xs"]), s = Convert.ToInt32(dr["s"] is DBNull ? 0 : dr["s"]), m = Convert.ToInt32(dr["m"] is DBNull ? 0 : dr["m"]), l = Convert.ToInt32(dr["l"] is DBNull ? 0 : dr["l"]), xl = Convert.ToInt32(dr["xl"] is DBNull ? 0 : dr["xl"]), xxl = Convert.ToInt32(dr["xxl"] is DBNull ? 0 : dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"] is DBNull ? 0 : dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 :dr["totalDePiezas"]) });
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
            foreach(horaProduccion item in lotesPorModulo)
            {
                listViewLotesPorModulo.Items.Add(item);
            }
        }
        private void textBoxLotePorModulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewLotesPorModulo.Items.Clear();
            if (String.IsNullOrEmpty(textBoxLotePorModulo.Text.Trim()) == false)
            {
                foreach (horaProduccion item in lotesPorModulo)
                {
                    if (item.lote.StartsWith(textBoxLotePorModulo.Text.Trim()))
                    {
                        listViewLotesPorModulo.Items.Add(item);
                    }
                }
            }
            else if (textBoxLotePorModulo.Text.Trim() == "")
            {
                foreach (horaProduccion item in lotesPorModulo)
                {
                    listViewLotesPorModulo.Items.Add(item);
                }
            }

        }
        #endregion
        #region lotesConfirmados
        private void comboBoxAnio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql = "select semana from semana_de_produccion where ano="+Convert.ToInt32(comboBoxAnio.SelectedItem)+" order by semana desc";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            comboBoxSemana.Items.Clear();
            // se llena la semana de ese anio
            while (dr.Read())
            {
                comboBoxSemana.Items.Add(Convert.ToInt32(dr["semana"]));
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();

            consultarDatosDeConfirma();
            listViewLotesConfirmados.Items.Clear();
            textBoxLoteConfirm.Clear();
            comboBoxModuloConfirm.SelectedIndex = -1;
            foreach(loteConfirma item in lotesConfirma)
            {
                if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem))
                {
                    listViewLotesConfirmados.Items.Add(item);
                }
            }
        }
        private void consultarDatosDeConfirma()
        {
            lotesConfirma.Clear();
            string sql = "select modulo, coordinadorNombre, lote, confirmacion, targetDate, poNumber, customer, seasonCode, styleColorName, make from confirmaPoly order by coordinadorNombre, modulo, confirmacion";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string fechaConfirm = Regex.Replace(dr["confirmacion"].ToString(), @"\s", "");
                try
                {
                    int dia_ = (int)Convert.ToDateTime(fechaConfirm).DayOfWeek;
                    int anio_ = Convert.ToDateTime(fechaConfirm).Year;
                    int semana_ = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(Convert.ToDateTime(fechaConfirm), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    if (dia_== 1 || dia_ == 2|| dia_ == 3) 
                    {
                        semana_ = semana_ - 1;
                    }
                    lotesConfirma.Add(new loteConfirma { modulo = dr["modulo"].ToString(), coordinador = dr["coordinadorNombre"].ToString(), numeroLote = dr["lote"].ToString(), anio=anio_, semana=semana_, confirmacion = Convert.ToDateTime(fechaConfirm).ToString("yyyy-MM-dd"), targetDate = Convert.ToDateTime(dr["targetDate"]).ToString("yyyy-MM-dd"), poNumber = dr["poNumber"].ToString(), cliente = dr["customer"].ToString(), temporada = dr["seasonCode"].ToString(), color = dr["styleColorName"].ToString(), piezas = Convert.ToInt32(dr["make"] is DBNull ? 0 : dr["make"]) });
                }
                catch
                {
                }
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
        }
        private void textBoxLoteConfirm_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewLotesConfirmados.Items.Clear();
            if(comboBoxAnio.SelectedIndex>-1 & comboBoxSemana.SelectedIndex>-1 & comboBoxModuloConfirm.SelectedIndex > -1)
            {
                if (String.IsNullOrEmpty(textBoxLoteConfirm.Text.Trim()) == false)
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.numeroLote.StartsWith(textBoxLoteConfirm.Text.Trim()) & item.anio==Convert.ToInt32(comboBoxAnio.SelectedItem) & item.semana==Convert.ToInt32(comboBoxSemana.SelectedItem) & item.modulo==comboBoxModuloConfirm.SelectedItem.ToString())
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
                else if (textBoxLoteConfirm.Text.Trim() == "")
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) & item.semana == Convert.ToInt32(comboBoxSemana.SelectedItem) & item.modulo == comboBoxModuloConfirm.SelectedItem.ToString())
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
            }
            else if(comboBoxAnio.SelectedIndex > -1 & comboBoxSemana.SelectedIndex > -1)
            {
                if (String.IsNullOrEmpty(textBoxLoteConfirm.Text.Trim()) == false)
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.numeroLote.StartsWith(textBoxLoteConfirm.Text.Trim()) & item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) & item.semana == Convert.ToInt32(comboBoxSemana.SelectedItem))
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
                else if (textBoxLoteConfirm.Text.Trim() == "")
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) & item.semana == Convert.ToInt32(comboBoxSemana.SelectedItem))
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
            }
            else if (comboBoxAnio.SelectedIndex > -1)
            {
                if (String.IsNullOrEmpty(textBoxLoteConfirm.Text.Trim()) == false)
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.numeroLote.StartsWith(textBoxLoteConfirm.Text.Trim()) & item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem))
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
                else if (textBoxLoteConfirm.Text.Trim() == "")
                {
                    foreach (loteConfirma item in lotesConfirma)
                    {
                        if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem))
                        {
                            listViewLotesConfirmados.Items.Add(item);
                        }
                    }
                }
            }
        }
        private void comboBoxSemana_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewLotesConfirmados.Items.Clear();
            textBoxLoteConfirm.Clear();
            if (comboBoxAnio.SelectedIndex > -1 && comboBoxSemana.SelectedIndex>-1 && comboBoxModuloConfirm.SelectedIndex==-1)
            {
                foreach (loteConfirma item in lotesConfirma)
                {
                    if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) && item.semana==Convert.ToInt32(comboBoxSemana.SelectedItem))
                    {
                        listViewLotesConfirmados.Items.Add(item);
                    }
                }
            }
            else if (comboBoxAnio.SelectedIndex > -1 && comboBoxSemana.SelectedIndex > -1 && comboBoxModuloConfirm.SelectedIndex>-1)
            {
                foreach (loteConfirma item in lotesConfirma)
                {
                    if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) && item.semana == Convert.ToInt32(comboBoxSemana.SelectedItem) && item.modulo == comboBoxModuloConfirm.SelectedItem.ToString())
                    {
                        listViewLotesConfirmados.Items.Add(item);
                    }
                }
            }
        }
        private void comboBoxModuloConfirm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewLotesConfirmados.Items.Clear();
            textBoxLoteConfirm.Clear();
            if (comboBoxAnio.SelectedIndex > -1 && comboBoxSemana.SelectedIndex > -1 && comboBoxModuloConfirm.SelectedIndex > -1)
            {
                foreach (loteConfirma item in lotesConfirma)
                {
                    if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) & item.semana == Convert.ToInt32(comboBoxSemana.SelectedItem) & item.modulo == comboBoxModuloConfirm.SelectedItem.ToString())
                    {
                        listViewLotesConfirmados.Items.Add(item);
                    }
                }
            }
            else if (comboBoxAnio.SelectedIndex > -1 && comboBoxSemana.SelectedIndex == -1 && comboBoxModuloConfirm.SelectedIndex> -1)
            {
                foreach (loteConfirma item in lotesConfirma)
                {
                    if (item.anio == Convert.ToInt32(comboBoxAnio.SelectedItem) & item.modulo == comboBoxModuloConfirm.SelectedItem.ToString())
                    {
                        listViewLotesConfirmados.Items.Add(item);
                    }
                }
            }
        }
        private void buttonDescargarConfirm_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("AÑO");
            buffer.Append(",");
            buffer.Append("SEMANA");
            buffer.Append(",");
            buffer.Append("MODULO");
            buffer.Append(",");
            buffer.Append("COORDINADOR");
            buffer.Append(",");
            buffer.Append("LOTE");
            buffer.Append(",");
            buffer.Append("CONFIRMACION");
            buffer.Append(",");
            buffer.Append("TARGET");
            buffer.Append(",");
            buffer.Append("PO NUMBER");
            buffer.Append(",");
            buffer.Append("CLIENTE");
            buffer.Append(",");
            buffer.Append("TEMPORADA");
            buffer.Append(",");
            buffer.Append("COLOR");
            buffer.Append(",");
            buffer.Append("MAKE");
            buffer.Append("\n");
            #endregion
            foreach (loteConfirma item in listViewLotesConfirmados.Items)
            {
                buffer.Append(item.anio);
                buffer.Append(",");
                buffer.Append(item.semana);
                buffer.Append(",");
                buffer.Append(item.modulo);
                buffer.Append(",");
                buffer.Append(item.coordinador);
                buffer.Append(",");
                buffer.Append(item.numeroLote);
                buffer.Append(",");
                buffer.Append(item.confirmacion);
                buffer.Append(",");
                buffer.Append(item.targetDate);
                buffer.Append(",");
                buffer.Append(item.poNumber);
                buffer.Append(",");
                buffer.Append(item.cliente);
                buffer.Append(",");
                buffer.Append(item.temporada);
                buffer.Append(",");
                buffer.Append(item.color);
                buffer.Append(",");
                buffer.Append(item.piezas);
                buffer.Append(",");
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
        #endregion

        private void buttonBuscar_Click(object sender, RoutedEventArgs e)
        {
            listViewPromedioPorEstilo.Items.Clear();
            string sql = "select modulo, estilo, temporada, round(avg(operarios),0) as operarios, AVG(piezas) as promedioPiezas, round(STDEVP(Piezas),4) as desviacion " +
                "from (select fecha, modulo, hora, estilo, temporada, avg(operarios) as operarios, sum(totalDePiezas) as piezas from horahora where estilo like '%" + textBoxBuscarEstilooo.Text +"%'"+
                "group by Fecha, modulo, hora, estilo, temporada) as a " +
                "group by a.estilo, a.temporada, a.Modulo " +
                "order by modulo, estilo, temporada, promedioPiezas ";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan los modulos que han cosido el estilo
            while (dr.Read())
            {
               listViewPromedioPorEstilo.Items.Add(new horaProduccion 
               {
                   modulo= dr["modulo"].ToString(), 
                   estilo = dr["estilo"].ToString(),
                   temporada= dr["temporada"].ToString(),
                   piezas=Convert.ToInt32(dr["promedioPiezas"] is DBNull ? 0 : dr["promedioPiezas"]),
                   sam= Convert.ToDouble(dr["desviacion"])
               });
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
        }
        private void buttonLimpiar_Click(object sender, RoutedEventArgs e)
        {
            textBoxBuscarEstilooo.Clear();
            listViewPromedioPorEstilo.Items.Clear();
            listViewDetallesEstilo.Items.Clear();
        }
        private void listViewPromedioPorEstilo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            horaProduccion moduloSeleccionado = (horaProduccion)listViewPromedioPorEstilo.SelectedItem;
            listViewDetallesEstilo.Items.Clear();
            string sql = "select fecha, hora, estilo, empaque, sam, operarios, TotalDePiezas " +
                "from horahora where modulo='"+ moduloSeleccionado.modulo +"' and estilo='"+ moduloSeleccionado.estilo +"' and temporada='"+ moduloSeleccionado.temporada +"' order by fecha, hora";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan los modulos que han cosido el estilo
            while (dr.Read())
            {
               listViewDetallesEstilo.Items.Add(new horaProduccion
                {
                    fecha = Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"),
                    hora = Convert.ToInt32(dr["hora"] is DBNull ? 0 : dr["hora"]),
                    estilo = dr["estilo"].ToString(),
                    empaque =dr["empaque"].ToString(),
                    sam = Convert.ToDouble(dr["sam"] is DBNull ? 0 : dr["sam"]),
                    opeCostura = Convert.ToInt32(dr["operarios"] is DBNull ? 0 : dr["operarios"]),
                    piezas = Convert.ToInt32(dr["TotalDePiezas"] is DBNull ? 0 : dr["TotalDePiezas"]),
                });
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
        }
    }
}
