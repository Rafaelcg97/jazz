using JazzCCO._0.clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace JazzCCO._0.pantallasKanban
{
    public partial class estadoLotes : Page
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnManto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        List<solicitudKanban> listaMovimientos = new List<solicitudKanban>();
        List<solicitudKanban> listaResumenMovimientos = new List<solicitudKanban>();
        public estadoLotes()
        {
            InitializeComponent();
            lstMovimientos.ItemsSource = consultarLotes("");
            lstResumen.ItemsSource = consultarResumenLotes("");
        }

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void txtBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            ellipseEstado.Fill = Brushes.Red;
        }

        private void btnBuscarLote_Click(object sender, RoutedEventArgs e)
        {
            if (txtBuscarLote.Text.Length > 0)
            {
                lstMovimientos.ItemsSource = consultarLotes(txtBuscarLote.Text, 1);
                lstResumen.ItemsSource = consultarResumenLotes(txtBuscarLote.Text,1);
            }
            else
            {
                lstMovimientos.ItemsSource = consultarLotes("");
                lstResumen.ItemsSource = consultarResumenLotes("");
            }
            lstMovimientos.Items.Refresh();
            lstResumen.Items.Refresh();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {

        }

        private List<solicitudKanban> consultarLotes(string lote, int consulta=0)
        {
            listaMovimientos.Clear();
            string sql = "SELECT TOP 100 tipoMovimiento, lote, material, paquetes, responsable, fechaMovimiento, ubicacion FROM movimientosKanban ORDER BY idMovimiento DESC";
            if (consulta == 1)
            {
                sql = "SELECT tipoMovimiento, lote, material, paquetes, responsable, fechaMovimiento, ubicacion FROM movimientosKanban WHERE lote LIKE'" + lote + "%' ORDER BY lote DESC, material";
            }
            try
            {
                cnKanban.Open();
                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string color_ = "White";
                    if (dr["material"].ToString().ToLower().Contains("hanger"))
                    {
                        color_ = "LightGreen";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("boxe"))
                    {
                        color_ = "LightBlue";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("accesorios"))
                    {
                        color_ = "LightYellow";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("binding"))
                    {
                        color_ = "LightCoral";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("copas"))
                    {
                        color_ = "LightSalmon";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("elastico"))
                    {
                        color_ = "LightGray";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("hilos"))
                    {
                        color_ = "Pink";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("tela"))
                    {
                        color_ = "LightSteelBlue";
                    }


                    string tipo_ = "Entrada";
                    if (Convert.ToInt32(dr["tipoMovimiento"]) == -1)
                    {
                        tipo_ = "Salida";
                    }
                    listaMovimientos.Add(
                        new solicitudKanban
                        {
                            movimiento = tipo_,
                            lote = dr["lote"].ToString(),
                            material = dr["material"].ToString(),
                            cantidad = Convert.ToInt32(dr["paquetes"]),
                            responsable = Convert.ToInt32(dr["responsable"]),
                            fechaSolicitud = Convert.ToDateTime(dr["fechaMovimiento"]).ToString("yyyy-MM-dd hh:mm:ss"),
                            modulo = dr["ubicacion"].ToString(),
                            color=color_
                        });
                }
                dr.Close();
                cnKanban.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            ellipseEstado.Fill = Brushes.Green;
            return listaMovimientos;
        }

        private List<solicitudKanban> consultarResumenLotes(string lote, int consulta = 0)
        {
            listaResumenMovimientos.Clear();
            string sql = "SELECT lote, material, totalMovimientos, balance, ubicacion  FROM resumenMovimientosKanban ORDER BY lote, material";
            if (consulta == 1)
            {
                sql = "SELECT lote, material, totalMovimientos, balance, ubicacion FROM resumenMovimientosKanban WHERE lote LIKE'" + lote + "%' ORDER BY lote, material";
            }
            try
            {
                cnKanban.Open();
                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string color_ = "White";
                    if (dr["material"].ToString().ToLower().Contains("hanger"))
                    {
                        color_ = "LightGreen";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("boxe"))
                    {
                        color_ = "LightBlue";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("accesorios"))
                    {
                        color_ = "LightYellow";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("binding"))
                    {
                        color_ = "LightCoral";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("copas"))
                    {
                        color_ = "LightSalmon";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("elastico"))
                    {
                        color_ = "LightGray";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("hilos"))
                    {
                        color_ = "Pink";
                    }
                    else if (dr["material"].ToString().ToLower().Contains("tela"))
                    {
                        color_ = "LightSteelBlue";
                    }

                    listaResumenMovimientos.Add(
                        new solicitudKanban
                        {
                            movimiento = dr["balance"].ToString(),
                            lote = dr["lote"].ToString(),
                            material = dr["material"].ToString(),
                            cantidad = Convert.ToInt32(dr["totalMovimientos"]),
                            modulo = dr["ubicacion"].ToString(),
                            color=color_
                        });
                }
                dr.Close();
                cnKanban.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            ellipseEstado.Fill = Brushes.Green;
            return listaResumenMovimientos;
        }
    }
}
