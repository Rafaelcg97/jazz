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

namespace Production_control_1._0
{
    public partial class balance : Page
    {
        #region clasesDeGraficas
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> Formatter2 { get; set; }

        public SeriesCollection DatosGraficaRebalance { get; set; }
        public string[] etiquetasRebalance { get; set; }
        public Func<double, string> FormatterRebalance { get; set; }
        public List<string> ListaDeOperarios { get; private set; }

        int codigoUsuario = 0;

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
            sam_.Content = Math.Round(datosBalance.sam,4).ToString();
            sam_2.Content = Math.Round(datosBalance.sam, 4).ToString();
            #endregion
            #region cargarListaDeModulos
            sql = "select Descripción from coordinadores";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulo.Items.Add(dr["Descripción"].ToString());
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
                    Title = "Carga",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.Green,
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
            Formatter = value => value.ToString("N");
            DataContext = this;
            #endregion
            #region datosInicialesDeGraficaDeRebalance

            // se cargan los datos iniciales para la grafica_reb
            DatosGraficaRebalance = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Carga",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.Green,
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
                },
            };
            FormatterRebalance = value => value.ToString("N");
            Formatter2 = value => value.ToString("P");
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
                List<elementoListBox> listaOperaciones = new List<elementoListBox>();
                //agregar operaciones de consulta
                while (dr.Read())
                {
                    listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["nombre"].ToString(), tituloOperacion = dr["titulo"].ToString().Replace("'",""), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = dr["maquina"].ToString(), categoriaMaquina = dr["categoria"].ToString() });
                };
                dr.Close();
                cnIngenieria.Close();
                //agregar operacion de empaque

                listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = 0, nombreOperacion = "empaque", tituloOperacion = datosBalance.nombreEmpaque, samOperacion = datosBalance.samEmpaque, asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = "Mesa de Empaque", categoriaMaquina = "manual" });
                Operaciones.ItemsSource = listaOperaciones;
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
                string llave = datosBalance.modulo + datosBalance.nombre + datosBalance.temporada + datosBalance.version;
                #region llenarEncabezado
                sql = "select fecha_creacion, estilo, temporada, sam, modulo, corrida, horas, operarios, ingeniero, version from lista_balances where identificador='" + llave + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                dr.Read();
               // modulo.SelectedItem = dr["modulo"].ToString();
                fecha_.Content = Convert.ToDateTime(dr["fecha_creacion"]).ToString("yyyy-MM-dd");
                version_.Text = dr["version"].ToString();
                piezas_de_corrida.Text = dr["corrida"].ToString();
                horas_de_corrida.Text = dr["horas"].ToString();
                operarios_.Content = dr["operarios"].ToString();
                operarios_2.Content= dr["operarios"].ToString();
                sam_.Content = dr["sam"].ToString();
                sam_2.Content = dr["sam"].ToString();
                modulo.SelectedItem = dr["modulo"].ToString();
                cnBalances.Close();
                dr.Close();
                #endregion
                #region llenarListaDeMaquinas
                int numeroEstacion = 0;
                #region areaPreparacion
                foreach (Border estacion in areaPreparacion.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                    cnBalances.Open();
                    cm = new SqlCommand(sql, cnBalances);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        string color = dr["color"].ToString();
                        #region colorEnString
                        switch (color)
                        {
                            case "rojo":
                                estacion.Background = Brushes.Red;
                                break;
                            case "azul":
                                estacion.Background = Brushes.Blue;
                                break;
                            case "verde":
                                estacion.Background = Brushes.Green;
                                break;
                            case "amarillo":
                                estacion.Background = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                estacion.Background = Brushes.Orange;
                                break;
                        }
                        #endregion
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)elemento).Text = dr["categoria"].ToString();
                            }
                            if (elemento.GetType() == typeof(Label))
                            {
                                ((Label)elemento).Content = dr["operario"].ToString();
                            }
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ((TextBox)elemento).Text = dr["ajuste"].ToString();
                            }
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                            }
                        }
                    };
                    dr.Close();
                    cnBalances.Close();
                }
                #endregion
                #region arteriaUno
                foreach (Border estacion in arteriaUno.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                    cnBalances.Open();
                    cm = new SqlCommand(sql, cnBalances);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        string color = dr["color"].ToString();
                        #region colorEnString
                        switch (color)
                        {
                            case "rojo":
                                estacion.Background = Brushes.Red;
                                break;
                            case "azul":
                                estacion.Background = Brushes.Blue;
                                break;
                            case "verde":
                                estacion.Background = Brushes.Green;
                                break;
                            case "amarillo":
                                estacion.Background = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                estacion.Background = Brushes.Orange;
                                break;
                        }
                        #endregion
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)elemento).Text = dr["categoria"].ToString();
                            }
                            if (elemento.GetType() == typeof(Label))
                            {
                                ((Label)elemento).Content = dr["operario"].ToString();
                            }
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ((TextBox)elemento).Text = dr["ajuste"].ToString();
                            }
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                            }
                        }
                    };
                    dr.Close();
                    cnBalances.Close();
                }
                #endregion
                #region arteriaDos
                foreach (Border estacion in arteriaDos.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                    cnBalances.Open();
                    cm = new SqlCommand(sql, cnBalances);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        string color = dr["color"].ToString();
                        #region colorEnString
                        switch (color)
                        {
                            case "rojo":
                                estacion.Background = Brushes.Red;
                                break;
                            case "azul":
                                estacion.Background = Brushes.Blue;
                                break;
                            case "verde":
                                estacion.Background = Brushes.Green;
                                break;
                            case "amarillo":
                                estacion.Background = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                estacion.Background = Brushes.Orange;
                                break;
                        }
                        #endregion
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)elemento).Text = dr["categoria"].ToString();
                            }
                            if (elemento.GetType() == typeof(Label))
                            {
                                ((Label)elemento).Content = dr["operario"].ToString();
                            }
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ((TextBox)elemento).Text = dr["ajuste"].ToString();
                            }
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                            }
                        }
                    };
                    dr.Close();
                    cnBalances.Close();
                }
                #endregion
                #region arteriaTres
                foreach (Border estacion in arteriaTres.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                    cnBalances.Open();
                    cm = new SqlCommand(sql, cnBalances);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        string color = dr["color"].ToString();
                        #region colorEnString
                        switch (color)
                        {
                            case "rojo":
                                estacion.Background = Brushes.Red;
                                break;
                            case "azul":
                                estacion.Background = Brushes.Blue;
                                break;
                            case "verde":
                                estacion.Background = Brushes.Green;
                                break;
                            case "amarillo":
                                estacion.Background = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                estacion.Background = Brushes.Orange;
                                break;
                        }
                        #endregion
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)elemento).Text = dr["categoria"].ToString();
                            }
                            if (elemento.GetType() == typeof(Label))
                            {
                                ((Label)elemento).Content = dr["operario"].ToString();
                            }
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ((TextBox)elemento).Text = dr["ajuste"].ToString();
                            }
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                            }
                        }
                    };
                    dr.Close();
                    cnBalances.Close();
                }
                #endregion
                #region arteriaCuatro
                foreach (Border estacion in arteriaCuatro.Children)
                {
                    numeroEstacion = numeroEstacion + 1;
                    sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                    cnBalances.Open();
                    cm = new SqlCommand(sql, cnBalances);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        string color = dr["color"].ToString();
                        #region colorEnString
                        switch (color)
                        {
                            case "rojo":
                                estacion.Background = Brushes.Red;
                                break;
                            case "azul":
                                estacion.Background = Brushes.Blue;
                                break;
                            case "verde":
                                estacion.Background = Brushes.Green;
                                break;
                            case "amarillo":
                                estacion.Background = Brushes.Yellow;
                                break;
                            case "anaranjado":
                                estacion.Background = Brushes.Orange;
                                break;
                        }
                        #endregion
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            if (elemento.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)elemento).Text = dr["categoria"].ToString();
                            }
                            if (elemento.GetType() == typeof(Label))
                            {
                                ((Label)elemento).Content = dr["operario"].ToString();
                            }
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ((TextBox)elemento).Text = dr["ajuste"].ToString();
                            }
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);
                                listaDeOperaciones.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                            }
                        }
                    };
                    dr.Close();
                    cnBalances.Close();
                }
                #endregion
                #region Enganche
                numeroEstacion = numeroEstacion + 1;
                borderEstacionEnganche.Background = Brushes.Blue;
                sql = "select ajuste, categoria, color, correlativo, operacion, codigo, sam, carga, operario from consultaDeMaquinas where identificador='" + llave + "' and maquina='" + numeroEstacion + "'";
                cnBalances.Open();
                cm = new SqlCommand(sql, cnBalances);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    textBlockEngancheCategoria.Text = dr["categoria"].ToString();
                    operarioEnganche.Content = dr["operario"].ToString();
                    ajusteEnganche.Text = dr["ajuste"].ToString();
                    listBoxEnganche.Items.Add(new elementoListBox { correlativoOperacion = Convert.ToInt32(dr["correlativo"]), tituloOperacion = dr["operacion"].ToString(), nombreOperacion = dr["codigo"].ToString(), asignadoOperacion = Convert.ToDouble(dr["carga"]), samOperacion = Convert.ToDouble(dr["sam"]), ajusteMaquina = dr["ajuste"].ToString() });
                }
                dr.Close();
                cnBalances.Close();
                #endregion
                #endregion
                #region llenarListaDeOperaciones
                sql = "select correlativo, codigo, titulo, sam, ajuste, categoria, requerimiento, asignado from operaciones where identificador='"+llave+"'";
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
                Operarios.ItemsSource=listaOperarios;
                #endregion
                #region calculosGenerales
                actualizarGrafica();
                operacionSobrecargadaOperacionSubutilizada();
                CalculoAsignadoPorOperacion();
                #endregion
            }
        }
        #endregion        
        #region zoom()

        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width >= 0) && ZoomViewbox.Height >= 0)
            {
                double alto = System.Windows.SystemParameters.PrimaryScreenHeight;
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
            GridMenu.Margin = new Thickness(0, 0, 0, 0);
            GridBackground.Margin = new Thickness(250, 30, 0, 0);
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
                    this.NavigationService.Navigate(new PagePrincipal());
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
            //eliminar registros anteriores
            cn.Open();
            sql = "delete from operaciones where identificador= '" + llave + "'";
            cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
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
            //eliminar registros anteriores
            cn.Open();
            sql = "delete from operarios where identificador= '" + llave + "'";
            cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
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
            //eliminar registros anteriores
            cn.Open();
            sql = "delete from maquinas where identificador= '" + llave + "'";
            cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
            //agregar nuevos registros
            int Correlativomaquina = 0;
            #region areaPreparacion
            foreach (Border estacion in areaPreparacion.Children)
            {
                Correlativomaquina = Correlativomaquina + 1;

                string categoriaEstacion = "";
                string colorEstacion = "";
                string operarioEstacion = "";
                List<elementoListBox> operacionesEstacion = new List<elementoListBox>();

                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    colorEstacion = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    colorEstacion = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    colorEstacion = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    colorEstacion = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    colorEstacion = "amarillo";
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
                        categoriaEstacion = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operarioEstacion = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            operacionesEstacion.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = item.ajusteMaquina });
                        }
                    }
                }

                foreach (elementoListBox item in operacionesEstacion)
                {
                    cn.Open();
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + Correlativomaquina + "', '" + item.ajusteMaquina + "', '" + categoriaEstacion + "', '" + colorEstacion + "', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEstacion + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion
            #region arteriaUno
            foreach (Border estacion in arteriaUno.Children)
            {
                Correlativomaquina = Correlativomaquina + 1;

                string categoriaEstacion = "";
                string colorEstacion = "";
                string operarioEstacion = "";
                List<elementoListBox> operacionesEstacion = new List<elementoListBox>();

                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    colorEstacion = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    colorEstacion = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    colorEstacion = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    colorEstacion = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    colorEstacion = "amarillo";
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
                        categoriaEstacion = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operarioEstacion = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            operacionesEstacion.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = item.ajusteMaquina });
                        }
                    }
                }

                foreach (elementoListBox item in operacionesEstacion)
                {
                    cn.Open();
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + Correlativomaquina + "', '" + item.ajusteMaquina + "', '" + categoriaEstacion + "', '" + colorEstacion + "', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEstacion + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion
            #region arteriaDos
            foreach (Border estacion in arteriaDos.Children)
            {
                Correlativomaquina = Correlativomaquina + 1;

                string categoriaEstacion = "";
                string colorEstacion = "";
                string operarioEstacion = "";
                List<elementoListBox> operacionesEstacion = new List<elementoListBox>();

                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    colorEstacion = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    colorEstacion = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    colorEstacion = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    colorEstacion = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    colorEstacion = "amarillo";
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
                        categoriaEstacion = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operarioEstacion = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            operacionesEstacion.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = item.ajusteMaquina });
                        }
                    }
                }

                foreach (elementoListBox item in operacionesEstacion)
                {
                    cn.Open();
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + Correlativomaquina + "', '" + item.ajusteMaquina + "', '" + categoriaEstacion + "', '" + colorEstacion + "', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEstacion + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion
            #region arteriaTres
            foreach (Border estacion in arteriaTres.Children)
            {
                Correlativomaquina = Correlativomaquina + 1;

                string categoriaEstacion = "";
                string colorEstacion = "";
                string operarioEstacion = "";
                List<elementoListBox> operacionesEstacion = new List<elementoListBox>();

                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    colorEstacion = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    colorEstacion = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    colorEstacion = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    colorEstacion = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    colorEstacion = "amarillo";
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
                        categoriaEstacion = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operarioEstacion = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            operacionesEstacion.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = item.ajusteMaquina });
                        }
                    }
                }

                foreach (elementoListBox item in operacionesEstacion)
                {
                    cn.Open();
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + Correlativomaquina + "', '" + item.ajusteMaquina + "', '" + categoriaEstacion + "', '" + colorEstacion + "', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEstacion + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion
            #region arteriaCuatro
            foreach (Border estacion in arteriaCuatro.Children)
            {
                Correlativomaquina = Correlativomaquina + 1;

                string categoriaEstacion = "";
                string colorEstacion = "";
                string operarioEstacion = "";
                List<elementoListBox> operacionesEstacion = new List<elementoListBox>();

                #region colorEnString
                if (estacion.Background == Brushes.Red)
                {
                    colorEstacion = "rojo";
                }
                else if (estacion.Background == Brushes.Blue)
                {
                    colorEstacion = "azul";
                }
                else if (estacion.Background == Brushes.Green)
                {
                    colorEstacion = "verde";
                }
                if (estacion.Background == Brushes.Orange)
                {
                    colorEstacion = "anaranjado";
                }
                if (estacion.Background == Brushes.Yellow)
                {
                    colorEstacion = "amarillo";
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
                        categoriaEstacion = ((TextBlock)elemento).Text;
                    }
                    //si es label se carga operario
                    if (elemento.GetType() == typeof(Label))
                    {
                        operarioEstacion = ((Label)elemento).Content.ToString();
                    }
                    //Si el objeto es el listBox se analizan los valores de las operaciones en el
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);

                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            //Cada elemento del listBox se agrega en la lista declarada al inicio
                            operacionesEstacion.Add(new elementoListBox { correlativoOperacion = item.correlativoOperacion, asignadoOperacion = item.asignadoOperacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = item.ajusteMaquina });
                        }
                    }
                }

                foreach (elementoListBox item in operacionesEstacion)
                {
                    cn.Open();
                    sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + Correlativomaquina + "', '" + item.ajusteMaquina + "', '" + categoriaEstacion + "', '" + colorEstacion + "', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEstacion + "')";
                    cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion
            #region enganche
            foreach (elementoListBox item in listBoxEnganche.Items)
                {
                cn.Open();
                sql = "insert into maquinas(identificador, maquina, ajuste, categoria, color, correlativo, operacion, carga, operario) values('" + llave + "', '" + 66 + "', '" + item.ajusteMaquina + "', '" + textBlockEngancheCategoria.Text + "', 'azul', '" + item.correlativoOperacion + "', '" + item.tituloOperacion + "', '" + item.asignadoOperacion + "', '" + operarioEnganche.Content.ToString() + "')";
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
                }
            #endregion
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
                    List<elementoListBox> listaDeMaquinas = new List<elementoListBox>();
                    List<maquina> resumenMaquinas = new List<maquina>();
                    clases.balance resumenGeneral =  new clases.balance();
                    #region agregarMaquinas
                    int Correlativomaquina = 0;
                    #region areaPreparacion
                    foreach (Border estacion in areaPreparacion.Children)
                    {
                        Correlativomaquina = Correlativomaquina + 1;
                        string colorEstacion = "";
                        string operarioEstacion = "";
                        string ajusteEstacion = "";
                        List<elementoListBox> operacionesEstacion = new List<elementoListBox>();
                        #region colorEnString
                        if (estacion.Background == Brushes.Red)
                        {
                            colorEstacion = "rojo";
                        }
                        else if (estacion.Background == Brushes.Blue)
                        {
                            colorEstacion = "azul";
                        }
                        else if (estacion.Background == Brushes.Green)
                        {
                            colorEstacion = "verde";
                        }
                        if (estacion.Background == Brushes.Orange)
                        {
                            colorEstacion = "anaranjado";
                        }
                        if (estacion.Background == Brushes.Yellow)
                        {
                            colorEstacion = "amarillo";
                        }
                        #endregion

                        //Dentro de los bordes hay un StackPanel que tiene todo 
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEstacion = ((Label)elemento).Content.ToString();
                            }
                            //si es textbox se carga operario
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    operacionesEstacion.Add(new elementoListBox { tituloOperacion = item.tituloOperacion});
                                }
                            }
                        }

                        foreach (elementoListBox item in operacionesEstacion)
                        {
                            listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = Correlativomaquina, nombreOperario = operarioEstacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = ajusteEstacion, colorAjuste = colorEstacion });
                        }
                    }
                    #endregion
                    #region arteriaUno
                    foreach (Border estacion in arteriaUno.Children)
                    {
                        Correlativomaquina = Correlativomaquina + 1;
                        string colorEstacion = "";
                        string operarioEstacion = "";
                        string ajusteEstacion = "";
                        List<elementoListBox> operacionesEstacion = new List<elementoListBox>();
                        #region colorEnString
                        if (estacion.Background == Brushes.Red)
                        {
                            colorEstacion = "rojo";
                        }
                        else if (estacion.Background == Brushes.Blue)
                        {
                            colorEstacion = "azul";
                        }
                        else if (estacion.Background == Brushes.Green)
                        {
                            colorEstacion = "verde";
                        }
                        if (estacion.Background == Brushes.Orange)
                        {
                            colorEstacion = "anaranjado";
                        }
                        if (estacion.Background == Brushes.Yellow)
                        {
                            colorEstacion = "amarillo";
                        }
                        #endregion

                        //Dentro de los bordes hay un StackPanel que tiene todo 
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEstacion = ((Label)elemento).Content.ToString();
                            }
                            //si es textbox se carga operario
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    operacionesEstacion.Add(new elementoListBox { tituloOperacion = item.tituloOperacion });
                                }
                            }
                        }

                        foreach (elementoListBox item in operacionesEstacion)
                        {
                            listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = Correlativomaquina, nombreOperario = operarioEstacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = ajusteEstacion, colorAjuste = colorEstacion });
                        }
                    }
                    #endregion
                    #region arteriaDos
                    foreach (Border estacion in arteriaDos.Children)
                    {
                        Correlativomaquina = Correlativomaquina + 1;
                        string colorEstacion = "";
                        string operarioEstacion = "";
                        string ajusteEstacion = "";
                        List<elementoListBox> operacionesEstacion = new List<elementoListBox>();
                        #region colorEnString
                        if (estacion.Background == Brushes.Red)
                        {
                            colorEstacion = "rojo";
                        }
                        else if (estacion.Background == Brushes.Blue)
                        {
                            colorEstacion = "azul";
                        }
                        else if (estacion.Background == Brushes.Green)
                        {
                            colorEstacion = "verde";
                        }
                        if (estacion.Background == Brushes.Orange)
                        {
                            colorEstacion = "anaranjado";
                        }
                        if (estacion.Background == Brushes.Yellow)
                        {
                            colorEstacion = "amarillo";
                        }
                        #endregion

                        //Dentro de los bordes hay un StackPanel que tiene todo 
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEstacion = ((Label)elemento).Content.ToString();
                            }
                            //si es textbox se carga operario
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    operacionesEstacion.Add(new elementoListBox { tituloOperacion = item.tituloOperacion });
                                }
                            }
                        }

                        foreach (elementoListBox item in operacionesEstacion)
                        {
                            listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = Correlativomaquina, nombreOperario = operarioEstacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = ajusteEstacion, colorAjuste = colorEstacion });
                        }
                    }
                    #endregion
                    #region arteriaTres
                    foreach (Border estacion in arteriaTres.Children)
                    {
                        Correlativomaquina = Correlativomaquina + 1;
                        string colorEstacion = "";
                        string operarioEstacion = "";
                        string ajusteEstacion = "";
                        List<elementoListBox> operacionesEstacion = new List<elementoListBox>();
                        #region colorEnString
                        if (estacion.Background == Brushes.Red)
                        {
                            colorEstacion = "rojo";
                        }
                        else if (estacion.Background == Brushes.Blue)
                        {
                            colorEstacion = "azul";
                        }
                        else if (estacion.Background == Brushes.Green)
                        {
                            colorEstacion = "verde";
                        }
                        if (estacion.Background == Brushes.Orange)
                        {
                            colorEstacion = "anaranjado";
                        }
                        if (estacion.Background == Brushes.Yellow)
                        {
                            colorEstacion = "amarillo";
                        }
                        #endregion

                        //Dentro de los bordes hay un StackPanel que tiene todo 
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEstacion = ((Label)elemento).Content.ToString();
                            }
                            //si es textbox se carga operario
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    operacionesEstacion.Add(new elementoListBox { tituloOperacion = item.tituloOperacion });
                                }
                            }
                        }

                        foreach (elementoListBox item in operacionesEstacion)
                        {
                            listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = Correlativomaquina, nombreOperario = operarioEstacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = ajusteEstacion, colorAjuste = colorEstacion });
                        }
                    }
                    #endregion
                    #region arteriaCuatro
                    foreach (Border estacion in arteriaCuatro.Children)
                    {
                        Correlativomaquina = Correlativomaquina + 1;
                        string colorEstacion = "";
                        string operarioEstacion = "";
                        string ajusteEstacion = "";
                        List<elementoListBox> operacionesEstacion = new List<elementoListBox>();
                        #region colorEnString
                        if (estacion.Background == Brushes.Red)
                        {
                            colorEstacion = "rojo";
                        }
                        else if (estacion.Background == Brushes.Blue)
                        {
                            colorEstacion = "azul";
                        }
                        else if (estacion.Background == Brushes.Green)
                        {
                            colorEstacion = "verde";
                        }
                        if (estacion.Background == Brushes.Orange)
                        {
                            colorEstacion = "anaranjado";
                        }
                        if (estacion.Background == Brushes.Yellow)
                        {
                            colorEstacion = "amarillo";
                        }
                        #endregion

                        //Dentro de los bordes hay un StackPanel que tiene todo 
                        StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                        //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                        foreach (object elemento in stackPanelEstacion.Children)
                        {
                            //si es label se carga operario
                            if (elemento.GetType() == typeof(Label))
                            {
                                operarioEstacion = ((Label)elemento).Content.ToString();
                            }
                            //si es textbox se carga operario
                            if (elemento.GetType() == typeof(TextBox))
                            {
                                ajusteEstacion = ((TextBox)elemento).Text.ToString();
                            }
                            //Si el objeto es el listBox se analizan los valores de las operaciones en el
                            if (elemento.GetType() == typeof(ListBox))
                            {
                                ListBox listaDeOperaciones = ((ListBox)elemento);

                                foreach (elementoListBox item in listaDeOperaciones.Items)
                                {
                                    //Cada elemento del listBox se agrega en la lista declarada al inicio
                                    operacionesEstacion.Add(new elementoListBox { tituloOperacion = item.tituloOperacion });
                                }
                            }
                        }

                        foreach (elementoListBox item in operacionesEstacion)
                        {
                            listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = Correlativomaquina, nombreOperario = operarioEstacion, tituloOperacion = item.tituloOperacion, ajusteMaquina = ajusteEstacion, colorAjuste = colorEstacion });
                        }
                    }
                    #endregion
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    foreach (elementoListBox item in listBoxEnganche.Items)
                    {
                        listaDeMaquinas.Add(new elementoListBox { correlativoOperacion = 66, nombreOperario = operarioEnganche.Content.ToString(), tituloOperacion = item.tituloOperacion, ajusteMaquina =ajusteEnganche.Text, colorAjuste = "azul" });
                    }
                    #endregion
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
                    #region imprimirGraficaTeorica

                    List<operario> listaDeOperarios = concatenacionOperariosOperacionA();
                    clases.balance datosBalance= new clases.balance();
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
                    this.NavigationService.Navigate(new imprimir_balance(listaDeOperarios, datosBalance));
                    #endregion
                    break;
                case 2:
                    #region imprimirGraficaRealcal
                    List<ElementoRebalance> listaDeOperariosRebalance = generarListaRebalance();
                    clases.balance datosRebalance = new clases.balance();
                    datosRebalance.sam = Convert.ToDouble(sam_.Content);
                    datosRebalance.nombre = estilo_.Content.ToString();
                    datosRebalance.operarios = Convert.ToInt32(operarios_.Content);
                    datosRebalance.eficiencia = eficiencia_.Content.ToString();
                    datosRebalance.modulo = modulo_2.Content.ToString();
                    datosRebalance.fechaCreacion = Convert.ToDateTime(fecha_.Content.ToString());
                    datosRebalance.corrida = Convert.ToInt32(string.IsNullOrEmpty(piezas_de_corrida.Text) ? "0" : piezas_de_corrida.Text);
                    datosRebalance.horas = Convert.ToInt32(string.IsNullOrEmpty(horas_de_corrida.Text) ? "0" : horas_de_corrida.Text);
                    #region salirPanelLatera
                    GridMenu.Margin = new Thickness(-250, 0, 0, 0);
                    GridBackground.Margin = new Thickness(5, 30, 0, 0);
                    #endregion
                    this.NavigationService.Navigate(new imprimir_rebalance(listaDeOperariosRebalance, datosRebalance));
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
            MessageBoxResult result = MessageBox.Show("Regenerar el balance implica consultar si existe algun cambio en las operaciones \n Aun así debes asegurarte que todo sea congruente con el nuevo SAM \n podria haber quedado asignada en el LayOut alguna operación o un ajuste ya no existente \n ¿Desea Continuar?", "Jazz-CCO", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    #region guardarDatosEmpaque
                    string nombreEmpaque = "";
                    double samEmpaque = 0;
                    foreach (elementoListBox item in Operaciones.Items)
                    {
                        nombreEmpaque = item.tituloOperacion;
                        samEmpaque = item.samOperacion;
                    }
                    #endregion
                    #region agregarOperaciones
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
                    //agregar operaciones de consulta
                    while (dr.Read())
                    {
                        listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = Convert.ToInt32(dr["correlativo"]), nombreOperacion = dr["nombre"].ToString(), tituloOperacion = dr["titulo"].ToString().Replace("'", ""), samOperacion = Convert.ToDouble(dr["sam"]), asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = dr["maquina"].ToString(), categoriaMaquina = dr["categoria"].ToString() });
                    };
                    dr.Close();
                    cnIngenieria.Close();
                    listaOperaciones.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = 0, nombreOperacion = "empaque", tituloOperacion = nombreEmpaque, samOperacion = samEmpaque, asignadoOperacion = 0, requeridoOperacion = 0, ajusteMaquina = "Mesa de Empaque", categoriaMaquina = "manual" });
                    //agregar operacion de empaque
                    #endregion
                    #region calcularAsignaciones
                    // se obtienen las piezas por hora
                    Double piezasRequeridasHora = Math.Round(Convert.ToDouble(piezas_de_corrida.Text) / Convert.ToDouble(horas_de_corrida.Text), 0);
                    piezas_por_hora.Content = piezasRequeridasHora;
                    foreach (elementoListBox item in listaOperaciones)
                    {
                        Double requerido = Math.Round(piezasRequeridasHora / (60 / item.samOperacion), 2);
                        listaOperaciones2.Add(new elementoListBox() { identificador = "operacion", correlativoOperacion = item.correlativoOperacion, nombreOperacion = item.nombreOperacion, tituloOperacion = item.tituloOperacion, samOperacion = item.samOperacion, asignadoOperacion = item.asignadoOperacion, requeridoOperacion = requerido, ajusteMaquina = item.ajusteMaquina, categoriaMaquina = item.categoriaMaquina });
                    }
                    Operaciones.ItemsSource = listaOperaciones2;
                    //recalcular_asignaciones();

                    #endregion
                    CalculoAsignadoPorOperacion();
                    actualizarGrafica();
                    operacionSobrecargadaOperacionSubutilizada();
                    MessageBox.Show("Terminado");
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
            #region Enganche
            foreach (elementoListBox item in listBoxEnganche.Items)
            {
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operarioEnganche.Content.ToString(), tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
            }
            #endregion
            #region areaPreparacion
            foreach (Border estacion in areaPreparacion.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                            listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
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
            foreach (Border estacion in arteriaUno.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
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
            foreach (Border estacion in arteriaDos.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
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
            foreach (Border estacion in arteriaTres.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
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
            foreach (Border estacion in arteriaCuatro.Children)
            {
                List<ElementoRebalance> listaOperariosBorde = new List<ElementoRebalance>();
                string operario = "";
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                foreach (object elemento in stackPanelEstacion.Children)
                {
                    if (elemento.GetType() == typeof(Label))
                    {
                        operario = ((Label)elemento).Content.ToString();
                    }
                    if (elemento.GetType() == typeof(ListBox))
                    {
                        ListBox listaDeOperaciones = ((ListBox)elemento);
                        foreach (elementoListBox item in listaDeOperaciones.Items)
                        {
                            listaOperariosBorde.Add(new ElementoRebalance { nombreOperacion = item.nombreOperacion, nombreOperario = operario, tituloOperacion = item.tituloOperacion, asignadoOperacion = item.asignadoOperacion, samOperacion = item.samOperacion, ajusteMaquina = item.ajusteMaquina, tiempoRebalance = 0, eficienciaRebalance = 0, cargaRebalance = 0 });
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
        }
        private void rebalance_bt_Click(object sender, RoutedEventArgs e)
        {
            generarListaDeOperacionesRebalance();
            eficienciaRebalance();
            actualizarGraficaReal();
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
                cn.Open();
                string sql2 = "insert into toma_de_tiempos (fecha, modulo, estilo, temporada, version, codigo, nombre, titulo, operacion, ajuste, sam, tiempo_objetivo) values('" + System.DateTime.Now.ToString() + "', '" + modulo_va + "', '" + estilo_.Content.ToString() + "', '" + temporada_.Content.ToString() + "', '" + version_.Text.ToString() + "', '" + item.codigoOperario+ "', '" + item.nombreOperario + "', '" + item.tituloOperacion + "', '" + item.nombreOperacion + "', '" + item.ajusteMaquina + "', '" + item.samOperacion + "', '" + item.tiempoRebalance + "')";
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlDataReader dr2 = cm2.ExecuteReader();
                dr2.Close();
                cn.Close();
            }

            MessageBox.Show("La Toma de Tiempos ha sido Cargada");
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
                    ((ListBox)elemento).Items.Add(new elementoListBox() { nombreOperacion = informacionElemento.nombreOperacion, tituloOperacion = informacionElemento.tituloOperacion, asignadoOperacion = informacionElemento.requeridoOperacion, correlativoOperacion = informacionElemento.correlativoOperacion, ajusteMaquina = informacionElemento.ajusteMaquina, samOperacion = informacionElemento.samOperacion });
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
            operacionSobrecargadaOperacionSubutilizada();

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

        private void colorBlanco(object sender, RoutedEventArgs e)
        {
            //se determina el tipo de objeto que realiza la accion (el objeto es el radioButton)
            RadioButton radioButton = (RadioButton)sender;
            //se sacan los objetos dentro del que esta contenido (el radioButton esta dentro de un stackPanel y ese stackPanel dento de otro y ese dentro del borde que debe tener el color
            (((radioButton.Parent as StackPanel).Parent as StackPanel).Parent as Border).Background = Brushes.White;
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
            //Codigo para verificar asignado por operacion se recorrera por cada arteria (para facilitar especificacion de donde estan los listBox que contienen los datos)

            //Recorrer el areadePreparacion (cada arteria es un StackPanel)
            //Dentro de cada arteria hay bordes que contienen todos los elementos de una estacion de trabajo
            #region areaPreparacion
            foreach (Border estacion in areaPreparacion.Children)
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
                consolidadoMaquinas.Add(new maquina { categoriaMaquina = categoria, colorAjuste = color });

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

                if (operacion.requeridoOperacion > 0)
                {
                    relacionAsignacion = asignadoTotal / operacion.requeridoOperacion;
                }
                else
                {
                    relacionAsignacion = 0;
                }


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
            grafico.AxisX.Add(new Axis() { Labels = listaDeOperarios.ToArray(), LabelsRotation = 89, ShowLabels = true, Separator = { Step = 1 }, FontSize=9, Foreground=Brushes.Black });
            //se agregan los valores de las cargas en las columnas
            foreach (operario item in listaOperariosConCarga)
            {
                if (item.asignadoOperario > 0)
                {
                    SeriesCollection[0].Values.Add(item.asignadoOperario);
                    SeriesCollection[1].Values.Add(tkt_);
                    SeriesCollection[2].Values.Add(0.9*tkt_);
                }
            };
        }
        private List<operario> concatenacionOperariosOperacionA()
        {
            List<ElementoRebalance> listaOperariosRebalance = new List<ElementoRebalance>();
            #region Enganche
            string operacionese = "";
            foreach (elementoListBox item in listBoxEnganche.Items)
            {
                operacionese = operacionese + item.tituloOperacion + "\n";
            }
            listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operarioEnganche.Content.ToString(), tituloOperacion = operacionese });
            #endregion
            #region areaPreparacion
            foreach (Border estacion in areaPreparacion.Children)
            {
                string operario = "";
                string operaciones = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                        }
                    }
                }

                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim() });
            }
            #endregion
            #region arteriaUno
            foreach (Border estacion in arteriaUno.Children)
            {
                string operario = "";
                string operaciones = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim() });
            }
            #endregion
            #region arteriaDos
            foreach (Border estacion in arteriaDos.Children)
            {
                string operario = "";
                string operaciones = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim() });
            }
            #endregion
            #region arteriaTres
            foreach (Border estacion in arteriaTres.Children)
            {
                string operario = "";
                string operaciones = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim() });
            }
            #endregion
            #region arteriaCuatro
            foreach (Border estacion in arteriaCuatro.Children)
            {
                string operario = "";
                string operaciones = "";
                //Dentro de los bordes hay un StackPanel que tiene todo 
                StackPanel stackPanelEstacion = (estacion.Child as StackPanel);
                //Se recorre el StackPanel ya que tiene mas de un tipo de objeto
                foreach (object elemento in stackPanelEstacion.Children)
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
                        }
                    }
                }
                listaOperariosRebalance.Add(new ElementoRebalance { nombreOperario = operario, tituloOperacion = operaciones.Trim() });
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
                    foreach (ElementoRebalance subitem in listaOperariosRebalance)
                    {
                        if (item == subitem.nombreOperario)
                        {
                            operaciones = operaciones + subitem.tituloOperacion + "\n";
                        }
                    }

                    operarioOperaciones.Add(new Operacion { nombreOperario = item, tituloOperacion = item + "\n" + operaciones });
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

                listaFinal.Add(new operario { nombreOperario = item.tituloOperacion, asignadoOperario = asignado * takt });
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
            graficoRebalance.AxisX.Add(new Axis() { Labels = nombreOperacion.ToArray(), LabelsRotation = 89, ShowLabels = true, Separator = { Step = 1 }, FontSize=9, Foreground=Brushes.Black });
            //se agregan los valores de las cargas en las columnas
            foreach (ElementoRebalance item in listaConsolidada)
            {
                DatosGraficaRebalance[0].Values.Add(item.cargaRebalance*tkt_);
                DatosGraficaRebalance[1].Values.Add(tkt_);
                DatosGraficaRebalance[2].Values.Add(0.9*tkt_);
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
                double sumaCargas2 = 0;
                double sumaSam = 0;
                double sumaTiempo = 0;
                double eficienciaTotal = 0;
                foreach (ElementoRebalance subitem in rebalance_.Items)
                {
                    if (item.ToString() == subitem.nombreOperario)
                    {
                        sumaCargas = sumaCargas + subitem.cargaRebalance;
                        sumaSam = sumaSam + subitem.samOperacion;
                        sumaTiempo = sumaTiempo + subitem.tiempoRebalance;
                        operaciones = operaciones + subitem.tituloOperacion + "\n";
                    }
                    if (sumaTiempo > 0)
                    {
                        eficienciaTotal = sumaSam / sumaTiempo;
                        sumaCargas2 = sumaCargas / eficienciaTotal;
                    }
                    else
                    {
                        eficienciaTotal = 0;
                        sumaCargas2 = 0;
                    }
                };

                listaConsolidada.Add(new ElementoRebalance { nombreOperario = item + "\n" + operaciones.Trim(), cargaRebalance = sumaCargas2 });
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
    }
}


