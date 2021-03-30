using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using Production_control_1._0.clases;
using System.Globalization;

namespace Production_control_1._0
{
    public partial class balance : Page
    {

        #region clasee_especiales()
        public class TodoItem
        {
            public string Title { get; set; }
            public decimal Completion { get; set; }
            public decimal sam_cod { get; set; }
            public decimal cap_cod { get; set; }
            public string ajuste_cod { get; set; }
            public string asignacion { get; set; }
            public int correlativo { get; set; }
            public string diferenciador { get; set; }
            public string categoria { get; set; }
            public string plana { get; set; }
            public string rana { get; set; }
            public string flat { get; set; }
            public string cover { get; set; }
            public string transfer { get; set; }
            public string atracadora { get; set; }
            public string plancha { get; set; }
            public string bonding { get; set; }
            public string zig_zag { get; set; }
            public string multiaguja { get; set; }
            public string manual { get; set; }
            public string varias { get; set; }
            public decimal eficiencia { get; set; }
            public int cod_oper { get; set; }
            public string cod_opera { get; set; }
        }

        public class ultimon
        {
            public string fecha { get; set; }
            public string modulo { get; set; }
            public int version { get; set; }
        }

        public class rebalance
        {
            public string operario { get; set; }
            public string operacion { get; set; }
            public decimal sam { get; set; }
            public decimal asignado { get; set; }
            public decimal tiempo { get; set; }
            public decimal eficiencia { get; set; }
            public decimal carga { get; set; }
            public string ajuste { get; set; }
            public int codigo { get; set; }
            public string operacion_cod { get; set; }
        }

        #endregion

        #region clases_para_la_grafica
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection_2 { get; set; }
        public string[] Labels_2 { get; set; }
        public Func<double, string> Formatter_2 { get; set; }

        #endregion

        #region colores_formato_hexadecimal()
        public string rojo = "#F91811";
        public string anaranjado = "#E67E22";
        public string amarillo = "#FAF611";
        public string verde = "#229954";
        public string azul = "#A8D6F5";
        #endregion

        #region datos_iniciales()

        public balance(clases.balance datosBalance)
        {
            InitializeComponent();
            #region tamanoDeZoom
            //datos generales para obtener la altura del zoom
            tamano.Value = System.Windows.SystemParameters.PrimaryScreenHeight;
            ZoomViewbox.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            ZoomViewbox.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            #endregion
            // determinar si el balance es nuevo o se esta abriendo una ya empezado
            if (datosBalance.tipo == "nuevo")
            {
                #region variablesDeConexion
                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                SqlConnection cn2 = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select correlativo, nombre, titulo, sam, maquina, categoria from operaciones where temporada= '" + datosBalance.temporada + "' and estilo= '" + datosBalance.nombre + "' and sam is not null";
                string sql2 = "select Descripción from coordinadores";
                cn.Open();
                #endregion
                #region ejecutarConexion
                // se llenan la lista de operaciones con los datos de la consulta
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                #endregion
                #region agregarOperaciones
                List<elementoListBox> listaOperaciones = new List<elementoListBox>();
                //agregar operaciones de consulta
                while (dr.Read())
                {
                    listaOperaciones.Add(new elementoListBox() { identificador="operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["nombre"].ToString(), tituloOperacion = dr["titulo"].ToString(), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = 0, requeridoOperacion=0, ajusteMaquina = dr["maquina"].ToString(), categoriaMaquina = dr["categoria"].ToString()});
                };
                dr.Close();
                cn.Close();
                //agregar operacion de empaque

                listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = 0, nombreOperacion ="empaque", tituloOperacion = datosBalance.nombreEmpaque, samOperacion = datosBalance.samEmpaque, asignadoOperacion = 0, requeridoOperacion=0, ajusteMaquina = "Mesa de Empaque", categoriaMaquina = "manual" });
                Operaciones.ItemsSource = listaOperaciones;
                #endregion
                #region agregarModulos
                // se agregan los modulos
                cn2.Open();
                SqlCommand cm2 = new SqlCommand(sql2, cn2);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    modulo.Items.Add(dr2["Descripción"].ToString());
                };
                dr2.Close();
                cn2.Close();
                #endregion
                #region agregarDatosDeEncabezado
                //se cargan los datos generales de las variables globales
                fecha_.Content = datosBalance.fechaCreacion;
                estilo_.Content = datosBalance.nombre;
                estilo_2.Content = datosBalance.nombre;
                temporada_.Content = datosBalance.temporada;
                temporada_2.Content = datosBalance.temporada;
                sam_.Content = datosBalance.sam;
                sam_2.Content = datosBalance.sam;
                version_.Text = datosBalance.version.ToString();
                autoriza.IsEnabled = false;
                #endregion
                #region cargarImagen
                // se carga la imagen
                try
                {
                    Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + Global.temporadaselec.ToString() + "/" + Global.estiloselec.ToString() + ".jpg");
                    foto_2.Source = new BitmapImage(fileUri);
                }

                // si no encuentra la imagen del estilo carga la imagen inicial
                catch
                {
                    Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                    foto_2.Source = new BitmapImage(fileUri);
                }

                #endregion
                #region datosInicialesDeGraficTeorica
                // se cargan los datos iniciales para la grafica
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
                }
            };
                Formatter = value => value.ToString("N");
                DataContext = this;
                #endregion
                #region datosInicialesDeGraficaDeRebalance

                // se cargan los datos iniciales para la grafica_reb
                SeriesCollection_2 = new SeriesCollection
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
                Formatter_2 = value => value.ToString("N");
                DataContext = this;
                #endregion
                #region datosInicialesDeResumenDeMaquinas
                // se cargan los datos del resumen de conteo de maquinas
                List<maquina> resumenMaquinas = new List<maquina>();
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "atracadora", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "bonding", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "cover", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "flat", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "transfer", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "manual", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "multiaguja", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "plana", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "plancha", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "rana", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "varias", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumenMaquinas.Add(new maquina() { categoriaMaquina = "zigZag", ii = 0, ie = 0, ee = 0, ei = 0, na = 0 });
                resumen_maquinas.ItemsSource = resumenMaquinas;
                #endregion
            }
        }
        #endregion

        #region zoom()

        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width >= 0) && ZoomViewbox.Height >= 0)
            {
                double alto= System.Windows.SystemParameters.PrimaryScreenHeight;
                double ancho = System.Windows.SystemParameters.PrimaryScreenWidth;
                double relacion = ancho / alto;

                ZoomViewbox.Width = newValue * relacion;
                ZoomViewbox.Height = newValue;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateViewBox(Convert.ToInt32(tamano.Value));
        }


        #endregion

        #region contro_general_programa
        #region accionesBarraDeTitulo
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
        #region botonesMenuGeneralPrograma

        private void salir_Click(object sender, RoutedEventArgs e)
        {
            PagePrincipal PagePrincipal = new PagePrincipal();
            this.NavigationService.Navigate(PagePrincipal);
        }
        private void imprimir_Click(object sender, RoutedEventArgs e)
        {
            int impresion_seleccionada = control_tab_maquinas.SelectedIndex;

            switch (impresion_seleccionada)
            {

                case 0:
                    break;

                case 1:

                    #region general
                    impresion_global.modulo = modulo_2.Content.ToString();
                    impresion_global.estilo = estilo_2.Content.ToString();
                    impresion_global.temporada = temporada_2.Content.ToString();
                    impresion_global.sam = sam_2.Content.ToString();
                    impresion_global.operarios = operarios_2.Content.ToString();
                    impresion_global.ingeniero = ingeniero_.Text.ToString();
                    impresion_global.tipo = "Balance";
                    impresion_global.fecha = fecha_.Content.ToString();
                    impresion_global.eficiencia = eficiencia_2.Content.ToString();

                    #endregion

                    imprimir_balance imprimir_balance = new imprimir_balance();
                    this.NavigationService.Navigate(imprimir_balance);
                    break;
                case 2:
                    #region general
                    impresion_global.modulo = modulo_2.Content.ToString();
                    impresion_global.estilo = estilo_2.Content.ToString();
                    impresion_global.temporada = temporada_2.Content.ToString();
                    impresion_global.sam = sam_2.Content.ToString();
                    impresion_global.operarios = operarios_2.Content.ToString();
                    impresion_global.ingeniero = ingeniero_.Text.ToString();
                    impresion_global.tipo = "Rebalance";
                    impresion_global.fecha = fecha_.Content.ToString();
                    impresion_global.eficiencia = eficiencia_2.Content.ToString();

                    #endregion

                    imprimir_rebalance imprimir_rebalance = new imprimir_rebalance();
                    this.NavigationService.Navigate(imprimir_rebalance);
                    break;

            }
        }
        private void guardar_Click(object sender, RoutedEventArgs e)
        {
            confirmar_guard.IsOpen = true;
                estilo_guard.Text = estilo_.Content.ToString();
                temporada_guard.Text = temporada_.Content.ToString();

                if(modulo.SelectedIndex > -1)
                {
                    modulo_guard.Text = modulo.SelectedItem.ToString();
                }
                else
                {
                    modulo_guard.Text = "GENERICO";

                };
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            cn.Open();
            string sql = "select fecha_modificacion, modulo, version from lista_balances where estilo= '" + estilo_.Content.ToString() + "' and temporada= '" + temporada_.Content.ToString() + "' and modulo= '" + modulo_guard.Text.ToString() + "' order by version desc";
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            ultimos_estilos.Items.Clear();
            while (dr.Read())
            {
                ultimos_estilos.Items.Add(new ultimon { modulo = dr["modulo"].ToString(), fecha = dr["fecha_modificacion"].ToString(), version = Convert.ToInt32(dr["version"]) });
            };
            dr.Close();
            cn.Close();
        }

        private void guard_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #endregion

        #region encabezado()
        #region calculoDeRequerimientoDeMaquinasEficiencia
        private void maquinaRequerimiento(object sender, TextChangedEventArgs e)
        {
            try
            {
                // se obtienen las piezas por hora
                Double piezasRequeridasHora = Math.Round(Convert.ToDouble(piezas_de_corrida.Text) / Convert.ToDouble(horas_de_corrida.Text), 0);
                piezas_por_hora.Content = piezasRequeridasHora;

                // se calcula la capacidad de cada operacion, se consulta otra vez y se calcula 
                List<elementoListBox> listaOperaciones = new List<elementoListBox>();
                foreach (elementoListBox item in Operaciones.Items)
                {
                    Double requerido = Math.Round(piezasRequeridasHora / (60 / item.samOperacion), 2);
                    listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = item.correlativoOperacion, nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, samOperacion = item.samOperacion, asignadoOperacion = item.asignadoOperacion, requeridoOperacion = requerido, ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina });
                }
                Operaciones.ItemsSource = listaOperaciones;

                //recalcular_asignaciones();
            }
            catch { };
            int operarios_conteo = 0;
            foreach (elementoListBox item in Operarios.Items)
            {
                operarios_conteo = operarios_conteo + 1;
            }

            //calcular eficiencia
                eficiencia_.Content = ejecutarCalculoDeEficiencia();
                eficiencia_2.Content = ejecutarCalculoDeEficiencia();
        }
        #endregion
        #region seleccionarElModulo
        private void modulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo, nombre, plana, rana, flat, cover, transfer, atracadora, plancha, bonding, zig_zag, multiaguja, manual, varias from eficiencia_codigo_maquina where asignado= '" + modulo.SelectedItem.ToString() + "'";
            // se agregan los modulos
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            List<elementoListBox> items = new List<elementoListBox>();
            int operarios_conteo = 0;
            while (dr.Read())
            {
                items.Add(new elementoListBox() { identificador = "operario", codigoOperario = Convert.ToInt32(dr["codigo"]), nombreOperario = dr["nombre"].ToString(), asignadoOperario = 0, planaOperario = Convert.ToDouble(dr["plana"] is DBNull ? 0.9 : dr["plana"]), ranaOperario = Convert.ToDouble(dr["rana"] is DBNull ? 0.9 : dr["rana"]), flatOperario = Convert.ToDouble(dr["flat"] is DBNull ? 0.9 : dr["flat"]), coverOperario = Convert.ToDouble(dr["cover"] is DBNull ? 0.9 : dr["cover"]), transferOperario = Convert.ToDouble(dr["transfer"] is DBNull ? 0.9 : dr["transfer"]), atracadoraOperario = Convert.ToDouble(dr["atracadora"] is DBNull ? 0.9 : dr["atracadora"]), planchaOperario = Convert.ToDouble(dr["plancha"] is DBNull ? 0.9 : dr["plancha"]), bondingOperario = Convert.ToDouble(dr["bonding"] is DBNull ? 0.9 : dr["bonding"]), zigzagOperario = Convert.ToDouble(dr["zig_zag"] is DBNull ? 0.9 : dr["zig_zag"]), multiagujaOperario = Convert.ToDouble(dr["multiaguja"] is DBNull ? 0.9 : dr["multiaguja"]), manualOperario = Convert.ToDouble(dr["manual"] is DBNull ? 0.9 : dr["manual"]), variasOperario = Convert.ToDouble(dr["varias"] is DBNull ? 0.9 : dr["varias"]) });
                operarios_conteo = operarios_conteo + 1;
            };
            Operarios.ItemsSource = items;
            dr.Close();
            cn.Close();
            modulo_2.Content = modulo.SelectedItem.ToString();
            operarios_.Content = operarios_conteo.ToString();
            operarios_2.Content = operarios_conteo.ToString();

            //obtener eficiencia
            eficiencia_.Content = ejecutarCalculoDeEficiencia();
            eficiencia_2.Content = ejecutarCalculoDeEficiencia();

        }
        #endregion
        #region agregarOperariosGenerales
        private void agregarListaDeOperariosGenerale(object sender, RoutedEventArgs e)
        {
            List<elementoListBox> items = new List<elementoListBox>();
            for (int i = 1; i < 21; i++)
            {
                items.Add(new elementoListBox { identificador = "operario", codigoOperario = 0000, nombreOperario = "operario " + i, asignadoOperario = 0, planaOperario = 0.9, ranaOperario = 0.9, flatOperario = 0.9, coverOperario = 0.9, transferOperario = 0.9, atracadoraOperario = 0.9, planchaOperario = 0.9, bondingOperario = 0.9, zigzagOperario = 0.9, multiagujaOperario = 0.9, manualOperario = 0.9, variasOperario = 0.9 });
            };
            Operarios.ItemsSource = items;
            int operarios_conteo = 20;
            operarios_.Content = operarios_conteo.ToString();
            operarios_2.Content = operarios_conteo.ToString();

            //calcular eficiencia
            eficiencia_.Content = ejecutarCalculoDeEficiencia();
            eficiencia_2.Content = ejecutarCalculoDeEficiencia();
        }
        #endregion
        #region popUpAgregarOperario()
        #region determinarEficiencia
        public double calcularEficiencia(double samEstilo, double unidadesFabricar, double tiempoDisponible, double operarios)
        {
            //eficiencia=(sam(unidadesTotales/tiempoDisponible))/operarios
            double eficienciaResultado = (samEstilo * (unidadesFabricar / tiempoDisponible)) / operarios;
            return eficienciaResultado;
        }
        public string ejecutarCalculoDeEficiencia()
        {
            string CalculoDeEficiencia = $"{0:0.#%}";

            try
            {
                CalculoDeEficiencia = $"{calcularEficiencia(Convert.ToDouble(sam_.Content), Convert.ToDouble(piezas_de_corrida.Text), Convert.ToDouble(horas_de_corrida.Text) * 60d, Convert.ToDouble(operarios_.Content)):0.##%}";
            }
            catch
            {
                CalculoDeEficiencia = $"{0:0.#%}";
            }

            return CalculoDeEficiencia;
        }
        #endregion
        private void agregar_operario_Click(object sender, RoutedEventArgs e)
        {
            codigo_op.Text = "";
            nombre_op.Text = "";
            datos_operario.IsOpen = true;
            agregar_op.IsEnabled = true;
            revision.Visibility = System.Windows.Visibility.Hidden;
        }
        private void agregar_cancel_Click(object sender, RoutedEventArgs e)
        {
            datos_operario.IsOpen = false;
        }
        private void agregar_op_Click(object sender, RoutedEventArgs e)
        {
            //se cargan los operarios con sus asignaciones en una nueva lista lista
            List<elementoListBox> items = new List<elementoListBox>();
            int operarios_conteo = 0;
            foreach (elementoListBox item in Operarios.Items)
            {
                items.Add(new elementoListBox { identificador = "operario", codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, asignadoOperario = item.asignadoOperario, planaOperario = item.planaOperario, ranaOperario = item.ranaOperario, flatOperario = item.flatOperario, coverOperario = item.coverOperario, transferOperario = item.transferOperario, atracadoraOperario = item.atracadoraOperario, planchaOperario = item.planchaOperario, bondingOperario = item.bondingOperario, zigzagOperario = item.zigzagOperario, multiagujaOperario = item.multiagujaOperario, manualOperario = item.manualOperario, variasOperario = item.variasOperario });
                operarios_conteo = operarios_conteo + 1;
            }
            // si el codigo de persona existe entonce se cargan sus datos
            if (revision.Visibility == System.Windows.Visibility.Visible)
            {
                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select nombre, plana, codigo, rana, flat, cover, transfer, atracadora, plancha, bonding, zig_zag, multiaguja, manual, varias from eficiencia_codigo_maquina where codigo= '" + codigo_op.Text.ToString() + "'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                //se realiza la consulta en sql del operario que se esta buscando
                while (dr.Read())
                {
                    items.Add(new elementoListBox() { identificador = "operario", codigoOperario = Convert.ToInt32(dr["codigo"]), nombreOperario = dr["nombre"].ToString(), asignadoOperario = 0, planaOperario = Convert.ToDouble(dr["plana"] is DBNull ? 0.9 : dr["plana"]), ranaOperario = Convert.ToDouble(dr["rana"] is DBNull ? 0.9 : dr["rana"]), flatOperario = Convert.ToDouble(dr["flat"] is DBNull ? 0.9 : dr["flat"]), coverOperario = Convert.ToDouble(dr["cover"] is DBNull ? 0.9 : dr["cover"]), transferOperario = Convert.ToDouble(dr["transfer"] is DBNull ? 0.9 : dr["transfer"]), atracadoraOperario = Convert.ToDouble(dr["atracadora"] is DBNull ? 0.9 : dr["atracadora"]), planchaOperario = Convert.ToDouble(dr["plancha"] is DBNull ? 0.9 : dr["plancha"]), bondingOperario = Convert.ToDouble(dr["bonding"] is DBNull ? 0.9 : dr["bonding"]), zigzagOperario = Convert.ToDouble(dr["zig_zag"] is DBNull ? 0.9 : dr["zig_zag"]), multiagujaOperario = Convert.ToDouble(dr["multiaguja"] is DBNull ? 0.9 : dr["multiaguja"]), manualOperario = Convert.ToDouble(dr["manual"] is DBNull ? 0.9 : dr["manual"]), variasOperario = Convert.ToDouble(dr["varias"] is DBNull ? 0.9 : dr["varias"]) });
                    operarios_conteo = operarios_conteo + 1;
                };
                dr.Close();
                cn.Close();
            }
            //si no existe la persona consultada se ingresara el nombre que se escriba manualmente
            else
            {
                items.Add(new elementoListBox { identificador = "operario", codigoOperario = 0000, nombreOperario = nombre_op.Text, asignadoOperario = 0, planaOperario = 0.9, ranaOperario = 0.9, flatOperario = 0.9, coverOperario = 0.9, transferOperario = 0.9, atracadoraOperario = 0.9, planchaOperario = 0.9, bondingOperario = 0.9, zigzagOperario = 0.9, multiagujaOperario = 0.9, manualOperario = 0.9, variasOperario = 0.9 });
                operarios_conteo = operarios_conteo + 1;
            }

            Operarios.ItemsSource = items;
            //se agrega el nuevo numero de operarios que se ha calculado
            operarios_.Content = operarios_conteo.ToString();
            operarios_2.Content = operarios_conteo.ToString();
            //calcular eficiencia
            eficiencia_.Content = ejecutarCalculoDeEficiencia();
            eficiencia_2.Content = ejecutarCalculoDeEficiencia();

            //se limpia el nombre del operario que se agrego
            codigo_op.Text = "";
            nombre_op.Text = "";
        }
        private void codigo_op_TextChanged(object sender, TextChangedEventArgs e)
        {
            nombre_op.Text = "";
            int conteo = 0;
            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select nombre from eficiencia_codigo_maquina where codigo= '" + codigo_op.Text.ToString() + "'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            //se realiza la consulta en sql del operario que se esta buscando
            while (dr.Read())
            {
                nombre_op.Text = dr["nombre"].ToString();
                conteo = conteo + 1;
            };
            dr.Close();
            cn.Close();
            if (conteo > 0)
            {
                revision.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                revision.Visibility = System.Windows.Visibility.Hidden;
            }


        }
        private void no_nomb_Click(object sender, RoutedEventArgs e)
        {

            int operarios_conteo = 0;
            //se cargan los operarios actuales en una lista y se cuentan
            List<elementoListBox> items = new List<elementoListBox>();
            foreach (elementoListBox item in Operarios.Items)
            {
                items.Add(new elementoListBox { identificador = "operario", codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, asignadoOperario = item.asignadoOperario, planaOperario = item.planaOperario, ranaOperario = item.ranaOperario, flatOperario = item.flatOperario, coverOperario = item.coverOperario, transferOperario = item.transferOperario, atracadoraOperario = item.atracadoraOperario, planchaOperario = item.planchaOperario, bondingOperario = item.bondingOperario, zigzagOperario = item.zigzagOperario, multiagujaOperario = item.multiagujaOperario, manualOperario = item.manualOperario, variasOperario = item.variasOperario });
                operarios_conteo = operarios_conteo + 1;
            }

            //se obtiene el numero de operario que se va a agregar
            operarios_conteo = operarios_conteo + 1;

            //se agrega el operario a la lista
            items.Add(new elementoListBox { identificador = "operario", codigoOperario = 0000, nombreOperario ="operario " + operarios_conteo, asignadoOperario = 0, planaOperario = 0.9, ranaOperario = 0.9, flatOperario = 0.9, coverOperario = 0.9, transferOperario = 0.9, atracadoraOperario = 0.9, planchaOperario = 0.9, bondingOperario = 0.9, zigzagOperario = 0.9, multiagujaOperario = 0.9, manualOperario = 0.9, variasOperario = 0.9 });

            // se establece la lista final ccomo origen de los operarios
            Operarios.ItemsSource = items;

            //se agrega el nuevo numero de operarios que se ha calculado
            operarios_.Content = operarios_conteo.ToString();
            operarios_2.Content = operarios_conteo.ToString();
            //calcular eficiencia
            eficiencia_.Content = ejecutarCalculoDeEficiencia();
            eficiencia_2.Content = ejecutarCalculoDeEficiencia();
        }

        #endregion
        #endregion

        #region obtener_datos_drag()

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            System.Windows.UIElement element = source.InputHitTest(point) as System.Windows.UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as System.Windows.UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }
        #endregion

        ListBox dragSource = null;

        private void Operaciones_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private void Operarios_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }
        #endregion

        #region codigo viejo

        //private void actualizar_grafica_reba()
        //{
        //    // se definen las listas donde se va a consolidar la informacion para el rebalanc
        //    List<rebalance> lista_resumen = new List<rebalance>();
        //    List<string> lista_de_operarios_2 = new List<string>();
        //    clases_globales.impresion_rebalance.lista_de_cargas_rebalance.Clear();
        //    clases_globales.impresion_rebalance.lista_de_eficiencias_rebalance.Clear();
        //    clases_globales.impresion_rebalance.lista_de_operarios_rebalance.Clear();

        //    //para una lista sin nombres repetidos se va a evaluar cada operario de rebalance contra los operarios de la lista operarios
        //    foreach (TodoItem elemento in Operarios.Items)
        //    {
        //        int conteo = 0;
        //        decimal asignacion = 0;
        //        decimal sam_total = 0;
        //        decimal tiempo_total = 0;
        //        foreach (rebalance elemento_2 in rebalance_.Items)
        //        {
        //            if (elemento.Title == elemento_2.operario)
        //            {
        //                conteo = conteo = 1;
        //                asignacion = asignacion + elemento_2.carga;
        //                sam_total = sam_total + elemento_2.sam;
        //                tiempo_total = tiempo_total + elemento_2.tiempo;
        //            }
        //        }

        //        // si se encuentra asignado se pone en la lista que se va a presentar en la grafic
        //        if (conteo > 0)
        //        {
        //            decimal eficiencia_total;
        //            if (tiempo_total > 0)
        //            {
        //                eficiencia_total = sam_total / tiempo_total;
        //            }
        //            else
        //            {
        //                eficiencia_total = 0;
        //            }
        //            lista_resumen.Add(new rebalance() { operario = elemento.Title, carga = asignacion, eficiencia = eficiencia_total });
        //        }

        //    }

        //    // se crea una lista con los nombres para poner en el eje horizontal

        //    foreach (rebalance item in lista_resumen)
        //    {
        //        lista_de_operarios_2.Add(item.operario);
        //        clases_globales.impresion_rebalance.lista_de_operarios_rebalance.Add(item.operario);

        //    }

        //    // se limpian las series de la grafic para cuando se actualice no tenga datos anteriores

        //    grafico_rebalance.AxisX.Clear();
        //    SeriesCollection_2[0].Values.Clear();
        //    SeriesCollection_2[1].Values.Clear();
        //    SeriesCollection_2[2].Values.Clear();
        //    SeriesCollection_2[3].Values.Clear();

        //    // se agregan los nombres al eje
        //    grafico_rebalance.AxisX.Add(new Axis() { Labels = lista_de_operarios_2.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 }, });


        //    // se agregan  los valores de cada nombre de la grafica
        //    foreach (rebalance item in lista_resumen)
        //    {
        //        SeriesCollection_2[0].Values.Add(Convert.ToDouble(item.carga));
        //        SeriesCollection_2[1].Values.Add(1d);
        //        SeriesCollection_2[2].Values.Add(0.9d);
        //        SeriesCollection_2[3].Values.Add(Convert.ToDouble(item.eficiencia));
        //        clases_globales.impresion_rebalance.lista_de_cargas_rebalance.Add(Convert.ToDouble(item.carga));
        //        clases_globales.impresion_rebalance.lista_de_eficiencias_rebalance.Add(Convert.ToDouble(item.eficiencia));

        //    };
        //}
      
  
        //private void operaciones_subcarga_sobrecarga()
        //{
        //    List<TodoItem> lista_2 = new List<TodoItem>();
        //    foreach (TodoItem item in Operarios.Items)
        //    {
        //        lista_2.Add(new TodoItem
        //        {
        //            Title = item.Title,
        //            Completion = item.Completion
        //        });
        //    };
        //    try
        //    {
        //        var elemento_minimo = lista_2.Min(x => x.Completion);
        //        var elemento_maximo = lista_2.Max(x => x.Completion);

        //        if (elemento_minimo < 0.7m)
        //        {
        //            foreach (TodoItem item in lista_2)
        //            {
        //                if (item.Completion == elemento_minimo)
        //                {
        //                    subutilizacion_2.Content = item.Title.ToString();
        //                }
        //            }
        //        };
        //        if (elemento_maximo > 1.05m)
        //        {
        //            foreach (TodoItem item in lista_2)
        //            {
        //                if (item.Completion == elemento_maximo)
        //                {
        //                    sobrecarga_2.Content = item.Title.ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}
        //private void consolidar_elemento_balance()
        //{
        //    // se define una lista donde se cargara la informacion para el listv
        //    List<rebalance> lista_r = new List<rebalance>();
        //    List<rebalance> lista_r2 = new List<rebalance>();

        //    #region crear_lista_con_operaciones_asignadas()
        //    //se define la lista donde se pondran
        //    List<rebalance> lista_operaciones_sam = new List<rebalance>();

        //    //a la lista se le ponen los datos de cada maquina
        //    foreach (TodoItem item in Operacion1.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario1.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion2.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario2.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion3.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario3.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion4.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario4.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion5.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario5.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion6.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario6.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion7.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario7.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion8.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario8.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion9.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario9.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion10.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario10.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion11.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario11.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion12.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario12.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion13.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario13.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion14.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario14.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion15.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario15.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion16.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario16.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion17.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario17.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion18.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario18.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion19.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario19.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion20.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario20.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion21.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario21.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion22.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario22.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion23.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario23.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion24.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario24.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion25.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario25.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion26.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario26.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion27.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario27.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion28.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario28.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion29.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario29.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion30.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario30.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion31.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario31.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion32.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario32.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion33.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario33.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion34.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario34.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion35.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario35.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion36.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario36.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion37.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario37.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion38.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario38.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion39.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario39.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion40.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario40.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion41.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario41.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion42.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario42.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion43.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario43.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion44.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario44.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion45.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario45.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion46.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario46.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion47.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario47.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion48.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario48.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion49.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario49.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion50.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario50.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion51.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario51.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion52.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario52.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion53.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario53.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion54.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario54.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion55.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario55.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion56.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario56.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion57.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario57.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion58.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario58.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion59.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario59.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion60.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario60.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion61.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario61.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion62.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario62.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion63.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario63.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion64.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario64.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion65.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario65.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    foreach (TodoItem item in Operacion66.Items)
        //    {
        //        lista_operaciones_sam.Add(new rebalance() { operario = operario66.Text.ToString(), operacion = item.Title.ToString(), asignado = Convert.ToDecimal(item.cap_cod) });
        //    }
        //    #endregion

        //    foreach (rebalance item in lista_operaciones_sam)
        //    {
        //        foreach (TodoItem subitem in Operaciones.Items)
        //        {
        //            if (item.operacion == subitem.Title)
        //            {
        //                lista_r.Add(new rebalance() { operario = item.operario.ToString(), operacion = item.operacion, sam = subitem.sam_cod * 60, asignado = item.asignado, tiempo = Convert.ToDecimal(0), ajuste = subitem.ajuste_cod, operacion_cod = subitem.cod_opera, codigo = item.codigo });
        //            }
        //        }
        //    }

        //    foreach (rebalance elemento in lista_r)
        //    {
        //        foreach (TodoItem linea in Operarios.Items)
        //        {
        //            if (elemento.operario == linea.Title)
        //            {
        //                lista_r2.Add(new rebalance() { operario = elemento.operario, operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = elemento.tiempo, ajuste = elemento.ajuste, operacion_cod = elemento.operacion_cod, codigo = linea.cod_oper });
        //            }
        //        }
        //    }

        //    rebalance_.ItemsSource = lista_r2;
        //}
        //private void recalcular_rebalance()
        //{
        //    List<rebalance> lista_r = new List<rebalance>();
        //    foreach (rebalance elemento in rebalance_.Items)
        //    {
        //        try
        //        {
        //            decimal eficiencia_r = Math.Round((elemento.sam / elemento.tiempo), 4);
        //            decimal carga_r = Math.Round(elemento.asignado / (elemento.sam / elemento.tiempo), 3);
        //            lista_r.Add(new rebalance() { operario = elemento.operario.ToString(), operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = elemento.tiempo, eficiencia = eficiencia_r, carga = carga_r, ajuste = elemento.ajuste, codigo = elemento.codigo, operacion_cod = elemento.operacion_cod });
        //        }
        //        catch
        //        {
        //            lista_r.Add(new rebalance() { operario = elemento.operario.ToString(), operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = elemento.tiempo, eficiencia = elemento.eficiencia, carga = elemento.carga, ajuste = elemento.ajuste, codigo = elemento.codigo, operacion_cod = elemento.operacion_cod });
        //        }
        //    }
        //    rebalance_.ItemsSource = lista_r;
        //}
        //private void consultar_toma_de_tiempos()
        //{
        //    List<rebalance> lista_r = new List<rebalance>();
        //    foreach (rebalance elemento in rebalance_.Items)
        //    {
        //        try
        //        {
        //            string modulo_consult;
        //            if (modulo.SelectedIndex < 0)
        //            {
        //                modulo_consult = "GENERICO".ToString();
        //            }
        //            else
        //            {
        //                modulo_consult = modulo.SelectedItem.ToString();
        //            }

        //            // se declaran las variables de conexion
        //            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        //            string sql = "select avg(tiempo_objetivo) as tiempo_objetivo, nombre, titulo, max(ajuste) as ajuste from toma_de_tiempos where modulo= '" + modulo_consult + "' and estilo= '" + estilo_.Content.ToString() + "' and temporada= '" + temporada_.Content.ToString() + "' and version= '" + version_.Text.ToString() + "' and nombre= '" + elemento.operario + "' and titulo= '" + elemento.operacion + "' group by modulo, estilo, temporada, version, nombre, titulo";
        //            // se agregan los modulos
        //            cn.Open();
        //            SqlCommand cm = new SqlCommand(sql, cn);
        //            SqlDataReader dr = cm.ExecuteReader();
        //            int conteo = 0;
        //            while (dr.Read())
        //            {
        //                conteo = conteo + 1;
        //                lista_r.Add(new rebalance() { operario = elemento.operario, operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = Convert.ToDecimal(dr["tiempo_objetivo"]), eficiencia = elemento.eficiencia, carga = elemento.carga, ajuste = dr["ajuste"].ToString(), operacion_cod = elemento.operacion_cod, codigo = elemento.codigo });
        //            }
        //            dr.Close();
        //            cn.Close();
        //            if (conteo == 0)
        //            {
        //                lista_r.Add(new rebalance() { operario = elemento.operario.ToString(), operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = elemento.tiempo, eficiencia = elemento.eficiencia, carga = elemento.carga, ajuste = elemento.ajuste, operacion_cod = elemento.operacion_cod, codigo = elemento.codigo });
        //            }
        //        }
        //        catch
        //        {
        //            lista_r.Add(new rebalance() { operario = elemento.operario.ToString(), operacion = elemento.operacion, sam = elemento.sam, asignado = elemento.asignado, tiempo = elemento.tiempo, eficiencia = elemento.eficiencia, carga = elemento.carga, ajuste = elemento.ajuste, operacion_cod = elemento.operacion_cod, codigo = elemento.codigo });
        //        }
        //    }
        //    rebalance_.ItemsSource = lista_r;

        //}

      
      
      
        #endregion

        #region pop_ip_guardar
        //private void guard_Click(object sender, RoutedEventArgs e)
        //{
        //    //se hace la consulta en sql para revisar si ya se ha guardado una version de ese balance
        //    SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        //    string identificador_g = modulo_guard.Text.ToString() + estilo_.Content.ToString() + temporada_.Content.ToString() + version_guard.Text.ToString();
        //    cn.Open();
        //    string sql_v = "select identificador from lista_balances where identificador= '" + identificador_g + "'";
        //    SqlCommand cm_v = new SqlCommand(sql_v, cn);
        //    SqlDataReader dr_v = cm_v.ExecuteReader();
        //    int conteo = 0;
        //    while (dr_v.Read())
        //    {
        //        conteo = conteo + 1;
        //    };
        //    dr_v.Close();
        //    cn.Close();

        //    // si no se ha guardado esa version debe guardarse directamente
        //    if (conteo < 1)
        //    {
        //        #region ingresar_datos_en_tabla_lista()
        //        //definir valores de los datos a guardar en la tabla de la lista de balances
        //        string fecha_g = fecha_.Content.ToString();
        //        string estilo_g = estilo_.Content.ToString();
        //        string temporada_g = temporada_.Content.ToString();
        //        string sam_g = sam_.Content.ToString();
        //        string modulo_g = modulo_guard.Text.ToString();
        //        string corrida_g;
        //        if (string.IsNullOrEmpty(piezas_de_corrida.Text))
        //        {
        //            corrida_g = 0.ToString();
        //        }
        //        else
        //        {
        //            corrida_g = piezas_de_corrida.Text.ToString();
        //        }
        //        string horas_g;
        //        if (string.IsNullOrEmpty(horas_de_corrida.Text))
        //        {
        //            horas_g = 0.ToString();
        //        }
        //        else
        //        {
        //            horas_g = horas_de_corrida.Text.ToString();
        //        }
        //        string operarios_g = operarios_.Content.ToString();
        //        string eficiencia_g;
        //        try
        //        {
        //            decimal calculo_efic = Math.Round(Convert.ToDecimal(sam_.Content) * (Convert.ToDecimal(piezas_de_corrida.Text) / Convert.ToDecimal(horas_de_corrida.Text)) / Convert.ToDecimal(operarios_.Content), 0);
        //            eficiencia_g = calculo_efic.ToString();
        //        }
        //        catch
        //        {
        //            eficiencia_g = 0.ToString();
        //        }

        //        string piezas_g = piezas_por_hora.Content.ToString();
        //        string ingeniero_g = ingeniero_.Text.ToString();

        //        // ingresar datos a SQL en lista de balances 
        //        string sql = "insert into lista_balances(identificador, fecha_creacion, estilo, temporada, sam, modulo, corrida, horas, operarios, eficiencia, piezash, ingeniero, fecha_modificacion, version) values('" + identificador_g + "', '" + fecha_g + "', '" + estilo_g + "', '" + temporada_g + "', " + sam_g + ", '" + modulo_g + "', " + corrida_g + ", " + horas_g + ", " + operarios_g + ", " + eficiencia_g + ", " + piezas_g + ", '" + ingeniero_g + "', '" + DateTime.Now + "', '" + version_guard.Text.ToString() +"')";
        //        cn.Open();
        //        // se llenan la lista de operaciones con los datos de la consulta
        //        SqlCommand cm = new SqlCommand(sql, cn);
        //        SqlDataReader dr = cm.ExecuteReader();
        //        dr.Close();
        //        cn.Close();
        //        #endregion
        //        #region ingresar_datos_en_tabla_operarios

        //        foreach (TodoItem item in Operarios.Items)
        //        {
        //            cn.Open();
        //            string sql2 = "insert into operarios(identificador, codigo, nombre, asignado) values('" + identificador_g + "', '" + item.cod_oper + "', '" + item.Title.ToString() + "', '" + item.Completion + "')";
        //            SqlCommand cm2 = new SqlCommand(sql2, cn);
        //            SqlDataReader dr2 = cm2.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        #endregion
        //        #region ingresar_datos_en_tabla_operaciones
        //        foreach (TodoItem item in Operaciones.Items)
        //        {
        //            string nombre = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql3 = "insert into operaciones(identificador, correlativo, codigo, titulo, sam, ajuste, requerimiento, asignado, categoria) values('" + identificador_g + "', '" + item.correlativo.ToString() + "', '" + item.cod_opera + "', '" + nombre + "', '" + item.sam_cod + "', '" + item.ajuste_cod + "', '" + item.cap_cod + "', '" + item.Completion + "', '" + item.categoria +"')";
        //            SqlCommand cm3 = new SqlCommand(sql3, cn);
        //            SqlDataReader dr3 = cm3.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        #endregion
        //        #region ingresar_datos_en_tabla_maquinas()

        //        foreach (TodoItem item in Operacion1.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 1 + "', '" + maquina1.Text.ToString() + "', '" + categoria1.Text.ToString() + "', '" + b1.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario1.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion2.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 2 + "', '" + maquina2.Text.ToString() + "', '" + categoria2.Text.ToString() + "', '" + b1.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario2.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion3.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 3 + "', '" + maquina3.Text.ToString() + "', '" + categoria3.Text.ToString() + "', '" + b3.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario3.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion4.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 4 + "', '" + maquina4.Text.ToString() + "', '" + categoria4.Text.ToString() + "', '" + b4.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario4.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion5.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 5 + "', '" + maquina5.Text.ToString() + "', '" + categoria5.Text.ToString() + "', '" + b5.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario5.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion6.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 6 + "', '" + maquina6.Text.ToString() + "', '" + categoria6.Text.ToString() + "', '" + b6.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario6.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion7.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 7 + "', '" + maquina7.Text.ToString() + "', '" + categoria7.Text.ToString() + "', '" + b7.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario7.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion8.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 8 + "', '" + maquina8.Text.ToString() + "', '" + categoria8.Text.ToString() + "', '" + b8.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario8.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion9.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 9 + "', '" + maquina9.Text.ToString() + "', '" + categoria9.Text.ToString() + "', '" + b9.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario9.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion10.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 10 + "', '" + maquina10.Text.ToString() + "', '" + categoria10.Text.ToString() + "', '" + b10.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario10.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion11.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 11 + "', '" + maquina11.Text.ToString() + "', '" + categoria11.Text.ToString() + "', '" + b11.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario11.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion12.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 12 + "', '" + maquina12.Text.ToString() + "', '" + categoria12.Text.ToString() + "', '" + b12.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario12.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion13.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 13 + "', '" + maquina13.Text.ToString() + "', '" + categoria13.Text.ToString() + "', '" + b13.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario13.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion14.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 14 + "', '" + maquina14.Text.ToString() + "', '" + categoria14.Text.ToString() + "', '" + b14.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario14.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion15.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 15 + "', '" + maquina15.Text.ToString() + "', '" + categoria15.Text.ToString() + "', '" + b15.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario15.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion16.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 16 + "', '" + maquina16.Text.ToString() + "', '" + categoria16.Text.ToString() + "', '" + b16.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario16.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion17.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 17 + "', '" + maquina17.Text.ToString() + "', '" + categoria17.Text.ToString() + "', '" + b17.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario17.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion18.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 18 + "', '" + maquina18.Text.ToString() + "', '" + categoria18.Text.ToString() + "', '" + b18.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario18.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion19.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 19 + "', '" + maquina19.Text.ToString() + "', '" + categoria19.Text.ToString() + "', '" + b19.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario19.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion20.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 20 + "', '" + maquina20.Text.ToString() + "', '" + categoria20.Text.ToString() + "', '" + b20.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario20.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion21.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 21 + "', '" + maquina21.Text.ToString() + "', '" + categoria21.Text.ToString() + "', '" + b21.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario21.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion22.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 22 + "', '" + maquina22.Text.ToString() + "', '" + categoria22.Text.ToString() + "', '" + b22.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario22.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion23.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 23 + "', '" + maquina23.Text.ToString() + "', '" + categoria23.Text.ToString() + "', '" + b23.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario23.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion24.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 24 + "', '" + maquina24.Text.ToString() + "', '" + categoria24.Text.ToString() + "', '" + b24.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario24.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion25.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 25 + "', '" + maquina25.Text.ToString() + "', '" + categoria25.Text.ToString() + "', '" + b25.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario25.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion26.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 26 + "', '" + maquina26.Text.ToString() + "', '" + categoria26.Text.ToString() + "', '" + b26.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario26.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion27.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 27 + "', '" + maquina27.Text.ToString() + "', '" + categoria27.Text.ToString() + "', '" + b27.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario27.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion28.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 28 + "', '" + maquina28.Text.ToString() + "', '" + categoria28.Text.ToString() + "', '" + b28.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario28.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion29.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 29 + "', '" + maquina29.Text.ToString() + "', '" + categoria29.Text.ToString() + "', '" + b29.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario29.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion30.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 30 + "', '" + maquina30.Text.ToString() + "', '" + categoria30.Text.ToString() + "', '" + b30.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario30.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion31.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 31 + "', '" + maquina31.Text.ToString() + "', '" + categoria31.Text.ToString() + "', '" + b31.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario31.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion32.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 32 + "', '" + maquina32.Text.ToString() + "', '" + categoria32.Text.ToString() + "', '" + b32.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario32.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion33.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 33 + "', '" + maquina33.Text.ToString() + "', '" + categoria33.Text.ToString() + "', '" + b33.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario33.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion34.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 34 + "', '" + maquina34.Text.ToString() + "', '" + categoria34.Text.ToString() + "', '" + b34.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario34.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion35.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 35 + "', '" + maquina35.Text.ToString() + "', '" + categoria35.Text.ToString() + "', '" + b35.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario35.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion36.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 36 + "', '" + maquina36.Text.ToString() + "', '" + categoria36.Text.ToString() + "', '" + b36.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario36.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion37.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 37 + "', '" + maquina37.Text.ToString() + "', '" + categoria37.Text.ToString() + "', '" + b37.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario37.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion38.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 38 + "', '" + maquina38.Text.ToString() + "', '" + categoria38.Text.ToString() + "', '" + b38.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario38.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion39.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 39 + "', '" + maquina39.Text.ToString() + "', '" + categoria39.Text.ToString() + "', '" + b39.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario39.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion40.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 40 + "', '" + maquina40.Text.ToString() + "', '" + categoria40.Text.ToString() + "', '" + b40.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario40.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion41.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 41 + "', '" + maquina41.Text.ToString() + "', '" + categoria41.Text.ToString() + "', '" + b41.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario41.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion42.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 42 + "', '" + maquina42.Text.ToString() + "', '" + categoria42.Text.ToString() + "', '" + b42.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario42.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion43.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 43 + "', '" + maquina43.Text.ToString() + "', '" + categoria43.Text.ToString() + "', '" + b43.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario43.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion44.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 44 + "', '" + maquina44.Text.ToString() + "', '" + categoria44.Text.ToString() + "', '" + b44.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario44.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion45.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 45 + "', '" + maquina45.Text.ToString() + "', '" + categoria45.Text.ToString() + "', '" + b45.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario45.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion46.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 46 + "', '" + maquina46.Text.ToString() + "', '" + categoria46.Text.ToString() + "', '" + b46.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario46.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion47.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 47 + "', '" + maquina47.Text.ToString() + "', '" + categoria47.Text.ToString() + "', '" + b47.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario47.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion48.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 48 + "', '" + maquina48.Text.ToString() + "', '" + categoria48.Text.ToString() + "', '" + b48.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario48.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion49.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 49 + "', '" + maquina49.Text.ToString() + "', '" + categoria49.Text.ToString() + "', '" + b49.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario49.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion50.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 50 + "', '" + maquina50.Text.ToString() + "', '" + categoria50.Text.ToString() + "', '" + b50.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario50.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion51.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 51 + "', '" + maquina51.Text.ToString() + "', '" + categoria51.Text.ToString() + "', '" + b51.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario51.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion52.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 52 + "', '" + maquina52.Text.ToString() + "', '" + categoria52.Text.ToString() + "', '" + b52.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario52.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion53.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 53 + "', '" + maquina53.Text.ToString() + "', '" + categoria53.Text.ToString() + "', '" + b53.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario53.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion54.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 54 + "', '" + maquina54.Text.ToString() + "', '" + categoria54.Text.ToString() + "', '" + b54.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario54.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion55.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 55 + "', '" + maquina55.Text.ToString() + "', '" + categoria55.Text.ToString() + "', '" + b55.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario55.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion56.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 56 + "', '" + maquina56.Text.ToString() + "', '" + categoria56.Text.ToString() + "', '" + b56.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario56.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion57.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 57 + "', '" + maquina57.Text.ToString() + "', '" + categoria57.Text.ToString() + "', '" + b57.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario57.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion58.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 58 + "', '" + maquina58.Text.ToString() + "', '" + categoria58.Text.ToString() + "', '" + b58.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario58.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion59.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 59 + "', '" + maquina59.Text.ToString() + "', '" + categoria59.Text.ToString() + "', '" + b59.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario59.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion60.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 60 + "', '" + maquina60.Text.ToString() + "', '" + categoria60.Text.ToString() + "', '" + b60.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario60.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion61.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 61 + "', '" + maquina61.Text.ToString() + "', '" + categoria61.Text.ToString() + "', '" + b61.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario61.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion62.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 62 + "', '" + maquina62.Text.ToString() + "', '" + categoria62.Text.ToString() + "', '" + b62.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario62.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion63.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 63 + "', '" + maquina63.Text.ToString() + "', '" + categoria63.Text.ToString() + "', '" + b63.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario63.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion64.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 64 + "', '" + maquina64.Text.ToString() + "', '" + categoria64.Text.ToString() + "', '" + b64.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario64.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion65.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 65 + "', '" + maquina65.Text.ToString() + "', '" + categoria65.Text.ToString() + "', '" + b65.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario65.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion66.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 66 + "', '" + maquina66.Text.ToString() + "', '" + categoria66.Text.ToString() + "', '" + b66.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario66.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }

        //        #endregion

        //        confirmar_guard.IsOpen = false;
        //        MessageBox.Show("Balance Guardado");
        //    }

        //    //si ya existe se eliminan los datos guardados y se cargan los nuevos
        //    else
        //    {
        //        #region eliminar_registros_anteriores()
        //        //se hace la consulta en sql para revisar si ya se ha guardado una version de ese balance
        //        string sql_a = "delete from lista_balances where identificador= '" + identificador_g + "'";
        //        string sql_b= "delete from maquinas where identificador= '" + identificador_g + "'";
        //        string sql_c = "delete from operarios where identificador= '" + identificador_g + "'";
        //        string sql_d = "delete from operaciones where identificador= '" + identificador_g + "'";
        //        SqlCommand cm_a= new SqlCommand(sql_a, cn);
        //        SqlCommand cm_b = new SqlCommand(sql_b, cn);
        //        SqlCommand cm_c = new SqlCommand(sql_c, cn);
        //        SqlCommand cm_d = new SqlCommand(sql_d, cn);


        //        cn.Open();
        //        SqlDataReader dr_a = cm_a.ExecuteReader();
        //        dr_a.Close();
        //        cn.Close();

        //        cn.Open();
        //        SqlDataReader dr_b = cm_b.ExecuteReader();
        //        dr_b.Close();
        //        cn.Close();

        //        cn.Open();
        //        SqlDataReader dr_c = cm_c.ExecuteReader();
        //        dr_c.Close();
        //        cn.Close();

        //        cn.Open();
        //        SqlDataReader dr_d = cm_d.ExecuteReader();
        //        dr_d.Close();
        //        cn.Close();
        //        #endregion
        //        #region ingresar_datos_en_tabla_lista()
        //        //definir valores de los datos a guardar en la tabla de la lista de balances
        //        string fecha_g = fecha_.Content.ToString();
        //        string estilo_g = estilo_.Content.ToString();
        //        string temporada_g = temporada_.Content.ToString();
        //        string sam_g = sam_.Content.ToString();
        //        string modulo_g = modulo_guard.Text.ToString();
        //        string corrida_g;
        //        if (string.IsNullOrEmpty(piezas_de_corrida.Text))
        //        {
        //            corrida_g = 0.ToString();
        //        }
        //        else
        //        {
        //            corrida_g = piezas_de_corrida.Text.ToString();
        //        }
        //        string horas_g;
        //        if (string.IsNullOrEmpty(horas_de_corrida.Text))
        //        {
        //            horas_g = 0.ToString();
        //        }
        //        else
        //        {
        //            horas_g = horas_de_corrida.Text.ToString();
        //        }
        //        string operarios_g = operarios_.Content.ToString();
        //        string eficiencia_g;
        //        try
        //        {
        //            decimal calculo_efic = Math.Round(Convert.ToDecimal(sam_.Content) * (Convert.ToDecimal(piezas_de_corrida.Text) / Convert.ToDecimal(horas_de_corrida.Text)) / Convert.ToDecimal(operarios_.Content), 0);
        //            eficiencia_g = calculo_efic.ToString();
        //        }
        //        catch
        //        {
        //            eficiencia_g = 0.ToString();
        //        }

        //        string piezas_g = piezas_por_hora.Content.ToString();
        //        string ingeniero_g = ingeniero_.Text.ToString();

        //        // ingresar datos a SQL en lista de balances 
        //        string sql = "insert into lista_balances(identificador, fecha_creacion, estilo, temporada, sam, modulo, corrida, horas, operarios, eficiencia, piezash, ingeniero, fecha_modificacion, version) values('" + identificador_g + "', '" + fecha_g + "', '" + estilo_g + "', '" + temporada_g + "', " + sam_g + ", '" + modulo_g + "', " + corrida_g + ", " + horas_g + ", " + operarios_g + ", " + eficiencia_g + ", " + piezas_g + ", '" + ingeniero_g + "', '" + DateTime.Now + "', '" + version_guard.Text.ToString() + "')";
        //        cn.Open();
        //        // se llenan la lista de operaciones con los datos de la consulta
        //        SqlCommand cm = new SqlCommand(sql, cn);
        //        SqlDataReader dr = cm.ExecuteReader();
        //        dr.Close();
        //        cn.Close();
        //        #endregion
        //        #region ingresar_datos_en_tabla_operarios

        //        foreach (TodoItem item in Operarios.Items)
        //        {
        //            cn.Open();
        //            string sql2 = "insert into operarios(identificador, codigo, nombre, asignado) values('" + identificador_g + "', '" + item.cod_oper + "', '" + item.Title.ToString() + "', '" + item.Completion + "')";
        //            SqlCommand cm2 = new SqlCommand(sql2, cn);
        //            SqlDataReader dr2 = cm2.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        #endregion
        //        #region ingresar_datos_en_tabla_operaciones
        //        foreach (TodoItem item in Operaciones.Items)
        //        {
        //            string nombre = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql3 = "insert into operaciones(identificador, correlativo, codigo, titulo, sam, ajuste, requerimiento, asignado, categoria) values('" + identificador_g + "', '" + item.correlativo.ToString() + "', '" + item.cod_opera + "', '" + nombre + "', '" + item.sam_cod + "', '" + item.ajuste_cod + "', '" + item.cap_cod + "', '" + item.Completion + "', '" + item.categoria + "')";
        //            SqlCommand cm3 = new SqlCommand(sql3, cn);
        //            SqlDataReader dr3 = cm3.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        #endregion
        //        #region ingresar_datos_en_tabla_maquinas()

        //        foreach (TodoItem item in Operacion1.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 1 + "', '" + maquina1.Text.ToString() + "', '" + categoria1.Text.ToString() + "', '" + b1.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario1.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion2.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 2 + "', '" + maquina2.Text.ToString() + "', '" + categoria2.Text.ToString() + "', '" + b2.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario2.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion3.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 3 + "', '" + maquina3.Text.ToString() + "', '" + categoria3.Text.ToString() + "', '" + b3.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario3.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion4.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 4 + "', '" + maquina4.Text.ToString() + "', '" + categoria4.Text.ToString() + "', '" + b4.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario4.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion5.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 5 + "', '" + maquina5.Text.ToString() + "', '" + categoria5.Text.ToString() + "', '" + b5.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario5.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion6.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 6 + "', '" + maquina6.Text.ToString() + "', '" + categoria6.Text.ToString() + "', '" + b6.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario6.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion7.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 7 + "', '" + maquina7.Text.ToString() + "', '" + categoria7.Text.ToString() + "', '" + b7.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario7.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion8.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 8 + "', '" + maquina8.Text.ToString() + "', '" + categoria8.Text.ToString() + "', '" + b8.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario8.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion9.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 9 + "', '" + maquina9.Text.ToString() + "', '" + categoria9.Text.ToString() + "', '" + b9.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario9.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion10.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 10 + "', '" + maquina10.Text.ToString() + "', '" + categoria10.Text.ToString() + "', '" + b10.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario10.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion11.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 11 + "', '" + maquina11.Text.ToString() + "', '" + categoria11.Text.ToString() + "', '" + b11.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario11.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion12.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 12 + "', '" + maquina12.Text.ToString() + "', '" + categoria12.Text.ToString() + "', '" + b12.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario12.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion13.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 13 + "', '" + maquina13.Text.ToString() + "', '" + categoria13.Text.ToString() + "', '" + b13.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario13.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion14.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 14 + "', '" + maquina14.Text.ToString() + "', '" + categoria14.Text.ToString() + "', '" + b14.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario14.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion15.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 15 + "', '" + maquina15.Text.ToString() + "', '" + categoria15.Text.ToString() + "', '" + b15.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario15.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion16.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 16 + "', '" + maquina16.Text.ToString() + "', '" + categoria16.Text.ToString() + "', '" + b16.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario16.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion17.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 17 + "', '" + maquina17.Text.ToString() + "', '" + categoria17.Text.ToString() + "', '" + b17.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario17.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion18.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 18 + "', '" + maquina18.Text.ToString() + "', '" + categoria18.Text.ToString() + "', '" + b18.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario18.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion19.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 19 + "', '" + maquina19.Text.ToString() + "', '" + categoria19.Text.ToString() + "', '" + b19.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario19.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion20.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 20 + "', '" + maquina20.Text.ToString() + "', '" + categoria20.Text.ToString() + "', '" + b20.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario20.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion21.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 21 + "', '" + maquina21.Text.ToString() + "', '" + categoria21.Text.ToString() + "', '" + b21.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario21.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion22.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 22 + "', '" + maquina22.Text.ToString() + "', '" + categoria22.Text.ToString() + "', '" + b22.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario22.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion23.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 23 + "', '" + maquina23.Text.ToString() + "', '" + categoria23.Text.ToString() + "', '" + b23.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario23.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion24.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 24 + "', '" + maquina24.Text.ToString() + "', '" + categoria24.Text.ToString() + "', '" + b24.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario24.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion25.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 25 + "', '" + maquina25.Text.ToString() + "', '" + categoria25.Text.ToString() + "', '" + b25.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario25.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion26.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 26 + "', '" + maquina26.Text.ToString() + "', '" + categoria26.Text.ToString() + "', '" + b26.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario26.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion27.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 27 + "', '" + maquina27.Text.ToString() + "', '" + categoria27.Text.ToString() + "', '" + b27.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario27.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion28.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 28 + "', '" + maquina28.Text.ToString() + "', '" + categoria28.Text.ToString() + "', '" + b28.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario28.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion29.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 29 + "', '" + maquina29.Text.ToString() + "', '" + categoria29.Text.ToString() + "', '" + b29.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario29.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion30.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 30 + "', '" + maquina30.Text.ToString() + "', '" + categoria30.Text.ToString() + "', '" + b30.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario30.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion31.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 31 + "', '" + maquina31.Text.ToString() + "', '" + categoria31.Text.ToString() + "', '" + b31.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario31.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion32.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 32 + "', '" + maquina32.Text.ToString() + "', '" + categoria32.Text.ToString() + "', '" + b32.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario32.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion33.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 33 + "', '" + maquina33.Text.ToString() + "', '" + categoria33.Text.ToString() + "', '" + b33.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario33.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion34.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 34 + "', '" + maquina34.Text.ToString() + "', '" + categoria34.Text.ToString() + "', '" + b34.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario34.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion35.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 35 + "', '" + maquina35.Text.ToString() + "', '" + categoria35.Text.ToString() + "', '" + b35.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario35.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion36.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 36 + "', '" + maquina36.Text.ToString() + "', '" + categoria36.Text.ToString() + "', '" + b36.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario36.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion37.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 37 + "', '" + maquina37.Text.ToString() + "', '" + categoria37.Text.ToString() + "', '" + b37.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario37.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion38.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 38 + "', '" + maquina38.Text.ToString() + "', '" + categoria38.Text.ToString() + "', '" + b38.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario38.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion39.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 39 + "', '" + maquina39.Text.ToString() + "', '" + categoria39.Text.ToString() + "', '" + b39.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario39.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion40.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 40 + "', '" + maquina40.Text.ToString() + "', '" + categoria40.Text.ToString() + "', '" + b40.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario40.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion41.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 41 + "', '" + maquina41.Text.ToString() + "', '" + categoria41.Text.ToString() + "', '" + b41.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario41.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion42.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 42 + "', '" + maquina42.Text.ToString() + "', '" + categoria42.Text.ToString() + "', '" + b42.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario42.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion43.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 43 + "', '" + maquina43.Text.ToString() + "', '" + categoria43.Text.ToString() + "', '" + b43.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario43.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion44.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 44 + "', '" + maquina44.Text.ToString() + "', '" + categoria44.Text.ToString() + "', '" + b44.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario44.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion45.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 45 + "', '" + maquina45.Text.ToString() + "', '" + categoria45.Text.ToString() + "', '" + b45.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario45.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion46.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 46 + "', '" + maquina46.Text.ToString() + "', '" + categoria46.Text.ToString() + "', '" + b46.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario46.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion47.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 47 + "', '" + maquina47.Text.ToString() + "', '" + categoria47.Text.ToString() + "', '" + b47.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario47.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion48.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 48 + "', '" + maquina48.Text.ToString() + "', '" + categoria48.Text.ToString() + "', '" + b48.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario48.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion49.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 49 + "', '" + maquina49.Text.ToString() + "', '" + categoria49.Text.ToString() + "', '" + b49.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario49.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion50.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 50 + "', '" + maquina50.Text.ToString() + "', '" + categoria50.Text.ToString() + "', '" + b50.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario50.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion51.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 51 + "', '" + maquina51.Text.ToString() + "', '" + categoria51.Text.ToString() + "', '" + b51.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario51.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion52.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 52 + "', '" + maquina52.Text.ToString() + "', '" + categoria52.Text.ToString() + "', '" + b52.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario52.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion53.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 53 + "', '" + maquina53.Text.ToString() + "', '" + categoria53.Text.ToString() + "', '" + b53.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario53.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion54.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 54 + "', '" + maquina54.Text.ToString() + "', '" + categoria54.Text.ToString() + "', '" + b54.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario54.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion55.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 55 + "', '" + maquina55.Text.ToString() + "', '" + categoria55.Text.ToString() + "', '" + b55.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario55.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion56.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 56 + "', '" + maquina56.Text.ToString() + "', '" + categoria56.Text.ToString() + "', '" + b56.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario56.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion57.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 57 + "', '" + maquina57.Text.ToString() + "', '" + categoria57.Text.ToString() + "', '" + b57.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario57.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion58.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 58 + "', '" + maquina58.Text.ToString() + "', '" + categoria58.Text.ToString() + "', '" + b58.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario58.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion59.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 59 + "', '" + maquina59.Text.ToString() + "', '" + categoria59.Text.ToString() + "', '" + b59.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario59.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion60.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 60 + "', '" + maquina60.Text.ToString() + "', '" + categoria60.Text.ToString() + "', '" + b60.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario60.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion61.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 61 + "', '" + maquina61.Text.ToString() + "', '" + categoria61.Text.ToString() + "', '" + b61.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario61.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion62.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 62 + "', '" + maquina62.Text.ToString() + "', '" + categoria62.Text.ToString() + "', '" + b62.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario62.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion63.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 63 + "', '" + maquina63.Text.ToString() + "', '" + categoria63.Text.ToString() + "', '" + b63.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario63.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion64.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 64 + "', '" + maquina64.Text.ToString() + "', '" + categoria64.Text.ToString() + "', '" + b64.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario64.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion65.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 65 + "', '" + maquina65.Text.ToString() + "', '" + categoria65.Text.ToString() + "', '" + b65.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario65.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }
        //        foreach (TodoItem item in Operacion66.Items)
        //        {
        //            string operacion = item.Title.ToString().Replace("'", "");
        //            cn.Open();
        //            string sql4 = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + identificador_g + "', '" + 66 + "', '" + maquina66.Text.ToString() + "', '" + categoria66.Text.ToString() + "', '" + b66.Background.ToString() + "', '" + item.correlativo + "', '" + operacion + "', '" + item.cap_cod + "', '" + operario66.Text.ToString() + "')";
        //            SqlCommand cm4 = new SqlCommand(sql4, cn);
        //            SqlDataReader dr4 = cm4.ExecuteReader();
        //            dr.Close();
        //            cn.Close();
        //        }

        //        #endregion

        //        confirmar_guard.IsOpen = false;
        //        MessageBox.Show("Balance Guardado");
        //    }
        //}
        #endregion

        #region rebalance()

        private void tiempo_tomado_LostFocus(object sender, RoutedEventArgs e)
        {
            //recalcular_rebalance();

            //actualizar_grafica_reba();
        }

        private void rebalance_bt_Click(object sender, RoutedEventArgs e)
        {
            //consolidar_elemento_balance();

            //consultar_toma_de_tiempos();

            //recalcular_rebalance();

            //actualizar_grafica_reba();
        }

        private void actuali_balance_Click(object sender, RoutedEventArgs e)
        {
            // se valdia el modulo que se ha escogido si no ha escogido podria generar error
            string modulo_va;
            if (modulo.SelectedIndex < 0)
            {
                modulo_va = "GENERICO";
            }
            else
            {
                modulo_va = modulo.SelectedItem.ToString();
            }

            // se eliminan tomas de tiempo anteriores para esa version de layout
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            cn.Open();
            string sql_v = "delete from toma_de_tiempos where modulo= '" + modulo_va + "' and estilo= '" + estilo_.Content.ToString() + "' and temporada= '" + temporada_.Content.ToString() + "' and version= '" + version_.Text.ToString() + "'";
            SqlCommand cm_v = new SqlCommand(sql_v, cn);
            SqlDataReader dr_v = cm_v.ExecuteReader();
            dr_v.Close();
            cn.Close();

            //se ingresan los datos nuevos de tiempo ingresados
            foreach (rebalance item in rebalance_.Items)
            {
                cn.Open();
                string sql2 = "insert into toma_de_tiempos (fecha, modulo, estilo, temporada, version, codigo, nombre, titulo, operacion, ajuste, sam, tiempo_objetivo) values('" + System.DateTime.Now.ToString() + "', '" + modulo_va + "', '" + estilo_.Content.ToString() + "', '" + temporada_.Content.ToString() + "', '" + version_.Text.ToString() + "', '" + item.codigo + "', '" + item.operario + "', '" + item.operacion + "', '" +  item.operacion_cod + "', '" +  item.ajuste + "', '" + item.sam + "', '" + item.tiempo + "')";
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlDataReader dr2 = cm2.ExecuteReader();
                dr2.Close();
                cn.Close();
            }
        }
        #endregion

        #region listBoxReceptorDeDatos
        private void receptor(object sender, DragEventArgs e)
        {
            StackPanel estacion = (StackPanel)sender;
            object informacion = e.Data.GetData(typeof(elementoListBox));

            elementoListBox informacionElemento = informacion as elementoListBox;

                foreach (Object elemento in estacion.Children)
                {
                    if (elemento.GetType() == typeof(ListBox) && informacionElemento.identificador == "operacion")
                    {
                        ((ListBox)elemento).Items.Add(new elementoListBox() { tituloOperacion= informacionElemento.tituloOperacion, asignadoOperacion=informacionElemento.requeridoOperacion, correlativoOperacion=informacionElemento.correlativoOperacion });
                    }
                    else if (elemento.GetType() == typeof(TextBox) && informacionElemento.identificador == "operacion")
                    {
                        ((TextBox)elemento).Text = informacionElemento.ajusteMaquina;
                    }
                    else if (elemento.GetType() == typeof(TextBlock) && informacionElemento.identificador == "operacion")
                    {
                    ((TextBlock)elemento).Text = informacionElemento.categoriaMaquina;
                    }
                    else if (elemento.GetType() == typeof(Label) && informacionElemento.identificador == "operario")
                    {
                        ((Label)elemento).Content = informacionElemento.nombreOperario;
                    }
                }
            CalculoAsignadoPorOperacion();
            actualizarGrafica();

        }
        #endregion
       
        #region coloresDeEstacion
        private void colorVerde(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.Green;
            //se ejecuta el calculo del resumen de operacion para obtener el resumen de maquina
            CalculoAsignadoPorOperacion();
        }

        private void colorRojo(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.Red;
            //se ejecuta el calculo del resumen de operacion para obtener el resumen de maquina
            CalculoAsignadoPorOperacion();
        }

        private void colorAzul(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.Blue;
            //se ejecuta el calculo del resumen de operacion para obtener el resumen de maquina
            CalculoAsignadoPorOperacion();
        }

        private void colorAnaranjado(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.Orange;
            //se ejecuta el calculo del resumen de operacion para obtener el resumen de maquina
            CalculoAsignadoPorOperacion();
        }

        private void colorAmarillo(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.Yellow;
            //se ejecuta el calculo del resumen de operacion para obtener el resumen de maquina
            CalculoAsignadoPorOperacion();
        }

        #endregion

        #region coloresBarraAsignado
        private void progressBarAsignado_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBar progressBar = (ProgressBar)sender;
            if (progressBar.Value > 0 && progressBar.Value <= 0.9)
            {
                progressBar.Foreground = Brushes.Yellow;
            }
            if (progressBar.Value > 0.9 && progressBar.Value <= 1)
            {
                progressBar.Foreground = Brushes.Green;
            }
            if (progressBar.Value > 1)
            {
                progressBar.Foreground = Brushes.Red;
            }


        }
        #endregion

        #region calculosGenerales
        #region soloIngresarNumerosenTextBox
        private void solo_numeros(object sender, KeyEventArgs e)
        {       
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void soloNumerosDecimales(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Decimal))
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region calculoDeAsigandoPorOperacion
        private void AsignadoPorOperacion(object sender, RoutedEventArgs e)
        {
            CalculoAsignadoPorOperacion();
            actualizarGrafica();
        }
        private void CalculoAsignadoPorOperacion()
        {
            // se declara la lista donde se consolidara lo asignado a cada operacion por estacion, operarios asignados y resumen de maquinas
            List<elementoListBox> consolidadoAsignaciones = new List<elementoListBox>();
            List<elementoListBox> consolidadoOperarios = new List<elementoListBox>();
            List<maquina> consolidadoMaquinas = new List<maquina>();
            //Codigo para verificar asignado por operacion se recorrera por cada arteria (para facilitar especificacion de donde estan los listBox que contienen los datos)

            //Recorrer el areadePreparacion (cada arteria es un StackPanel)
            //Dentro de cada arteria hay bordes que contienen todos los elementos de una estacion de trabajo
            #region areaPreparacion
            foreach (Border estacion in areaPreparacion.Children)
            {
                string operario="";
                double asignado=0;
                string categoria="";
                string color = "blanco";
                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    color = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    color = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    color = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    color = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    color = "amarillo";
                }
                #endregion

                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    //Si el objeto es textBlock se carga la categoria (el operario tiene eficiencia historia por categoria)
                    if (elemento.GetType() == typeof(TextBlock))
                    {
                        categoria = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                            asignado = asignado + item.asignadoOperacion;
                        }
                    }
                }

                //Por cada estacion (border) se agrega el operario y los datos que le corresponden
                consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = asignado, categoriaMaquina = categoria });
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color});
                
            }
            #endregion

            //Repetir codigo para demas arterias y para el anganche
            #region arteriaUno
            foreach (Border estacion in arteriaUno.Children)
            {
                string operario = "";
                double asignado = 0;
                string categoria = "";
                string color = "blanco";
                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    color = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    color = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    color = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    color = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    color = "amarillo";
                }
                #endregion
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(TextBlock))
                    {
                        categoria = ((TextBlock)elemento).Text;
                    }
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                            asignado = asignado + item.asignadoOperacion;
                        }
                    }
                }
                consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = asignado, categoriaMaquina = categoria });
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
            }
            #endregion
            #region arteriaDos
            foreach (Border estacion in arteriaDos.Children)
            {
                string operario = "";
                double asignado = 0;
                string categoria = "";
                string color = "blanco";
                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    color = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    color = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    color = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    color = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    color = "amarillo";
                }
                #endregion
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(TextBlock))
                    {
                        categoria = ((TextBlock)elemento).Text;
                    }
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                            asignado = asignado + item.asignadoOperacion;
                        }
                    }
                }
                consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = asignado, categoriaMaquina = categoria });
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
            }
            #endregion
            #region arteriaTres
            foreach (Border estacion in arteriaTres.Children)
            {
                string operario = "";
                double asignado = 0;
                string categoria = "";
                string color = "blanco";
                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    color = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    color = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    color = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    color = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    color = "amarillo";
                }
                #endregion
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(TextBlock))
                    {
                        categoria = ((TextBlock)elemento).Text;
                    }
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                            asignado = asignado + item.asignadoOperacion;
                        }
                    }
                }
                consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = asignado, categoriaMaquina = categoria });
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
            }
            #endregion
            #region arteriaCuatro
            foreach (Border estacion in arteriaCuatro.Children)
            {
                string operario = "";
                double asignado = 0;
                string categoria = "";
                string color = "blanco";
                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    color = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    color = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    color = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    color = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    color = "amarillo";
                }
                #endregion
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(TextBlock))
                    {
                        categoria = ((TextBlock)elemento).Text;
                    }
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                            asignado = asignado + item.asignadoOperacion;
                        }
                    }
                }
                consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = asignado, categoriaMaquina = categoria });
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
            }
            #endregion
            #region estacionEngancha
            //lista de operaciones para enganche se revisa directamente el listbox porque no esta dentro de las arterias
            double asignadoEnganche = 0;
            foreach (elementoListBox item in listBoxEnganche.Items)
            {
                consolidadoAsignaciones.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion });
                asignadoEnganche = asignadoEnganche + item.asignadoOperacion;
            }

            #region colorEnString
            string color_ = "";
            if (borderEstacionEnganche.Background == Brushes.Red)
            {
                color_ = "rojo";
            }
            else if (borderEstacionEnganche.Background == Brushes.Blue)
            {
                color_ = "azul";
            }
            else if (borderEstacionEnganche.Background == Brushes.Green)
            {
                color_ = "verde";
            }
            if (borderEstacionEnganche.Background == Brushes.Orange)
            {
                color_ = "anaranjado";
            }
            if (borderEstacionEnganche.Background == Brushes.Yellow)
            {
                color_ = "amarillo";
            }
            #endregion

            //datos de enganche se cargan aparte
            consolidadoOperarios.Add(new elementoListBox { nombreOperario = operarioEnganche.Content.ToString(), asignadoOperario = asignadoEnganche, categoriaMaquina = "manual" });
            consolidadoMaquinas.Add(new maquina { categoriaMaquina = textBlockEngancheCategoria.Text, colorAjuste = color_ });
            #endregion

            //se hacen las nuevas listas
            #region crearNuevaListaDeOperaciones
            //se crea una lista de operaciones donde se colocara el valor de lo asignado
            List<elementoListBox> nuevaListaOperaciones = new List<elementoListBox>();
            foreach (elementoListBox operacion in Operaciones.Items)
            {
                double asignadoTotal = 0;
                double relacionAsignacion = 0;
                foreach (elementoListBox asigando in consolidadoAsignaciones)
                {
                    if (operacion.correlativoOperacion == asigando.correlativoOperacion)
                    {
                        asignadoTotal = asignadoTotal + asigando.asignadoOperacion;
                    }
                }
                relacionAsignacion = asignadoTotal / operacion.requeridoOperacion;

                nuevaListaOperaciones.Add(new elementoListBox { identificador = "operacion", correlativoOperacion = operacion.correlativoOperacion, nombreOperacion = operacion.nombreOperacion, tituloOperacion = operacion.tituloOperacion, samOperacion = operacion.samOperacion, asignadoOperacion = relacionAsignacion, requeridoOperacion = operacion.requeridoOperacion, ajusteMaquina = operacion.ajusteMaquina, categoriaMaquina = operacion.categoriaMaquina });
            }
            Operaciones.ItemsSource = nuevaListaOperaciones;
            #endregion
            #region crearNuevaListaOperarios
            //se crea nueva lista de opearios con lo asignado
            List<elementoListBox> nuevaListaOperarios = new List<elementoListBox>();
            foreach (elementoListBox operario in Operarios.Items)
            {
                double asignadoTotal = 0;
                foreach (elementoListBox asigando in consolidadoOperarios)
                {
                    if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina=="atracadora")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.atracadoraOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "varias")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.variasOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "bonding")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.bondingOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "multiaguja")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.multiagujaOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "zig-zag")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.zigzagOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "flat")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.flatOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "cover")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.coverOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "transfer")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.transferOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "manual")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.manualOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "rana")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.ranaOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "plana")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.planaOperario;
                    }
                    else if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "plancha")
                    {
                        asignadoTotal = (asignadoTotal + asigando.asignadoOperario) / operario.variasOperario;
                    }
                }
                nuevaListaOperarios.Add(new elementoListBox() { identificador = "operario", codigoOperario = operario.codigoOperario, nombreOperario = operario.nombreOperario, asignadoOperario = asignadoTotal, planaOperario = operario.planaOperario, ranaOperario = operario.ranaOperario, flatOperario = operario.flatOperario, coverOperario = operario.coverOperario, transferOperario = operario.transferOperario, atracadoraOperario = operario.atracadoraOperario, planchaOperario = operario.planchaOperario, bondingOperario = operario.bondingOperario, zigzagOperario = operario.zigzagOperario, multiagujaOperario = operario.multiagujaOperario, manualOperario = operario.manualOperario, variasOperario = operario.variasOperario});
            }
            Operarios.ItemsSource = nuevaListaOperarios;


            #endregion
            #region crearNuevaListaMaquinas
            List<maquina> nuevaListaMaquinas = new List<maquina>();
            foreach(maquina maquina in resumen_maquinas.Items)
            {
                int ii=0, ie=0, ee=0, ei=0, na = 0;
                foreach(maquina item in consolidadoMaquinas)
                {
                    if (maquina.categoriaMaquina == item.categoriaMaquina)
                    {
                        switch (item.colorAjuste)
                        {
                            case "rojo":
                                ii = ii + 1;
                                break;
                            case "verde":
                                ee = ee + 1;
                                break;
                            case "amarillo":
                                ie = ie + 1;
                                break;
                            case "azul":
                                na = na + 1;
                                break;
                            case "anaranjado":
                                ei = ei + 1;
                                break;
                            default:
                                break;
                        }
                    }
                }
                nuevaListaMaquinas.Add(new maquina { categoriaMaquina = maquina.categoriaMaquina, ii = ii, ie = ie, ee = ee, ei = ei, na = na });
            }
            resumen_maquinas.ItemsSource = nuevaListaMaquinas;
            #endregion
        }
        #endregion
        #region eliminarOperacionAsigandaEnUnaEstacionDeTrabajo
        private void eliminarItem(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            try
            {
                listBox.Items.RemoveAt(listBox.Items.IndexOf(listBox.SelectedItem));
                CalculoAsignadoPorOperacion();
                actualizarGrafica();
            }
            catch
            {
            }

        }
        #endregion
        #region EliminarOperarioDeListaDeOperarios
        private void Operarios_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar el operario seleccionado cuando se presiona una letra definida
            if (e.Key == Key.D)
            {
                int elementoSeleccionado = Operarios.SelectedIndex + 1;
                List<elementoListBox> items = new List<elementoListBox>();
                int operarios_conteo = 0;
                foreach (elementoListBox item in Operarios.Items)
                {
                    operarios_conteo = operarios_conteo + 1;
                    if (operarios_conteo == elementoSeleccionado)
                    {
                    }
                    else
                    {
                        items.Add(new elementoListBox {identificador = "operario", codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, asignadoOperario = item.asignadoOperario , planaOperario = item.planaOperario, ranaOperario = item.ranaOperario, flatOperario = item.flatOperario, coverOperario = item.coverOperario, transferOperario = item.transferOperario, atracadoraOperario = item.atracadoraOperario, planchaOperario = item.planchaOperario, bondingOperario = item.bondingOperario, zigzagOperario = item.zigzagOperario, multiagujaOperario = item.multiagujaOperario, manualOperario = item.manualOperario, variasOperario = item.variasOperario});
                    }
                }
                Operarios.ItemsSource = items;

                // Actualizar grafica
                actualizarGrafica();

                //Actualizar operacion con mas y menos carga
            //    operaciones_subcarga_sobrecarga();

                //se agrega el nuevo numero de operarios que se ha calculado
                operarios_.Content = (operarios_conteo - 1).ToString();
                operarios_2.Content = (operarios_conteo - 1).ToString();


                eficiencia_.Content = ejecutarCalculoDeEficiencia();
                eficiencia_2.Content = ejecutarCalculoDeEficiencia();
            }
        }
        #endregion
        #region ActualizarDatosDeGrafica
        private void actualizarGrafica()
        {
            //se crea una lista de strings para las etiquetas del eje horizontal (los nombres de los operarios) solo se agregan los que ya han sido asignados
            List<string> listaDeOperarios = new List<string>();
            foreach (elementoListBox item in Operarios.Items)
            {
                if (item.asignadoOperario > 0)
                {
                    listaDeOperarios.Add(item.nombreOperario);
                }
            }

            //se limpian los datos cargados anteriormente para poder volver a cargar
            grafico.AxisX.Clear();
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();

            //se agrega la lista de operarios hecha al principio
            grafico.AxisX.Add(new Axis() { Labels = listaDeOperarios.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 }, });

            //se agregan los valores de las cargas en las columnas
            foreach (elementoListBox item in Operarios.Items)
            {
                if (item.asignadoOperario > 0)
                {
                    SeriesCollection[0].Values.Add(item.asignadoOperario);
                    SeriesCollection[1].Values.Add(1d);
                    SeriesCollection[2].Values.Add(0.9d);
                }
            };
        }
        #endregion
        #endregion
    }
    #region datos_globales
    static class Global2
    {
        private static string _operacion_s = "";

        public static string operacion_s
        {
            get { return _operacion_s; }
            set { _operacion_s = value; }
        }
    }

    static class impresion_global
    {

        #region consolidado_maquinas
        public class elementos
        {
            public string maquina_lista { get; set; }
            public int ii { get; set; }
            public int ie { get; set; }
            public int ei { get; set; }
            public int ee { get; set; }
            public int na { get; set; }
        }
        #endregion

        #region maquinas
        private static string _maquina_1 = "";
        private static string _maquina_2 = "";
        private static string _maquina_3 = "";
        private static string _maquina_4 = "";
        private static string _maquina_5 = "";
        private static string _maquina_6 = "";
        private static string _maquina_7 = "";
        private static string _maquina_8 = "";
        private static string _maquina_9 = "";
        private static string _maquina_10 = "";
        private static string _maquina_11 = "";
        private static string _maquina_12 = "";
        private static string _maquina_13 = "";
        private static string _maquina_14 = "";
        private static string _maquina_15 = "";
        private static string _maquina_16 = "";
        private static string _maquina_17 = "";
        private static string _maquina_18 = "";
        private static string _maquina_19 = "";
        private static string _maquina_20 = "";
        private static string _maquina_21 = "";
        private static string _maquina_22 = "";
        private static string _maquina_23 = "";
        private static string _maquina_24 = "";
        private static string _maquina_25 = "";
        private static string _maquina_26 = "";
        private static string _maquina_27 = "";
        private static string _maquina_28 = "";
        private static string _maquina_29 = "";
        private static string _maquina_30 = "";
        private static string _maquina_31 = "";
        private static string _maquina_32 = "";
        private static string _maquina_33 = "";
        private static string _maquina_34 = "";
        private static string _maquina_35 = "";
        private static string _maquina_36 = "";
        private static string _maquina_37 = "";
        private static string _maquina_38 = "";
        private static string _maquina_39 = "";
        private static string _maquina_40 = "";
        private static string _maquina_41 = "";
        private static string _maquina_42 = "";
        private static string _maquina_43 = "";
        private static string _maquina_44 = "";
        private static string _maquina_45 = "";
        private static string _maquina_46 = "";
        private static string _maquina_47 = "";
        private static string _maquina_48 = "";
        private static string _maquina_49 = "";
        private static string _maquina_50 = "";
        private static string _maquina_51 = "";
        private static string _maquina_52 = "";
        private static string _maquina_53 = "";
        private static string _maquina_54 = "";
        private static string _maquina_55 = "";
        private static string _maquina_56 = "";
        private static string _maquina_57 = "";
        private static string _maquina_58 = "";
        private static string _maquina_59 = "";
        private static string _maquina_60 = "";
        private static string _maquina_61 = "";
        private static string _maquina_62 = "";
        private static string _maquina_63 = "";
        private static string _maquina_64 = "";
        private static string _maquina_65 = "";
        private static string _maquina_66 = "";


        #endregion

        #region operarios
        private static string _operario_1 = "";
        private static string _operario_2 = "";
        private static string _operario_3 = "";
        private static string _operario_4 = "";
        private static string _operario_5 = "";
        private static string _operario_6 = "";
        private static string _operario_7 = "";
        private static string _operario_8 = "";
        private static string _operario_9 = "";
        private static string _operario_10 = "";
        private static string _operario_11 = "";
        private static string _operario_12 = "";
        private static string _operario_13 = "";
        private static string _operario_14 = "";
        private static string _operario_15 = "";
        private static string _operario_16 = "";
        private static string _operario_17 = "";
        private static string _operario_18 = "";
        private static string _operario_19 = "";
        private static string _operario_20 = "";
        private static string _operario_21 = "";
        private static string _operario_22 = "";
        private static string _operario_23 = "";
        private static string _operario_24 = "";
        private static string _operario_25 = "";
        private static string _operario_26 = "";
        private static string _operario_27 = "";
        private static string _operario_28 = "";
        private static string _operario_29 = "";
        private static string _operario_30 = "";
        private static string _operario_31 = "";
        private static string _operario_32 = "";
        private static string _operario_33 = "";
        private static string _operario_34 = "";
        private static string _operario_35 = "";
        private static string _operario_36 = "";
        private static string _operario_37 = "";
        private static string _operario_38 = "";
        private static string _operario_39 = "";
        private static string _operario_40 = "";
        private static string _operario_41 = "";
        private static string _operario_42 = "";
        private static string _operario_43 = "";
        private static string _operario_44 = "";
        private static string _operario_45 = "";
        private static string _operario_46 = "";
        private static string _operario_47 = "";
        private static string _operario_48 = "";
        private static string _operario_49 = "";
        private static string _operario_50 = "";
        private static string _operario_51 = "";
        private static string _operario_52 = "";
        private static string _operario_53 = "";
        private static string _operario_54 = "";
        private static string _operario_55 = "";
        private static string _operario_56 = "";
        private static string _operario_57 = "";
        private static string _operario_58 = "";
        private static string _operario_59 = "";
        private static string _operario_60 = "";
        private static string _operario_61 = "";
        private static string _operario_62 = "";
        private static string _operario_63 = "";
        private static string _operario_64 = "";
        private static string _operario_65 = "";
        private static string _operario_66 = "";
        #endregion

        #region colores
        private static Brush _color_1 = Brushes.White;
        private static Brush _color_2 = Brushes.White;
        private static Brush _color_3 = Brushes.White;
        private static Brush _color_4 = Brushes.White;
        private static Brush _color_5 = Brushes.White;
        private static Brush _color_6 = Brushes.White;
        private static Brush _color_7 = Brushes.White;
        private static Brush _color_8 = Brushes.White;
        private static Brush _color_9 = Brushes.White;
        private static Brush _color_10 = Brushes.White;
        private static Brush _color_11 = Brushes.White;
        private static Brush _color_12 = Brushes.White;
        private static Brush _color_13 = Brushes.White;
        private static Brush _color_14 = Brushes.White;
        private static Brush _color_15 = Brushes.White;
        private static Brush _color_16 = Brushes.White;
        private static Brush _color_17 = Brushes.White;
        private static Brush _color_18 = Brushes.White;
        private static Brush _color_19 = Brushes.White;
        private static Brush _color_20 = Brushes.White;
        private static Brush _color_21 = Brushes.White;
        private static Brush _color_22 = Brushes.White;
        private static Brush _color_23 = Brushes.White;
        private static Brush _color_24 = Brushes.White;
        private static Brush _color_25 = Brushes.White;
        private static Brush _color_26 = Brushes.White;
        private static Brush _color_27 = Brushes.White;
        private static Brush _color_28 = Brushes.White;
        private static Brush _color_29 = Brushes.White;
        private static Brush _color_30 = Brushes.White;
        private static Brush _color_31 = Brushes.White;
        private static Brush _color_32 = Brushes.White;
        private static Brush _color_33 = Brushes.White;
        private static Brush _color_34 = Brushes.White;
        private static Brush _color_35 = Brushes.White;
        private static Brush _color_36 = Brushes.White;
        private static Brush _color_37 = Brushes.White;
        private static Brush _color_38 = Brushes.White;
        private static Brush _color_39 = Brushes.White;
        private static Brush _color_40 = Brushes.White;
        private static Brush _color_41 = Brushes.White;
        private static Brush _color_42 = Brushes.White;
        private static Brush _color_43 = Brushes.White;
        private static Brush _color_44 = Brushes.White;
        private static Brush _color_45 = Brushes.White;
        private static Brush _color_46 = Brushes.White;
        private static Brush _color_47 = Brushes.White;
        private static Brush _color_48 = Brushes.White;
        private static Brush _color_49 = Brushes.White;
        private static Brush _color_50 = Brushes.White;
        private static Brush _color_51 = Brushes.White;
        private static Brush _color_52 = Brushes.White;
        private static Brush _color_53 = Brushes.White;
        private static Brush _color_54 = Brushes.White;
        private static Brush _color_55 = Brushes.White;
        private static Brush _color_56 = Brushes.White;
        private static Brush _color_57 = Brushes.White;
        private static Brush _color_58 = Brushes.White;
        private static Brush _color_59 = Brushes.White;
        private static Brush _color_60 = Brushes.White;
        private static Brush _color_61 = Brushes.White;
        private static Brush _color_62 = Brushes.White;
        private static Brush _color_63 = Brushes.White;
        private static Brush _color_64 = Brushes.White;
        private static Brush _color_65 = Brushes.White;
        private static Brush _color_66 = Brushes.White;


        #endregion

        #region general
        private static string _estilo = "";
        private static string _temporada = "";
        private static string _sam = "";
        private static string _modulo = "";
        private static string _operarios = "";
        private static string _eficiencia = "";
        private static string _sobrecarga = "";
        private static string _subutilizado = "";
        private static string _lote = "";
        private static string _ingeniero = "";
        private static string _fecha = "";
        private static string _tipo = "";

        #endregion

        #region maquina_varaible
        public static string maquina_1 { get { return _maquina_1; } set { _maquina_1 = value; } }
        public static string maquina_2 { get { return _maquina_2; } set { _maquina_2 = value; } }
        public static string maquina_3 { get { return _maquina_3; } set { _maquina_3 = value; } }
        public static string maquina_4 { get { return _maquina_4; } set { _maquina_4 = value; } }
        public static string maquina_5 { get { return _maquina_5; } set { _maquina_5 = value; } }
        public static string maquina_6 { get { return _maquina_6; } set { _maquina_6 = value; } }
        public static string maquina_7 { get { return _maquina_7; } set { _maquina_7 = value; } }
        public static string maquina_8 { get { return _maquina_8; } set { _maquina_8 = value; } }
        public static string maquina_9 { get { return _maquina_9; } set { _maquina_9 = value; } }
        public static string maquina_10 { get { return _maquina_10; } set { _maquina_10 = value; } }
        public static string maquina_11 { get { return _maquina_11; } set { _maquina_11 = value; } }
        public static string maquina_12 { get { return _maquina_12; } set { _maquina_12 = value; } }
        public static string maquina_13 { get { return _maquina_13; } set { _maquina_13 = value; } }
        public static string maquina_14 { get { return _maquina_14; } set { _maquina_14 = value; } }
        public static string maquina_15 { get { return _maquina_15; } set { _maquina_15 = value; } }
        public static string maquina_16 { get { return _maquina_16; } set { _maquina_16 = value; } }
        public static string maquina_17 { get { return _maquina_17; } set { _maquina_17 = value; } }
        public static string maquina_18 { get { return _maquina_18; } set { _maquina_18 = value; } }
        public static string maquina_19 { get { return _maquina_19; } set { _maquina_19 = value; } }
        public static string maquina_20 { get { return _maquina_20; } set { _maquina_20 = value; } }
        public static string maquina_21 { get { return _maquina_21; } set { _maquina_21 = value; } }
        public static string maquina_22 { get { return _maquina_22; } set { _maquina_22 = value; } }
        public static string maquina_23 { get { return _maquina_23; } set { _maquina_23 = value; } }
        public static string maquina_24 { get { return _maquina_24; } set { _maquina_24 = value; } }
        public static string maquina_25 { get { return _maquina_25; } set { _maquina_25 = value; } }
        public static string maquina_26 { get { return _maquina_26; } set { _maquina_26 = value; } }
        public static string maquina_27 { get { return _maquina_27; } set { _maquina_27 = value; } }
        public static string maquina_28 { get { return _maquina_28; } set { _maquina_28 = value; } }
        public static string maquina_29 { get { return _maquina_29; } set { _maquina_29 = value; } }
        public static string maquina_30 { get { return _maquina_30; } set { _maquina_30 = value; } }
        public static string maquina_31 { get { return _maquina_31; } set { _maquina_31 = value; } }
        public static string maquina_32 { get { return _maquina_32; } set { _maquina_32 = value; } }
        public static string maquina_33 { get { return _maquina_33; } set { _maquina_33 = value; } }
        public static string maquina_34 { get { return _maquina_34; } set { _maquina_34 = value; } }
        public static string maquina_35 { get { return _maquina_35; } set { _maquina_35 = value; } }
        public static string maquina_36 { get { return _maquina_36; } set { _maquina_36 = value; } }
        public static string maquina_37 { get { return _maquina_37; } set { _maquina_37 = value; } }
        public static string maquina_38 { get { return _maquina_38; } set { _maquina_38 = value; } }
        public static string maquina_39 { get { return _maquina_39; } set { _maquina_39 = value; } }
        public static string maquina_40 { get { return _maquina_40; } set { _maquina_40 = value; } }
        public static string maquina_41 { get { return _maquina_41; } set { _maquina_41 = value; } }
        public static string maquina_42 { get { return _maquina_42; } set { _maquina_42 = value; } }
        public static string maquina_43 { get { return _maquina_43; } set { _maquina_43 = value; } }
        public static string maquina_44 { get { return _maquina_44; } set { _maquina_44 = value; } }
        public static string maquina_45 { get { return _maquina_45; } set { _maquina_45 = value; } }
        public static string maquina_46 { get { return _maquina_46; } set { _maquina_46 = value; } }
        public static string maquina_47 { get { return _maquina_47; } set { _maquina_47 = value; } }
        public static string maquina_48 { get { return _maquina_48; } set { _maquina_48 = value; } }
        public static string maquina_49 { get { return _maquina_49; } set { _maquina_49 = value; } }
        public static string maquina_50 { get { return _maquina_50; } set { _maquina_50 = value; } }
        public static string maquina_51 { get { return _maquina_51; } set { _maquina_51 = value; } }
        public static string maquina_52 { get { return _maquina_52; } set { _maquina_52 = value; } }
        public static string maquina_53 { get { return _maquina_53; } set { _maquina_53 = value; } }
        public static string maquina_54 { get { return _maquina_54; } set { _maquina_54 = value; } }
        public static string maquina_55 { get { return _maquina_55; } set { _maquina_55 = value; } }
        public static string maquina_56 { get { return _maquina_56; } set { _maquina_56 = value; } }
        public static string maquina_57 { get { return _maquina_57; } set { _maquina_57 = value; } }
        public static string maquina_58 { get { return _maquina_58; } set { _maquina_58 = value; } }
        public static string maquina_59 { get { return _maquina_59; } set { _maquina_59 = value; } }
        public static string maquina_60 { get { return _maquina_60; } set { _maquina_60 = value; } }
        public static string maquina_61 { get { return _maquina_61; } set { _maquina_61 = value; } }
        public static string maquina_62 { get { return _maquina_62; } set { _maquina_62 = value; } }
        public static string maquina_63 { get { return _maquina_63; } set { _maquina_63 = value; } }
        public static string maquina_64 { get { return _maquina_64; } set { _maquina_64 = value; } }
        public static string maquina_65 { get { return _maquina_65; } set { _maquina_65 = value; } }
        public static string maquina_66 { get { return _maquina_66; } set { _maquina_66 = value; } }

        #endregion

        #region operario_variable

        public static string operario_1 {get { return _operario_1; } set { _operario_1 = value; }}
        public static string operario_2 { get { return _operario_2; } set { _operario_2 = value; } }
        public static string operario_3 { get { return _operario_3; } set { _operario_3 = value; } }
        public static string operario_4 { get { return _operario_4; } set { _operario_4 = value; } }
        public static string operario_5 { get { return _operario_5; } set { _operario_5 = value; } }
        public static string operario_6 { get { return _operario_6; } set { _operario_6 = value; } }
        public static string operario_7 { get { return _operario_7; } set { _operario_7 = value; } }
        public static string operario_8 { get { return _operario_8; } set { _operario_8 = value; } }
        public static string operario_9 { get { return _operario_9; } set { _operario_9 = value; } }
        public static string operario_10 { get { return _operario_10; } set { _operario_10 = value; } }
        public static string operario_11 { get { return _operario_11; } set { _operario_11 = value; } }
        public static string operario_12 { get { return _operario_12; } set { _operario_12 = value; } }
        public static string operario_13 { get { return _operario_13; } set { _operario_13 = value; } }
        public static string operario_14 { get { return _operario_14; } set { _operario_14 = value; } }
        public static string operario_15 { get { return _operario_15; } set { _operario_15 = value; } }
        public static string operario_16 { get { return _operario_16; } set { _operario_16 = value; } }
        public static string operario_17 { get { return _operario_17; } set { _operario_17 = value; } }
        public static string operario_18 { get { return _operario_18; } set { _operario_18 = value; } }
        public static string operario_19 { get { return _operario_19; } set { _operario_19 = value; } }
        public static string operario_20 { get { return _operario_20; } set { _operario_20 = value; } }
        public static string operario_21 { get { return _operario_21; } set { _operario_21 = value; } }
        public static string operario_22 { get { return _operario_22; } set { _operario_22 = value; } }
        public static string operario_23 { get { return _operario_23; } set { _operario_23 = value; } }
        public static string operario_24 { get { return _operario_24; } set { _operario_24 = value; } }
        public static string operario_25 { get { return _operario_25; } set { _operario_25 = value; } }
        public static string operario_26 { get { return _operario_26; } set { _operario_26 = value; } }
        public static string operario_27 { get { return _operario_27; } set { _operario_27 = value; } }
        public static string operario_28 { get { return _operario_28; } set { _operario_28 = value; } }
        public static string operario_29 { get { return _operario_29; } set { _operario_29 = value; } }
        public static string operario_30 { get { return _operario_30; } set { _operario_30 = value; } }
        public static string operario_31 { get { return _operario_31; } set { _operario_31 = value; } }
        public static string operario_32 { get { return _operario_32; } set { _operario_32 = value; } }
        public static string operario_33 { get { return _operario_33; } set { _operario_33 = value; } }
        public static string operario_34 { get { return _operario_34; } set { _operario_34 = value; } }
        public static string operario_35 { get { return _operario_35; } set { _operario_35 = value; } }
        public static string operario_36 { get { return _operario_36; } set { _operario_36 = value; } }
        public static string operario_37 { get { return _operario_37; } set { _operario_37 = value; } }
        public static string operario_38 { get { return _operario_38; } set { _operario_38 = value; } }
        public static string operario_39 { get { return _operario_39; } set { _operario_39 = value; } }
        public static string operario_40 { get { return _operario_40; } set { _operario_40 = value; } }
        public static string operario_41 { get { return _operario_41; } set { _operario_41 = value; } }
        public static string operario_42 { get { return _operario_42; } set { _operario_42 = value; } }
        public static string operario_43 { get { return _operario_43; } set { _operario_43 = value; } }
        public static string operario_44 { get { return _operario_44; } set { _operario_44 = value; } }
        public static string operario_45 { get { return _operario_45; } set { _operario_45 = value; } }
        public static string operario_46 { get { return _operario_46; } set { _operario_46 = value; } }
        public static string operario_47 { get { return _operario_47; } set { _operario_47 = value; } }
        public static string operario_48 { get { return _operario_48; } set { _operario_48 = value; } }
        public static string operario_49 { get { return _operario_49; } set { _operario_49 = value; } }
        public static string operario_50 { get { return _operario_50; } set { _operario_50 = value; } }
        public static string operario_51 { get { return _operario_51; } set { _operario_51 = value; } }
        public static string operario_52 { get { return _operario_52; } set { _operario_52 = value; } }
        public static string operario_53 { get { return _operario_53; } set { _operario_53 = value; } }
        public static string operario_54 { get { return _operario_54; } set { _operario_54 = value; } }
        public static string operario_55 { get { return _operario_55; } set { _operario_55 = value; } }
        public static string operario_56 { get { return _operario_56; } set { _operario_56 = value; } }
        public static string operario_57 { get { return _operario_57; } set { _operario_57 = value; } }
        public static string operario_58 { get { return _operario_58; } set { _operario_58 = value; } }
        public static string operario_59 { get { return _operario_59; } set { _operario_59 = value; } }
        public static string operario_60 { get { return _operario_60; } set { _operario_60 = value; } }
        public static string operario_61 { get { return _operario_61; } set { _operario_61 = value; } }
        public static string operario_62 { get { return _operario_62; } set { _operario_62 = value; } }
        public static string operario_63 { get { return _operario_63; } set { _operario_63 = value; } }
        public static string operario_64 { get { return _operario_64; } set { _operario_64 = value; } }
        public static string operario_65 { get { return _operario_65; } set { _operario_65 = value; } }
        public static string operario_66 { get { return _operario_66; } set { _operario_66 = value; } }

        #endregion

        #region color_variable
        public static Brush color_1 {get { return _color_1; } set { _color_1 = value; }}
        public static Brush color_2 { get { return _color_2; } set { _color_2 = value; } }
        public static Brush color_3 { get { return _color_3; } set { _color_3 = value; } }
        public static Brush color_4 { get { return _color_4; } set { _color_4 = value; } }
        public static Brush color_5 { get { return _color_5; } set { _color_5 = value; } }
        public static Brush color_6 { get { return _color_6; } set { _color_6 = value; } }
        public static Brush color_7 { get { return _color_7; } set { _color_7 = value; } }
        public static Brush color_8 { get { return _color_8; } set { _color_8 = value; } }
        public static Brush color_9 { get { return _color_9; } set { _color_9 = value; } }
        public static Brush color_10 { get { return _color_10; } set { _color_10 = value; } }
        public static Brush color_11 { get { return _color_11; } set { _color_11 = value; } }
        public static Brush color_12 { get { return _color_12; } set { _color_12 = value; } }
        public static Brush color_13 { get { return _color_13; } set { _color_13 = value; } }
        public static Brush color_14 { get { return _color_14; } set { _color_14 = value; } }
        public static Brush color_15 { get { return _color_15; } set { _color_15 = value; } }
        public static Brush color_16 { get { return _color_16; } set { _color_16 = value; } }
        public static Brush color_17 { get { return _color_17; } set { _color_17 = value; } }
        public static Brush color_18 { get { return _color_18; } set { _color_18 = value; } }
        public static Brush color_19 { get { return _color_19; } set { _color_19 = value; } }
        public static Brush color_20 { get { return _color_20; } set { _color_20 = value; } }
        public static Brush color_21 { get { return _color_21; } set { _color_21 = value; } }
        public static Brush color_22 { get { return _color_22; } set { _color_22 = value; } }
        public static Brush color_23 { get { return _color_23; } set { _color_23 = value; } }
        public static Brush color_24 { get { return _color_24; } set { _color_24 = value; } }
        public static Brush color_25 { get { return _color_25; } set { _color_25 = value; } }
        public static Brush color_26 { get { return _color_26; } set { _color_26 = value; } }
        public static Brush color_27 { get { return _color_27; } set { _color_27 = value; } }
        public static Brush color_28 { get { return _color_28; } set { _color_28 = value; } }
        public static Brush color_29 { get { return _color_29; } set { _color_29 = value; } }
        public static Brush color_30 { get { return _color_30; } set { _color_30 = value; } }
        public static Brush color_31 { get { return _color_31; } set { _color_31 = value; } }
        public static Brush color_32 { get { return _color_32; } set { _color_32 = value; } }
        public static Brush color_33 { get { return _color_33; } set { _color_33 = value; } }
        public static Brush color_34 { get { return _color_34; } set { _color_34 = value; } }
        public static Brush color_35 { get { return _color_35; } set { _color_35 = value; } }
        public static Brush color_36 { get { return _color_36; } set { _color_36 = value; } }
        public static Brush color_37 { get { return _color_37; } set { _color_37 = value; } }
        public static Brush color_38 { get { return _color_38; } set { _color_38 = value; } }
        public static Brush color_39 { get { return _color_39; } set { _color_39 = value; } }
        public static Brush color_40 { get { return _color_40; } set { _color_40 = value; } }
        public static Brush color_41 { get { return _color_41; } set { _color_41 = value; } }
        public static Brush color_42 { get { return _color_42; } set { _color_42 = value; } }
        public static Brush color_43 { get { return _color_43; } set { _color_43 = value; } }
        public static Brush color_44 { get { return _color_44; } set { _color_44 = value; } }
        public static Brush color_45 { get { return _color_45; } set { _color_45 = value; } }
        public static Brush color_46 { get { return _color_46; } set { _color_46 = value; } }
        public static Brush color_47 { get { return _color_47; } set { _color_47 = value; } }
        public static Brush color_48 { get { return _color_48; } set { _color_48 = value; } }
        public static Brush color_49 { get { return _color_49; } set { _color_49 = value; } }
        public static Brush color_50 { get { return _color_50; } set { _color_50 = value; } }
        public static Brush color_51 { get { return _color_51; } set { _color_51 = value; } }
        public static Brush color_52 { get { return _color_52; } set { _color_52 = value; } }
        public static Brush color_53 { get { return _color_53; } set { _color_53 = value; } }
        public static Brush color_54 { get { return _color_54; } set { _color_54 = value; } }
        public static Brush color_55 { get { return _color_55; } set { _color_55 = value; } }
        public static Brush color_56 { get { return _color_56; } set { _color_56 = value; } }
        public static Brush color_57 { get { return _color_57; } set { _color_57 = value; } }
        public static Brush color_58 { get { return _color_58; } set { _color_58 = value; } }
        public static Brush color_59 { get { return _color_59; } set { _color_59 = value; } }
        public static Brush color_60 { get { return _color_60; } set { _color_60 = value; } }
        public static Brush color_61 { get { return _color_61; } set { _color_61 = value; } }
        public static Brush color_62 { get { return _color_62; } set { _color_62 = value; } }
        public static Brush color_63 { get { return _color_63; } set { _color_63 = value; } }
        public static Brush color_64 { get { return _color_64; } set { _color_64 = value; } }
        public static Brush color_65 { get { return _color_65; } set { _color_65 = value; } }
        public static Brush color_66 { get { return _color_66; } set { _color_66 = value; } }


        #endregion

        #region operaciones

        public static List<string> operaciones_1 = new List<string>();
        public static List<string> operaciones_2 = new List<string>();
        public static List<string> operaciones_3 = new List<string>();
        public static List<string> operaciones_4 = new List<string>();
        public static List<string> operaciones_5 = new List<string>();
        public static List<string> operaciones_6 = new List<string>();
        public static List<string> operaciones_7 = new List<string>();
        public static List<string> operaciones_8 = new List<string>();
        public static List<string> operaciones_9 = new List<string>();
        public static List<string> operaciones_10 = new List<string>();
        public static List<string> operaciones_11 = new List<string>();
        public static List<string> operaciones_12 = new List<string>();
        public static List<string> operaciones_13 = new List<string>();
        public static List<string> operaciones_14 = new List<string>();
        public static List<string> operaciones_15 = new List<string>();
        public static List<string> operaciones_16 = new List<string>();
        public static List<string> operaciones_17 = new List<string>();
        public static List<string> operaciones_18 = new List<string>();
        public static List<string> operaciones_19 = new List<string>();
        public static List<string> operaciones_20 = new List<string>();
        public static List<string> operaciones_21 = new List<string>();
        public static List<string> operaciones_22 = new List<string>();
        public static List<string> operaciones_23 = new List<string>();
        public static List<string> operaciones_24 = new List<string>();
        public static List<string> operaciones_25 = new List<string>();
        public static List<string> operaciones_26 = new List<string>();
        public static List<string> operaciones_27 = new List<string>();
        public static List<string> operaciones_28 = new List<string>();
        public static List<string> operaciones_29 = new List<string>();
        public static List<string> operaciones_30 = new List<string>();
        public static List<string> operaciones_31 = new List<string>();
        public static List<string> operaciones_32 = new List<string>();
        public static List<string> operaciones_33 = new List<string>();
        public static List<string> operaciones_34 = new List<string>();
        public static List<string> operaciones_35 = new List<string>();
        public static List<string> operaciones_36 = new List<string>();
        public static List<string> operaciones_37 = new List<string>();
        public static List<string> operaciones_38 = new List<string>();
        public static List<string> operaciones_39 = new List<string>();
        public static List<string> operaciones_40 = new List<string>();
        public static List<string> operaciones_41 = new List<string>();
        public static List<string> operaciones_42 = new List<string>();
        public static List<string> operaciones_43 = new List<string>();
        public static List<string> operaciones_44 = new List<string>();
        public static List<string> operaciones_45 = new List<string>();
        public static List<string> operaciones_46 = new List<string>();
        public static List<string> operaciones_47 = new List<string>();
        public static List<string> operaciones_48 = new List<string>();
        public static List<string> operaciones_49 = new List<string>();
        public static List<string> operaciones_50 = new List<string>();
        public static List<string> operaciones_51 = new List<string>();
        public static List<string> operaciones_52 = new List<string>();
        public static List<string> operaciones_53 = new List<string>();
        public static List<string> operaciones_54 = new List<string>();
        public static List<string> operaciones_55 = new List<string>();
        public static List<string> operaciones_56 = new List<string>();
        public static List<string> operaciones_57 = new List<string>();
        public static List<string> operaciones_58 = new List<string>();
        public static List<string> operaciones_59 = new List<string>();
        public static List<string> operaciones_60 = new List<string>();
        public static List<string> operaciones_61 = new List<string>();
        public static List<string> operaciones_62 = new List<string>();
        public static List<string> operaciones_63 = new List<string>();
        public static List<string> operaciones_64 = new List<string>();
        public static List<string> operaciones_65 = new List<string>();
        public static List<string> operaciones_66 = new List<string>();

        #endregion

        #region general_variable
        public static string estilo { get { return _estilo; } set { _estilo = value; } }
        public static string temporada { get { return _temporada; } set { _temporada = value; } }
        public static string sam { get { return _sam; } set { _sam = value; } }
        public static string modulo { get { return _modulo; } set { _modulo = value; } }
        public static string operarios { get { return _operarios; } set { _operarios = value; } }
        public static string eficiencia { get { return _eficiencia; } set { _eficiencia = value; } }
        public static string sobrecarga { get { return _sobrecarga; } set { _sobrecarga = value; } }
        public static string subutilizado { get { return _subutilizado; } set { _subutilizado = value; } }
        public static string lote { get { return _lote; } set { _lote = value; } }
        public static string ingeniero { get { return _ingeniero; } set { _ingeniero = value; } }
        public static string fecha { get { return _fecha; } set { _fecha = value; } }
        public static string tipo { get { return _tipo; } set { _tipo = value; } }

        #endregion

        #region consolidado_variable

        public static List<elementos> consolidado_maquinas = new List<elementos>();

        #endregion
    }
    #endregion



}


