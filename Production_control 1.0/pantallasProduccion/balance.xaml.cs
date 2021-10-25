using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using Production_control_1._0.clases;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Production_control_1._0
{
    public partial class balance : Page
    {
        #region clasesDeGraficas
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        public SeriesCollection DatosGraficaRebalance { get; set; }
        public string[] etiquetasRebalance { get; set; }
        public List<string> ListaDeOperarios { get; private set; }

        public SeriesCollection DatosGraficaEficiencia { get; set; }
        public string[] etiquetasEficiencia { get; set; }

        int codigoUsuario = 0;

        //formatos de numero grafica
        public Func<double, string> PorcentajeFormatter { get; set; }

        #endregion
        #region datos_iniciales()
        string modulo_ = "";
        string nomre = "";
        string temporada = "";
        int version = 0;
        string tipo = "nuevo";
        public balance(clases.balance datosBalance)
        {
            InitializeComponent();
            #region tamanoDeZoom
            //datos generales para obtener la altura del zoom

            double alturaBase = System.Windows.SystemParameters.PrimaryScreenHeight;

            //valor inicial de slicer
            tamano.Value = alturaBase;
            //tamanio zoom layout
            ZoomViewbox.Height = alturaBase;
            ZoomViewbox.Width = alturaBase * 1.5;
            //tamnio zoom grafica teorica
            zoomGraficaTeorica.Height = alturaBase; ;
            zoomGraficaTeorica.Width = alturaBase * 1.5;
            //tamanio zoom grafica real
            zoomGraficaReal.Height = alturaBase; ;
            zoomGraficaReal.Width = alturaBase * 1.5;
            //tamanio zoom grafica eficiencia
            zoomGraficaEficiencia.Height = alturaBase; ;
            zoomGraficaEficiencia.Width = alturaBase * 1.5;

            #endregion
            #region conexionesConBasesSQL
            SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlConnection cnBalances = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql; //Consulta que se hace en sql
            SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
            SqlDataReader dr; //leer los resultados del comando sql
            #endregion
            #region datosEncabezado
            estilo_.Content = datosBalance.nombre;
            estilo_2.Content = datosBalance.nombre;
            temporada_.Content = datosBalance.temporada;
            temporada_2.Content = datosBalance.temporada;
            sam_.Content = Math.Round(datosBalance.sam, 4).ToString();
            sam_2.Content = Math.Round(datosBalance.sam, 4).ToString();
            #endregion
            #region cargarListaDeModulos
            sql = "select min(id) as id, modulo from modulosProduccion where coordinadorCodigo>0 group by modulo order by id";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cnProduccion.Close();
            #endregion
            #region cargarImagen
            // se carga la imagen
            try
            {
                Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + datosBalance.temporada + "/" + datosBalance.nombre + ".jpg");
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
                    Title = "Ciclo",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.Green,
                    DataLabels=true,
                    LabelsPosition=BarLabelPosition.Top,
                    FontSize=25,
                    MaxColumnWidth = double.PositiveInfinity,
                },
                new LineSeries
                {
                    Title="Tkt",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = Brushes.Transparent,
                    PointGeometry= DefaultGeometries.Circle,
                },
                 new LineSeries
                {
                    Title="TktO",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Blue,
                    Fill = Brushes.Transparent,
                    PointGeometry= DefaultGeometries.Circle,
                }
            };
            DataContext = this;
            #endregion
            #region datosInicialesDeGraficaDeRebalance
            // se cargan los datos iniciales para la grafica_reb
            DatosGraficaRebalance = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Ciclo",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.Green,
                    DataLabels=true,
                    LabelsPosition=BarLabelPosition.Top,
                    FontSize=25,
                    MaxColumnWidth = double.PositiveInfinity,
                },
                new LineSeries
                {
                    Title="Tkt",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = Brushes.Transparent,
                    PointGeometry= DefaultGeometries.Circle,
                },
                 new LineSeries
                {
                    Title="TktO",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Blue,
                    Fill = Brushes.Transparent,
                    PointGeometry= DefaultGeometries.Circle,
                }
            };
            DataContext = this;
            #endregion
            #region datosInicialesDeGraficaDeEficiencia
            // se cargan los datos iniciales para la grafica_reb
            DatosGraficaEficiencia = new SeriesCollection
            {
                new LineSeries
                {
                    Title="Eficiencia",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Blue,
                    PointGeometry= DefaultGeometries.Circle,
                    DataLabels = true,
                    FontSize=25,
                },
                 new LineSeries
                {
                    Title="100%",
                    Values= new ChartValues<double> {0},
                    Stroke = System.Windows.Media.Brushes.Red,
                    PointGeometry= DefaultGeometries.Circle,
                },
            };
            PorcentajeFormatter = value => value.ToString("P");
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
            // determinar si el balance es nuevo o se esta abriendo una ya empezado
            if (datosBalance.tipo == "nuevo")
            {
                #region agregarOperaciones
                cnIngenieria.Open();
                sql = "select correlativo, nombre, titulo, sam, maquina, categoria from operaciones where temporada= '" + datosBalance.temporada + "' and estilo= '" + datosBalance.nombre + "' and sam is not null";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                List<elementoListBox> listaOperaciones2 = new List<elementoListBox>();
                //agregar operaciones de consulta
                while (dr.Read())
                {
                    listaOperaciones2.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["nombre"].ToString(), tituloOperacion = dr["titulo"].ToString().Replace("'", ""), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = dr["maquina"].ToString(), categoriaMaquina = dr["categoria"].ToString() });
                };
                dr.Close();
                cnIngenieria.Close();
                //agregar operacion de empaque

                listaOperaciones2.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = 0, nombreOperacion = "empaque", tituloOperacion = datosBalance.nombreEmpaque, samOperacion = datosBalance.samEmpaque, asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = "Mesa de Empaque", categoriaMaquina = "manual" });
                Operaciones.ItemsSource = listaOperaciones2;
                #endregion
                #region agregarDatosDeEncabezado
                //se cargan los datos generales de las variables globales
                fecha_.Content = datosBalance.fechaCreacion.ToString("yyyy-MM-dd");
                version_.Text = datosBalance.version.ToString();
                autoriza.IsEnabled = false;
                #endregion
            }
            else if (datosBalance.tipo == "edicion")
            {
                //datos se cargan en evento loaded
                modulo_ = datosBalance.modulo;
                nomre = datosBalance.nombre;
                temporada = datosBalance.temporada;
                version = datosBalance.version;
                tipo = "edicion";
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (tipo == "edicion")
            {
                #region conexionesConBasesSQL
                SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                SqlConnection cnBalances = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql; //Consulta que se hace en sql
                SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
                SqlDataReader dr; //leer los resultados del comando sql
                #endregion
                string llave = modulo_ + nomre + temporada + version;
                #region llenarEncabezado
                sql = "select fecha_creacion, estilo, temporada, sam, modulo, corrida, horas, operarios, ingeniero, version from lista_balances where identificador='" + llave + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                dr.Read();
                fecha_.Content = Convert.ToDateTime(dr["fecha_creacion"]).ToString("yyyy-MM-dd");
                version_.Text = dr["version"].ToString();
                piezas_de_corrida.Text = dr["corrida"].ToString();
                horas_de_corrida.Text = dr["horas"].ToString();
                sam_.Content = dr["sam"].ToString();
                sam_2.Content = dr["sam"].ToString();
                modulo.SelectedItem = dr["modulo"].ToString();
                operarios_.Content = dr["operarios"].ToString();
                operarios_2.Content = dr["operarios"].ToString();
                cnBalances.Close();
                dr.Close();
                #endregion
                #region llenarListaDeMaquinas
                List<elementoListBox> listaDeMaquinas = new List<elementoListBox>();
                sql = "select ajuste, categoria, color, maquina, correlativo, operacion, codigo, sam, carga, operario, codigoMaquina from consultaDeMaquinas where identificador='" + llave + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listaDeMaquinas.Add(
                        new elementoListBox
                        {
                            correlativoMaquina = Convert.ToInt32(dr["maquina"]),
                            colorAjuste = dr["color"].ToString(),
                            categoriaMaquina = dr["categoria"].ToString(),
                            nombreOperario = dr["operario"].ToString(),
                            ajusteMaquina = dr["ajuste"].ToString(),
                            correlativoOperacion = Convert.ToInt32(dr["correlativo"]),
                            tituloOperacion = dr["operacion"].ToString(),
                            nombreOperacion = dr["codigo"].ToString(),
                            asignadoOperacion = Convert.ToDouble(dr["carga"]),
                            samOperacion = Convert.ToDouble(dr["sam"]),
                            codigoMaquina = dr["codigoMaquina"].ToString()
                        });
                }
                dr.Close();
                cnBalances.Close();
                #endregion
                int numeroEstacion = 0;
                #region areaPreparacion
                foreach (GroupBox groupBox in areaPreparacion.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;

                            //limpiar los listbox con anterioridad
                            foreach (object visual in estacion.Children)
                            {
                                if (visual.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)visual);
                                    listaDeOperaciones.Items.Clear();
                                }
                            }
                            foreach (elementoListBox item in listaDeMaquinas)
                            {
                                foreach (object elemento in estacion.Children)
                                {
                                    if (item.correlativoMaquina == numeroEstacion)
                                    {
                                        string color = item.colorAjuste;
                                        #region colorEnString

                                        if (color == "rojo" || color == "Pink")
                                        {
                                            estacion.Background = Brushes.Pink;
                                        }
                                        else if (color == "azul" || color == "LightBlue")
                                        {
                                            estacion.Background = Brushes.LightBlue;
                                        }
                                        else if (color == "verde" || color == "LightGreen")
                                        {
                                            estacion.Background = Brushes.LightGreen;
                                        }
                                        else if (color == "amarillo" || color == "Yellow")
                                        {
                                            estacion.Background = Brushes.Yellow;
                                        }
                                        else if (color == "anaranjado" || color == "Orange")
                                        {
                                            estacion.Background = Brushes.Orange;
                                        }
                                        else
                                        {
                                            estacion.Background = Brushes.White;
                                        }
                                        #endregion
                                        if (elemento.GetType() == typeof(StackPanel))
                                        {
                                            StackPanel contenedorSecundario = (StackPanel)elemento;
                                            foreach (object subElemento in contenedorSecundario.Children)
                                            {
                                                if (subElemento.GetType() == typeof(TextBox))
                                                {
                                                    ((TextBox)subElemento).Text = item.codigoMaquina;
                                                }
                                            }
                                        }
                                        if (elemento.GetType() == typeof(TextBlock))
                                        {
                                            ((TextBlock)elemento).Text = item.categoriaMaquina;
                                        }
                                        if (elemento.GetType() == typeof(Label))
                                        {
                                            ((Label)elemento).Content = item.nombreOperario;
                                        }
                                        if (elemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)elemento).Text = item.ajusteMaquina;
                                        }
                                        if (elemento.GetType() == typeof(ListBox))
                                        {
                                            ListBox listaDeOperaciones = ((ListBox)elemento);
                                            listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region arteriaUno
                foreach (GroupBox groupBox in arteriaUno.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;
                            //limpiar los listbox con anterioridad
                            foreach (object visual in estacion.Children)
                            {
                                if (visual.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)visual);
                                    listaDeOperaciones.Items.Clear();
                                }
                            }
                            foreach (elementoListBox item in listaDeMaquinas)
                            {
                                foreach (object elemento in estacion.Children)
                                {
                                    if (item.correlativoMaquina == numeroEstacion)
                                    {
                                        string color = item.colorAjuste;
                                        #region colorEnString

                                        if (color == "rojo" || color == "Pink")
                                        {
                                            estacion.Background = Brushes.Pink;
                                        }
                                        else if (color == "azul" || color == "LightBlue")
                                        {
                                            estacion.Background = Brushes.LightBlue;
                                        }
                                        else if (color == "verde" || color == "LightGreen")
                                        {
                                            estacion.Background = Brushes.LightGreen;
                                        }
                                        else if (color == "amarillo" || color == "Yellow")
                                        {
                                            estacion.Background = Brushes.Yellow;
                                        }
                                        else if (color == "anaranjado" || color == "Orange")
                                        {
                                            estacion.Background = Brushes.Orange;
                                        }
                                        else
                                        {
                                            estacion.Background = Brushes.White;
                                        }
                                        #endregion
                                        if (elemento.GetType() == typeof(StackPanel))
                                        {
                                            StackPanel contenedorSecundario = (StackPanel)elemento;
                                            foreach (object subElemento in contenedorSecundario.Children)
                                            {
                                                if (subElemento.GetType() == typeof(TextBox))
                                                {
                                                    ((TextBox)subElemento).Text = item.codigoMaquina;
                                                }
                                            }
                                        }
                                        if (elemento.GetType() == typeof(TextBlock))
                                        {
                                            ((TextBlock)elemento).Text = item.categoriaMaquina;
                                        }
                                        if (elemento.GetType() == typeof(Label))
                                        {
                                            ((Label)elemento).Content = item.nombreOperario;
                                        }
                                        if (elemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)elemento).Text = item.ajusteMaquina;
                                        }
                                        if (elemento.GetType() == typeof(ListBox))
                                        {
                                            ListBox listaDeOperaciones = ((ListBox)elemento);
                                            listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region arteriaDos
                foreach (GroupBox groupBox in arteriaDos.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;
                            //limpiar los listbox con anterioridad
                            foreach (object visual in estacion.Children)
                            {
                                if (visual.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)visual);
                                    listaDeOperaciones.Items.Clear();
                                }
                            }
                            foreach (elementoListBox item in listaDeMaquinas)
                            {
                                foreach (object elemento in estacion.Children)
                                {
                                    if (item.correlativoMaquina == numeroEstacion)
                                    {
                                        string color = item.colorAjuste;
                                        #region colorEnString

                                        if (color == "rojo" || color == "Pink")
                                        {
                                            estacion.Background = Brushes.Pink;
                                        }
                                        else if (color == "azul" || color == "LightBlue")
                                        {
                                            estacion.Background = Brushes.LightBlue;
                                        }
                                        else if (color == "verde" || color == "LightGreen")
                                        {
                                            estacion.Background = Brushes.LightGreen;
                                        }
                                        else if (color == "amarillo" || color == "Yellow")
                                        {
                                            estacion.Background = Brushes.Yellow;
                                        }
                                        else if (color == "anaranjado" || color == "Orange")
                                        {
                                            estacion.Background = Brushes.Orange;
                                        }
                                        else
                                        {
                                            estacion.Background = Brushes.White;
                                        }
                                        #endregion
                                        if (elemento.GetType() == typeof(StackPanel))
                                        {
                                            StackPanel contenedorSecundario = (StackPanel)elemento;
                                            foreach (object subElemento in contenedorSecundario.Children)
                                            {
                                                if (subElemento.GetType() == typeof(TextBox))
                                                {
                                                    ((TextBox)subElemento).Text = item.codigoMaquina;
                                                }
                                            }
                                        }
                                        if (elemento.GetType() == typeof(TextBlock))
                                        {
                                            ((TextBlock)elemento).Text = item.categoriaMaquina;
                                        }
                                        if (elemento.GetType() == typeof(Label))
                                        {
                                            ((Label)elemento).Content = item.nombreOperario;
                                        }
                                        if (elemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)elemento).Text = item.ajusteMaquina;
                                        }
                                        if (elemento.GetType() == typeof(ListBox))
                                        {
                                            ListBox listaDeOperaciones = ((ListBox)elemento);
                                            listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region arteriaTres
                foreach (GroupBox groupBox in arteriaTres.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;
                            //limpiar los listbox con anterioridad
                            foreach (object visual in estacion.Children)
                            {
                                if (visual.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)visual);
                                    listaDeOperaciones.Items.Clear();
                                }
                            }
                            foreach (elementoListBox item in listaDeMaquinas)
                            {
                                foreach (object elemento in estacion.Children)
                                {
                                    if (item.correlativoMaquina == numeroEstacion)
                                    {
                                        string color = item.colorAjuste;
                                        #region colorEnString

                                        if (color == "rojo" || color == "Pink")
                                        {
                                            estacion.Background = Brushes.Pink;
                                        }
                                        else if (color == "azul" || color == "LightBlue")
                                        {
                                            estacion.Background = Brushes.LightBlue;
                                        }
                                        else if (color == "verde" || color == "LightGreen")
                                        {
                                            estacion.Background = Brushes.LightGreen;
                                        }
                                        else if (color == "amarillo" || color == "Yellow")
                                        {
                                            estacion.Background = Brushes.Yellow;
                                        }
                                        else if (color == "anaranjado" || color == "Orange")
                                        {
                                            estacion.Background = Brushes.Orange;
                                        }
                                        else
                                        {
                                            estacion.Background = Brushes.White;
                                        }
                                        #endregion
                                        if (elemento.GetType() == typeof(StackPanel))
                                        {
                                            StackPanel contenedorSecundario = (StackPanel)elemento;
                                            foreach (object subElemento in contenedorSecundario.Children)
                                            {
                                                if (subElemento.GetType() == typeof(TextBox))
                                                {
                                                    ((TextBox)subElemento).Text = item.codigoMaquina;
                                                }
                                            }
                                        }
                                        if (elemento.GetType() == typeof(TextBlock))
                                        {
                                            ((TextBlock)elemento).Text = item.categoriaMaquina;
                                        }
                                        if (elemento.GetType() == typeof(Label))
                                        {
                                            ((Label)elemento).Content = item.nombreOperario;
                                        }
                                        if (elemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)elemento).Text = item.ajusteMaquina;
                                        }
                                        if (elemento.GetType() == typeof(ListBox))
                                        {
                                            ListBox listaDeOperaciones = ((ListBox)elemento);
                                            listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region arteriaCuatro
                foreach (GroupBox groupBox in arteriaCuatro.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;
                            //limpiar los listbox con anterioridad
                            foreach (object visual in estacion.Children)
                            {
                                if (visual.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)visual);
                                    listaDeOperaciones.Items.Clear();
                                }
                            }
                            foreach (elementoListBox item in listaDeMaquinas)
                            {
                                foreach (object elemento in estacion.Children)
                                {
                                    if (item.correlativoMaquina == numeroEstacion)
                                    {
                                        string color = item.colorAjuste;
                                        #region colorEnString

                                        if (color == "rojo" || color == "Pink")
                                        {
                                            estacion.Background = Brushes.Pink;
                                        }
                                        else if (color == "azul" || color == "LightBlue")
                                        {
                                            estacion.Background = Brushes.LightBlue;
                                        }
                                        else if (color == "verde" || color == "LightGreen")
                                        {
                                            estacion.Background = Brushes.LightGreen;
                                        }
                                        else if (color == "amarillo" || color == "Yellow")
                                        {
                                            estacion.Background = Brushes.Yellow;
                                        }
                                        else if (color == "anaranjado" || color == "Orange")
                                        {
                                            estacion.Background = Brushes.Orange;
                                        }
                                        else
                                        {
                                            estacion.Background = Brushes.White;
                                        }
                                        #endregion
                                        if (elemento.GetType() == typeof(StackPanel))
                                        {
                                            StackPanel contenedorSecundario = (StackPanel)elemento;
                                            foreach (object subElemento in contenedorSecundario.Children)
                                            {
                                                if (subElemento.GetType() == typeof(TextBox))
                                                {
                                                    ((TextBox)subElemento).Text = item.codigoMaquina;
                                                }
                                            }
                                        }
                                        if (elemento.GetType() == typeof(TextBlock))
                                        {
                                            ((TextBlock)elemento).Text = item.categoriaMaquina;
                                        }
                                        if (elemento.GetType() == typeof(Label))
                                        {
                                            ((Label)elemento).Content = item.nombreOperario;
                                        }
                                        if (elemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)elemento).Text = item.ajusteMaquina;
                                        }
                                        if (elemento.GetType() == typeof(ListBox))
                                        {
                                            ListBox listaDeOperaciones = ((ListBox)elemento);
                                            listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region enganchador
                numeroEstacion = numeroEstacion + 1;
                foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;
                        //limpiar los listbox con anterioridad
                        foreach (object visual in estacion.Children)
                        {
                            if (visual.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)visual);
                                listaDeOperaciones.Items.Clear();
                            }
                        }
                        foreach (elementoListBox item in listaDeMaquinas)
                        {
                            foreach (object elemento in estacion.Children)
                            {
                                if (item.correlativoMaquina == numeroEstacion)
                                {
                                    string color = item.colorAjuste;
                                    #region colorEnString

                                    if (color == "rojo" || color == "Pink")
                                    {
                                        estacion.Background = Brushes.Pink;
                                    }
                                    else if (color == "azul" || color == "LightBlue")
                                    {
                                        estacion.Background = Brushes.LightBlue;
                                    }
                                    else if (color == "verde" || color == "LightGreen")
                                    {
                                        estacion.Background = Brushes.LightGreen;
                                    }
                                    else if (color == "amarillo" || color == "Yellow")
                                    {
                                        estacion.Background = Brushes.Yellow;
                                    }
                                    else if (color == "anaranjado" || color == "Orange")
                                    {
                                        estacion.Background = Brushes.Orange;
                                    }
                                    else
                                    {
                                        estacion.Background = Brushes.White;
                                    }
                                    #endregion
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina });
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region llenarListaDeOperaciones
                sql = "select correlativo, codigo, titulo, sam, ajuste, categoria, requerimiento, asignado from operaciones where identificador='" + llave + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                List<elementoListBox> listaOperaciones = new List<elementoListBox>();
                while (dr.Read())
                {
                    listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["codigo"].ToString(), tituloOperacion = dr["titulo"].ToString(), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = Convert.ToDouble(dr["asignado"]), requeridoOperacion = Convert.ToDouble(dr["requerimiento"]), ajusteMaquina = dr["ajuste"].ToString(), categoriaMaquina = dr["categoria"].ToString() });
                };
                dr.Close();
                cnBalances.Close();
                Operaciones.ItemsSource = listaOperaciones;
                #endregion
                #region llenarListaDeOperarios
                sql = "select codigo, nombre, asignado, plana, rana, flat, cover, transfer, atracadora, plancha, bonding, zig_zag, multiaguja, manual, varias from operarios_2 where identificador='" + llave + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                List<elementoListBox> listaOperarios = new List<elementoListBox>();
                while (dr.Read())
                {
                    listaOperarios.Add(new elementoListBox() { identificador = "operario", codigoOperario = Convert.ToInt32(dr["codigo"]), nombreOperario = dr["nombre"].ToString(), asignadoOperario = Convert.ToDouble(dr["asignado"] is DBNull ? 0 : dr["asignado"]), planaOperario = Convert.ToDouble(dr["plana"] is DBNull ? 0.9 : dr["plana"]), ranaOperario = Convert.ToDouble(dr["rana"] is DBNull ? 0.9 : dr["rana"]), flatOperario = Convert.ToDouble(dr["flat"] is DBNull ? 0.9 : dr["flat"]), coverOperario = Convert.ToDouble(dr["cover"] is DBNull ? 0.9 : dr["cover"]), transferOperario = Convert.ToDouble(dr["transfer"] is DBNull ? 0.9 : dr["transfer"]), atracadoraOperario = Convert.ToDouble(dr["atracadora"] is DBNull ? 0.9 : dr["atracadora"]), planchaOperario = Convert.ToDouble(dr["plancha"] is DBNull ? 0.9 : dr["plancha"]), bondingOperario = Convert.ToDouble(dr["bonding"] is DBNull ? 0.9 : dr["bonding"]), zigzagOperario = Convert.ToDouble(dr["zig_zag"] is DBNull ? 0.9 : dr["zig_zag"]), multiagujaOperario = Convert.ToDouble(dr["multiaguja"] is DBNull ? 0.9 : dr["multiaguja"]), manualOperario = Convert.ToDouble(dr["manual"] is DBNull ? 0.9 : dr["manual"]), variasOperario = Convert.ToDouble(dr["varias"] is DBNull ? 0.9 : dr["varias"]) });
                };
                dr.Close();
                cnBalances.Close();
                Operarios.ItemsSource = listaOperarios;
                #endregion
                #region calculosGenerales
                CalculoAsignadoPorOperacion();
                actualizarGrafica();
                operacionSobrecargadaOperacionSubutilizada();
                #endregion

                tipo = "";
            }
        }
        #endregion
        #region zoom()
        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width >= 0) && ZoomViewbox.Height >= 0)
            {
                ZoomViewbox.Width = newValue * 1.5;
                ZoomViewbox.Height = newValue;

                zoomGraficaTeorica.Width = newValue * 1.5;
                zoomGraficaTeorica.Height = newValue;

                zoomGraficaReal.Width = newValue * 1.5;
                zoomGraficaReal.Height = newValue;

                zoomGraficaEficiencia.Width = newValue * 1.5;
                zoomGraficaEficiencia.Height = newValue;
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateViewBox(Convert.ToInt32(tamano.Value));
        }
        #endregion
        #region contro_general_programa
        #region abrirPanelLateral
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            //agregar datos generales a los labels para guardar en base a lo cargado en edicion
            #region etiquetasGenerals
            LabelEstiloGuardar.Content = estilo_.Content;
            if (modulo.SelectedIndex > -1)
            {
                LabelModuloGuardar.Content = modulo.SelectedItem.ToString();
            }
            else
            {
                LabelModuloGuardar.Content = "GENERICO";
            }
            LabelTemporadaGuardar.Content = temporada_.Content;
            LabelVersionGuardar.Content = version_.Text;
            #endregion
            #region consultarUltimasVersiones
            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select TOP 5 modulo, estilo, fecha_modificacion, version from lista_balances where modulo='" + LabelModuloGuardar.Content.ToString() +"' and estilo='" + LabelEstiloGuardar.Content.ToString() +"' and temporada='" + LabelTemporadaGuardar.Content.ToString() +"' order by fecha_modificacion desc";
            // se agregan los modulos
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            List<clases.balance> items = new List<clases.balance>();
            while (dr.Read())
            {
                items.Add(new clases.balance() {modulo=dr["modulo"].ToString(), fechaModificacion=Convert.ToDateTime(Convert.ToDateTime(dr["fecha_modificacion"]).ToString("yyyy-MM-dd")), version=Convert.ToInt32(dr["version"])});
            };
            UltimasVersiones.ItemsSource = items;
            dr.Close();
            cn.Close();
            #endregion
            //mostrar menu para guardar
            #region mostrarDatosParaGuardar
            GridMenu.Margin = new Thickness(0, -38, 0, 0);
            GridBackground.Margin = new Thickness(250, 0, 0, 0);
            #endregion
        }
        #endregion
        #region CerrarPanelLateral
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            GridMenu.Margin = new Thickness(-250, 0, 0, 0);
            GridBackground.Margin = new Thickness(5, 30, 0, 0);
        }
        #endregion
        #region ElementosPanelLateral
        private void passWordBoxUsuario_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlDataReader dr;
            cn.Open();
            string sql = "select codigo from usuarios where produccion=1 and contrasena='" + passWordBoxUsuario.Password + "'";
            SqlCommand cm = new SqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                codigoUsuario = Convert.ToInt32(dr["codigo"]);
                ButtonGuardarBalance.IsEnabled = true;
            }
            else
            {
                ButtonGuardarBalance.IsEnabled = false;
            }
            dr.Close();
            cn.Close();
        }
        private void ButtonCerrarBalance_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea salir?", "Jazz-CCO", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.NavigationService.Navigate(new abrir());
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void ButtonGuardarBalance_Click(object sender, RoutedEventArgs e)
        {
            //los datos se guardan en 4 tablas (la primera abarca los datos generales del balance, la segunda la lista de operarios, la tercer la lista de operaciones y la cuarta los datos ya asignados a las maquinas)
            //se crea la llave que identifica a los datos
            string llave = LabelModuloGuardar.Content.ToString() + LabelEstiloGuardar.Content.ToString() + LabelTemporadaGuardar.Content.ToString() + LabelVersionGuardar.Content.ToString();
            //se crea la variable de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            #region datosParaTablaListaDeBalances
            // se elimina la version previa guardada
            cn.Open();
            string sql = "delete from lista_balances where identificador= '" + llave + "'";
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();

            //se carga los datos de la nueva version
            string sql2 = "insert into lista_balances(identificador, fecha_creacion, estilo, temporada, sam, modulo, corrida, horas, operarios, piezash, ingeniero, fecha_modificacion, version, usuario)" +
                            "values('" + llave + "','" + fecha_.Content + "', '" + LabelEstiloGuardar.Content.ToString() + "', '" + LabelTemporadaGuardar.Content.ToString() + "', '" + sam_.Content.ToString() + "', '" + LabelModuloGuardar.Content.ToString() + "', '" + piezas_de_corrida.Text + "', '" + horas_de_corrida.Text + "', '" + operarios_.Content.ToString() + "', '" + piezas_por_hora.Content.ToString() + "', '" + ingeniero_.Text + "', '" + DateTime.Now.ToString() + "', '" + LabelVersionGuardar.Content.ToString() + "', '"+codigoUsuario+"')";
            cn.Open();
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            cm2.ExecuteNonQuery();
            cn.Close();
            #endregion
            #region datosParaTablaOperaciones
            //insertar nuevos registros con datos actualizados
            cn.Open();
            foreach (elementoListBox item in Operaciones.Items)
            {
                sql = "insert into operaciones(identificador, correlativo, codigo, titulo, sam, ajuste, categoria, requerimiento, asignado) values('" + llave + "', '" + item.correlativoOperacion + "', '" + item.nombreOperacion + "', '" + item.tituloOperacion + "', " + item.samOperacion + ", '" + item.ajusteMaquina + "', '" + item.categoriaMaquina + "', " + item.requeridoOperacion + ", " + item.asignadoOperacion + ")";
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
            }
            cn.Close();
            #endregion
            #region datosParaTablaOperarios
            //insertar nuevos registros con datos actualizados
            cn.Open();
            foreach (elementoListBox item in Operarios.Items)
            {
                sql = "insert into operarios(identificador, codigo, nombre, asignado) values('" + llave + "', '" + item.codigoOperario + "', '" + item.nombreOperario + "', '" + item.asignadoOperario + "')";
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
            }
            cn.Close();
            #endregion
            #region datosParaTablaMaquinas
            //agregar nuevos registros
            List<elementoListBox> listaOperacionesAgregadas_ = listaDeOperacionesAgregadas();
            cn.Open();
            foreach (elementoListBox item in listaOperacionesAgregadas_)
            {
                foreach(operacionesAgregadas subitem in item.operacionesAgregadas)
                {
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario, codigoMaquina) values('" + llave + "', '" + item.correlativoMaquina + "', '" + subitem.ajusteMaquina + "', '" + item.categoriaMaquina+ "', '" + item.colorAjuste + "', '" + subitem.correlativoOperacion + "', '" + subitem.tituloOperacion + "', '" + subitem.asignadoOperacion + "', '" + item.nombreOperario + "', '" + item.codigoMaquina + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                }
            }
            cn.Close();
            #endregion
            #region salirPanelLatera
            GridMenu.Margin = new Thickness(-250, 0, 0, 0);
            GridBackground.Margin = new Thickness(5, 30, 0, 0);
            MessageBox.Show("Balance Guardado");
            #endregion
        }
        private void ButtonImprimirBalance_Click(object sender, RoutedEventArgs e)
        {
            int impresion_seleccionada = control_tab_maquinas.SelectedIndex;
            clases.balance datosBalance = new clases.balance();
            switch (impresion_seleccionada)
            {
                case 0:
                    bool asignado = true;
                    foreach (elementoListBox item in Operaciones.Items)
                    {
                        if (item.asignadoOperacion==0)
                        {
                            asignado = false;
                        }
                    }
                    if (asignado == true)
                    {
                        #region imprimirLay
                        List<elementoListBox> listaDeMaquinas = listaDeOperacionesAgregadas();
                    List<maquina> resumenMaquinas = new List<maquina>();
                    clases.balance resumenGeneral =  new clases.balance();

                    #region agregarResumenMaquinas
                    foreach(maquina item in resumen_maquinas.Items)
                    {
                        resumenMaquinas.Add(new maquina { categoriaMaquina = item.categoriaMaquina, ee = item.ee, ei = item.ei, ii = item.ii, ie = item.ie, na = item.na });
                    }
                    #endregion
                    #region agregarInformacionGeneral
                    resumenGeneral.nombre = estilo_2.Content.ToString();
                    resumenGeneral.sam = Convert.ToDouble(sam_.Content);
                    resumenGeneral.temporada = temporada_.Content.ToString();
                    resumenGeneral.modulo = modulo_2.Content.ToString();
                    resumenGeneral.operarios = Convert.ToInt32(operarios_.Content);
                    resumenGeneral.ingeniero = ingeniero_.Text;
                    resumenGeneral.sub = subutilizacion_2.Content.ToString();
                    resumenGeneral.sobre = sobrecarga_2.Content.ToString();
                    resumenGeneral.lote = lote.Text;
                    resumenGeneral.fechaModificacion = Convert.ToDateTime(fecha_.Content);
                    #endregion
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    this.NavigationService.Navigate(new impresion(listaDeMaquinas, resumenMaquinas, resumenGeneral));
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Parece que hay Operaciones no Asignadas");
                    }
                    break;
                case 1:
                    MessageBox.Show("Vuelva a modo layout");
                    break;
                case 2:
                    #region imprimirGraficaTeorica
                    datosBalance.sam = Convert.ToDouble(sam_.Content);
                    datosBalance.nombre = estilo_.Content.ToString();
                    datosBalance.operarios = Convert.ToInt32(operarios_.Content);
                    datosBalance.eficiencia = eficiencia_.Content.ToString();
                    datosBalance.modulo = modulo_2.Content.ToString();
                    datosBalance.fechaCreacion = Convert.ToDateTime(fecha_.Content.ToString());
                    datosBalance.corrida =Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
                    datosBalance.horas = Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    this.NavigationService.Navigate(new imprimir_balance("Balance", datosBalance, grafico));
                    #endregion
                    break;
                case 3:
                    #region imprimirGraficaReal
                    datosBalance.sam = Convert.ToDouble(sam_.Content);
                    datosBalance.nombre = estilo_.Content.ToString();
                    datosBalance.operarios = Convert.ToInt32(operarios_.Content);
                    datosBalance.eficiencia = eficiencia_.Content.ToString();
                    datosBalance.modulo = modulo_2.Content.ToString();
                    datosBalance.fechaCreacion = Convert.ToDateTime(fecha_.Content.ToString());
                    datosBalance.corrida = Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
                    datosBalance.horas = Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    this.NavigationService.Navigate(new imprimir_balance("Rebalance", datosBalance, graficoRebalance));
                    #endregion
                    break;
                case 4:
                    #region imprimirGraficaEficiencia
                    datosBalance.sam = Convert.ToDouble(sam_.Content);
                    datosBalance.nombre = estilo_.Content.ToString();
                    datosBalance.operarios = Convert.ToInt32(operarios_.Content);
                    datosBalance.eficiencia = eficiencia_.Content.ToString();
                    datosBalance.modulo = modulo_2.Content.ToString();
                    datosBalance.fechaCreacion = Convert.ToDateTime(fecha_.Content.ToString());
                    datosBalance.corrida = Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
                    datosBalance.horas = Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    this.NavigationService.Navigate(new imprimir_balance("Eficiencia", datosBalance, graficoEficiencia));
                    #endregion
                    break;
            }
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

            CalculoAsignadoPorOperacion();
            actualizarGrafica();
            operacionSobrecargadaOperacionSubutilizada();
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
            items.Add(new elementoListBox { identificador = "operario", codigoOperario = 0000, nombreOperario = "operario " + operarios_conteo, asignadoOperario = 0, planaOperario = 0.9, ranaOperario = 0.9, flatOperario = 0.9, coverOperario = 0.9, transferOperario = 0.9, atracadoraOperario = 0.9, planchaOperario = 0.9, bondingOperario = 0.9, zigzagOperario = 0.9, multiagujaOperario = 0.9, manualOperario = 0.9, variasOperario = 0.9 });

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
        #region regenerarListaOperaciones
        private void regenerar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Regenerar el balance implica consultar si existe " +
                "algun cambio en las operaciones \n Aun así debes asegurarte que todo sea " +
                "congruente con el nuevo SAM \n podria haber quedado asignada en el LayOut " +
                "alguna operación o un ajuste ya no existente \n ¿Desea Continuar?", "Jazz-CCO", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    if (!string.IsNullOrEmpty(piezas_de_corrida.Text) && !string.IsNullOrEmpty(horas_de_corrida.Text))
                    {
                        #region guardarDatosEmpaque
                        string nombreEmpaque = "";
                        double samEmpaque = 0;
                        foreach (elementoListBox item in Operaciones.Items)
                        {
                            nombreEmpaque = item.tituloOperacion;
                            samEmpaque = item.samOperacion;
                        }
                        #endregion
                        #region obtenerNuevaListaDeOperaciones
                        SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                        string sql; //Consulta que se hace en sql
                        SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
                        SqlDataReader dr; //leer los resultados del comando sql
                        cnIngenieria.Open();
                        sql = "select correlativo, nombre, titulo, sam, maquina, categoria from operaciones where temporada= '" + temporada_.Content + "' and estilo= '" + estilo_.Content + "' and sam is not null";
                        cm = new SqlCommand(sql, cnIngenieria);
                        dr = cm.ExecuteReader();
                        List<elementoListBox> listaOperaciones = new List<elementoListBox>();
                        List<elementoListBox> listaOperaciones2 = new List<elementoListBox>();
                        List<elementoListBox> listaOperaciones3 = new List<elementoListBox>();
                        //agregar operaciones de consulta
                        while (dr.Read())
                        {
                            listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["nombre"].ToString(), tituloOperacion = dr["titulo"].ToString().Replace("'", ""), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = dr["maquina"].ToString(), categoriaMaquina = dr["categoria"].ToString() });
                        };
                        dr.Close();
                        cnIngenieria.Close();
                        //agregar operacion de empaque
                        listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = 0, nombreOperacion = "empaque", tituloOperacion = nombreEmpaque, samOperacion = samEmpaque, asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = "Mesa de Empaque", categoriaMaquina = "manual" });
                        #endregion

                        List<elementoListBox> agregadas = new List<elementoListBox>();
                        foreach (elementoListBox item in Operaciones.Items)
                        {
                            agregadas.Add(item);
                        }
                        double sam = 0;
                        #region calcularAsignaciones
                        foreach (elementoListBox item in listaOperaciones)
                        {
                            sam = sam + item.samOperacion;
                            bool agregado = false;
                            elementoListBox subitemValido= new elementoListBox();
                            foreach (elementoListBox subitem in agregadas)
                            {
                                if (item.nombreOperacion == subitem.nombreOperacion && item.samOperacion == subitem.samOperacion)
                                {
                                    agregado = true;
                                    subitemValido = subitem;
                                    agregadas.Remove(subitem);
                                    break;
                                }
                            }
                            if (agregado == true)
                            {
                                listaOperaciones2.Add(subitemValido);
                            }
                            else if (agregado == false)
                            {
                                listaOperaciones2.Add(item);
                            }
                        }

                        sam_.Content = Math.Round(sam,4);
                        sam_2.Content = Math.Round(sam, 4);
                        // se obtienen las piezas por hora
                        Double piezasRequeridasHora = Math.Round(Convert.ToDouble(piezas_de_corrida.Text) / Convert.ToDouble(horas_de_corrida.Text), 0);
                        piezas_por_hora.Content = piezasRequeridasHora;
                        foreach (elementoListBox item in listaOperaciones2)
                        {
                            Double requerido = Math.Round(piezasRequeridasHora / (60 / item.samOperacion), 2);
                            listaOperaciones3.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = item.correlativoOperacion, nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, samOperacion = item.samOperacion, asignadoOperacion = item.asignadoOperacion, requeridoOperacion = requerido, ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina });
                        }
                        Operaciones.ItemsSource = listaOperaciones3;
                        #endregion
                        CalculoAsignadoPorOperacion();
                        actualizarGrafica();
                        operacionSobrecargadaOperacionSubutilizada();
                        MessageBox.Show("Terminado");
                    }
                    else
                    {
                        MessageBox.Show("¿Ya notaste que no has colocado piezas por hora ni horas de corrida? \n " +
                            "esta opción es cuando previa validación sabes que el desgloce de operaciones de un estilo \n" +
                            "ha cambiado y ya tienes un layout generado, soluciona esto y vuelve a intentarlo.");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        #endregion
        #endregion
        #region ListViewEmisorDeData

        #region GetDataFromListView
        private static object GetDataFromListBox(ListView source, Point point)
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
        ListView dragSource = null;
        private void obtenerDatosListView(object sender, MouseButtonEventArgs e)
        {
            ListView parent = (ListView)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }
        #endregion
        #region rebalance()
        private void generarListaDeOperacionesRebalance()
        {
            List<ElementoRebalance> listaOperariosRebalance = new List<ElementoRebalance>();
            double piezasPorHora = Convert.ToDouble(piezas_por_hora.Content);
            #region areaEnganchador
            List<ElementoRebalance> listaOperariosBordeEnganchador = new List<ElementoRebalance>();
            string operarioEnganche = "";
            foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
            {
                if (objeto.Name == "stackPanelContenedor")
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEnganche = ((Label)elemento).Content.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperariosBordeEnganchador.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operarioEnganche, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
            }
            foreach (ElementoRebalance item in listaOperariosBordeEnganchador)
            {
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operarioEnganche, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

            }
            #endregion
            #region areaPreparacion
            foreach (GroupBox groupBox in areaPreparacion.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
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
                                    listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
                foreach (ElementoRebalance item in listaOperariosBorde)
                {
                    listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

                }
            }
            #endregion
            #region arteriaUno
            foreach (GroupBox groupBox in arteriaUno.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
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
                                    listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
                foreach (ElementoRebalance item in listaOperariosBorde)
                {
                    listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

                }
            }
            #endregion
            #region arteriaDos
            foreach (GroupBox groupBox in arteriaDos.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
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
                                    listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
                foreach (ElementoRebalance item in listaOperariosBorde)
                {
                    listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

                }
            }
            #endregion
            #region arteriaTres
            foreach (GroupBox groupBox in arteriaTres.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
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
                                    listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
                foreach (ElementoRebalance item in listaOperariosBorde)
                {
                    listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

                }
            }
            #endregion
            #region arteriaCuatro
            foreach (GroupBox groupBox in arteriaCuatro.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel detalleEstacion = (StackPanel)objeto;
                        foreach (object elemento in detalleEstacion.Children)
                        {
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
                                    listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion * Math.Round(piezasPorHora / (60 / item.samOperacion), 2), samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                                }
                            }
                        }
                    }
                }
                foreach (ElementoRebalance item in listaOperariosBorde)
                {
                    listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });

                }
            }
            #endregion

            //se completa listaOperariosRebalance que son los operarios con su operacion (pero no tienen el codigo porque este solo esta en la lista de operarios)
            //se hace una nueva lista donde se agrega el codigo
            #region listaDeOperariosConCodigo
            List<ElementoRebalance> listaOperariosConCodigo = new List<ElementoRebalance>();
            foreach (ElementoRebalance item in listaOperariosRebalance)
            {
                foreach (elementoListBox subitem in Operarios.Items)
                {
                    if (item.nombreOperario == subitem.nombreOperario)
                    {
                        listaOperariosConCodigo.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, codigoOperario = subitem.codigoOperario, nombreOperario = item.nombreOperario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
                    }
                }
            }
            #endregion
            // se declaran las variables de conexion
            #region verificarTomaDeTiempos
            List<ElementoRebalance> tiemposCargados = new List<ElementoRebalance>();
            if (modulo.SelectedIndex > -1)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select  codigo, operacion, tiempo_objetivo from toma_de_tiempos where modulo ='" + modulo.SelectedItem.ToString() + "' and estilo = '" + estilo_.Content.ToString() + "' and temporada ='" + temporada_.Content.ToString() + "' and version = '" + version_.Text + "'";
                // se agregan los modulos
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tiemposCargados.Add(new ElementoRebalance { codigoOperario = Convert.ToInt32(dr["codigo"]), nombreOperacion = dr["operacion"].ToString(), tiempoRebalance = Convert.ToDouble(dr["tiempo_objetivo"]) });
                };
                dr.Close();
                cn.Close();
            }
            #endregion
            #region lista agregandoTiempos
            List<ElementoRebalance> listaConTiempos = new List<ElementoRebalance>();
            foreach (ElementoRebalance item in listaOperariosConCodigo)
            {
                double tiempo = item.tiempoRebalance;
                foreach (ElementoRebalance subitem in tiemposCargados)
                {
                    if (item.nombreOperacion == subitem.nombreOperacion && item.codigoOperario == subitem.codigoOperario)
                    {
                        tiempo = subitem.tiempoRebalance;
                    }
                }
                listaConTiempos.Add(new ElementoRebalance { codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion * 60d, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = tiempo, eficienciaRebalance = 0, cargaRebalance = 0 });
            }
            #endregion
            rebalance_.ItemsSource = listaConTiempos;
        }
        private void eficienciaRebalance()
        {
            List<ElementoRebalance> rebalanceConDatos = new List<ElementoRebalance>();
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                double eficiencia = 0;
                double carga = 0;
                if (item.tiempoRebalance > 0)
                {
                    eficiencia = item.samOperacion / (item.tiempoRebalance);
                    carga = item.asignadoOperacion / eficiencia;
                }
                else
                {
                    eficiencia = 0;
                    carga = 0;
                }
                rebalanceConDatos.Add(new ElementoRebalance { codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, tituloOperacion = item.tituloOperacion, nombreOperacion = item.nombreOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = item.tiempoRebalance, eficienciaRebalance = eficiencia, cargaRebalance = carga });
            }
            rebalance_.ItemsSource = rebalanceConDatos;
        }
        private void tiempo_tomado_LostFocus(object sender, RoutedEventArgs e)
        {
            eficienciaRebalance();
            actualizarGraficaReal();
            actualizarGraficaEficiencia();
        }
        private void rebalance_bt_Click(object sender, RoutedEventArgs e)
        {
            generarListaDeOperacionesRebalance();
            eficienciaRebalance();
            actualizarGraficaReal();
            actualizarGraficaEficiencia();
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
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                if (item.tiempoRebalance > 0)
                {
                    cn.Open();
                    string sql2 = "insert into toma_de_tiempos (fecha, modulo, estilo, temporada, version, codigo, nombre, titulo, operacion, ajuste, sam, tiempo_objetivo) values('" + System.DateTime.Now.ToString() + "', '" + modulo_va + "', '" + estilo_.Content.ToString() + "', '" + temporada_.Content.ToString() + "', '" + version_.Text.ToString() + "', '" + item.codigoOperario + "', '" + item.nombreOperario + "', '" + item.tituloOperacion + "', '" + item.nombreOperacion + "', '" + item.ajusteMaquina + "', '" + item.samOperacion + "', '" + item.tiempoRebalance + "')";
                    SqlCommand cm2 = new SqlCommand(sql2, cn);
                    SqlDataReader dr2 = cm2.ExecuteReader();
                    dr2.Close();
                    cn.Close();
                }
            }

            MessageBox.Show("La Toma de Tiempos ha sido Cargada");
        }
        private void descargar_balance_Click(object sender, RoutedEventArgs e)
        {
            System.Text.StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("NOMBRE");
            buffer.Append(",");
            buffer.Append("OPERACION");
            buffer.Append(",");
            buffer.Append("SAM");
            buffer.Append(",");
            buffer.Append("ASIGNADO");
            buffer.Append(",");
            buffer.Append("TIEMPO OBJETIVO");
            buffer.Append(",");
            buffer.Append("EFICIENCIA");
            buffer.Append(",");
            buffer.Append("CARGA");
            buffer.Append("\n");
            #endregion
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                buffer.Append(item.nombreOperario);
                buffer.Append(",");
                buffer.Append(item.tituloOperacion);
                buffer.Append(",");
                buffer.Append(item.samOperacion);
                buffer.Append(",");
                buffer.Append(item.asignadoOperacion);
                buffer.Append(",");
                buffer.Append(item.tiempoRebalance);
                buffer.Append(",");
                buffer.Append(item.eficienciaRebalance);
                buffer.Append(",");
                buffer.Append(item.cargaRebalance);
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
        #region listBoxReceptorDeDatos
        private void receptor(object sender, DragEventArgs e)
        {
            StackPanel estacion = (StackPanel)sender;
            object informacion = e.Data.GetData(typeof(elementoListBox));

            elementoListBox informacionElemento = informacion as elementoListBox;

            //si es un operario el que se va a agregar ponerlo en el textbox directamente
            if(informacionElemento.identificador == "operario")
            {
                foreach (Object elemento in estacion.Children)
                {
                    if (elemento.GetType() == typeof(Label))
                    {
                        ((Label)elemento).Content = informacionElemento.nombreOperario;
                    }
                }
            }
            //si es una operacion validar cada informacion donde debe agregarse
            else if(informacionElemento.identificador == "operacion")
            {
                foreach (Object elemento in estacion.Children)
                {

                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ((ListBox)elemento).Items.Add(new elementoListBox() { nombreOperacion = informacionElemento.nombreOperacion, tituloOperacion = informacionElemento.tituloOperacion, asignadoOperacion = 1, correlativoOperacion = informacionElemento.correlativoOperacion, ajusteMaquina = informacionElemento.ajusteMaquina, samOperacion = informacionElemento.samOperacion });
                    }
                    else if (elemento.GetType() == typeof(TextBox))
                    {
                        ((TextBox)elemento).Text = informacionElemento.ajusteMaquina;
                    }
                    else if (elemento.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)elemento).Text = informacionElemento.categoriaMaquina;
                    }
                }
            }
            CalculoAsignadoPorOperacion();
            actualizarGrafica();
            operacionSobrecargadaOperacionSubutilizada();
        }
        #endregion
        #region coloresDeEstacion
        private void ColorEstacion(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Name)
            {
                case "Green":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.LightGreen;
                    break;
                case "Yellow":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.Yellow;
                    break;
                case "Red":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.Pink;
                    break;
                case "Orange":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.Orange;
                    break;
                case "Blue":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.LightBlue;
                    break;
                case "White":
                    ((radioButton.Parent as StackPanel).Parent as StackPanel).Background = Brushes.White;
                    break;
            }
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
        #region obtenerObjetosHijosDeDataTemplate
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
        #endregion
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
        private void piezas_de_corrida_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(piezas_de_corrida.Text) || Convert.ToInt32(piezas_de_corrida.Text)==0)
            {
                piezas_de_corrida.Text = "1";
            }
        }
        private void horas_de_corrida_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(horas_de_corrida.Text) || Convert.ToInt32(horas_de_corrida.Text) == 0)
            {
                horas_de_corrida.Text = "1";
            }

        }
        #endregion
        #region calculoDeAsigandoPorOperacion
        private void AsignadoPorOperacion(object sender, RoutedEventArgs e)
        {
            CalculoAsignadoPorOperacion();
            actualizarGrafica();
            operacionSobrecargadaOperacionSubutilizada();
        }
        private void CalculoAsignadoPorOperacion()
        {
            // se declara la lista donde se consolidara lo asignado a cada operacion por estacion, operarios asignados y resumen de maquinas
            List<elementoListBox> consolidadoAsignaciones = new List<elementoListBox>();
            List<elementoListBox> consolidadoOperarios = new List<elementoListBox>();
            List<maquina> consolidadoMaquinas = new List<maquina>();
            double piezasPorHora = Convert.ToDouble(piezas_por_hora.Content);
            //Codigo para verificar asignado por operacion se recorrera por cada arteria (para facilitar especificacion de donde estan los listBox que contienen los datos)

            //Recorrer el areadePreparacion (cada arteria es un StackPanel)
            //Dentro de cada arteria las estaciones estan formadas dentro de stack panels
            #region areaPreparacion
            foreach (GroupBox groupBox in areaPreparacion.Children)
            {
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        string operario = "";
                        string categoria = "";
                        string color = "blanco";
                        double cargaProporcional = 0;

                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "rojo";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "azul";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
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

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
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
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }
                        //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                        consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                        consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                    }
                }
            }
            #endregion
            #region arteriaUno
            foreach (GroupBox groupBox in arteriaUno.Children)
            {
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        string operario = "";
                        string categoria = "";
                        string color = "blanco";
                        double cargaProporcional = 0;

                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "rojo";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "azul";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
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

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
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
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }
                        //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                        consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                        consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                    }
                }
            }
            #endregion
            #region arteriaDos
            foreach (GroupBox groupBox in arteriaDos.Children)
            {
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        string operario = "";
                        string categoria = "";
                        string color = "blanco";
                        double cargaProporcional = 0;

                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "rojo";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "azul";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
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

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
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
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }
                        //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                        consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                        consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                    }
                }
            }
            #endregion
            #region arteriaTres
            foreach (GroupBox groupBox in arteriaTres.Children)
            {
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        string operario = "";
                        string categoria = "";
                        string color = "blanco";
                        double cargaProporcional = 0;

                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "rojo";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "azul";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
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

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
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
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }
                        //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                        consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                        consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                    }
                }
            }
            #endregion
            #region arteriaCuatro
            foreach (GroupBox groupBox in arteriaCuatro.Children)
            {
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        string operario = "";
                        string categoria = "";
                        string color = "blanco";
                        double cargaProporcional = 0;

                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "rojo";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "azul";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
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

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
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
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }
                        //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                        consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                        consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                    }
                }
            }
            #endregion
            #region enganche
            foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
            {
                if (objeto.Name == "stackPanelContenedor")
                {
                    StackPanel estacion = (StackPanel)objeto;

                    string operario = "";
                    string categoria = "";
                    string color = "blanco";
                    double cargaProporcional = 0;

                    #region colorEnString
                    if (estacion.Background == Brushes.Pink)
                    {
                        color = "rojo";
                    }
                    else if (estacion.Background == Brushes.LightBlue)
                    {
                        color = "azul";
                    }
                    else if (estacion.Background == Brushes.LightGreen)
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

                    //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                    foreach (object elemento in estacion.Children)
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
                                cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                            }
                        }
                    }
                    //Por cada estacion (stackPanel) se agrega el operario y los datos que le corresponden
                    consolidadoOperarios.Add(new elementoListBox { nombreOperario = operario, asignadoOperario = cargaProporcional, categoriaMaquina = categoria });
                    consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });
                }
            }
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

                if (operacion.requeridoOperacion > 0)
                {
                    relacionAsignacion = asignadoTotal / operacion.requeridoOperacion;
                }
                else
                {
                    relacionAsignacion = 0;
                }


                nuevaListaOperaciones.Add(new elementoListBox { identificador = "operacion", correlativoOperacion = operacion.correlativoOperacion, nombreOperacion = operacion.nombreOperacion, tituloOperacion = operacion.tituloOperacion, samOperacion = operacion.samOperacion, asignadoOperacion = asignadoTotal, requeridoOperacion = operacion.requeridoOperacion, ajusteMaquina = operacion.ajusteMaquina, categoriaMaquina = operacion.categoriaMaquina });
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
                    if (operario.nombreOperario == asigando.nombreOperario && asigando.categoriaMaquina == "atracadora")
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
                nuevaListaOperarios.Add(new elementoListBox() { identificador = "operario", codigoOperario = operario.codigoOperario, nombreOperario = operario.nombreOperario, asignadoOperario = asignadoTotal, planaOperario = operario.planaOperario, ranaOperario = operario.ranaOperario, flatOperario = operario.flatOperario, coverOperario = operario.coverOperario, transferOperario = operario.transferOperario, atracadoraOperario = operario.atracadoraOperario, planchaOperario = operario.planchaOperario, bondingOperario = operario.bondingOperario, zigzagOperario = operario.zigzagOperario, multiagujaOperario = operario.multiagujaOperario, manualOperario = operario.manualOperario, variasOperario = operario.variasOperario });
            }
            Operarios.ItemsSource = nuevaListaOperarios;
            #endregion
            #region crearNuevaListaMaquinas
            List<maquina> nuevaListaMaquinas = new List<maquina>();
            foreach (maquina maquina in resumen_maquinas.Items)
            {
                int ii = 0, ie = 0, ee = 0, ei = 0, na = 0;
                foreach (maquina item in consolidadoMaquinas)
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
                operacionSobrecargadaOperacionSubutilizada();
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
                        items.Add(new elementoListBox { identificador = "operario", codigoOperario = item.codigoOperario, nombreOperario = item.nombreOperario, asignadoOperario = item.asignadoOperario, planaOperario = item.planaOperario, ranaOperario = item.ranaOperario, flatOperario = item.flatOperario, coverOperario = item.coverOperario, transferOperario = item.transferOperario, atracadoraOperario = item.atracadoraOperario, planchaOperario = item.planchaOperario, bondingOperario = item.bondingOperario, zigzagOperario = item.zigzagOperario, multiagujaOperario = item.multiagujaOperario, manualOperario = item.manualOperario, variasOperario = item.variasOperario });
                    }
                }
                Operarios.ItemsSource = items;

                // Actualizar grafica
                actualizarGrafica();

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
            List<operario> listaOperariosConCarga = concatenacionOperariosOperacionA();
            double piezasCorrida= Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
            double horasCorrida= Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
            double tkt_ = 3600 / (piezasCorrida / horasCorrida);

            //se crea una lista de strings para las etiquetas del eje horizontal (los nombres de los operarios) solo se agregan los que ya han sido asignados
            List<string> listaDeOperarios = new List<string>();
            foreach (operario item in listaOperariosConCarga)
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
            grafico.AxisX.Add(new Axis() { Labels = listaDeOperarios.ToArray(), LabelsRotation = 89, ShowLabels = true, Separator = { Step = 1 }, FontSize=20, Foreground=Brushes.Black });
            //se agregan los valores de las cargas en las columnas
            foreach (operario item in listaOperariosConCarga)
            {
                if (item.asignadoOperario > 0)
                {
                    SeriesCollection[0].Values.Add(Math.Round(item.asignadoOperario, 2));
                    SeriesCollection[1].Values.Add(tkt_);
                    SeriesCollection[2].Values.Add(0.9*tkt_);
                }
            };
        }
        private List<operario> concatenacionOperariosOperacionA()
        {
            List<ElementoRebalance> listaOperariosRebalance = new List<ElementoRebalance>();
            double piezasPorHora= Convert.ToDouble(piezas_por_hora.Content);
            #region Enganche
            string operarioe = "";
            string operacionese = "";
            double cargaProporcionale = 0;
            foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
            {
                if (objeto.Name == "stackPanelContenedor")
                {
                    StackPanel estacion = (StackPanel)objeto;
                    foreach (object elemento in estacion.Children)
                    {

                        if (elemento.GetType() == typeof(Label))
                        {
                            operarioe = ((Label)elemento).Content.ToString();
                        }
                        if (elemento.GetType() == typeof(ListBox))
                        {
                            ListBox listaDeOperaciones = ((ListBox)elemento);

                            foreach (elementoListBox item in listaDeOperaciones.Items)
                            {
                                operacionese = operacionese + item.tituloOperacion + "\n";
                                cargaProporcionale = cargaProporcionale + Math.Round(piezasPorHora / (60 / item.samOperacion));
                            }
                        }
                    }

                }
            }
            listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operarioe, tituloOperacion = operacionese, requeridoOperacion = cargaProporcionale });
            #endregion
            #region arteriaUno
            foreach (GroupBox groupBox in arteriaUno.Children)
            {
                string operario = "";
                string operaciones = "";
                double cargaProporcional = 0;
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
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
                                    operaciones = operaciones + item.tituloOperacion + "\n";
                                    cargaProporcional = cargaProporcional + Math.Round(piezasPorHora / (60 / item.samOperacion), 2);
                                }
                            }
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim(), requeridoOperacion = cargaProporcional });
            }
            #endregion
            #region arteriaDos
            foreach (GroupBox groupBox in arteriaDos.Children)
            {
                string operario = "";
                string operaciones = "";
                double cargaProporcional = 0;
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
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
                                    operaciones = operaciones + item.tituloOperacion + "\n";
                                    cargaProporcional = cargaProporcional + Math.Round(piezasPorHora / (60 / item.samOperacion), 2);
                                }
                            }
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim(), requeridoOperacion = cargaProporcional });
            }
            #endregion
            #region arteriaTres
            foreach (GroupBox groupBox in arteriaTres.Children)
            {
                string operario = "";
                string operaciones = "";
                double cargaProporcional = 0;
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
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
                                    operaciones = operaciones + item.tituloOperacion + "\n";
                                    cargaProporcional = cargaProporcional + Math.Round(piezasPorHora / (60 / item.samOperacion), 2);
                                }
                            }
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim(), requeridoOperacion = cargaProporcional });
            }
            #endregion
            #region arteriaCuatro
            foreach (GroupBox groupBox in arteriaCuatro.Children)
            {
                string operario = "";
                string operaciones = "";
                double cargaProporcional = 0;
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
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
                                    operaciones = operaciones + item.tituloOperacion + "\n";
                                    cargaProporcional = cargaProporcional + Math.Round(piezasPorHora / (60 / item.samOperacion), 2);
                                }
                            }
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim(), requeridoOperacion = cargaProporcional });
            }
            #endregion
            var results = listaOperariosRebalance.Select(x => x.nombreOperario).Distinct();
            List<string> listaNombresUnicos = new List<string>();
            listaNombresUnicos = results.ToList();
            List<Operacion> operarioOperaciones = new List<Operacion>();
            foreach (string item in listaNombresUnicos)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string operaciones = "";
                    double requeridoTotal = 0;
                    foreach (ElementoRebalance subitem in listaOperariosRebalance)
                    {
                        if (item == subitem.nombreOperario)
                        {
                            operaciones = operaciones + subitem.tituloOperacion + "\n";
                            requeridoTotal = requeridoTotal + subitem.requeridoOperacion;
                        }
                    }

                    operarioOperaciones.Add(new Operacion { nombreOperario = item, tituloOperacion = item + "\n" + operaciones, requeridoOperacion=requeridoTotal });
                }
            }
            List<operario> listaFinal = new List<operario>();
            double takt = 0;
            if (!string.IsNullOrEmpty(piezas_de_corrida.Text) & !string.IsNullOrEmpty(horas_de_corrida.Text))
            {
                if (Convert.ToDouble(piezas_de_corrida.Text) > 0 & Convert.ToDouble(horas_de_corrida.Text) > 0)
                {
                    takt = 3600 / (Convert.ToDouble(piezas_de_corrida.Text) / Convert.ToDouble(horas_de_corrida.Text));
                }
            }
            foreach (Operacion item in operarioOperaciones)
            {
                double asignado = 0;
                foreach (operario subitem in Operarios.Items)
                {
                    if (item.nombreOperario == subitem.nombreOperario)
                    {
                        asignado = asignado + subitem.asignadoOperario;
                    }
                }
                listaFinal.Add(new operario { nombreOperario = item.tituloOperacion, asignadoOperario = asignado * takt*item.requeridoOperacion});
            }
            string cadena = "";
            foreach (operario item in listaFinal)
            {
                cadena = cadena + item.nombreOperario + "-" + item.asignadoOperario + "\n";
            }
            return listaFinal;
        }
        #endregion
        #region operarioSobrecargadoSubutilizado
        private void operacionSobrecargadaOperacionSubutilizada()
        {
            List<elementoListBox> lista = new List<elementoListBox>();
            foreach (elementoListBox item in Operarios.Items)
            {
                lista.Add(new elementoListBox { nombreOperario = item.nombreOperario, asignadoOperario = item.asignadoOperario });
            };
            try
            {
                if (lista.Min(x => x.asignadoOperario) < 0.75)
                {
                    int datoMinimo = lista.FindIndex(r => r.asignadoOperario.Equals(lista.Min(x => x.asignadoOperario)));
                    subutilizacion_2.Content = lista[datoMinimo].nombreOperario;
                }
                else
                {
                    subutilizacion_2.Content = "----";
                }
                if (lista.Max(x => x.asignadoOperario) > 1.0)
                {
                    int datoMaximo = lista.FindIndex(r => r.asignadoOperario.Equals(lista.Max(x => x.asignadoOperario)));
                    sobrecarga_2.Content = lista[datoMaximo].nombreOperario;
                }
                else
                {
                    sobrecarga_2.Content = "----";
                }
            }
            catch { }
        }
        #endregion
        #region ActualizarDatosDeGraficaRebalance
        private void actualizarGraficaReal()
        {
            List<ElementoRebalance> listaConsolidada = generarListaRebalance();
            double piezasCorrida = Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
            double horasCorrida = Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
            double tkt_ = 3600 / (piezasCorrida / horasCorrida);

            List<string> nombreOperacion = new List<string>();
            foreach (ElementoRebalance item in listaConsolidada)
            {
                nombreOperacion.Add(item.nombreOperario);
            }
            //se limpian los datos cargados anteriormente para poder volver a cargar
            graficoRebalance.AxisX.Clear();
            DatosGraficaRebalance[0].Values.Clear();
            DatosGraficaRebalance[1].Values.Clear();
            DatosGraficaRebalance[2].Values.Clear();
            //se agrega la lista de operarios hecha al principio
            graficoRebalance.AxisX.Add(new Axis() { Labels = nombreOperacion.ToArray(), LabelsRotation = 89, ShowLabels = true, Separator = { Step = 1 }, FontSize=20, Foreground=Brushes.Black });
            //se agregan los valores de las cargas en las columnas
            foreach (ElementoRebalance item in listaConsolidada)
            {
                DatosGraficaRebalance[0].Values.Add(Math.Round(item.cargaRebalance*tkt_,2));
                DatosGraficaRebalance[1].Values.Add(tkt_);
                DatosGraficaRebalance[2].Values.Add(0.9*tkt_);
            };
        }
        private void actualizarGraficaEficiencia()
        {
            List<string> nombres = new List<string>();
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                nombres.Add(item.nombreOperario + "\n" + item.tituloOperacion);
            }
            //se limpian los datos cargados anteriormente para poder volver a cargar
            graficoEficiencia.AxisX.Clear();
            DatosGraficaEficiencia[0].Values.Clear();
            DatosGraficaEficiencia[1].Values.Clear();
            //se agrega la lista de operarios hecha al principio
            graficoEficiencia.AxisX.Add(new Axis() { Labels = nombres.ToArray(), LabelsRotation = 89, ShowLabels = true, Separator = { Step = 1 }, FontSize = 20, Foreground = Brushes.Black });
            //se agregan los valores de las cargas en las columnas
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                DatosGraficaEficiencia[0].Values.Add(Math.Round(item.eficienciaRebalance,2));
                DatosGraficaEficiencia[1].Values.Add(1.0);
            };
        }
        private List<ElementoRebalance> generarListaRebalance()
        {
            //se crea una lista de strings para las etiquetas del eje horizontal (los nombres de los operarios) solo se agregan los que ya han sido asignados
            List<string> listaDeOperarios = new List<string>();
            foreach (ElementoRebalance item in rebalance_.Items)
            {
                listaDeOperarios.Add(item.nombreOperario);
            }

            //En esa lista se dejan nombres unicos (a algunos operarios se les asigna mas de una operacion)
            IList<string> listaDeOperariosUnica = listaDeOperarios.Distinct().ToList();

            //se crea lista donde a los nombres unicos de operarios se le asigna la suma de sus datos
            List<ElementoRebalance> listaConsolidada = new List<ElementoRebalance>();
            foreach (string item in listaDeOperariosUnica)
            {
                double sumaCargas = 0;
                string operaciones = "";
                foreach (ElementoRebalance subitem in rebalance_.Items)
                {
                    if (item.ToString() == subitem.nombreOperario)
                    {
                        sumaCargas = sumaCargas + subitem.cargaRebalance;
                        operaciones = operaciones + subitem.tituloOperacion + "\n";
                    }
                };

                listaConsolidada.Add(new ElementoRebalance { nombreOperario = item + "\n" + operaciones.Trim(), cargaRebalance = sumaCargas });
            }
            return listaConsolidada;
        }
        #endregion
        #region removerOperario
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Content = "";
            CalculoAsignadoPorOperacion();
            actualizarGrafica();
        }
        #endregion
        #endregion
        #region modoTabla
        private void btnArriba_Click(object sender, RoutedEventArgs e)
        {
            if (lstModoTabla.SelectedIndex > -1)
            {
                int nuevoIndex = lstModoTabla.SelectedIndex - 1;

                if (nuevoIndex < 0)
                {
                    nuevoIndex = 0;
                }

                elementoListBox itemSeleccionado = (elementoListBox)lstModoTabla.SelectedItem;

                //eliminar el item seleccionado
                lstModoTabla.Items.Remove(itemSeleccionado);
                lstModoTabla.Items.Insert(nuevoIndex,itemSeleccionado);
                ordenarOperaciones();
                lstModoTabla.SelectedIndex = nuevoIndex;
            }
        }
        private void btnAbajo_Click(object sender, RoutedEventArgs e)
        {
            if (lstModoTabla.SelectedIndex > -1)
            {
                int nuevoIndex = lstModoTabla.SelectedIndex +1;

                if (nuevoIndex > 64)
                {
                    nuevoIndex = 64;
                }

                elementoListBox itemSeleccionado = (elementoListBox)lstModoTabla.SelectedItem;

                //eliminar el item seleccionado
                lstModoTabla.Items.Remove(itemSeleccionado);
                lstModoTabla.Items.Insert(nuevoIndex, itemSeleccionado);
                ordenarOperaciones();
                lstModoTabla.SelectedIndex = nuevoIndex;
            }

        }
        private void ordenarOperaciones()
        {
            int ordenCorrelativo = 0;
            List<elementoListBox> elementos = new List<elementoListBox>();
            foreach(elementoListBox item in lstModoTabla.Items)
            {
                elementos.Add(item);
            }
            lstModoTabla.Items.Clear();

            foreach(elementoListBox item in elementos)
            {
                if (ordenCorrelativo < 13)
                {
                    lstModoTabla.Items.Add(new elementoListBox { correlativoMaquina = item.correlativoMaquina, codigoMaquina = item.codigoMaquina, asignadoOperario = item.asignadoOperario, nombreEstacion = "Preparación " + (ordenCorrelativo+1), ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina, nombreOperario = item.nombreOperario, colorAjuste = item.colorAjuste, operacionesAgregadas = item.operacionesAgregadas });
                }
                else if (ordenCorrelativo < 65)
                {
                    lstModoTabla.Items.Add(new elementoListBox {correlativoMaquina=item.correlativoMaquina, codigoMaquina=item.codigoMaquina, asignadoOperario=item.asignadoOperario, nombreEstacion = "Estación " + (ordenCorrelativo - 12), ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina, nombreOperario = item.nombreOperario, colorAjuste = item.colorAjuste, operacionesAgregadas = item.operacionesAgregadas });
                }
                else
                {
                    lstModoTabla.Items.Add(new elementoListBox { correlativoMaquina = item.correlativoMaquina, codigoMaquina = item.codigoMaquina, asignadoOperario = item.asignadoOperario, nombreEstacion = "Enganche", ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina, nombreOperario = item.nombreOperario, colorAjuste = item.colorAjuste, operacionesAgregadas = item.operacionesAgregadas });
                }
                ordenCorrelativo = ordenCorrelativo + 1;
            }
        }
        private void btnModoTabla_Checked(object sender, RoutedEventArgs e)
        {
            lstModoTabla.Items.Clear();
            control_tab_maquinas.SelectedIndex = 1;
            tab0.Visibility = Visibility.Collapsed;
            tab1.Visibility = Visibility.Visible;

            List<elementoListBox> nuevalista = listaDeOperacionesAgregadas();
            foreach(elementoListBox item in nuevalista)
            {
                lstModoTabla.Items.Add(item);
            }
        }
        private void btnModoTabla_Unchecked(object sender, RoutedEventArgs e)
        {
            #region areaPreparacion
            foreach (GroupBox groupBox in areaPreparacion.Children)
            {
                string nombreEstacion = groupBox.Header.ToString();
                foreach(elementoListBox item in lstModoTabla.Items)
                {
                    if(nombreEstacion== item.nombreEstacion)
                    {
                        foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                        {
                            if (objeto.Name == "stackPanelContenedor")
                            {
                                StackPanel estacion = (StackPanel)objeto;

                                //determinar el color
                                #region agregarColor
                                switch (item.colorAjuste)
                                {
                                    case "Pink":
                                        estacion.Background = Brushes.Pink;
                                        break;
                                    case "LightBlue":
                                        estacion.Background = Brushes.LightBlue;
                                        break;
                                    case "LightGreen":
                                        estacion.Background = Brushes.LightGreen;
                                        break;
                                    case "Yellow":
                                        estacion.Background = Brushes.Yellow;
                                        break;
                                    case "Orange":
                                        estacion.Background = Brushes.Orange;
                                        break;
                                    case "White":
                                        estacion.Background = Brushes.White;
                                        break;
                                }
                                #endregion

                                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                                foreach (object elemento in estacion.Children)
                                {
                                    //determinar la categoria de maquina
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    //si es label se carga operario
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    //cargar codigo de maquina
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    //si es textBox se carga ajuste
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Clear();

                                        foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                        {
                                            listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region arteriaUno
            foreach (GroupBox groupBox in arteriaUno.Children)
            {
                string nombreEstacion = groupBox.Header.ToString();
                foreach (elementoListBox item in lstModoTabla.Items)
                {
                    if (nombreEstacion == item.nombreEstacion)
                    {
                        foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                        {
                            if (objeto.Name == "stackPanelContenedor")
                            {
                                StackPanel estacion = (StackPanel)objeto;

                                //determinar el color
                                #region agregarColor
                                switch (item.colorAjuste)
                                {
                                    case "Pink":
                                        estacion.Background = Brushes.Pink;
                                        break;
                                    case "LightBlue":
                                        estacion.Background = Brushes.LightBlue;
                                        break;
                                    case "LightGreen":
                                        estacion.Background = Brushes.LightGreen;
                                        break;
                                    case "Yellow":
                                        estacion.Background = Brushes.Yellow;
                                        break;
                                    case "Orange":
                                        estacion.Background = Brushes.Orange;
                                        break;
                                    case "White":
                                        estacion.Background = Brushes.White;
                                        break;
                                }
                                #endregion

                                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                                foreach (object elemento in estacion.Children)
                                {
                                    //determinar la categoria de maquina
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    //si es label se carga operario
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    //cargar codigo de maquina
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    //si es textBox se carga ajuste
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Clear();

                                        foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                        {
                                            listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region arteriaDos
            foreach (GroupBox groupBox in arteriaDos.Children)
            {
                string nombreEstacion = groupBox.Header.ToString();
                foreach (elementoListBox item in lstModoTabla.Items)
                {
                    if (nombreEstacion == item.nombreEstacion)
                    {
                        foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                        {
                            if (objeto.Name == "stackPanelContenedor")
                            {
                                StackPanel estacion = (StackPanel)objeto;

                                //determinar el color
                                #region agregarColor
                                switch (item.colorAjuste)
                                {
                                    case "Pink":
                                        estacion.Background = Brushes.Pink;
                                        break;
                                    case "LightBlue":
                                        estacion.Background = Brushes.LightBlue;
                                        break;
                                    case "LightGreen":
                                        estacion.Background = Brushes.LightGreen;
                                        break;
                                    case "Yellow":
                                        estacion.Background = Brushes.Yellow;
                                        break;
                                    case "Orange":
                                        estacion.Background = Brushes.Orange;
                                        break;
                                    case "White":
                                        estacion.Background = Brushes.White;
                                        break;
                                }
                                #endregion

                                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                                foreach (object elemento in estacion.Children)
                                {
                                    //determinar la categoria de maquina
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    //si es label se carga operario
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    //cargar codigo de maquina
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    //si es textBox se carga ajuste
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Clear();

                                        foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                        {
                                            listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region arteriaTres
            foreach (GroupBox groupBox in arteriaTres.Children)
            {
                string nombreEstacion = groupBox.Header.ToString();
                foreach (elementoListBox item in lstModoTabla.Items)
                {
                    if (nombreEstacion == item.nombreEstacion)
                    {
                        foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                        {
                            if (objeto.Name == "stackPanelContenedor")
                            {
                                StackPanel estacion = (StackPanel)objeto;

                                //determinar el color
                                #region agregarColor
                                switch (item.colorAjuste)
                                {
                                    case "Pink":
                                        estacion.Background = Brushes.Pink;
                                        break;
                                    case "LightBlue":
                                        estacion.Background = Brushes.LightBlue;
                                        break;
                                    case "LightGreen":
                                        estacion.Background = Brushes.LightGreen;
                                        break;
                                    case "Yellow":
                                        estacion.Background = Brushes.Yellow;
                                        break;
                                    case "Orange":
                                        estacion.Background = Brushes.Orange;
                                        break;
                                    case "White":
                                        estacion.Background = Brushes.White;
                                        break;
                                }
                                #endregion

                                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                                foreach (object elemento in estacion.Children)
                                {
                                    //determinar la categoria de maquina
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    //si es label se carga operario
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    //cargar codigo de maquina
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    //si es textBox se carga ajuste
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Clear();

                                        foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                        {
                                            listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region arteriaCuatro
            foreach (GroupBox groupBox in arteriaCuatro.Children)
            {
                string nombreEstacion = groupBox.Header.ToString();
                foreach (elementoListBox item in lstModoTabla.Items)
                {
                    if (nombreEstacion == item.nombreEstacion)
                    {
                        foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                        {
                            if (objeto.Name == "stackPanelContenedor")
                            {
                                StackPanel estacion = (StackPanel)objeto;

                                //determinar el color
                                #region agregarColor
                                switch (item.colorAjuste)
                                {
                                    case "Pink":
                                        estacion.Background = Brushes.Pink;
                                        break;
                                    case "LightBlue":
                                        estacion.Background = Brushes.LightBlue;
                                        break;
                                    case "LightGreen":
                                        estacion.Background = Brushes.LightGreen;
                                        break;
                                    case "Yellow":
                                        estacion.Background = Brushes.Yellow;
                                        break;
                                    case "Orange":
                                        estacion.Background = Brushes.Orange;
                                        break;
                                    case "White":
                                        estacion.Background = Brushes.White;
                                        break;
                                }
                                #endregion

                                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                                foreach (object elemento in estacion.Children)
                                {
                                    //determinar la categoria de maquina
                                    if (elemento.GetType() == typeof(TextBlock))
                                    {
                                        ((TextBlock)elemento).Text = item.categoriaMaquina;
                                    }
                                    //si es label se carga operario
                                    if (elemento.GetType() == typeof(Label))
                                    {
                                        ((Label)elemento).Content = item.nombreOperario;
                                    }
                                    //cargar codigo de maquina
                                    if (elemento.GetType() == typeof(StackPanel))
                                    {
                                        StackPanel contenedorSecundario = (StackPanel)elemento;
                                        foreach (object subElemento in contenedorSecundario.Children)
                                        {
                                            if (subElemento.GetType() == typeof(TextBox))
                                            {
                                                ((TextBox)subElemento).Text = item.codigoMaquina;
                                            }
                                        }
                                    }
                                    //si es textBox se carga ajuste
                                    if (elemento.GetType() == typeof(TextBox))
                                    {
                                        ((TextBox)elemento).Text = item.ajusteMaquina;
                                    }
                                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                    if (elemento.GetType() == typeof(ListBox))
                                    {
                                        ListBox listaDeOperaciones = ((ListBox)elemento);
                                        listaDeOperaciones.Items.Clear();

                                        foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                        {
                                            listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region enganche
            foreach (elementoListBox item in lstModoTabla.Items)
            {
                if ("Enganche" == item.nombreEstacion)
                {
                    foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
                    {
                        if (objeto.Name == "stackPanelContenedor")
                        {
                            StackPanel estacion = (StackPanel)objeto;

                            //determinar el color
                            #region agregarColor
                            switch (item.colorAjuste)
                            {
                                case "Pink":
                                    estacion.Background = Brushes.Pink;
                                    break;
                                case "LightBlue":
                                    estacion.Background = Brushes.LightBlue;
                                    break;
                                case "LightGreen":
                                    estacion.Background = Brushes.LightGreen;
                                    break;
                                case "Yellow":
                                    estacion.Background = Brushes.Yellow;
                                    break;
                                case "Orange":
                                    estacion.Background = Brushes.Orange;
                                    break;
                                case "White":
                                    estacion.Background = Brushes.White;
                                    break;
                            }
                            #endregion

                            //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                            foreach (object elemento in estacion.Children)
                            {
                                //determinar la categoria de maquina
                                if (elemento.GetType() == typeof(TextBlock))
                                {
                                    ((TextBlock)elemento).Text = item.categoriaMaquina;
                                }
                                //si es label se carga operario
                                if (elemento.GetType() == typeof(Label))
                                {
                                    ((Label)elemento).Content = item.nombreOperario;
                                }
                                //cargar codigo de maquina
                                if (elemento.GetType() == typeof(StackPanel))
                                {
                                    StackPanel contenedorSecundario = (StackPanel)elemento;
                                    foreach (object subElemento in contenedorSecundario.Children)
                                    {
                                        if (subElemento.GetType() == typeof(TextBox))
                                        {
                                            ((TextBox)subElemento).Text = item.codigoMaquina;
                                        }
                                    }
                                }
                                //si es textBox se carga ajuste
                                if (elemento.GetType() == typeof(TextBox))
                                {
                                    ((TextBox)elemento).Text = item.ajusteMaquina;
                                }
                                //Si el objeto es el listBox se analizan los valores de las operaciones en el
                                if (elemento.GetType() == typeof(ListBox))
                                {
                                    ListBox listaDeOperaciones = ((ListBox)elemento);
                                    listaDeOperaciones.Items.Clear();

                                    foreach (operacionesAgregadas subitem in item.operacionesAgregadas)
                                    {
                                        listaDeOperaciones.Items.Add(new elementoListBox { nombreOperacion = subitem.nombreOperacion, tituloOperacion = subitem.tituloOperacion, asignadoOperacion = subitem.asignadoOperacion, correlativoOperacion = subitem.correlativoOperacion, ajusteMaquina = subitem.ajusteMaquina, samOperacion = subitem.samOperacion });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            control_tab_maquinas.SelectedIndex = 0;
            tab0.Visibility = Visibility.Visible;
            tab1.Visibility = Visibility.Collapsed;
        }
        #endregion

        private List<elementoListBox> listaDeOperacionesAgregadas()
        {
            int correlativoMaquina = 0;
            List<elementoListBox> lista = new List<elementoListBox>();
            double piezasPorHora = Convert.ToDouble(piezas_por_hora.Content);
            #region areaPreparacion
            foreach (GroupBox groupBox in areaPreparacion.Children)
            {
                correlativoMaquina = correlativoMaquina + 1;
                string nombreEstacion = groupBox.Header.ToString();
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                        string operario = "";
                        string ajusteEstacion = "";
                        string categoria = "";
                        string color = "White";
                        double cargaProporcional = 0;
                        string codigoMaquina = "";

                        //determinar el color
                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "Pink";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "LightBlue";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
                        {
                            color = "LightGreen";
                        }
                        else if (estacion.Background == Brushes.Orange)
                        {
                            color = "Orange";
                        }
                        else if (estacion.Background == Brushes.Yellow)
                        {
                            color = "Yellow";
                        }
                        #endregion

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
                            //determinar la categoria de maquina
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                categoria = ((TextBlock)elemento).Text;
                            }
                            //determinar el codigo de la maquina
                            if (elemento.GetType() == typeof(StackPanel))
                            {
                                StackPanel contenedorSecundario = (StackPanel)elemento;
                                foreach(object subElemento in contenedorSecundario.Children)
                                {
                                    if(subElemento.GetType() == typeof(TextBox))
                                    {
                                        codigoMaquina = ((TextBox)subElemento).Text;
                                    }
                                }
                            }
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operario = ((Label)elemento).Content.ToString();
                            }
                            //si es textBox se carga ajuste
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text;
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }

                        lista.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, codigoMaquina=codigoMaquina, nombreOperario = operario, nombreEstacion = nombreEstacion, ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                    }
                }
            }
            #endregion
            #region arteriaUno
            foreach (GroupBox groupBox in arteriaUno.Children)
            {
                correlativoMaquina = correlativoMaquina + 1;
                string nombreEstacion = groupBox.Header.ToString();
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                        string operario = "";
                        string ajusteEstacion = "";
                        string categoria = "";
                        string color = "White";
                        double cargaProporcional = 0;
                        string codigoMaquina = "";

                        //determinar el color
                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "Pink";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "LightBlue";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
                        {
                            color = "LightGreen";
                        }
                        else if (estacion.Background == Brushes.Orange)
                        {
                            color = "Orange";
                        }
                        else if (estacion.Background == Brushes.Yellow)
                        {
                            color = "Yellow";
                        }
                        #endregion

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
                            //determinar la categoria de maquina
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                categoria = ((TextBlock)elemento).Text;
                            }
                            //determinar el codigo de la maquina
                            if (elemento.GetType() == typeof(StackPanel))
                            {
                                StackPanel contenedorSecundario = (StackPanel)elemento;
                                foreach (object subElemento in contenedorSecundario.Children)
                                {
                                    if (subElemento.GetType() == typeof(TextBox))
                                    {
                                        codigoMaquina = ((TextBox)subElemento).Text;
                                    }
                                }
                            }
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operario = ((Label)elemento).Content.ToString();
                            }
                            //si es textBox se carga ajuste
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text;
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }

                        lista.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, codigoMaquina = codigoMaquina, nombreOperario = operario, nombreEstacion = nombreEstacion, ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                    }
                }
            }
            #endregion
            #region arteriaDos
            List<elementoListBox> listaConsolidadaEstacion2 = new List<elementoListBox>();
            foreach (GroupBox groupBox in arteriaDos.Children)
            {
                correlativoMaquina = correlativoMaquina + 1;
                string nombreEstacion = groupBox.Header.ToString();
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                        string operario = "";
                        string ajusteEstacion = "";
                        string categoria = "";
                        string color = "White";
                        double cargaProporcional = 0;
                        string codigoMaquina = "";

                        //determinar el color
                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "Pink";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "LightBlue";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
                        {
                            color = "LightGreen";
                        }
                        else if (estacion.Background == Brushes.Orange)
                        {
                            color = "Orange";
                        }
                        else if (estacion.Background == Brushes.Yellow)
                        {
                            color = "Yellow";
                        }
                        #endregion

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
                            //determinar la categoria de maquina
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                categoria = ((TextBlock)elemento).Text;
                            }
                            //determinar el codigo de la maquina
                            if (elemento.GetType() == typeof(StackPanel))
                            {
                                StackPanel contenedorSecundario = (StackPanel)elemento;
                                foreach (object subElemento in contenedorSecundario.Children)
                                {
                                    if (subElemento.GetType() == typeof(TextBox))
                                    {
                                        codigoMaquina = ((TextBox)subElemento).Text;
                                    }
                                }
                            }
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operario = ((Label)elemento).Content.ToString();
                            }
                            //si es textBox se carga ajuste
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text;
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }

                        listaConsolidadaEstacion2.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, codigoMaquina = codigoMaquina, nombreOperario = operario, nombreEstacion = nombreEstacion, ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                    }
                }
            }
            listaConsolidadaEstacion2.Reverse();
            foreach (elementoListBox item in listaConsolidadaEstacion2)
            {
                lista.Add(item);
            }
            #endregion
            #region arteriaTres
            foreach (GroupBox groupBox in arteriaTres.Children)
            {
                correlativoMaquina = correlativoMaquina + 1;
                string nombreEstacion = groupBox.Header.ToString();
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                        string operario = "";
                        string ajusteEstacion = "";
                        string categoria = "";
                        string color = "White";
                        double cargaProporcional = 0;
                        string codigoMaquina = "";

                        //determinar el color
                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "Pink";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "LightBlue";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
                        {
                            color = "LightGreen";
                        }
                        else if (estacion.Background == Brushes.Orange)
                        {
                            color = "Orange";
                        }
                        else if (estacion.Background == Brushes.Yellow)
                        {
                            color = "Yellow";
                        }
                        #endregion

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
                            //determinar la categoria de maquina
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                categoria = ((TextBlock)elemento).Text;
                            }
                            //determinar el codigo de la maquina
                            if (elemento.GetType() == typeof(StackPanel))
                            {
                                StackPanel contenedorSecundario = (StackPanel)elemento;
                                foreach (object subElemento in contenedorSecundario.Children)
                                {
                                    if (subElemento.GetType() == typeof(TextBox))
                                    {
                                        codigoMaquina = ((TextBox)subElemento).Text;
                                    }
                                }
                            }
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operario = ((Label)elemento).Content.ToString();
                            }
                            //si es textBox se carga ajuste
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text;
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }

                        lista.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, codigoMaquina = codigoMaquina, nombreOperario = operario, nombreEstacion = nombreEstacion, ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                    }
                }
            }
            #endregion
            #region arteriaCuatro
            List<elementoListBox> listaConsolidadaEstacion4 = new List<elementoListBox>();
            foreach (GroupBox groupBox in arteriaCuatro.Children)
            {
                correlativoMaquina = correlativoMaquina + 1;
                string nombreEstacion = groupBox.Header.ToString();
                foreach (var objeto in FindVisualChildren<StackPanel>(groupBox))
                {
                    if (objeto.Name == "stackPanelContenedor")
                    {
                        StackPanel estacion = (StackPanel)objeto;

                        List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                        string operario = "";
                        string ajusteEstacion = "";
                        string categoria = "";
                        string color = "White";
                        double cargaProporcional = 0;
                        string codigoMaquina = "";

                        //determinar el color
                        #region colorEnString
                        if (estacion.Background == Brushes.Pink)
                        {
                            color = "Pink";
                        }
                        else if (estacion.Background == Brushes.LightBlue)
                        {
                            color = "LightBlue";
                        }
                        else if (estacion.Background == Brushes.LightGreen)
                        {
                            color = "LightGreen";
                        }
                        else if (estacion.Background == Brushes.Orange)
                        {
                            color = "Orange";
                        }
                        else if (estacion.Background == Brushes.Yellow)
                        {
                            color = "Yellow";
                        }
                        #endregion

                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in estacion.Children)
                        {
                            //determinar la categoria de maquina
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                categoria = ((TextBlock)elemento).Text;
                            }
                            //determinar el codigo de la maquina
                            if (elemento.GetType() == typeof(StackPanel))
                            {
                                StackPanel contenedorSecundario = (StackPanel)elemento;
                                foreach (object subElemento in contenedorSecundario.Children)
                                {
                                    if (subElemento.GetType() == typeof(TextBox))
                                    {
                                        codigoMaquina = ((TextBox)subElemento).Text;
                                    }
                                }
                            }
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operario = ((Label)elemento).Content.ToString();
                            }
                            //si es textBox se carga ajuste
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text;
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                    cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                                }
                            }
                        }

                        listaConsolidadaEstacion4.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, codigoMaquina = codigoMaquina, nombreOperario = operario, nombreEstacion = nombreEstacion, ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                    }
                }
            }
            listaConsolidadaEstacion4.Reverse();
            foreach (elementoListBox item in listaConsolidadaEstacion4)
            {
                lista.Add(item);
            }
            #endregion
            #region enganche
            correlativoMaquina = correlativoMaquina + 1;
            foreach (var objeto in FindVisualChildren<StackPanel>(enganche))
            {
                if (objeto.Name == "stackPanelContenedor")
                {
                    StackPanel estacion = (StackPanel)objeto;

                    List<operacionesAgregadas> listaOperacionesPorEstacion = new List<operacionesAgregadas>();
                    string operario = "";
                    string ajusteEstacion = "";
                    string categoria = "";
                    string color = "White";
                    double cargaProporcional = 0;

                    //determinar el color
                    #region colorEnString
                    if (estacion.Background == Brushes.Pink)
                    {
                        color = "Pink";
                    }
                    else if (estacion.Background == Brushes.LightBlue)
                    {
                        color = "LightBlue";
                    }
                    else if (estacion.Background == Brushes.LightGreen)
                    {
                        color = "LightGreen";
                    }
                    else if (estacion.Background == Brushes.Orange)
                    {
                        color = "Orange";
                    }
                    else if (estacion.Background == Brushes.Yellow)
                    {
                        color = "Yellow";
                    }
                    #endregion

                    //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                    foreach (object elemento in estacion.Children)
                    {
                        //determinar el codigo de maquina
                        if (elemento.GetType() == typeof(TextBlock))
                        {
                            categoria = ((TextBlock)elemento).Text;
                        }
                        //si es label se carga operario
                        if (elemento.GetType() == typeof(Label))
                        {
                            operario = ((Label)elemento).Content.ToString();
                        }
                        //si es textBox se carga ajuste
                        if (elemento.GetType() == typeof(TextBox))
                        {
                            ajusteEstacion = ((TextBox)elemento).Text;
                        }
                        //Si el objeto es el listBox se analizan los valores de las operaciones en el
                        if (elemento.GetType() == typeof(ListBox))
                        {
                            ListBox listaDeOperaciones = ((ListBox)elemento);

                            foreach (elementoListBox item in listaDeOperaciones.Items)
                            {
                                //Cada elemento del listBox se agrega en la lista declarada al inicio
                                listaOperacionesPorEstacion.Add(new operacionesAgregadas { nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, correlativoOperacion = item.correlativoOperacion, ajusteMaquina = item.ajusteMaquina, samOperacion = item.samOperacion });
                                cargaProporcional = cargaProporcional + (Math.Round(piezasPorHora / (60 / item.samOperacion), 2)) * item.asignadoOperacion;
                            }
                        }
                    }

                    lista.Add(new elementoListBox { correlativoMaquina = correlativoMaquina, nombreOperario = operario, nombreEstacion = "Enganche", ajusteMaquina = ajusteEstacion, asignadoOperario = cargaProporcional, categoriaMaquina = categoria, colorAjuste = color, operacionesAgregadas = listaOperacionesPorEstacion });
                }
            }
            #endregion
            return lista;
        }

        private void version__TextChanged(object sender, TextChangedEventArgs e)
        {
            LabelVersionGuardar.Content = version_.Text;
        }
    }
}


