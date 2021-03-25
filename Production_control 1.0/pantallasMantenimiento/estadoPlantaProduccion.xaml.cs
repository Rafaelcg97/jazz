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
                #region agregar_nombre_de_modulo()
                if (Convert.ToInt32(dr["id"]) == 1)
                {
                    l1.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 2)
                {
                    l2.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 3)
                {
                    l3.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 4)
                {
                    l4.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 5)
                {
                    l5.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 6)
                {
                    l6.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 7)
                {
                    l7.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 8)
                {
                    l8.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 9)
                {
                    l9.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 10)
                {
                    l10.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 11)
                {
                    l11.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 12)
                {
                    l12.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 13)
                {
                    l13.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 14)
                {
                    l14.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 15)
                {
                    l15.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 16)
                {
                    l16.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 17)
                {
                    l17.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 18)
                {
                    l18.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 19)
                {
                    l19.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 20)
                {
                    l20.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 21)
                {
                    l21.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 22)
                {
                    l22.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 23)
                {
                    l23.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 24)
                {
                    l24.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 25)
                {
                    l25.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 26)
                {
                    l26.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 27)
                {
                    l27.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 28)
                {
                    l28.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 29)
                {
                    l29.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 30)
                {
                    l30.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 31)
                {
                    l31.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 32)
                {
                    l32.Content = dr["modulo"].ToString();
                }
                if (Convert.ToInt32(dr["id"]) == 33)
                {
                    l33.Content = dr["modulo"].ToString();
                }
                #endregion

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
            modulo_1.Items.Clear();
            modulo_2.Items.Clear();
            modulo_3.Items.Clear();
            modulo_4.Items.Clear();
            modulo_5.Items.Clear();
            modulo_6.Items.Clear();
            modulo_7.Items.Clear();
            modulo_8.Items.Clear();
            modulo_9.Items.Clear();
            modulo_10.Items.Clear();
            modulo_11.Items.Clear();
            modulo_12.Items.Clear();
            modulo_13.Items.Clear();
            modulo_14.Items.Clear();
            modulo_15.Items.Clear();
            modulo_16.Items.Clear();
            modulo_17.Items.Clear();
            modulo_18.Items.Clear();
            modulo_19.Items.Clear();
            modulo_20.Items.Clear();
            modulo_21.Items.Clear();
            modulo_22.Items.Clear();
            modulo_23.Items.Clear();
            modulo_24.Items.Clear();
            modulo_25.Items.Clear();
            modulo_26.Items.Clear();
            modulo_27.Items.Clear();
            modulo_28.Items.Clear();
            modulo_29.Items.Clear();
            modulo_30.Items.Clear();
            modulo_31.Items.Clear();
            modulo_32.Items.Clear();
            modulo_33.Items.Clear();
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
                #endregion
            }

            //se declaran las variables donde se contara cuantos reportes y de que tipo tiene cada modulo
            #region variables_de_conteo
            int problemas_mantenimiento_reportados_1 = 0;
            int problemas_mantenimiento_abiertos_1 = 0;
            int problemas_smed_reportados_1 = 0;
            int problemas_smed_abiertos_1 = 0;
            int cambio_1 = 0;
            int problemas_mantenimiento_reportados_2 = 0;
            int problemas_mantenimiento_abiertos_2 = 0;
            int problemas_smed_reportados_2 = 0;
            int problemas_smed_abiertos_2 = 0;
            int cambio_2 = 0;
            int problemas_mantenimiento_reportados_3 = 0;
            int problemas_mantenimiento_abiertos_3 = 0;
            int problemas_smed_reportados_3 = 0;
            int problemas_smed_abiertos_3 = 0;
            int cambio_3 = 0;
            int problemas_mantenimiento_reportados_4 = 0;
            int problemas_mantenimiento_abiertos_4 = 0;
            int problemas_smed_reportados_4 = 0;
            int problemas_smed_abiertos_4 = 0;
            int cambio_4 = 0;
            int problemas_mantenimiento_reportados_5 = 0;
            int problemas_mantenimiento_abiertos_5 = 0;
            int problemas_smed_reportados_5 = 0;
            int problemas_smed_abiertos_5 = 0;
            int cambio_5 = 0;
            int problemas_mantenimiento_reportados_6 = 0;
            int problemas_mantenimiento_abiertos_6 = 0;
            int problemas_smed_reportados_6 = 0;
            int problemas_smed_abiertos_6 = 0;
            int cambio_6 = 0;
            int problemas_mantenimiento_reportados_7 = 0;
            int problemas_mantenimiento_abiertos_7 = 0;
            int problemas_smed_reportados_7 = 0;
            int problemas_smed_abiertos_7 = 0;
            int cambio_7 = 0;
            int problemas_mantenimiento_reportados_8 = 0;
            int problemas_mantenimiento_abiertos_8 = 0;
            int problemas_smed_reportados_8 = 0;
            int problemas_smed_abiertos_8 = 0;
            int cambio_8 = 0;
            int problemas_mantenimiento_reportados_9 = 0;
            int problemas_mantenimiento_abiertos_9 = 0;
            int problemas_smed_reportados_9 = 0;
            int problemas_smed_abiertos_9 = 0;
            int cambio_9 = 0;
            int problemas_mantenimiento_reportados_10 = 0;
            int problemas_mantenimiento_abiertos_10 = 0;
            int problemas_smed_reportados_10 = 0;
            int problemas_smed_abiertos_10 = 0;
            int cambio_10 = 0;
            int problemas_mantenimiento_reportados_11 = 0;
            int problemas_mantenimiento_abiertos_11 = 0;
            int problemas_smed_reportados_11 = 0;
            int problemas_smed_abiertos_11 = 0;
            int cambio_11 = 0;
            int problemas_mantenimiento_reportados_12 = 0;
            int problemas_mantenimiento_abiertos_12 = 0;
            int problemas_smed_reportados_12 = 0;
            int problemas_smed_abiertos_12 = 0;
            int cambio_12 = 0;
            int problemas_mantenimiento_reportados_13 = 0;
            int problemas_mantenimiento_abiertos_13 = 0;
            int problemas_smed_reportados_13 = 0;
            int problemas_smed_abiertos_13 = 0;
            int cambio_13 = 0;
            int problemas_mantenimiento_reportados_14 = 0;
            int problemas_mantenimiento_abiertos_14 = 0;
            int problemas_smed_reportados_14 = 0;
            int problemas_smed_abiertos_14 = 0;
            int cambio_14 = 0;
            int problemas_mantenimiento_reportados_15 = 0;
            int problemas_mantenimiento_abiertos_15 = 0;
            int problemas_smed_reportados_15 = 0;
            int problemas_smed_abiertos_15 = 0;
            int cambio_15 = 0;
            int problemas_mantenimiento_reportados_16 = 0;
            int problemas_mantenimiento_abiertos_16 = 0;
            int problemas_smed_reportados_16 = 0;
            int problemas_smed_abiertos_16 = 0;
            int cambio_16 = 0;
            int problemas_mantenimiento_reportados_17 = 0;
            int problemas_mantenimiento_abiertos_17 = 0;
            int problemas_smed_reportados_17 = 0;
            int problemas_smed_abiertos_17 = 0;
            int cambio_17 = 0;
            int problemas_mantenimiento_reportados_18 = 0;
            int problemas_mantenimiento_abiertos_18 = 0;
            int problemas_smed_reportados_18 = 0;
            int problemas_smed_abiertos_18 = 0;
            int cambio_18 = 0;
            int problemas_mantenimiento_reportados_19 = 0;
            int problemas_mantenimiento_abiertos_19 = 0;
            int problemas_smed_reportados_19 = 0;
            int problemas_smed_abiertos_19 = 0;
            int cambio_19 = 0;
            int problemas_mantenimiento_reportados_20 = 0;
            int problemas_mantenimiento_abiertos_20 = 0;
            int problemas_smed_reportados_20 = 0;
            int problemas_smed_abiertos_20 = 0;
            int cambio_20 = 0;
            int problemas_mantenimiento_reportados_21 = 0;
            int problemas_mantenimiento_abiertos_21 = 0;
            int problemas_smed_reportados_21 = 0;
            int problemas_smed_abiertos_21 = 0;
            int cambio_21 = 0;
            int problemas_mantenimiento_reportados_22 = 0;
            int problemas_mantenimiento_abiertos_22 = 0;
            int problemas_smed_reportados_22 = 0;
            int problemas_smed_abiertos_22 = 0;
            int cambio_22 = 0;
            int problemas_mantenimiento_reportados_23 = 0;
            int problemas_mantenimiento_abiertos_23 = 0;
            int problemas_smed_reportados_23 = 0;
            int problemas_smed_abiertos_23 = 0;
            int cambio_23 = 0;
            int problemas_mantenimiento_reportados_24 = 0;
            int problemas_mantenimiento_abiertos_24 = 0;
            int problemas_smed_reportados_24 = 0;
            int problemas_smed_abiertos_24 = 0;
            int cambio_24 = 0;
            int problemas_mantenimiento_reportados_25 = 0;
            int problemas_mantenimiento_abiertos_25 = 0;
            int problemas_smed_reportados_25 = 0;
            int problemas_smed_abiertos_25 = 0;
            int cambio_25 = 0;
            int problemas_mantenimiento_reportados_26 = 0;
            int problemas_mantenimiento_abiertos_26 = 0;
            int problemas_smed_reportados_26 = 0;
            int problemas_smed_abiertos_26 = 0;
            int cambio_26 = 0;
            int problemas_mantenimiento_reportados_27 = 0;
            int problemas_mantenimiento_abiertos_27 = 0;
            int problemas_smed_reportados_27 = 0;
            int problemas_smed_abiertos_27 = 0;
            int cambio_27 = 0;
            int problemas_mantenimiento_reportados_28 = 0;
            int problemas_mantenimiento_abiertos_28 = 0;
            int problemas_smed_reportados_28 = 0;
            int problemas_smed_abiertos_28 = 0;
            int cambio_28 = 0;
            int problemas_mantenimiento_reportados_29 = 0;
            int problemas_mantenimiento_abiertos_29 = 0;
            int problemas_smed_reportados_29 = 0;
            int problemas_smed_abiertos_29 = 0;
            int cambio_29 = 0;
            int problemas_mantenimiento_reportados_30 = 0;
            int problemas_mantenimiento_abiertos_30 = 0;
            int problemas_smed_reportados_30 = 0;
            int problemas_smed_abiertos_30 = 0;
            int cambio_30 = 0;
            int problemas_mantenimiento_reportados_31 = 0;
            int problemas_mantenimiento_abiertos_31 = 0;
            int problemas_smed_reportados_31 = 0;
            int problemas_smed_abiertos_31 = 0;
            int cambio_31 = 0;
            int problemas_mantenimiento_reportados_32 = 0;
            int problemas_mantenimiento_abiertos_32 = 0;
            int problemas_smed_reportados_32 = 0;
            int problemas_smed_abiertos_32 = 0;
            int cambio_32 = 0;
            int problemas_mantenimiento_reportados_33 = 0;
            int problemas_mantenimiento_abiertos_33 = 0;
            int problemas_smed_reportados_33 = 0;
            int problemas_smed_abiertos_33 = 0;
            int cambio_33 = 0;
            #endregion

            //se cuentan los tipos de reportes de cada modulo
            #region conteo_tipo_de_solicitud

            #region maquina_1
            foreach (item_solicitud problema in modulo_1.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_1 = problemas_mantenimiento_reportados_1 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_1 = problemas_mantenimiento_abiertos_1 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_1 = problemas_smed_reportados_1 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_1 = problemas_smed_abiertos_1 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_1 = cambio_1 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_2
            foreach (item_solicitud problema in modulo_2.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_2 = problemas_mantenimiento_reportados_2 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_2 = problemas_mantenimiento_abiertos_2 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_2 = problemas_smed_reportados_2 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_2 = problemas_smed_abiertos_2 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_2 = cambio_2 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_3
            foreach (item_solicitud problema in modulo_3.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_3 = problemas_mantenimiento_reportados_3 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_3 = problemas_mantenimiento_abiertos_3 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_3 = problemas_smed_reportados_3 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_3 = problemas_smed_abiertos_3 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_3 = cambio_3 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_4
            foreach (item_solicitud problema in modulo_4.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_4 = problemas_mantenimiento_reportados_4 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_4 = problemas_mantenimiento_abiertos_4 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_4 = problemas_smed_reportados_4 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_4 = problemas_smed_abiertos_4 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_4 = cambio_4 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_5
            foreach (item_solicitud problema in modulo_5.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_5 = problemas_mantenimiento_reportados_5 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_5 = problemas_mantenimiento_abiertos_5 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_5 = problemas_smed_reportados_5 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_5 = problemas_smed_abiertos_5 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_5 = cambio_5 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_6
            foreach (item_solicitud problema in modulo_6.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_6 = problemas_mantenimiento_reportados_6 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_6 = problemas_mantenimiento_abiertos_6 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_6 = problemas_smed_reportados_6 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_6 = problemas_smed_abiertos_6 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_6 = cambio_6 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_7
            foreach (item_solicitud problema in modulo_7.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_7 = problemas_mantenimiento_reportados_7 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_7 = problemas_mantenimiento_abiertos_7 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_7 = problemas_smed_reportados_7 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_7 = problemas_smed_abiertos_7 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_7 = cambio_7 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_8
            foreach (item_solicitud problema in modulo_8.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_8 = problemas_mantenimiento_reportados_8 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_8 = problemas_mantenimiento_abiertos_8 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_8 = problemas_smed_reportados_8 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_8 = problemas_smed_abiertos_8 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_8 = cambio_8 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_9
            foreach (item_solicitud problema in modulo_9.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_9 = problemas_mantenimiento_reportados_9 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_9 = problemas_mantenimiento_abiertos_9 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_9 = problemas_smed_reportados_9 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_9 = problemas_smed_abiertos_9 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_9 = cambio_9 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_10
            foreach (item_solicitud problema in modulo_10.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_10 = problemas_mantenimiento_reportados_10 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_10 = problemas_mantenimiento_abiertos_10 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_10 = problemas_smed_reportados_10 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_10 = problemas_smed_abiertos_10 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_10 = cambio_10 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_11
            foreach (item_solicitud problema in modulo_11.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_11 = problemas_mantenimiento_reportados_11 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_11 = problemas_mantenimiento_abiertos_11 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_11 = problemas_smed_reportados_11 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_11 = problemas_smed_abiertos_11 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_11 = cambio_11 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_12
            foreach (item_solicitud problema in modulo_12.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_12 = problemas_mantenimiento_reportados_12 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_12 = problemas_mantenimiento_abiertos_12 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_12 = problemas_smed_reportados_12 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_12 = problemas_smed_abiertos_12 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_12 = cambio_12 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_13
            foreach (item_solicitud problema in modulo_13.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_13 = problemas_mantenimiento_reportados_13 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_13 = problemas_mantenimiento_abiertos_13 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_13 = problemas_smed_reportados_13 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_13 = problemas_smed_abiertos_13 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_13 = cambio_13 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_14
            foreach (item_solicitud problema in modulo_14.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_14 = problemas_mantenimiento_reportados_14 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_14 = problemas_mantenimiento_abiertos_14 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_14 = problemas_smed_reportados_14 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_14 = problemas_smed_abiertos_14 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_14 = cambio_14 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_15
            foreach (item_solicitud problema in modulo_15.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_15 = problemas_mantenimiento_reportados_15 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_15 = problemas_mantenimiento_abiertos_15 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_15 = problemas_smed_reportados_15 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_15 = problemas_smed_abiertos_15 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_15 = cambio_15 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_16
            foreach (item_solicitud problema in modulo_16.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_16 = problemas_mantenimiento_reportados_16 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_16 = problemas_mantenimiento_abiertos_16 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_16 = problemas_smed_reportados_16 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_16 = problemas_smed_abiertos_16 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_16 = cambio_16 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_17
            foreach (item_solicitud problema in modulo_17.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_17 = problemas_mantenimiento_reportados_17 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_17 = problemas_mantenimiento_abiertos_17 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_17 = problemas_smed_reportados_17 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_17 = problemas_smed_abiertos_17 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_17 = cambio_17 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_18
            foreach (item_solicitud problema in modulo_18.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_18 = problemas_mantenimiento_reportados_18 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_18 = problemas_mantenimiento_abiertos_18 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_18 = problemas_smed_reportados_18 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_18 = problemas_smed_abiertos_18 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_18 = cambio_18 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_19
            foreach (item_solicitud problema in modulo_19.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_19 = problemas_mantenimiento_reportados_19 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_19 = problemas_mantenimiento_abiertos_19 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_19 = problemas_smed_reportados_19 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_19 = problemas_smed_abiertos_19 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_19 = cambio_19 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_20
            foreach (item_solicitud problema in modulo_20.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_20 = problemas_mantenimiento_reportados_20 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_20 = problemas_mantenimiento_abiertos_20 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_20 = problemas_smed_reportados_20 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_20 = problemas_smed_abiertos_20 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_20 = cambio_20 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_21
            foreach (item_solicitud problema in modulo_21.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_21 = problemas_mantenimiento_reportados_21 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_21 = problemas_mantenimiento_abiertos_21 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_21 = problemas_smed_reportados_21 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_21 = problemas_smed_abiertos_21 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_21 = cambio_21 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_22
            foreach (item_solicitud problema in modulo_22.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_22 = problemas_mantenimiento_reportados_22 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_22 = problemas_mantenimiento_abiertos_22 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_22 = problemas_smed_reportados_22 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_22 = problemas_smed_abiertos_22 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_22 = cambio_22 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_23
            foreach (item_solicitud problema in modulo_23.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_23 = problemas_mantenimiento_reportados_23 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_23 = problemas_mantenimiento_abiertos_23 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_23 = problemas_smed_reportados_23 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_23 = problemas_smed_abiertos_23 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_23 = cambio_23 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_24
            foreach (item_solicitud problema in modulo_24.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_24 = problemas_mantenimiento_reportados_24 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_24 = problemas_mantenimiento_abiertos_24 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_24 = problemas_smed_reportados_24 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_24 = problemas_smed_abiertos_24 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_24 = cambio_24 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_25
            foreach (item_solicitud problema in modulo_25.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_25 = problemas_mantenimiento_reportados_25 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_25 = problemas_mantenimiento_abiertos_25 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_25 = problemas_smed_reportados_25 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_25 = problemas_smed_abiertos_25 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_25 = cambio_25 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_26
            foreach (item_solicitud problema in modulo_26.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_26 = problemas_mantenimiento_reportados_26 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_26 = problemas_mantenimiento_abiertos_26 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_26 = problemas_smed_reportados_26 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_26 = problemas_smed_abiertos_26 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_26 = cambio_26 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_27
            foreach (item_solicitud problema in modulo_27.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_27 = problemas_mantenimiento_reportados_27 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_27 = problemas_mantenimiento_abiertos_27 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_27 = problemas_smed_reportados_27 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_27 = problemas_smed_abiertos_27 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_27 = cambio_27 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_28
            foreach (item_solicitud problema in modulo_28.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_28 = problemas_mantenimiento_reportados_28 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_28 = problemas_mantenimiento_abiertos_28 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_28 = problemas_smed_reportados_28 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_28 = problemas_smed_abiertos_28 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_28 = cambio_28 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_29
            foreach (item_solicitud problema in modulo_29.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_29 = problemas_mantenimiento_reportados_29 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_29 = problemas_mantenimiento_abiertos_29 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_29 = problemas_smed_reportados_29 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_29 = problemas_smed_abiertos_29 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_29 = cambio_29 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_30
            foreach (item_solicitud problema in modulo_30.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_30 = problemas_mantenimiento_reportados_30 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_30 = problemas_mantenimiento_abiertos_30 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_30 = problemas_smed_reportados_30 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_30 = problemas_smed_abiertos_30 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_30 = cambio_30 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_31
            foreach (item_solicitud problema in modulo_31.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_31 = problemas_mantenimiento_reportados_31 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_31 = problemas_mantenimiento_abiertos_31 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_31 = problemas_smed_reportados_31 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_31 = problemas_smed_abiertos_31 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_31 = cambio_31 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_32
            foreach (item_solicitud problema in modulo_32.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_32 = problemas_mantenimiento_reportados_32 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_32 = problemas_mantenimiento_abiertos_32 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_32 = problemas_smed_reportados_32 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_32 = problemas_smed_abiertos_32 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_32 = cambio_32 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region maquina_33
            foreach (item_solicitud problema in modulo_33.Items)
            {
                switch (problema.corresponde)
                {
                    case "MANTENIMIENTO":
                        {
                            if (problema.hora_apertura == "0")
                            {
                                problemas_mantenimiento_reportados_33 = problemas_mantenimiento_reportados_33 + 1;
                            }
                            else
                            {
                                problemas_mantenimiento_abiertos_33 = problemas_mantenimiento_abiertos_33 + 1;
                            }
                        }
                        break;
                    case "SMED":
                        {
                            if (problema.hora_apertura == "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_reportados_33 = problemas_smed_reportados_33 + 1;
                            }
                            if (problema.hora_apertura != "0" & problema.problema_reportado != "CAMBIO")
                            {
                                problemas_smed_abiertos_33 = problemas_smed_abiertos_33 + 1;
                            }
                            if (problema.problema_reportado == "CAMBIO")
                            {
                                cambio_33 = cambio_33 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion

            #endregion

            //por los tipos de solicitudes se da color
            #region color_modulo

            #region maquina_1
            #region color_de_fondo
            switch (cambio_1)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_1 > 0)
                        {
                            modulo_1.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_1 == 0 & problemas_smed_reportados_1 > 0)
                        {
                            modulo_1.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_1 > 0 || problemas_smed_abiertos_1 > 0)
                        {
                            modulo_1.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_1.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_1 > 0)
                        {
                            modulo_1.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_1 == 0 & problemas_smed_reportados_1 > 0)
                        {
                            modulo_1.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_1 > 0 || problemas_smed_abiertos_1 > 0)
                        {
                            modulo_1.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_1.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_1)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_1 > 0)
                        {
                            modulo_1.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_1 == 0 & problemas_mantenimiento_reportados_1 > 0)
                        {
                            modulo_1.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_1 == 0 & problemas_mantenimiento_reportados_1 == 0 & (problemas_mantenimiento_abiertos_1 > 0 || problemas_smed_abiertos_1 > 0))
                        {
                            modulo_1.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_1.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_1.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_2
            #region color_de_fondo
            switch (cambio_2)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_2 > 0)
                        {
                            modulo_2.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_2 == 0 & problemas_smed_reportados_2 > 0)
                        {
                            modulo_2.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_2 > 0 || problemas_smed_abiertos_2 > 0)
                        {
                            modulo_2.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_2.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_2 > 0)
                        {
                            modulo_2.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_2 == 0 & problemas_smed_reportados_2 > 0)
                        {
                            modulo_2.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_2 > 0 || problemas_smed_abiertos_2 > 0)
                        {
                            modulo_2.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_2.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_2)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_2 > 0)
                        {
                            modulo_2.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_2 == 0 & problemas_mantenimiento_reportados_2 > 0)
                        {
                            modulo_2.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_2 == 0 & problemas_mantenimiento_reportados_2 == 0 & (problemas_mantenimiento_abiertos_2 > 0 || problemas_smed_abiertos_2 > 0))
                        {
                            modulo_2.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_2.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_2.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_3
            #region color_de_fondo
            switch (cambio_3)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_3 > 0)
                        {
                            modulo_3.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_3 == 0 & problemas_smed_reportados_3 > 0)
                        {
                            modulo_3.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_3 > 0 || problemas_smed_abiertos_3 > 0)
                        {
                            modulo_3.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_3.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_3 > 0)
                        {
                            modulo_3.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_3 == 0 & problemas_smed_reportados_3 > 0)
                        {
                            modulo_3.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_3 > 0 || problemas_smed_abiertos_3 > 0)
                        {
                            modulo_3.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_3.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_3)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_3 > 0)
                        {
                            modulo_3.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_3 == 0 & problemas_mantenimiento_reportados_3 > 0)
                        {
                            modulo_3.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_3 == 0 & problemas_mantenimiento_reportados_3 == 0 & (problemas_mantenimiento_abiertos_3 > 0 || problemas_smed_abiertos_3 > 0))
                        {
                            modulo_3.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_3.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_3.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_4
            #region color_de_fondo
            switch (cambio_4)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_4 > 0)
                        {
                            modulo_4.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_4 == 0 & problemas_smed_reportados_4 > 0)
                        {
                            modulo_4.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_4 > 0 || problemas_smed_abiertos_4 > 0)
                        {
                            modulo_4.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_4.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_4 > 0)
                        {
                            modulo_4.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_4 == 0 & problemas_smed_reportados_4 > 0)
                        {
                            modulo_4.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_4 > 0 || problemas_smed_abiertos_4 > 0)
                        {
                            modulo_4.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_4.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_4)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_4 > 0)
                        {
                            modulo_4.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_4 == 0 & problemas_mantenimiento_reportados_4 > 0)
                        {
                            modulo_4.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_4 == 0 & problemas_mantenimiento_reportados_4 == 0 & (problemas_mantenimiento_abiertos_4 > 0 || problemas_smed_abiertos_4 > 0))
                        {
                            modulo_4.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_4.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_4.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_5
            #region color_de_fondo
            switch (cambio_5)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_5 > 0)
                        {
                            modulo_5.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_5 == 0 & problemas_smed_reportados_5 > 0)
                        {
                            modulo_5.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_5 > 0 || problemas_smed_abiertos_5 > 0)
                        {
                            modulo_5.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_5.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_5 > 0)
                        {
                            modulo_5.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_5 == 0 & problemas_smed_reportados_5 > 0)
                        {
                            modulo_5.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_5 > 0 || problemas_smed_abiertos_5 > 0)
                        {
                            modulo_5.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_5.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_5)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_5 > 0)
                        {
                            modulo_5.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_5 == 0 & problemas_mantenimiento_reportados_5 > 0)
                        {
                            modulo_5.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_5 == 0 & problemas_mantenimiento_reportados_5 == 0 & (problemas_mantenimiento_abiertos_5 > 0 || problemas_smed_abiertos_5 > 0))
                        {
                            modulo_5.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_5.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_5.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_6
            #region color_de_fondo
            switch (cambio_6)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_6 > 0)
                        {
                            modulo_6.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_6 == 0 & problemas_smed_reportados_6 > 0)
                        {
                            modulo_6.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_6 > 0 || problemas_smed_abiertos_6 > 0)
                        {
                            modulo_6.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_6.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_6 > 0)
                        {
                            modulo_6.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_6 == 0 & problemas_smed_reportados_6 > 0)
                        {
                            modulo_6.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_6 > 0 || problemas_smed_abiertos_6 > 0)
                        {
                            modulo_6.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_6.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_6)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_6 > 0)
                        {
                            modulo_6.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_6 == 0 & problemas_mantenimiento_reportados_6 > 0)
                        {
                            modulo_6.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_6 == 0 & problemas_mantenimiento_reportados_6 == 0 & (problemas_mantenimiento_abiertos_6 > 0 || problemas_smed_abiertos_6 > 0))
                        {
                            modulo_6.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_6.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_6.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_7
            #region color_de_fondo
            switch (cambio_7)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_7 > 0)
                        {
                            modulo_7.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_7 == 0 & problemas_smed_reportados_7 > 0)
                        {
                            modulo_7.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_7 > 0 || problemas_smed_abiertos_7 > 0)
                        {
                            modulo_7.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_7.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_7 > 0)
                        {
                            modulo_7.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_7 == 0 & problemas_smed_reportados_7 > 0)
                        {
                            modulo_7.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_7 > 0 || problemas_smed_abiertos_7 > 0)
                        {
                            modulo_7.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_7.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_7)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_7 > 0)
                        {
                            modulo_7.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_7 == 0 & problemas_mantenimiento_reportados_7 > 0)
                        {
                            modulo_7.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_7 == 0 & problemas_mantenimiento_reportados_7 == 0 & (problemas_mantenimiento_abiertos_7 > 0 || problemas_smed_abiertos_7 > 0))
                        {
                            modulo_7.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_7.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_7.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_8
            #region color_de_fondo
            switch (cambio_8)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_8 > 0)
                        {
                            modulo_8.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_8 == 0 & problemas_smed_reportados_8 > 0)
                        {
                            modulo_8.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_8 > 0 || problemas_smed_abiertos_8 > 0)
                        {
                            modulo_8.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_8.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_8 > 0)
                        {
                            modulo_8.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_8 == 0 & problemas_smed_reportados_8 > 0)
                        {
                            modulo_8.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_8 > 0 || problemas_smed_abiertos_8 > 0)
                        {
                            modulo_8.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_8.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_8)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_8 > 0)
                        {
                            modulo_8.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_8 == 0 & problemas_mantenimiento_reportados_8 > 0)
                        {
                            modulo_8.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_8 == 0 & problemas_mantenimiento_reportados_8 == 0 & (problemas_mantenimiento_abiertos_8 > 0 || problemas_smed_abiertos_8 > 0))
                        {
                            modulo_8.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_8.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_8.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_9
            #region color_de_fondo
            switch (cambio_9)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_9 > 0)
                        {
                            modulo_9.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_9 == 0 & problemas_smed_reportados_9 > 0)
                        {
                            modulo_9.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_9 > 0 || problemas_smed_abiertos_9 > 0)
                        {
                            modulo_9.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_9.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_9 > 0)
                        {
                            modulo_9.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_9 == 0 & problemas_smed_reportados_9 > 0)
                        {
                            modulo_9.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_9 > 0 || problemas_smed_abiertos_9 > 0)
                        {
                            modulo_9.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_9.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_9)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_9 > 0)
                        {
                            modulo_9.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_9 == 0 & problemas_mantenimiento_reportados_9 > 0)
                        {
                            modulo_9.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_9 == 0 & problemas_mantenimiento_reportados_9 == 0 & (problemas_mantenimiento_abiertos_9 > 0 || problemas_smed_abiertos_9 > 0))
                        {
                            modulo_9.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_9.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_9.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_10
            #region color_de_fondo
            switch (cambio_10)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_10 > 0)
                        {
                            modulo_10.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_10 == 0 & problemas_smed_reportados_10 > 0)
                        {
                            modulo_10.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_10 > 0 || problemas_smed_abiertos_10 > 0)
                        {
                            modulo_10.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_10.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_10 > 0)
                        {
                            modulo_10.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_10 == 0 & problemas_smed_reportados_10 > 0)
                        {
                            modulo_10.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_10 > 0 || problemas_smed_abiertos_10 > 0)
                        {
                            modulo_10.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_10.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_10)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_10 > 0)
                        {
                            modulo_10.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_10 == 0 & problemas_mantenimiento_reportados_10 > 0)
                        {
                            modulo_10.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_10 == 0 & problemas_mantenimiento_reportados_10 == 0 & (problemas_mantenimiento_abiertos_10 > 0 || problemas_smed_abiertos_10 > 0))
                        {
                            modulo_10.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_10.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_10.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_11
            #region color_de_fondo
            switch (cambio_11)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_11 > 0)
                        {
                            modulo_11.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_11 == 0 & problemas_smed_reportados_11 > 0)
                        {
                            modulo_11.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_11 > 0 || problemas_smed_abiertos_11 > 0)
                        {
                            modulo_11.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_11.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_11 > 0)
                        {
                            modulo_11.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_11 == 0 & problemas_smed_reportados_11 > 0)
                        {
                            modulo_11.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_11 > 0 || problemas_smed_abiertos_11 > 0)
                        {
                            modulo_11.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_11.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_11)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_11 > 0)
                        {
                            modulo_11.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_11 == 0 & problemas_mantenimiento_reportados_11 > 0)
                        {
                            modulo_11.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_11 == 0 & problemas_mantenimiento_reportados_11 == 0 & (problemas_mantenimiento_abiertos_11 > 0 || problemas_smed_abiertos_11 > 0))
                        {
                            modulo_11.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_11.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_11.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_12
            #region color_de_fondo
            switch (cambio_12)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_12 > 0)
                        {
                            modulo_12.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_12 == 0 & problemas_smed_reportados_12 > 0)
                        {
                            modulo_12.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_12 > 0 || problemas_smed_abiertos_12 > 0)
                        {
                            modulo_12.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_12.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_12 > 0)
                        {
                            modulo_12.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_12 == 0 & problemas_smed_reportados_12 > 0)
                        {
                            modulo_12.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_12 > 0 || problemas_smed_abiertos_12 > 0)
                        {
                            modulo_12.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_12.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_12)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_12 > 0)
                        {
                            modulo_12.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_12 == 0 & problemas_mantenimiento_reportados_12 > 0)
                        {
                            modulo_12.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_12 == 0 & problemas_mantenimiento_reportados_12 == 0 & (problemas_mantenimiento_abiertos_12 > 0 || problemas_smed_abiertos_12 > 0))
                        {
                            modulo_12.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_12.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_12.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_13
            #region color_de_fondo
            switch (cambio_13)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_13 > 0)
                        {
                            modulo_13.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_13 == 0 & problemas_smed_reportados_13 > 0)
                        {
                            modulo_13.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_13 > 0 || problemas_smed_abiertos_13 > 0)
                        {
                            modulo_13.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_13.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_13 > 0)
                        {
                            modulo_13.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_13 == 0 & problemas_smed_reportados_13 > 0)
                        {
                            modulo_13.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_13 > 0 || problemas_smed_abiertos_13 > 0)
                        {
                            modulo_13.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_13.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_13)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_13 > 0)
                        {
                            modulo_13.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_13 == 0 & problemas_mantenimiento_reportados_13 > 0)
                        {
                            modulo_13.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_13 == 0 & problemas_mantenimiento_reportados_13 == 0 & (problemas_mantenimiento_abiertos_13 > 0 || problemas_smed_abiertos_13 > 0))
                        {
                            modulo_13.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_13.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_13.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_14
            #region color_de_fondo
            switch (cambio_14)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_14 > 0)
                        {
                            modulo_14.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_14 == 0 & problemas_smed_reportados_14 > 0)
                        {
                            modulo_14.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_14 > 0 || problemas_smed_abiertos_14 > 0)
                        {
                            modulo_14.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_14.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_14 > 0)
                        {
                            modulo_14.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_14 == 0 & problemas_smed_reportados_14 > 0)
                        {
                            modulo_14.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_14 > 0 || problemas_smed_abiertos_14 > 0)
                        {
                            modulo_14.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_14.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_14)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_14 > 0)
                        {
                            modulo_14.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_14 == 0 & problemas_mantenimiento_reportados_14 > 0)
                        {
                            modulo_14.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_14 == 0 & problemas_mantenimiento_reportados_14 == 0 & (problemas_mantenimiento_abiertos_14 > 0 || problemas_smed_abiertos_14 > 0))
                        {
                            modulo_14.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_14.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_14.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_15
            #region color_de_fondo
            switch (cambio_15)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_15 > 0)
                        {
                            modulo_15.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_15 == 0 & problemas_smed_reportados_15 > 0)
                        {
                            modulo_15.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_15 > 0 || problemas_smed_abiertos_15 > 0)
                        {
                            modulo_15.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_15.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_15 > 0)
                        {
                            modulo_15.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_15 == 0 & problemas_smed_reportados_15 > 0)
                        {
                            modulo_15.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_15 > 0 || problemas_smed_abiertos_15 > 0)
                        {
                            modulo_15.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_15.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_15)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_15 > 0)
                        {
                            modulo_15.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_15 == 0 & problemas_mantenimiento_reportados_15 > 0)
                        {
                            modulo_15.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_15 == 0 & problemas_mantenimiento_reportados_15 == 0 & (problemas_mantenimiento_abiertos_15 > 0 || problemas_smed_abiertos_15 > 0))
                        {
                            modulo_15.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_15.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_15.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_16
            #region color_de_fondo
            switch (cambio_16)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_16 > 0)
                        {
                            modulo_16.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_16 == 0 & problemas_smed_reportados_16 > 0)
                        {
                            modulo_16.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_16 > 0 || problemas_smed_abiertos_16 > 0)
                        {
                            modulo_16.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_16.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_16 > 0)
                        {
                            modulo_16.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_16 == 0 & problemas_smed_reportados_16 > 0)
                        {
                            modulo_16.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_16 > 0 || problemas_smed_abiertos_16 > 0)
                        {
                            modulo_16.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_16.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_16)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_16 > 0)
                        {
                            modulo_16.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_16 == 0 & problemas_mantenimiento_reportados_16 > 0)
                        {
                            modulo_16.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_16 == 0 & problemas_mantenimiento_reportados_16 == 0 & (problemas_mantenimiento_abiertos_16 > 0 || problemas_smed_abiertos_16 > 0))
                        {
                            modulo_16.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_16.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_16.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_17
            #region color_de_fondo
            switch (cambio_17)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_17 > 0)
                        {
                            modulo_17.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_17 == 0 & problemas_smed_reportados_17 > 0)
                        {
                            modulo_17.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_17 > 0 || problemas_smed_abiertos_17 > 0)
                        {
                            modulo_17.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_17.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_17 > 0)
                        {
                            modulo_17.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_17 == 0 & problemas_smed_reportados_17 > 0)
                        {
                            modulo_17.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_17 > 0 || problemas_smed_abiertos_17 > 0)
                        {
                            modulo_17.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_17.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_17)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_17 > 0)
                        {
                            modulo_17.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_17 == 0 & problemas_mantenimiento_reportados_17 > 0)
                        {
                            modulo_17.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_17 == 0 & problemas_mantenimiento_reportados_17 == 0 & (problemas_mantenimiento_abiertos_17 > 0 || problemas_smed_abiertos_17 > 0))
                        {
                            modulo_17.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_17.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_17.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_18
            #region color_de_fondo
            switch (cambio_18)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_18 > 0)
                        {
                            modulo_18.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_18 == 0 & problemas_smed_reportados_18 > 0)
                        {
                            modulo_18.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_18 > 0 || problemas_smed_abiertos_18 > 0)
                        {
                            modulo_18.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_18.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_18 > 0)
                        {
                            modulo_18.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_18 == 0 & problemas_smed_reportados_18 > 0)
                        {
                            modulo_18.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_18 > 0 || problemas_smed_abiertos_18 > 0)
                        {
                            modulo_18.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_18.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_18)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_18 > 0)
                        {
                            modulo_18.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_18 == 0 & problemas_mantenimiento_reportados_18 > 0)
                        {
                            modulo_18.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_18 == 0 & problemas_mantenimiento_reportados_18 == 0 & (problemas_mantenimiento_abiertos_18 > 0 || problemas_smed_abiertos_18 > 0))
                        {
                            modulo_18.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_18.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_18.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_19
            #region color_de_fondo
            switch (cambio_19)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_19 > 0)
                        {
                            modulo_19.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_19 == 0 & problemas_smed_reportados_19 > 0)
                        {
                            modulo_19.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_19 > 0 || problemas_smed_abiertos_19 > 0)
                        {
                            modulo_19.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_19.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_19 > 0)
                        {
                            modulo_19.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_19 == 0 & problemas_smed_reportados_19 > 0)
                        {
                            modulo_19.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_19 > 0 || problemas_smed_abiertos_19 > 0)
                        {
                            modulo_19.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_19.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_19)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_19 > 0)
                        {
                            modulo_19.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_19 == 0 & problemas_mantenimiento_reportados_19 > 0)
                        {
                            modulo_19.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_19 == 0 & problemas_mantenimiento_reportados_19 == 0 & (problemas_mantenimiento_abiertos_19 > 0 || problemas_smed_abiertos_19 > 0))
                        {
                            modulo_19.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_19.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_19.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_20
            #region color_de_fondo
            switch (cambio_20)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_20 > 0)
                        {
                            modulo_20.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_20 == 0 & problemas_smed_reportados_20 > 0)
                        {
                            modulo_20.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_20 > 0 || problemas_smed_abiertos_20 > 0)
                        {
                            modulo_20.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_20.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_20 > 0)
                        {
                            modulo_20.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_20 == 0 & problemas_smed_reportados_20 > 0)
                        {
                            modulo_20.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_20 > 0 || problemas_smed_abiertos_20 > 0)
                        {
                            modulo_20.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_20.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_20)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_20 > 0)
                        {
                            modulo_20.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_20 == 0 & problemas_mantenimiento_reportados_20 > 0)
                        {
                            modulo_20.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_20 == 0 & problemas_mantenimiento_reportados_20 == 0 & (problemas_mantenimiento_abiertos_20 > 0 || problemas_smed_abiertos_20 > 0))
                        {
                            modulo_20.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_20.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_20.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_21
            #region color_de_fondo
            switch (cambio_21)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_21 > 0)
                        {
                            modulo_21.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_21 == 0 & problemas_smed_reportados_21 > 0)
                        {
                            modulo_21.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_21 > 0 || problemas_smed_abiertos_21 > 0)
                        {
                            modulo_21.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_21.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_21 > 0)
                        {
                            modulo_21.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_21 == 0 & problemas_smed_reportados_21 > 0)
                        {
                            modulo_21.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_21 > 0 || problemas_smed_abiertos_21 > 0)
                        {
                            modulo_21.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_21.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_21)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_21 > 0)
                        {
                            modulo_21.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_21 == 0 & problemas_mantenimiento_reportados_21 > 0)
                        {
                            modulo_21.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_21 == 0 & problemas_mantenimiento_reportados_21 == 0 & (problemas_mantenimiento_abiertos_21 > 0 || problemas_smed_abiertos_21 > 0))
                        {
                            modulo_21.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_21.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_21.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_22
            #region color_de_fondo
            switch (cambio_22)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_22 > 0)
                        {
                            modulo_22.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_22 == 0 & problemas_smed_reportados_22 > 0)
                        {
                            modulo_22.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_22 > 0 || problemas_smed_abiertos_22 > 0)
                        {
                            modulo_22.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_22.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_22 > 0)
                        {
                            modulo_22.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_22 == 0 & problemas_smed_reportados_22 > 0)
                        {
                            modulo_22.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_22 > 0 || problemas_smed_abiertos_22 > 0)
                        {
                            modulo_22.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_22.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_22)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_22 > 0)
                        {
                            modulo_22.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_22 == 0 & problemas_mantenimiento_reportados_22 > 0)
                        {
                            modulo_22.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_22 == 0 & problemas_mantenimiento_reportados_22 == 0 & (problemas_mantenimiento_abiertos_22 > 0 || problemas_smed_abiertos_22 > 0))
                        {
                            modulo_22.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_22.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_22.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_23
            #region color_de_fondo
            switch (cambio_23)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_23 > 0)
                        {
                            modulo_23.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_23 == 0 & problemas_smed_reportados_23 > 0)
                        {
                            modulo_23.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_23 > 0 || problemas_smed_abiertos_23 > 0)
                        {
                            modulo_23.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_23.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_23 > 0)
                        {
                            modulo_23.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_23 == 0 & problemas_smed_reportados_23 > 0)
                        {
                            modulo_23.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_23 > 0 || problemas_smed_abiertos_23 > 0)
                        {
                            modulo_23.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_23.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_23)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_23 > 0)
                        {
                            modulo_23.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_23 == 0 & problemas_mantenimiento_reportados_23 > 0)
                        {
                            modulo_23.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_23 == 0 & problemas_mantenimiento_reportados_23 == 0 & (problemas_mantenimiento_abiertos_23 > 0 || problemas_smed_abiertos_23 > 0))
                        {
                            modulo_23.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_23.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_23.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_24
            #region color_de_fondo
            switch (cambio_24)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_24 > 0)
                        {
                            modulo_24.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_24 == 0 & problemas_smed_reportados_24 > 0)
                        {
                            modulo_24.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_24 > 0 || problemas_smed_abiertos_24 > 0)
                        {
                            modulo_24.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_24.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_24 > 0)
                        {
                            modulo_24.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_24 == 0 & problemas_smed_reportados_24 > 0)
                        {
                            modulo_24.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_24 > 0 || problemas_smed_abiertos_24 > 0)
                        {
                            modulo_24.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_24.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_24)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_24 > 0)
                        {
                            modulo_24.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_24 == 0 & problemas_mantenimiento_reportados_24 > 0)
                        {
                            modulo_24.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_24 == 0 & problemas_mantenimiento_reportados_24 == 0 & (problemas_mantenimiento_abiertos_24 > 0 || problemas_smed_abiertos_24 > 0))
                        {
                            modulo_24.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_24.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_24.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_25
            #region color_de_fondo
            switch (cambio_25)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_25 > 0)
                        {
                            modulo_25.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_25 == 0 & problemas_smed_reportados_25 > 0)
                        {
                            modulo_25.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_25 > 0 || problemas_smed_abiertos_25 > 0)
                        {
                            modulo_25.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_25.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_25 > 0)
                        {
                            modulo_25.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_25 == 0 & problemas_smed_reportados_25 > 0)
                        {
                            modulo_25.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_25 > 0 || problemas_smed_abiertos_25 > 0)
                        {
                            modulo_25.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_25.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_25)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_25 > 0)
                        {
                            modulo_25.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_25 == 0 & problemas_mantenimiento_reportados_25 > 0)
                        {
                            modulo_25.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_25 == 0 & problemas_mantenimiento_reportados_25 == 0 & (problemas_mantenimiento_abiertos_25 > 0 || problemas_smed_abiertos_25 > 0))
                        {
                            modulo_25.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_25.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_25.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_26
            #region color_de_fondo
            switch (cambio_26)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_26 > 0)
                        {
                            modulo_26.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_26 == 0 & problemas_smed_reportados_26 > 0)
                        {
                            modulo_26.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_26 > 0 || problemas_smed_abiertos_26 > 0)
                        {
                            modulo_26.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_26.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_26 > 0)
                        {
                            modulo_26.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_26 == 0 & problemas_smed_reportados_26 > 0)
                        {
                            modulo_26.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_26 > 0 || problemas_smed_abiertos_26 > 0)
                        {
                            modulo_26.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_26.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_26)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_26 > 0)
                        {
                            modulo_26.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_26 == 0 & problemas_mantenimiento_reportados_26 > 0)
                        {
                            modulo_26.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_26 == 0 & problemas_mantenimiento_reportados_26 == 0 & (problemas_mantenimiento_abiertos_26 > 0 || problemas_smed_abiertos_26 > 0))
                        {
                            modulo_26.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_26.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_26.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_27
            #region color_de_fondo
            switch (cambio_27)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_27 > 0)
                        {
                            modulo_27.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_27 == 0 & problemas_smed_reportados_27 > 0)
                        {
                            modulo_27.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_27 > 0 || problemas_smed_abiertos_27 > 0)
                        {
                            modulo_27.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_27.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_27 > 0)
                        {
                            modulo_27.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_27 == 0 & problemas_smed_reportados_27 > 0)
                        {
                            modulo_27.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_27 > 0 || problemas_smed_abiertos_27 > 0)
                        {
                            modulo_27.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_27.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_27)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_27 > 0)
                        {
                            modulo_27.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_27 == 0 & problemas_mantenimiento_reportados_27 > 0)
                        {
                            modulo_27.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_27 == 0 & problemas_mantenimiento_reportados_27 == 0 & (problemas_mantenimiento_abiertos_27 > 0 || problemas_smed_abiertos_27 > 0))
                        {
                            modulo_27.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_27.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_27.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_28
            #region color_de_fondo
            switch (cambio_28)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_28 > 0)
                        {
                            modulo_28.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_28 == 0 & problemas_smed_reportados_28 > 0)
                        {
                            modulo_28.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_28 > 0 || problemas_smed_abiertos_28 > 0)
                        {
                            modulo_28.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_28.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_28 > 0)
                        {
                            modulo_28.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_28 == 0 & problemas_smed_reportados_28 > 0)
                        {
                            modulo_28.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_28 > 0 || problemas_smed_abiertos_28 > 0)
                        {
                            modulo_28.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_28.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_28)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_28 > 0)
                        {
                            modulo_28.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_28 == 0 & problemas_mantenimiento_reportados_28 > 0)
                        {
                            modulo_28.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_28 == 0 & problemas_mantenimiento_reportados_28 == 0 & (problemas_mantenimiento_abiertos_28 > 0 || problemas_smed_abiertos_28 > 0))
                        {
                            modulo_28.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_28.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_28.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_29
            #region color_de_fondo
            switch (cambio_29)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_29 > 0)
                        {
                            modulo_29.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_29 == 0 & problemas_smed_reportados_29 > 0)
                        {
                            modulo_29.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_29 > 0 || problemas_smed_abiertos_29 > 0)
                        {
                            modulo_29.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_29.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_29 > 0)
                        {
                            modulo_29.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_29 == 0 & problemas_smed_reportados_29 > 0)
                        {
                            modulo_29.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_29 > 0 || problemas_smed_abiertos_29 > 0)
                        {
                            modulo_29.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_29.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_29)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_29 > 0)
                        {
                            modulo_29.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_29 == 0 & problemas_mantenimiento_reportados_29 > 0)
                        {
                            modulo_29.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_29 == 0 & problemas_mantenimiento_reportados_29 == 0 & (problemas_mantenimiento_abiertos_29 > 0 || problemas_smed_abiertos_29 > 0))
                        {
                            modulo_29.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_29.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_29.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_30
            #region color_de_fondo
            switch (cambio_30)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_30 > 0)
                        {
                            modulo_30.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_30 == 0 & problemas_smed_reportados_30 > 0)
                        {
                            modulo_30.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_30 > 0 || problemas_smed_abiertos_30 > 0)
                        {
                            modulo_30.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_30.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_30 > 0)
                        {
                            modulo_30.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_30 == 0 & problemas_smed_reportados_30 > 0)
                        {
                            modulo_30.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_30 > 0 || problemas_smed_abiertos_30 > 0)
                        {
                            modulo_30.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_30.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_30)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_30 > 0)
                        {
                            modulo_30.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_30 == 0 & problemas_mantenimiento_reportados_30 > 0)
                        {
                            modulo_30.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_30 == 0 & problemas_mantenimiento_reportados_30 == 0 & (problemas_mantenimiento_abiertos_30 > 0 || problemas_smed_abiertos_30 > 0))
                        {
                            modulo_30.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_30.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_30.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_31
            #region color_de_fondo
            switch (cambio_31)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_31 > 0)
                        {
                            modulo_31.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_31 == 0 & problemas_smed_reportados_31 > 0)
                        {
                            modulo_31.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_31 > 0 || problemas_smed_abiertos_31 > 0)
                        {
                            modulo_31.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_31.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_31 > 0)
                        {
                            modulo_31.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_31 == 0 & problemas_smed_reportados_31 > 0)
                        {
                            modulo_31.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_31 > 0 || problemas_smed_abiertos_31 > 0)
                        {
                            modulo_31.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_31.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_31)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_31 > 0)
                        {
                            modulo_31.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_31 == 0 & problemas_mantenimiento_reportados_31 > 0)
                        {
                            modulo_31.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_31 == 0 & problemas_mantenimiento_reportados_31 == 0 & (problemas_mantenimiento_abiertos_31 > 0 || problemas_smed_abiertos_31 > 0))
                        {
                            modulo_31.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_31.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_31.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_32
            #region color_de_fondo
            switch (cambio_32)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_32 > 0)
                        {
                            modulo_32.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_32 == 0 & problemas_smed_reportados_32 > 0)
                        {
                            modulo_32.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_32 > 0 || problemas_smed_abiertos_32 > 0)
                        {
                            modulo_32.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_32.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_32 > 0)
                        {
                            modulo_32.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_32 == 0 & problemas_smed_reportados_32 > 0)
                        {
                            modulo_32.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_32 > 0 || problemas_smed_abiertos_32 > 0)
                        {
                            modulo_32.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_32.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_32)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_32 > 0)
                        {
                            modulo_32.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_32 == 0 & problemas_mantenimiento_reportados_32 > 0)
                        {
                            modulo_32.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_32 == 0 & problemas_mantenimiento_reportados_32 == 0 & (problemas_mantenimiento_abiertos_32 > 0 || problemas_smed_abiertos_32 > 0))
                        {
                            modulo_32.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_32.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_32.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion
            #region maquina_33
            #region color_de_fondo
            switch (cambio_33)
            {
                case 0:
                    {
                        if (problemas_mantenimiento_reportados_33 > 0)
                        {
                            modulo_33.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_33 == 0 & problemas_smed_reportados_33 > 0)
                        {
                            modulo_33.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_33 > 0 || problemas_smed_abiertos_33 > 0)
                        {
                            modulo_33.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_33.Background = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        if (problemas_mantenimiento_reportados_33 > 0)
                        {
                            modulo_33.Background = Brushes.Red;
                        }
                        else if (problemas_mantenimiento_reportados_33 == 0 & problemas_smed_reportados_33 > 0)
                        {
                            modulo_33.Background = Brushes.Orange;
                        }
                        else if (problemas_mantenimiento_abiertos_33 > 0 || problemas_smed_abiertos_33 > 0)
                        {
                            modulo_33.Background = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_33.Background = Brushes.Blue;
                        };
                    }
                    break;
            }
            #endregion
            #region color_de_borde
            switch (cambio_33)
            {
                case 0:
                    {
                        if (problemas_smed_reportados_33 > 0)
                        {
                            modulo_33.BorderBrush = Brushes.Orange;
                        }
                        else if (problemas_smed_reportados_33 == 0 & problemas_mantenimiento_reportados_33 > 0)
                        {
                            modulo_33.BorderBrush = Brushes.Red;
                        }
                        else if (problemas_smed_reportados_33 == 0 & problemas_mantenimiento_reportados_33 == 0 & (problemas_mantenimiento_abiertos_33 > 0 || problemas_smed_abiertos_33 > 0))
                        {
                            modulo_33.BorderBrush = Brushes.Yellow;
                        }
                        else
                        {
                            modulo_33.BorderBrush = Brushes.Green;
                        };
                    }
                    break;
                default:
                    {
                        modulo_33.BorderBrush = Brushes.Blue;
                    }
                    break;
            }


            #endregion
            #endregion


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
