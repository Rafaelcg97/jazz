using Production_control_1._0.clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace Production_control_1._0.pantallasBMP
{
    public partial class controlTiemposTrims : Page
    {
        #region listasConexiones
        Queue<tarjetaKanban> listaTandas = new Queue<tarjetaKanban>();
        List<preparacionTanda> listaTandasEnProceso = new List<preparacionTanda>();
        List<tarjetaKanban> listaLotes = new List<tarjetaKanban>();
        SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        public controlTiemposTrims()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTMxMjA3QDMxMzkyZTMzMmUzMFoyY0F2MWI5RE1uVy9UdG5EeFgyTCt5bmV4dXNCNUlGZ3VsTkpISlpGRW89");
            InitializeComponent();
            lstTandas.ItemsSource = consultarTandas();
            lstTandasEnProceso.ItemsSource = consultarTandasEnProceso();
        }
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        #endregion
        #region consultas
        private Queue<tarjetaKanban> consultarTandas()
        {
            listaTandas.Clear();
            string sql = "SELECT a.tanda, SUM(sam) AS sam " +
                "FROM programacionTandasTrims a " +
                "LEFT JOIN ordenTandaTrims b ON a.tanda = b.tanda " +
                "LEFT JOIN preparacionTrims c ON a.tanda = c.tanda " +
                "WHERE c.tanda IS NULL GROUP BY a.tanda " +
                "ORDER BY MAX(b.correlativo) ASC, MAX(b.orden) ASC";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listaTandas.Enqueue(new tarjetaKanban
                    {
                        tanda = dr["tanda"].ToString(),
                        sam = Convert.ToDouble(dr["sam"])
                    });
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return listaTandas;
        }
        private List<preparacionTanda> consultarTandasEnProceso()
        {
            listaTandasEnProceso.Clear();
            string sql = "SELECT idProcesoTanda, tanda, sam, codigoPreparador, nombre, horaInicio, pausa, estatus FROM resumenPreparacionTrims WHERE horaFin IS NULL";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string imagen = "/imagenes/pausa.png";
                    string imagenTerminar = "/imagenes/terminar.png";
                    bool terminar = true;
                    if (dr["estatus"].ToString() == "pausada")
                    {
                        imagen = "/imagenes/iniciar.png";
                        imagenTerminar = "/imagenes/terminar_in.png";
                        terminar = false;
                    }

                    listaTandasEnProceso.Add(new preparacionTanda
                    {
                        idTanda = Convert.ToInt32(dr["idProcesoTanda"]),
                        tanda = dr["tanda"].ToString(),
                        codigoPreparador = Convert.ToInt32(dr["codigoPreparador"] is DBNull ? 0 : dr["codigoPreparador"]),
                        nombrePreparador = dr["nombre"].ToString(),
                        horaInicio = Convert.ToDateTime(dr["horaInicio"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        pausa = Convert.ToDouble(dr["pausa"] is DBNull ? 0 : dr["pausa"]),
                        imagenStatus = imagen,
                        imagenTerminar = imagenTerminar,
                        terminar = terminar,
                        sam = Convert.ToDouble(dr["sam"] is DBNull ? 0 : dr["sam"])
                    });
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return listaTandasEnProceso;
        }
        private List<tarjetaKanban> consultarLotes(string tanda)
        {
            listaLotes.Clear();
            string sql = "SELECT Lote, estilo, cliente, modulo, temporada, color, make, sam FROM programacionTandasTrims WHERE tanda='" + tanda + "'";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listaLotes.Add(new tarjetaKanban
                    {
                        lote = dr["lote"].ToString(),
                        estilo = dr["estilo"].ToString(),
                        temporada = dr["temporada"].ToString(),
                        color = dr["color"].ToString(),
                        make = Convert.ToInt32(dr["make"] is DBNull ? 0 : dr["make"]),
                        sam = Convert.ToDouble(dr["sam"] is DBNull ? 0 : dr["sam"]),
                        cliente=dr["cliente"].ToString(),
                        modulo= dr["modulo"].ToString()
                    });
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return listaLotes;
        }
        #endregion
        private void btnTomarTanda_Click(object sender, RoutedEventArgs e)
        {
            pwbContraseniaApertura.Clear();
            lblTanda.Content = "A-########-#";
            popUpIniciar.IsOpen = true;
            lblNombreAutoriza.Content = "****";
            try
            {
                lblTanda.Content = listaTandas.Peek().tanda;
            }
            catch
            {

            }
        }

        private void btnCerrarPop_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            Border border = (Border)grid.Parent;
            Popup popup = (Popup)border.Parent;
            popup.IsOpen = false;
        }


        private void pwbContraseniaApertura_LostFocus(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT codigo FROM usuarios WHERE bmp=1 AND contrasena='" + pwbContraseniaApertura.Password + "'";
            try
            {
                cnIngenieria.Open();
                SqlCommand cm = new SqlCommand(sql, cnIngenieria);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    lblNombreAutoriza.Content = dr["codigo"].ToString();
                }
                else
                {
                    lblNombreAutoriza.Content = "****";
                }
                dr.Close();
                cnIngenieria.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void btnValidarApertura_Click(object sender, RoutedEventArgs e)
        {
            if (lblNombreAutoriza.Content.ToString() != "****")
            {
                string sql = "INSERT INTO preparacionTrims(tanda, codigoPreparador, horaInicio) VALUES('" + lblTanda.Content + "', '" + lblNombreAutoriza.Content + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    popUpIniciar.IsOpen = false;
                    lstTandas.ItemsSource = consultarTandas();
                    lstTandasEnProceso.ItemsSource = consultarTandasEnProceso();
                    lstTandas.Items.Refresh();
                    lstTandasEnProceso.Items.Refresh();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpPausar.IsOpen = true;
            pwbContraseniaPausa.Clear();
            lblTandaPausar.Content = "A -########-#";
            lblNombreAutorizaPausa.Content = "****";
            lblTanda.Content = "####";
            try
            {
                preparacionTanda itemSeleccionado = (preparacionTanda)lstTandasEnProceso.SelectedItem;
                lblTandaPausar.Content = itemSeleccionado.tanda;
                lblPreparador.Content = itemSeleccionado.codigoPreparador;
                lblIdTanda.Content = itemSeleccionado.idTanda;
                if (itemSeleccionado.imagenStatus == "/imagenes/pausa.png")
                {
                    lblPausar.Content = "Pausar Tanda:";
                }
                else
                {
                    lblPausar.Content = "Reanudar Tanda:";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            lstTandasEnProceso.SelectedItem = button.DataContext;
        }

        private void pwbContraseniaPausa_LostFocus(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT codigo FROM usuarios WHERE bmp=1 AND contrasena='" + pwbContraseniaPausa.Password + "'";
            if(lblPausar.Content.ToString() == "Pausar tanda:")
            {
                sql = "SELECT codigo FROM usuarios WHERE nivel=1 AND bmp=1 AND contrasena='" + pwbContraseniaPausa.Password + "'";
            }
            try
            {
                cnIngenieria.Open();
                SqlCommand cm = new SqlCommand(sql, cnIngenieria);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    lblNombreAutorizaPausa.Content = dr["codigo"].ToString();
                }
                else
                {
                    lblNombreAutorizaPausa.Content = "****";
                }
                dr.Close();
                cnIngenieria.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void btnPausarTanda_Click(object sender, RoutedEventArgs e)
        {
            if (lblNombreAutorizaPausa.Content.ToString() == lblPreparador.Content.ToString())
            {
                //validar que no se duplicara dato de pausa
                int ultimoIngreso = 0;
                string sql = "SELECT TOP 1 tipoAccion FROM pausasPreparacionTrims WHERE idProcesoTanda='" + lblIdTanda.Content.ToString() + "' ORDER BY idPausaTandaTrims DESC";
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.Read())
                    {
                        ultimoIngreso = Convert.ToInt32(dr["tipoAccion"]);
                    }
                    dr.Close();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

                if (lblPausar.Content.ToString() == "Reanudar Tanda:" && ultimoIngreso == -1)
                {
                    sql = "INSERT INTO pausasPreparacionTrims(idProcesoTanda, horaAccion, tipoAccion) VALUES('" + lblIdTanda.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + 1 + "')";
                    try
                    {
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else if (lblPausar.Content.ToString() == "Pausar Tanda:" && (ultimoIngreso == 1 || ultimoIngreso == 0))
                {
                    sql = "INSERT INTO pausasPreparacionTrims(idProcesoTanda, horaAccion, tipoAccion) VALUES('" + lblIdTanda.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + -1 + "')";
                    try
                    {
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else if (lblPausar.Content.ToString() == "Terminar Tanda:")
                {
                    sql = "UPDATE preparacionTrims SET horaFin='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE idProcesoTanda='" + lblIdTanda.Content.ToString() + "'";
                    try
                    {
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                popUpPausar.IsOpen = false;
                lstTandas.ItemsSource = consultarTandas();
                lstTandasEnProceso.ItemsSource = consultarTandasEnProceso();
                lstTandas.Items.Refresh();
                lstTandasEnProceso.Items.Refresh();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popUpPausar.IsOpen = true;
            pwbContraseniaPausa.Clear();
            lblTandaPausar.Content = "A -########-#";
            lblNombreAutorizaPausa.Content = "****";
            lblTanda.Content = "####";
            try
            {
                preparacionTanda itemSeleccionado = (preparacionTanda)lstTandasEnProceso.SelectedItem;
                lblTandaPausar.Content = itemSeleccionado.tanda;
                lblPreparador.Content = itemSeleccionado.codigoPreparador;
                lblIdTanda.Content = itemSeleccionado.idTanda;
                lblPausar.Content = "Terminar Tanda:";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            preparacionTanda itemsSeleccionado = (preparacionTanda)lstTandasEnProceso.SelectedItem;
            popUpLista.IsOpen = true;
            lstbListaLotes.ItemsSource = consultarLotes(itemsSeleccionado.tanda);
            lstbListaLotes.Items.Refresh();
            lblplote.Content = "----";
            lblpestilo.Content = "----";
            lblpcolor.Content = "----";
            lblpmodulo.Content = "----";
            lblppiezas.Content = "----";
            lblpprograma.Content = "----";
            lblppreparador.Content = itemsSeleccionado.nombrePreparador;
            barcode.Text = "LOTE-PRUEBA";
        }


        private void lstbListaLotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstbListaLotes.SelectedIndex > -1)
            {
                tarjetaKanban itemSeleccionado = (tarjetaKanban)lstbListaLotes.SelectedItem;
                lblplote.Content = itemSeleccionado.lote;
                lblpestilo.Content = itemSeleccionado.estilo;
                lblpcolor.Content = itemSeleccionado.color;
                lblpmodulo.Content = itemSeleccionado.modulo;
                lblppiezas.Content = itemSeleccionado.make;
                lblpprograma.Content = itemSeleccionado.cliente;
                barcode.Text = itemSeleccionado.lote;
            }
        }
    }
}