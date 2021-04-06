using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace Production_control_1._0
{
    public partial class estadoPlantaProduccion : Page
    {
        #region clases_especiales()
        public class item_solicitud
        {
            public int id_solicitud { get; set; }
            public string modulo { get; set; }
            public string maquina { get; set; }
            public float operario { get; set; }
            public string problema_reportado { get; set; }
            public string hora_reportada { get; set; }
            public string hora_apertura { get; set; }
            public string hora_cierre { get; set; }
            public string corresponde { get; set; }
            public int prioridad { get; set; }
        }

        public class item_actualizacion
        {
            public int id { get; set; }
        }

        public string CS = "Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"];
        #endregion

        #region datos_iniciales()
        public estadoPlantaProduccion()
        {
            InitializeComponent();
            //se revisa cual es la distribucion de los modulos
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select id, modulo from orden_modulos";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                foreach(object objeto in areaDeTrabajo.Children)
                {
                    if (objeto.GetType() == typeof(Label))
                    {
                        if (((Label)objeto).Name == "l"+ dr["id"].ToString())
                        {
                            ((Label)objeto).Content = dr["modulo"].ToString();
                        }
                    }
                }
            };
            dr.Close();
            cn.Close();
            datos_llamados();
            WatchTable();
        }
        #endregion

        #region actualizacion_planta()

        public void WatchTable()
        {
            try
            {
                var connectionString = CS;
                var tableName = "actualizacion";
                var tableDependency = new SqlTableDependency<item_actualizacion>(connectionString, tableName);
                tableDependency.OnChanged += OnNotificationReceived;
                tableDependency.Start();
            }
            catch
            {

            }
        }

        private void OnNotificationReceived(object sender, RecordChangedEventArgs<item_actualizacion> e)
        {
            this.Dispatcher.Invoke(() =>
            {
                datos_llamados();
            });
        }

        private void datos_llamados()
        {
            #region limpiar_datos_Actuales
            orden_prioridad.Items.Clear();
            foreach(object objeto in areaDeTrabajo.Children)
            {
                if (objeto.GetType() == typeof(ListBox))
                {
                    ((ListBox)objeto).Items.Clear();
                }
            }
            #endregion

            //se crea una lista donde se insertan los problemas actuales del piso

            List<item_solicitud> pendientes = new List<item_solicitud>();
            pendientes.Clear();

            #region conexion a base
            //se llaman los datos de solicitudes pendientes y abiertas
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select id_solicitud, modulo, maquina, operario, hora_reportada, hora_apertura, hora_cierre, problema_reportado, corresponde from solicitudes where hora_apertura is null or hora_cierre is null";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.CommandTimeout = 60;
            SqlDataReader dr = cm.ExecuteReader();

            #endregion
            //se crea una lista con todos los problemas actuales del piso para luego ser clasificados
            while (dr.Read())
            {
                pendientes.Add(new item_solicitud { id_solicitud = Convert.ToInt32(dr["id_solicitud"]), modulo = dr["modulo"].ToString(), maquina = Convert.ToString(dr["maquina"] is DBNull ? 0 : dr["maquina"]), operario = (float)Convert.ToDouble(dr["operario"] is DBNull ? 0 : dr["operario"]), hora_reportada = Convert.ToString(dr["hora_reportada"] is DBNull ? 0 : dr["hora_reportada"]), hora_apertura = Convert.ToString(dr["hora_apertura"] is DBNull ? 0 : dr["hora_apertura"]), hora_cierre = Convert.ToString(dr["hora_cierre"] is DBNull ? 0 : dr["hora_cierre"]), problema_reportado = Convert.ToString(dr["problema_reportado"] is DBNull ? 0 : dr["problema_reportado"]), corresponde = dr["corresponde"].ToString() });
            };
            dr.Close();
            cn.Close();

            //se hace una variable de entero para contar las prioridades
            int conteo_prioridad = 0;
            // 
            foreach (item_solicitud item in pendientes)
            {
                #region agregar_problemas_a_modulos
                #region orde_atencion
                //agregar items para orden de prioridades
                if (item.corresponde == "MANTENIMIENTO" & item.hora_apertura == "0")
                {
                    conteo_prioridad = conteo_prioridad + 1;
                    orden_prioridad.Items.Add(new item_solicitud { prioridad = conteo_prioridad, modulo = item.modulo, maquina = item.maquina, problema_reportado = item.problema_reportado });
                };
                #endregion
                #region maquina_1
                if (item.modulo == l1.Content.ToString())
                {
                    modulo_1.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_2
                if (item.modulo == l2.Content.ToString())
                {
                    modulo_2.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_3
                if (item.modulo == l3.Content.ToString())
                {
                    modulo_3.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_4
                if (item.modulo == l4.Content.ToString())
                {
                    modulo_4.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_5
                if (item.modulo == l5.Content.ToString())
                {
                    modulo_5.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_6
                if (item.modulo == l6.Content.ToString())
                {
                    modulo_6.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_7
                if (item.modulo == l7.Content.ToString())
                {
                    modulo_7.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_8
                if (item.modulo == l8.Content.ToString())
                {
                    modulo_8.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_9
                if (item.modulo == l9.Content.ToString())
                {
                    modulo_9.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_10
                if (item.modulo == l10.Content.ToString())
                {
                    modulo_10.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_11
                if (item.modulo == l11.Content.ToString())
                {
                    modulo_11.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_12
                if (item.modulo == l12.Content.ToString())
                {
                    modulo_12.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_13
                if (item.modulo == l13.Content.ToString())
                {
                    modulo_13.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_14
                if (item.modulo == l14.Content.ToString())
                {
                    modulo_14.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_15
                if (item.modulo == l15.Content.ToString())
                {
                    modulo_15.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_16
                if (item.modulo == l16.Content.ToString())
                {
                    modulo_16.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_17
                if (item.modulo == l17.Content.ToString())
                {
                    modulo_17.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_18
                if (item.modulo == l18.Content.ToString())
                {
                    modulo_18.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_19
                if (item.modulo == l19.Content.ToString())
                {
                    modulo_19.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_20
                if (item.modulo == l20.Content.ToString())
                {
                    modulo_20.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_21
                if (item.modulo == l21.Content.ToString())
                {
                    modulo_21.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_22
                if (item.modulo == l22.Content.ToString())
                {
                    modulo_22.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_23
                if (item.modulo == l23.Content.ToString())
                {
                    modulo_23.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_24
                if (item.modulo == l24.Content.ToString())
                {
                    modulo_24.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_25
                if (item.modulo == l25.Content.ToString())
                {
                    modulo_25.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_26
                if (item.modulo == l26.Content.ToString())
                {
                    modulo_26.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_27
                if (item.modulo == l27.Content.ToString())
                {
                    modulo_27.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_28
                if (item.modulo == l28.Content.ToString())
                {
                    modulo_28.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_29
                if (item.modulo == l29.Content.ToString())
                {
                    modulo_29.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_30
                if (item.modulo == l30.Content.ToString())
                {
                    modulo_30.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_31
                if (item.modulo == l31.Content.ToString())
                {
                    modulo_31.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_32
                if (item.modulo == l32.Content.ToString())
                {
                    modulo_32.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_33
                if (item.modulo == l33.Content.ToString())
                {
                    modulo_33.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #region maquina_34
                if (item.modulo == l34.Content.ToString())
                {
                    modulo_34.Items.Add(new item_solicitud { id_solicitud = item.id_solicitud, maquina = item.maquina, operario = item.operario, problema_reportado = item.problema_reportado, corresponde = item.corresponde, hora_apertura = item.hora_apertura, hora_reportada = item.hora_reportada, hora_cierre = item.hora_cierre });
                };
                #endregion
                #endregion
            }

            //se cuentan los tipos de reportes de cada modulo
            #region conteo_tipo_de_solicitud
            foreach(object objeto in areaDeTrabajo.Children)
            {
                #region variablesConteo
                int problemasMantenimientoReportados = 0;
                int problemasMantenimientoAbiertos = 0;
                int problemasSmedReportados = 0;
                int problemasSmedAbiertos = 0;
                int cambio = 0;
                #endregion

                if (objeto.GetType() == typeof(ListBox))
                {
                    #region realizarConteo
                    foreach (item_solicitud problema in ((ListBox)objeto).Items)
                    {
                        switch (problema.corresponde)
                        {
                            case "MANTENIMIENTO":
                                {
                                    if (problema.hora_apertura == "0")
                                    {
                                        problemasMantenimientoReportados = problemasMantenimientoReportados + 1;
                                    }
                                    else
                                    {
                                        problemasMantenimientoAbiertos = problemasMantenimientoAbiertos + 1;
                                    }
                                }
                                break;
                            case "SMED":
                                {
                                    if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                                    {
                                        problemasSmedReportados = problemasSmedReportados + 1;
                                    }
                                    if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                                    {
                                        problemasSmedAbiertos = problemasSmedAbiertos + 1;
                                    }
                                    if (problema.problema_reportado == "CAMBIO")
                                    {
                                        cambio = cambio + 1;
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #region color_de_fondo
                    switch (cambio)
                    {
                        case 0:
                            {
                                if (problemasMantenimientoReportados > 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Red;
                                }
                                else if (problemasMantenimientoReportados == 0 & problemasSmedReportados > 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Orange;
                                }
                                else if (problemasMantenimientoAbiertos > 0 || problemasSmedAbiertos > 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Yellow;
                                }
                                else
                                {
                                    ((ListBox)objeto).Background = Brushes.Green;
                                };
                            }
                            break;
                        default:
                            {
                                if (problemasMantenimientoReportados > 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Red;
                                }
                                else if (problemasMantenimientoReportados == 0 & problemasSmedReportados> 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Orange;
                                }
                                else if (problemasMantenimientoAbiertos > 0 || problemasSmedAbiertos > 0)
                                {
                                    ((ListBox)objeto).Background = Brushes.Yellow;
                                }
                                else
                                {
                                    ((ListBox)objeto).Background = Brushes.Blue;
                                };
                            }
                            break;
                    }
                    #endregion
                    #region color_de_borde
                    switch (cambio)
                    {
                        case 0:
                            {
                                if (problemasSmedReportados > 0)
                                {
                                    ((ListBox)objeto).BorderBrush = Brushes.Orange;
                                }
                                else if (problemasSmedReportados == 0 & problemasMantenimientoReportados > 0)
                                {
                                    ((ListBox)objeto).BorderBrush = Brushes.Red;
                                }
                                else if (problemasSmedReportados == 0 & problemasMantenimientoReportados == 0 & (problemasMantenimientoAbiertos > 0 || problemasSmedAbiertos > 0))
                                {
                                    ((ListBox)objeto).BorderBrush = Brushes.Yellow;
                                }
                                else
                                {
                                    ((ListBox)objeto).BorderBrush = Brushes.Green;
                                };
                            }
                            break;
                        default:
                            {
                                ((ListBox)objeto).BorderBrush = Brushes.Blue;
                            }
                            break;
                    }


                    #endregion
                }
            }
            #endregion

            WatchTable();
        }

        #endregion

        #region control_general_del_programa()

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            PagePrincipal PagePrincipal = new PagePrincipal();
            this.NavigationService.Navigate(PagePrincipal);

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

        #endregion

        #region tamanos_de_letra_/_tipo_de_texto

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height*0.8 / tmp.FontFamily.LineSpacing;
        }

        private void letra_modulos_nombres(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
        }

        private void letra_priori_solicitudes (object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }

        private void letra_pop_cerrar (object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }

        private void letra_pequena(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.06 / tmp.FontFamily.LineSpacing;
        }

        private void letra_mediana(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.3 / tmp.FontFamily.LineSpacing;
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion

        #region abrir_pop_up_con_datos_de_modulo
        private void mostrarPopUp(object sender, MouseButtonEventArgs e)
        {
            ListBox modulo = ((ListBox)sender);
            //strings generales para la conexion a la base y la direccion de las imagenes de los botones (se cambian para hacer evidente si estan habilitados o no)
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            Uri iniciar_habilitado = new Uri("/imagenes/iniciar.png", UriKind.RelativeOrAbsolute);
            Uri pausar_habilitado = new Uri("/imagenes/pausa.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_habilitado = new Uri("/imagenes/reanudar.png", UriKind.RelativeOrAbsolute);
            Uri terminar_habilitado = new Uri("/imagenes/terminar.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            Uri pausar_inhabilitado = new Uri("/imagenes/pausa_in.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_inhabilitado = new Uri("/imagenes/reanudar_in.png", UriKind.RelativeOrAbsolute);
            Uri terminar_inhabilitado = new Uri("/imagenes/terminar_in.png", UriKind.RelativeOrAbsolute);

            //si no hay ningun elemento seleccionado no se muestran la ventana emergente
            if (modulo.SelectedIndex > -1)
            {
                //se determina el tamano de la ventana emergente
                #region tamano_de_pop
                datos_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                datos_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                #endregion
                //se abre la ventana emergente
                datos_solicitud.IsOpen = true;
                //se evalua el problema seleccionado
                foreach (item_solicitud item in modulo.SelectedItems)
                {
                    // se agregan el problema, maquina, solicitud y la hora a la que se hizo del problema seleccionado
                    problema.Content = item.problema_reportado.ToString();
                    maquina.Content = item.maquina.ToString();
                    solicitud.Content = item.id_solicitud.ToString();
                    hora_reporte.Content = item.hora_reportada.ToString();

                    //se evalua si ya esta abierto
                    if (item.hora_apertura.ToString() != "0")
                    {
                        //si ya esta abierto se coloca la hora de apertura, por defecto el estado se coloca en abierto y se inabilita el boton de iniciar
                        hora_apertura.Content = item.hora_apertura.ToString();
                        estado.Content = "Abierta";
                        iniciar.IsEnabled = false;
                        pausar.IsEnabled = true;
                        reanudar.IsEnabled = false;
                        terminar.IsEnabled = true;

                        //se cargan las imagenes de acuerdo a si estan o no habilitados
                        img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                        img_pausar.Source = new BitmapImage(pausar_habilitado);
                        img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                        img_terminar.Source = new BitmapImage(terminar_habilitado);

                        //se revisa en la tabla de tiempos por mecanico quien abrio el problema 
                        string sql = "select top 1 mecanico, mecanicos.nombre from tiempos_por_mecanico left join mecanicos on mecanicos.codigo = mecanico where num_solicitud=" + item.id_solicitud + "order by hora desc";
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            mecanico.Content = Convert.ToString(dr["nombre"] is DBNull ? "----" : dr["nombre"]);
                            codigo_mecanico.Content = Convert.ToString(dr["mecanico"] is DBNull ? "----" : dr["mecanico"]);
                        };
                        dr.Close();

                        //se revisa si existen pausas abiertas de la solicitud (eso se ve en la tabla pausas: 1 es inicio de pausa un -1 es un cierre de pausa)
                        string sql2 = "select top 1 tipo from pausas where num_solicitud=" + item.id_solicitud + "order by hora desc";
                        SqlCommand cm2 = new SqlCommand(sql2, cn);
                        SqlDataReader dr2 = cm2.ExecuteReader();
                        while (dr2.Read())
                        {
                            if (dr2["tipo"].ToString() == "1")
                            {
                                //si esta pausado se coloca el estado y la imagen correspondiente a cada boton
                                estado.Content = "Pausado";
                                iniciar.IsEnabled = false;
                                pausar.IsEnabled = false;
                                reanudar.IsEnabled = true;
                                terminar.IsEnabled = false;

                                img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                                img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                                img_reanudar.Source = new BitmapImage(reanudar_habilitado);
                                img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                            }
                            else
                            {
                                //si no esta pausado se coloca el estado y la imagen correspondiente a cada boton
                                estado.Content = "Abierta";
                                iniciar.IsEnabled = false;
                                pausar.IsEnabled = true;
                                reanudar.IsEnabled = false;
                                terminar.IsEnabled = true;

                                img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                                img_pausar.Source = new BitmapImage(pausar_habilitado);
                                img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                                img_terminar.Source = new BitmapImage(terminar_habilitado);
                            }
                        };
                        cn.Close();
                    }
                    else
                    {
                        //si esta sin abrir se coloca el estado y la imagen que corresponde a cada boton
                        hora_apertura.Content = "----";
                        estado.Content = "Sin Abrir";
                        mecanico.Content = "----";
                        iniciar.IsEnabled = true;
                        pausar.IsEnabled = false;
                        reanudar.IsEnabled = false;
                        terminar.IsEnabled = false;

                        img_iniciar.Source = new BitmapImage(iniciar_habilitado);
                        img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                        img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                        img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                    }
                }
            }
        }
        #endregion

        #region botones_pop_uo

        #region botones_pop_principal
        private void iniciar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            abrir_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            abrir_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            abrir_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            abrir_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            codigo_mec.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;
            codigo_mec.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;

            #endregion
            codigo_mec.Text = "";
            id_1.Content = solicitud.Content.ToString();
            abrir_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }

        private void pausar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            pausar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            pausar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            pausar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            pausar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            #endregion
            //se limpian los items de motivos de pausa
            motivo_de_pausa.Items.Clear();
            id_2.Content = solicitud.Content.ToString();
            meca_.Content = codigo_mecanico.Content.ToString();
            //se consulta en la base la lista y se agregan
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select motivo from motivos_de_pausa";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_de_pausa.Items.Add(dr["motivo"].ToString());
            };
            dr.Close();
            cn.Close();

            //se abre el pop_up para reportar motivo de pausa
            pausar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }

        private void reanudar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            reanudar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            reanudar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            reanudar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            reanudar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            codigo_mec_re.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec_re.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec_re.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;
            codigo_mec_re.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;

            #endregion
            codigo_mec_re.Text = "";
            id_3.Content = solicitud.Content.ToString();
            reanudar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }

        private void terminar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            terminar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            terminar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            terminar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            terminar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            #endregion
            //se limpian los items de motivos de pausa
            id_4.Content = solicitud.Content.ToString();
            meca_2.Content = codigo_mecanico.Content.ToString();
            motivo_real.Items.Clear();

            //se consulta en la base la lista y se agregan
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select falla from defectos_totales";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_real.Items.Add(dr["falla"].ToString());
            };
            dr.Close();
            cn.Close();

            //se abre el pop_up para reportar motivo de pausa
            terminar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }

        #endregion

        #region botones_por_pop_up

        private void btn_iniciar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigo_mec.Text))
            {

            }
            else
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "update solicitudes set hora_apertura='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'  where id_solicitud= '" + id_1.Content.ToString() + "'";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_1.Content.ToString() + "', '" + codigo_mec.Text.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                string sql3 = "insert into actualizacion(evento) values(1)";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlCommand cm3 = new SqlCommand(sql3, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cm3.ExecuteNonQuery();
                cn.Close();
                codigo_mec.Text = "";
                abrir_solicitud.IsOpen = false;
            }
        }

        private void motivo_de_pausa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (motivo_de_pausa.SelectedIndex>=0)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausas (num_solicitud, hora, tipo, motivo) values('" + id_2.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1', '" + motivo_de_pausa.SelectedItem.ToString() + "')";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_2.Content.ToString() + "', '" + meca_.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                pausar_solicitud.IsOpen = false;
            }
            else
            {

            }
        }

        private void btn_reanudar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigo_mec_re.Text))
            {

            }
            else
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausas (num_solicitud, hora, tipo) values('" + id_3.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" +id_3.Content.ToString() +"', '" + codigo_mec_re.Text.ToString() + "', '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") +"', '-1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                codigo_mec_re.Text = "";
                reanudar_solicitud.IsOpen = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (motivo_real.SelectedIndex>= 0 & nombre_autoriza.Content.ToString() !="*")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "update solicitudes set hora_cierre='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', problema_real= '" + motivo_real.SelectedItem.ToString() + "', autoriza= '" + codigo_autoriza.Password.ToString() +"' where id_solicitud= '" + id_4.Content.ToString() + "'";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_4.Content.ToString() + "', '" + meca_2.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
                string sql3 = "insert into actualizacion(evento) values(1)";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlCommand cm3 = new SqlCommand(sql3, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cm3.ExecuteNonQuery();
                cn.Close();
                codigo_autoriza.Password = "";
                nombre_autoriza.Content = "*";
                terminar_solicitud.IsOpen = false;
            }
            else
            {

            }
        }

        private void buscar_motivo_real_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se consultan las coincidencias de problemas
            motivo_real.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select falla from defectos_totales where falla like '%" + buscar_motivo_real.Text.ToString()  +  "%'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_real.Items.Add(dr["falla"].ToString());
            };
            dr.Close();
            cn.Close();
        }

        private void codigo_autoriza_PasswordChanged(object sender, RoutedEventArgs e)
        {
            nombre_autoriza.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select nombre from soportes where contrasena= '" + codigo_autoriza.Password.ToString() + "'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nombre_autoriza.Content = dr["nombre"].ToString();
            };
            dr.Close();
            cn.Close();
        }

        #endregion

        #endregion
    }
}
