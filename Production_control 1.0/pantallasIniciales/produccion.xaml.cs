using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class produccion : UserControl
    {
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
            #region cargarListaCoordinadores
            comboBoxCoordinadorNombre.Items.Add("-");
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select distinct nombre from usuarios where cargo='COORDINADOR' and produccion=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxCoordinadorNombre.Items.Add(dr["nombre"].ToString());
            };
            dr.Close();
            cn.Close();
            comboBoxCoordinadorNombre.SelectedIndex = 0;
            #endregion
            #region cargarListaModulos
            comboBoxCoordinadorNombre.Items.Add("-");
            cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            sql = "select modulo from orden_modulos";
            cn.Open();
            cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            comboBoxModulo.Items.Add("-");
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cn.Close();
            comboBoxCoordinadorNombre.SelectedIndex = 0;
            #endregion
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

            #region datosOlo
            if (tabControlInicio.SelectedIndex == 2)
            {
                listViewProduccionPorModulo.Items.Clear();
                listViewProduccionPorSemana.Items.Clear();
                listViewProduccionLote.Items.Clear();
                int totalPiezas = 0;
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_olocuilta"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select fecha, modulo2, piezas2 from produccion2 where fecha='" + fecha + "'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewProduccionPorModulo.Items.Add(new elemento_grafica { modulo = dr["modulo2"].ToString(), piezas = Convert.ToInt32(dr["piezas2"]), coordinador = dr["fecha"].ToString() });
                    totalPiezas = totalPiezas + Convert.ToInt32(dr["piezas2"]);
                };
                dr.Close();
                sql = "select ano, semana, produccion from produccion4 order by ano desc, semana desc";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewProduccionPorSemana.Items.Add(new elemento_grafica { h1 = Convert.ToInt32(dr["ano"]), h2 = Convert.ToInt32(dr["semana"]), piezas = Convert.ToInt32(dr["produccion"]) });
                };
                dr.Close();
                sql = "select lote3, piezas3 from produccion3 order by lote3 desc";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewProduccionLote.Items.Add(new elemento_grafica { coordinador = dr["lote3"].ToString(), piezas = Convert.ToInt32(dr["piezas3"]) });
                };
                dr.Close();
                cn.Close();
                labelTotalPiezasOlocuilta.Content = totalPiezas;
            }
            #endregion

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
            if (comboBoxCoordinadorNombre.SelectedIndex>0)
            {
                sql = "select modart, coordinador, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12, totalDePiezas, minutosTrabajados, minutosDisponibles, eficiencia from vistaKPI where fecha='" + fecha + "' and turno='" + turno +  "' and coordinador='"+ comboBoxCoordinadorNombre.SelectedItem.ToString() + "'";
            }
            else
            {
               sql = "select modart, coordinador, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12, totalDePiezas, minutosTrabajados, minutosDisponibles, eficiencia from vistaKPI where fecha='" + fecha + "' and turno='" + turno + "' order by coordinador";
            }
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulos.Add(dr["modart"].ToString());
                totalPiezas = totalPiezas + Convert.ToInt32(dr["totalDePiezas"] is DBNull? 0: dr["totalDePiezas"]);
                trabajado = trabajado + Convert.ToDouble(dr["minutosTrabajados"] is DBNull? 0: dr["minutosTrabajados"]);
                disponible = disponible + Convert.ToDouble(dr["minutosDisponibles"] is DBNull? 0: dr["minutosDisponibles"]);
                modulosProduccionEficiencia.Add(new elemento_grafica { modulo = dr["modart"].ToString(), eficiencia = Convert.ToDouble(dr["eficiencia"] is DBNull? 0: dr["eficiencia"]), piezas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]), coordinador = dr["coordinador"].ToString(), h1 = Convert.ToInt32(dr["H1"] is DBNull ? 0 : dr["H1"]), h2 = Convert.ToInt32(dr["H2"] is DBNull ? 0 :dr["H2"]), h3 = Convert.ToInt32(dr["H3"] is DBNull ? 0 : dr["H3"]), h4 = Convert.ToInt32(dr["H4"] is DBNull ? 0 : dr["H4"]), h5 = Convert.ToInt32(dr["H5"] is DBNull ? 0 : dr["H5"]), h6 = Convert.ToInt32(dr["H6"] is DBNull ? 0 : dr["H6"]), h7 = Convert.ToInt32(dr["H7"] is DBNull ? 0 : dr["H7"]), h8 = Convert.ToInt32(dr["H8"] is DBNull ? 0 : dr["H8"]), h9 = Convert.ToInt32(dr["H9"] is DBNull ? 0 : dr["H9"]), h10 = Convert.ToInt32(dr["H10"] is DBNull ? 0 : dr["H10"]), h11 = Convert.ToInt32(dr["H11"] is DBNull ? 0 : dr["H11"]), h12 = Convert.ToInt32(dr["H12"] is DBNull ? 0 : dr["H12"]) });
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
            labelTotalPiezas.Content = totalPiezas;
            labelTotalEficiencia.Content = (trabajado / disponible).ToString("P");
            gridProduccion.Children.Clear();
            gridProduccion.Children.Add(new produccionHora(modulosProduccionEficiencia));
        }
        #endregion
        private void comboBoxCoordinadorNombre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            actualizarGrafica("Diurno", fecha);

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
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlInicio.SelectedIndex == 2)
            {
                listViewProduccionPorModulo.Items.Clear();
                listViewProduccionPorSemana.Items.Clear();
                listViewProduccionLote.Items.Clear();
                int totalPiezas = 0;
                string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
                if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
                {
                    fecha = DateTime.Now.ToString("yyyy-MM-dd");
                }
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_olocuilta"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select fecha, modulo2, piezas2 from produccion2 where fecha='" + fecha + "'" ;
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewProduccionPorModulo.Items.Add(new elemento_grafica { modulo = dr["modulo2"].ToString(), piezas = Convert.ToInt32(dr["piezas2"]), coordinador = dr["fecha"].ToString() });
                    totalPiezas = totalPiezas + Convert.ToInt32(dr["piezas2"]);
                };
                dr.Close();
                sql = "select ano, semana, produccion from produccion4 order by ano desc, semana desc";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                   listViewProduccionPorSemana.Items.Add(new elemento_grafica { h1 = Convert.ToInt32(dr["ano"]), h2= Convert.ToInt32(dr["semana"]), piezas = Convert.ToInt32(dr["produccion"])});
                };
                dr.Close();
                sql = "select lote3, piezas3 from produccion3 order by lote3 desc";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewProduccionLote.Items.Add(new elemento_grafica { coordinador = dr["lote3"].ToString(), piezas = Convert.ToInt32(dr["piezas3"]) });
                };
                dr.Close();
                cn.Close();
                labelTotalPiezasOlocuilta.Content = totalPiezas;
            }
            if (tabControlInicio.SelectedIndex == 3)
            {
                if (comboBoxModulo.SelectedIndex > -1)
                {
                    consultarProgra(comboBoxModulo.SelectedItem.ToString());
                }
                else
                {
                    consultarProgra();
                }
            }
        }
        private void textBoxLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewProduccionLote.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_olocuilta"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select lote3, piezas3 from produccion3 where lote3 like '%" + textBoxLote.Text +"%'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewProduccionLote.Items.Add(new elemento_grafica { coordinador = dr["lote3"].ToString(), piezas = Convert.ToInt32(dr["piezas3"]) });
            };
            dr.Close();
            cn.Close();
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlInicio.SelectedIndex == 3)
            {
                if (comboBoxModulo.SelectedIndex > -1)
                {
                    consultarProgra(comboBoxModulo.SelectedItem.ToString());
                }
                else
                {
                    consultarProgra();
                }
            }
        }
        private void buttonDescargar_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("MODULO");
            buffer.Append(",");
            buffer.Append("SCH START");
            buffer.Append(",");
            buffer.Append("ESTATUS");
            buffer.Append(",");
            buffer.Append("TARGET DATE");
            buffer.Append(",");
            buffer.Append("MO CUT");
            buffer.Append(",");
            buffer.Append("PO NUMBER");
            buffer.Append(",");
            buffer.Append("STYLE NUMBER");
            buffer.Append(",");
            buffer.Append("STYLE NAME");
            buffer.Append(",");
            buffer.Append("STYLE COLOR NAME");
            buffer.Append(",");
            buffer.Append("TIPO EMPAQUE");
            buffer.Append(",");
            buffer.Append("PIEZAS DE EMPAQUE");
            buffer.Append(",");
            buffer.Append("TEMPORADA");
            buffer.Append(",");
            buffer.Append("CLIENTE");
            buffer.Append(",");
            buffer.Append("MAKE");
            buffer.Append(",");
            buffer.Append("TERMINADAS");
            buffer.Append("\n");
            #endregion
            foreach (loteProgramacion item in listViewCumplimientoProgra.Items)
            {
                buffer.Append(item.modulo);
                buffer.Append(",");
                buffer.Append(item.SchStart);
                buffer.Append(",");
                buffer.Append(item.estatus);
                buffer.Append(",");
                buffer.Append(item.targetDate);
                buffer.Append(",");
                buffer.Append(item.MOCut);
                buffer.Append(",");
                buffer.Append(item.PONumber);
                buffer.Append(",");
                buffer.Append(item.StyleNumber);
                buffer.Append(",");
                buffer.Append(item.StyleName);
                buffer.Append(",");
                buffer.Append(item.StyleColorName);
                buffer.Append(",");
                buffer.Append(item.tipoEmpaque);
                buffer.Append(",");
                buffer.Append(item.packQuantity);
                buffer.Append(",");
                buffer.Append(item.SeasonCode);
                buffer.Append(",");
                buffer.Append(item.CompanyNumber);
                buffer.Append(",");
                buffer.Append(item.QuantityOrdered);
                buffer.Append(",");
                buffer.Append(item.terminadas);
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
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void consultarProgra(string modulo="")
        {
            listViewCumplimientoProgra.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sqlp = "";
            if (modulo == "" || modulo=="-")
            {
                sqlp = "select modulo, SchStart, estatus, targetDate, MOCut, PONumber, StyleNumber, StyleName, " +
                       "StyleColorName, tipoEmpaque, packQuantity, SeasonCode, CompanyNumber, QuantityOrdered, terminadas " +
                       "from programacionPoly order by modulo, SchStart, StartSequence";
            }
            else
            {
                sqlp = "select modulo, SchStart, estatus, targetDate, MOCut, PONumber, StyleNumber, StyleName, " +
                "StyleColorName, tipoEmpaque, packQuantity, SeasonCode, CompanyNumber, QuantityOrdered, terminadas " +
                "from programacionPoly where modulo='" + modulo + "' order by modulo, SchStart, StartSequence";

            }

            cn.Open();
            SqlCommand cm = new SqlCommand(sqlp, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewCumplimientoProgra.Items.Add(new loteProgramacion
                {
                    modulo = dr["modulo"].ToString(),
                    SchStart = Convert.ToDateTime(dr["SchStart"]).ToString("yyyy-MM-dd"),
                    estatus = dr["estatus"].ToString(),
                    targetDate = Convert.ToDateTime(dr["targetDate"]).ToString("yyyy-MM-dd"),
                    MOCut = dr["MOCut"].ToString(),
                    PONumber = dr["PONumber"].ToString(),
                    StyleNumber = dr["StyleNumber"].ToString(),
                    StyleName = dr["StyleName"].ToString(),
                    StyleColorName = dr["StyleColorName"].ToString(),
                    tipoEmpaque = dr["tipoEmpaque"].ToString(),
                    packQuantity = dr["packQuantity"].ToString(),
                    SeasonCode = dr["seasonCode"].ToString(),
                    CompanyNumber = dr["CompanyNumber"].ToString(),
                    QuantityOrdered = Convert.ToInt32(dr["QuantityOrdered"] is DBNull ? 0 : dr["QuantityOrdered"]),
                    terminadas = Convert.ToInt32(dr["terminadas"] is DBNull ? 0 : dr["terminadas"])
                });
            };
            cn.Close();
        }

    }
}
