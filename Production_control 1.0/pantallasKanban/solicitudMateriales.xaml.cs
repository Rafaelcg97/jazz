﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SQLConnection;
using ClasesTexops;

namespace JazzCCO._0.pantallasKanban
{
    public partial class solicitudMateriales : UserControl
    {
        #region listasGlobales
        List<Lote> listaCompletaLotes = new List<Lote>();
        List<Partnumber> listaDeMaterialesCompleta = new List<Partnumber>();
        #endregion
        #region datosIniciales
        public solicitudMateriales()
        {
            InitializeComponent();
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region calculosGenerales
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.kanban());
        }
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
        #region botonesDeAgregarMaterialLista
        private void buttonAgregarAccesorios_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //se verifica si aun no ha sido entregado
                if (labelEstadoAccesorios.Content.ToString() == "Pendiente")
                {
                    listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "Accesorios", "Unica", 1));
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarBinding_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //se verifica si aun no ha sido entregado
                if (labelEstadoBinding.Content.ToString() == "Pendiente")
                {
                    listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "Binding", "Unica", 1));
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarHilo_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //se verifica si aun no ha sido entregado
                if (labelEstadoHilos.Content.ToString() == "Pendiente")
                {
                    listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "Hilos", "Unica", 1));
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarHiloSmed_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //se verifica si aun no ha sido entregado
                if (labelEstadoHilosSmed.Content.ToString() == "Pendiente")
                {
                    listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "HilosSmed", "Unica", 1));
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarElastico_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //se verifica si aun no ha sido entregado
                if (labelEstadoElastico.Content.ToString() == "Pendiente")
                {
                    listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "Elastico", "Unica", 1));
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarTela_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                foreach (KeyValuePair<string,Partnumber> item in listViewTallasTela.Items)
                {
                    if (item.Value.SolicitadoKanban==true)
                    {
                        listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem),"Tela",item.Value.TallaPartnumber,item.Value.RequeridoPartnumber));
                    }
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarBra_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                foreach (KeyValuePair<string, Partnumber> item in listViewTallasBra.Items)
                {
                    if (item.Value.SolicitadoKanban == true)
                    {
                        listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), "Copas", item.Value.TallaPartnumber, item.Value.RequeridoPartnumber));
                    }
                }
                resetEtiquetas();
            }
        }
        private void buttonAgregarCajas_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                if (listViewListaMateriales.Items.Count > 0)
                {
                    bool coincidencia = false;
                    foreach (Partnumber item in listViewCajas.Items)
                    {
                        if (item.ValorSeleccionadoKanban > 0)
                        {
                            foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                            {
                                if (item.CodigoPartNumber == subitem.CodigoPartNumber && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    coincidencia = true;
                                    subitem.RequeridoPartnumber = subitem.RequeridoPartnumber + item.ValorSeleccionadoKanban;
                                }
                            }

                            if (coincidencia == false)
                            {
                                listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.CodigoPartNumber, item.TallaPartnumber, item.ValorSeleccionadoKanban));
                            }
                            resetEtiquetas();
                        }
                    }

                    listViewListaMateriales.Items.Refresh();
                }
                else
                {
                    foreach (Partnumber item in listViewCajas.Items)
                    {
                        if (item.ValorSeleccionadoKanban > 0)
                        {
                            listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.CodigoPartNumber, item.TallaPartnumber, item.ValorSeleccionadoKanban));
                            resetEtiquetas();
                        }
                    }
                }
            }
        }
        private void buttonAgregarCajasParciales_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                if (listViewListaMateriales.Items.Count > 0)
                {
                    bool coincidencia = false;
                    foreach (Partnumber item in listViewCajasParciales.Items)
                    {
                        if (item.ValorSeleccionadoKanban > 0)
                        {
                            foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                            {
                                if (item.CodigoPartNumber == subitem.CodigoPartNumber && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    coincidencia = true;
                                    subitem.RequeridoPartnumber = subitem.RequeridoPartnumber + item.ValorSeleccionadoKanban;
                                }
                            }

                            if (coincidencia == false)
                            {
                                listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.CodigoPartNumber, item.TallaPartnumber, item.ValorSeleccionadoKanban));
                            }
                            resetEtiquetas();
                        }
                    }

                    listViewListaMateriales.Items.Refresh();
                }
                else
                {
                    foreach (Partnumber item in listViewCajasParciales.Items)
                    {
                        if (item.ValorSeleccionadoKanban > 0)
                        {
                            listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.CodigoPartNumber, item.TallaPartnumber, item.ValorSeleccionadoKanban));
                            resetEtiquetas();
                        }
                    }
                }
            }
        }
        private void buttonAgregarGanchos_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                if (listViewListaMateriales.Items.Count > 0)
                {
                    bool coincidencia = false;
                    foreach (KeyValuePair<string,Partnumber> item in listViewGancho.Items)
                    {
                        if (item.Value.ValorSeleccionadoKanban > 0)
                        {
                            foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                            {
                                if (item.Value.CodigoPartNumber == subitem.CodigoPartNumber && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    coincidencia = true;
                                    subitem.RequeridoPartnumber = subitem.RequeridoPartnumber + item.Value.ValorSeleccionadoKanban;
                                }
                            }

                            if (coincidencia == false)
                            {
                                listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.Value.CodigoPartNumber, item.Value.TallaPartnumber, item.Value.ValorSeleccionadoKanban));
                            }
                            resetEtiquetas();
                        }
                    }

                    listViewListaMateriales.Items.Refresh();
                }
                else
                {
                    foreach (KeyValuePair<string, Partnumber> item in listViewGancho.Items)
                    {
                        if (item.Value.ValorSeleccionadoKanban > 0)
                        {
                            listViewListaMateriales.Items.Add(new SolicitudKanban(new Modulo(listBoxModulo.SelectedItem.ToString()), (Lote)(listBoxLote.SelectedItem), item.Value.CodigoPartNumber, item.Value.TallaPartnumber, item.Value.ValorSeleccionadoKanban));
                            resetEtiquetas();
                        }
                    }
                }
            }
        }
        #endregion
        #region datosGeneralesDeFormulario
        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //limpiar datos
            limpiarListas();

            if (listBoxModulo.SelectedIndex > -1)
            {
                if (listBoxModulo.SelectedItem.ToString() == "SMED")
                {
                    chBoxLoteSmed.IsChecked = true;
                }
            }


             if (listBoxLote.SelectedIndex > -1)
            {
                //mostrar datos general de el lote
                Lote loteSeleccionado = (Lote)listBoxLote.SelectedItem;
                labelEstilo.Content = loteSeleccionado.NombreEstilo;
                labelTemporada.Content = loteSeleccionado.TemporadaEstilo;
                labelPiezas.Content = loteSeleccionado.Make;
                labelColor.Content = loteSeleccionado.ColorEstilo;

                //cargar en lista todos los materiales que tiene lote

                using (SqlConnection cn = ConexionTexopsServer.Kanban())
                {
                    listaDeMaterialesCompleta.Clear();
                    string sql = "consultarEntregasLotes '" + loteSeleccionado.NumeroLote + "'";
                    try
                    {
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            listaDeMaterialesCompleta.Add(new Partnumber(dr["PartNumber"].ToString(), dr["categoria"].ToString(), dr["talla"].ToString(), Convert.ToDouble(dr["requerido"]), Convert.ToDouble(dr["solicitado"]), Convert.ToDouble(dr["solicitado"])));
                        }
                        dr.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }

                //colecciones con valores unicos de partnumbers
                HashSet<string> HSaccesorios = new HashSet<string>();
                HashSet<string> HSbinding = new HashSet<string>();
                HashSet<string> HShilos = new HashSet<string>();
                HashSet<string> HScopas = new HashSet<string>();
                HashSet<string> HSelastico= new HashSet<string>();
                HashSet<string> HStela = new HashSet<string>();
                Dictionary<string,Partnumber> DTallasTela = new Dictionary<string,Partnumber>();
                Dictionary<string, Partnumber> DTallasCopa = new Dictionary<string, Partnumber>();
                HashSet<Partnumber> HScajas = new HashSet<Partnumber>();
                HashSet<Partnumber> HScajasParciales = new HashSet<Partnumber>();
                HashSet<Partnumber> HScajasPrincipales = new HashSet<Partnumber>();
                Dictionary<string,Partnumber> DGanchos = new Dictionary<string,Partnumber>();

                foreach (Partnumber item in listaDeMaterialesCompleta)
                {
                    switch (item.CategoryPartNumber)
                    {
                        case "Accesorios":
                            HSaccesorios.Add(item.CodigoPartNumber);
                            break;
                        case "Hilos":
                            HShilos.Add(item.CodigoPartNumber);
                            break;
                        case "HilosSmed":

                            break;
                        case "Elastico":
                            HSelastico.Add(item.CodigoPartNumber);
                            break;
                        case "Tela":
                            HStela.Add(item.CodigoPartNumber);
                            try
                            {
                                DTallasTela.Add(item.TallaPartnumber, item);
                            }
                            catch
                            {

                            }
                            break;
                        case "Copas":
                            HScopas.Add(item.CodigoPartNumber);
                            try
                            {
                                DTallasCopa.Add(item.TallaPartnumber, item);
                            }
                            catch
                            {

                            }
                            break;
                        case "Binding":
                            HSbinding.Add(item.CodigoPartNumber);
                            break;
                        default:
                            if (item.CategoryPartNumber.ToLower().Contains("box"))
                            {
                                HScajas.Add(item);
                            }
                            else if(item.CategoryPartNumber.ToLower().Contains("hanger"))
                            {
                                try
                                {
                                    DGanchos.Add(item.CodigoPartNumber, item);
                                }
                                catch
                                {

                                }
                            }
                            break;
                    }
                }

                ItemsControlAccesorios.ItemsSource = HSaccesorios;
                ItemsControlBinding.ItemsSource = HSbinding;
                ItemsControlTela.ItemsSource = HStela;
                ItemsControlHilos.ItemsSource = HShilos;
                ItemsControlHilosSmed.ItemsSource = HShilos;
                ItemsControlElastico.ItemsSource = HSelastico;
                ItemsControlBra.ItemsSource = HScopas;
                listViewTallasTela.ItemsSource = DTallasTela;
                listViewTallasBra.ItemsSource = DTallasCopa;
                listViewGancho.ItemsSource = DGanchos;

                if (HScajas.Count() > 1)
                {
                    HScajasParciales.Add(HScajas.OrderBy(m => m.RequeridoPartnumber).ToHashSet().First());
                    HScajasPrincipales.Add(HScajas.OrderByDescending(m => m.RequeridoPartnumber).ToHashSet().First());

                    listViewCajasParciales.ItemsSource = HScajasParciales;
                    listViewCajas.ItemsSource = HScajasPrincipales;
                }
                else
                {
                    listViewCajas.ItemsSource = HScajas;
                }

                resetEtiquetas();
            }
        }
        private void textBoxBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            //limpiar lista de lotes
            List<Lote> listaAuxiliar = new List<Lote>();

            //revisar lotes que coinciden con la busqueda
            foreach (Lote item in listaCompletaLotes)
            {
                if (!string.IsNullOrEmpty(textBoxBuscarLote.Text))
                {
                    if (item.NumeroLote.StartsWith(textBoxBuscarLote.Text))
                    {
                        listaAuxiliar.Add(item);
                    }
                }
                else
                {
                    listaAuxiliar.Add(item);
                }
            }

            listBoxLote.ItemsSource = listaAuxiliar;
            listBoxLote.Items.Refresh();

            //limpiar las etiquetas de solicitud, piezas, tempordas
            labelEstilo.Content = "----";
            labelTemporada.Content = "----";
            labelPiezas.Content = "----";
            labelColor.Content = "----";
        }
        private void listBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxModulo.SelectedIndex > -1)
            {
                //limpiar datos cargados
                listaCompletaLotes.Clear();
                textBoxBuscarLote.Clear();
                limpiarListas();
                listViewListaMateriales.Items.Clear();
                if (listBoxModulo.SelectedItem.ToString() == "SMED")
                {
                    tab.SelectedIndex = 3;
                    tabAccesorios.Visibility = Visibility.Collapsed;
                    tabBinding.Visibility = Visibility.Collapsed;
                    tabHilos.Visibility = Visibility.Collapsed;
                    tbHilosSmed.Visibility = Visibility.Visible;
                    tabElastico.Visibility = Visibility.Collapsed;
                    tabCajas.Visibility = Visibility.Collapsed;
                    tabCajasParciales.Visibility = Visibility.Collapsed;
                    tabGancho.Visibility = Visibility.Collapsed;
                    tabTela.Visibility = Visibility.Collapsed;
                    tabBra.Visibility = Visibility.Collapsed;
                    chBoxCajaParcial.IsChecked = false;
                    chBoxCajaParcial.IsEnabled = false;
                    chBoxLoteSmed.IsChecked = true;
                    chBoxLoteSmed.IsEnabled = false;
                }
                else
                {
                    tab.SelectedIndex = 0;
                    chBoxLoteSmed.IsChecked = true;
                    tabAccesorios.Visibility = Visibility.Visible;
                    tabBinding.Visibility = Visibility.Visible;
                    tabHilos.Visibility = Visibility.Visible;
                    tbHilosSmed.Visibility = Visibility.Collapsed;
                    tabElastico.Visibility = Visibility.Visible;
                    tabCajas.Visibility = Visibility.Visible;
                    tabCajasParciales.Visibility = Visibility.Collapsed;
                    tabGancho.Visibility = Visibility.Visible;
                    tabTela.Visibility = Visibility.Visible;
                    tabBra.Visibility = Visibility.Visible;
                    chBoxCajaParcial.IsChecked = false;
                    chBoxCajaParcial.IsEnabled = true;
                    chBoxLoteSmed.IsChecked = false;
                    chBoxLoteSmed.IsEnabled = true;
                }

                //carga ubicacion de 
                using(SqlConnection cn = ConexionTexopsServer.Mantenimiento())
                {
                    try
                    {
                        string sql = "select id from orden_modulos where modulo = '" + listBoxModulo.SelectedItem.ToString() + "'";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        if (dr.Read())
                        {
                            labelUbicacion.Content = dr["id"].ToString();
                        }
                        dr.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                using(SqlConnection cn = ConexionTexopsServer.Kanban())
                {
                    try
                    {
                        string sql = "select manufactureId, lote, make, estilo, estiloId, temporada, color from lotes where modulo='" + listBoxModulo.SelectedItem.ToString() + "'";
                        if (listBoxModulo.SelectedItem.ToString() == "SMED")
                        {
                            sql = "select manufactureId, lote, make, estilo, estiloId, temporada, color from lotes WHERE kanban LIKE '%SMED%'";
                        }
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            listaCompletaLotes.Add(new Lote(dr["lote"].ToString(), Convert.ToInt32(dr["manufactureId"]), dr["estilo"].ToString(), dr["temporada"].ToString(), dr["color"].ToString(), Convert.ToInt32(dr["make"])));
                        };
                        dr.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    listBoxLote.ItemsSource = listaCompletaLotes;
                    listBoxLote.Items.Refresh();
                }
            }
        }
        private void passwordboxUsuario_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelUsuario.Content = "*";
            buttonEnviarSolicitud.IsEnabled = false;

            //limpiar datos cargados
            listBoxModulo.Items.Clear();
            listaCompletaLotes.Clear();
            listBoxLote.Items.Refresh();
            textBoxBuscarLote.Clear();
            listViewListaMateriales.Items.Clear();
            limpiarListas();

            #region consultarUsuario
            //consultar
            string sql = "SELECT a.codigo, b.modulo FROM usuarios a " +
                "LEFT JOIN produccion.dbo.modulosProduccion b ON a.codigo=b.coordinadorCodigo or a.codigo=b.ingenieroProcesosCodigo " +
                "WHERE produccion=1 AND modulo IS NOT NULL AND CONVERT(NVARCHAR(255),DECRYPTBYPASSPHRASE('"+Properties.Settings.Default.desencriptarContrasenia +"',contrasenaEncriptada))='"+ passwordboxUsuario.Password +"'";
           using(SqlConnection cn = ConexionTexopsServer.Ingenieria())
            {
                try
                {
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        listBoxModulo.Items.Add(dr["modulo"].ToString());
                        labelUsuario.Content = dr["codigo"].ToString();
                        buttonEnviarSolicitud.IsEnabled = true;
                    }
                    dr.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            #endregion
        }
        private void buttonEnviarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "insert into solicitudesKanban(tipo, modulo, ubicacion, fechaSolicitud, loteSmed, validadoSmed, solicitudCajaParcial) values('solicitud', '" + listBoxModulo.SelectedItem.ToString() + "', '" + labelUbicacion.Content + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + chBoxLoteSmed.IsChecked + "', '" + !chBoxLoteSmed.IsChecked + "', '" + chBoxCajaParcial.IsChecked +"') SELECT SCOPE_IDENTITY()";
            cnKanban.Open();
            SqlCommand cm = new SqlCommand(sql, cnKanban);
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            int idIngreso = Convert.ToInt32(dr[0]);
            dr.Close();
            //ingresar detalles de la orden
            foreach (SolicitudKanban item in listViewListaMateriales.Items)
            {
                sql = "insert into detalleSolicitudeKanban(solicitudKanbanId, lote, material, talla, cantidad, entregado) values('" + idIngreso + "', '" + item.NumeroLote + "', '" + item.CodigoPartNumber + "', '" + item.TallaPartnumber + "', '" + item.RequeridoPartnumber+ "', 0)";
                if (item.CodigoPartNumber.Contains("HANGER"))
                {
                    //sql = "insert into detalleSolicitudeKanban(solicitudKanbanId, lote, material, talla, cantidad, diferencia, requerido, entregado) values('" + idIngreso + "', '" + item.NumeroLote + "', '" + item.CodigoPartNumber + "', '" + item.TallaPartnumber + "', '" + item.RequeridoPartnumber + "', '" + (item.diferencia - item.cantidad) + "','" + item.solicitado + "', 0)";
                }
                cm = new SqlCommand(sql, cnKanban);
                cm.ExecuteNonQuery();
            }
            cnKanban.Close();

            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new estadoPlanta();
        }
        #endregion
        private void listViewListaMateriales_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                if (listViewListaMateriales.SelectedIndex > -1)
                {
                    listViewListaMateriales.Items.Remove(listViewListaMateriales.SelectedItem);
                    resetEtiquetas();   
                }
            }
        }
        private void resetEtiquetas()
        {
            //limpiar labels de estado de material
            labelEstadoAccesorios.Content = "No necesario";
            labelEstadoBinding.Content = "No necesario";
            labelEstadoHilos.Content = "No necesario";
            labelEstadoHilosSmed.Content = "No necesario";
            labelEstadoElastico.Content = "No necesario";
            labelEstadoCajas.Content = "No necesario";
            labelEstadoCajasParciales.Content = "No necesario";
            labelEstadoGanchos.Content = "No necesario";
            labelEstadoTela.Content = "No necesario";
            labelEstadoBra.Content = "No necesario";

            int materialesAgregados = listViewListaMateriales.Items.Count;

            foreach (Partnumber item in listaDeMaterialesCompleta)
            {
                switch (item.CategoryPartNumber)
                {
                    case "Accesorios":
                        if (item.SolicitadoPartnumber == 1)
                        {
                            labelEstadoAccesorios.Content = "Solicitado";
                        }
                        else
                        {
                            if (materialesAgregados > 0)
                            {
                                foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                                {
                                    if (subitem.CodigoPartNumber == "Accesorios" && subitem.NumeroLote==((Lote)listBoxLote.SelectedItem).NumeroLote)
                                    {
                                        labelEstadoAccesorios.Content = "Agregado";
                                        break;
                                    }
                                    else
                                    {
                                        labelEstadoAccesorios.Content = "Pendiente";
                                    }
                                }
                            }
                            else
                            {
                                labelEstadoAccesorios.Content = "Pendiente";
                            }
                        }
                        break;
                    case "Hilos":
                        if (item.SolicitadoPartnumber == 1)
                        {
                            labelEstadoHilos.Content = "Solicitado";
                        }
                        else
                        {
                            if (materialesAgregados > 0)
                            {
                                foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                                {
                                    if (subitem.CodigoPartNumber == "Hilos" && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                    {
                                        labelEstadoHilos.Content = "Agregado";
                                        break;
                                    }
                                    else
                                    {
                                        labelEstadoHilos.Content = "Pendiente";
                                    }
                                }
                            }
                            else
                            {
                                labelEstadoHilos.Content = "Pendiente";
                            }
                        }
                        break;
                    case "HilosSmed":
                        if (item.SolicitadoPartnumber == 1)
                        {
                            labelEstadoHilosSmed.Content = "Solicitado";
                        }
                        else
                        {
                            if (materialesAgregados > 0)
                            {
                                foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                                {
                                    if (subitem.CodigoPartNumber == "HilosSmed" && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                    {
                                        labelEstadoHilosSmed.Content = "Agregado";
                                        break;
                                    }
                                    else
                                    {
                                        labelEstadoHilosSmed.Content = "Pendiente";
                                    }
                                }
                            }
                            else
                            {
                                labelEstadoHilosSmed.Content = "Pendiente";
                            }
                        }
                        break;
                    case "Elastico":
                        if (item.SolicitadoPartnumber == 1)
                        {
                            labelEstadoElastico.Content = "Solicitado";
                        }
                        else
                        {
                            if (materialesAgregados > 0)
                            {
                                foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                                {
                                    if (subitem.CodigoPartNumber == "Elastico" && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                    {
                                        labelEstadoElastico.Content = "Agregado";
                                        break;
                                    }
                                    else
                                    {
                                        labelEstadoElastico.Content = "Pendiente";
                                    }
                                }
                            }
                            else
                            {
                                labelEstadoElastico.Content = "Pendiente";
                            }
                        }
                        break;
                    case "Binding":
                        if (item.SolicitadoPartnumber == 1)
                        {
                            labelEstadoBinding.Content = "Solicitado";
                        }
                        else
                        {
                            if (materialesAgregados > 0)
                            {
                                foreach (SolicitudKanban subitem in listViewListaMateriales.Items)
                                {
                                    if (subitem.CodigoPartNumber == "Binding" && subitem.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                    {
                                        labelEstadoBinding.Content = "Agregado";
                                        break;
                                    }
                                    else
                                    {
                                        labelEstadoBinding.Content = "Pendiente";
                                    }
                                }
                            }
                            else
                            {
                                labelEstadoBinding.Content = "Pendiente";
                            }
                        }
                        break;
                }
            }

            foreach(SolicitudKanban item in listViewListaMateriales.Items)
            {
                switch (item.CodigoPartNumber)
                {
                    case "Tela":
                        foreach (KeyValuePair <string,Partnumber> subitem in listViewTallasTela.Items)
                        {
                            if (item.TallaPartnumber == subitem.Value.TallaPartnumber)
                            {
                                subitem.Value.SolicitadoKanban = false;
                                subitem.Value.SolicitadoPartnumber = item.RequeridoPartnumber;
                            }
                        }

                        listViewTallasTela.Items.Refresh();
                        break;
                    case "Copas":
                        foreach (KeyValuePair<string, Partnumber> subitem in listViewTallasBra.Items)
                        {
                            if (item.TallaPartnumber == subitem.Value.TallaPartnumber)
                            {
                                subitem.Value.SolicitadoKanban = false;
                                subitem.Value.SolicitadoPartnumber = item.RequeridoPartnumber;
                            }
                        }
                        listViewTallasBra.Items.Refresh();
                        break;
                    default:
                        if (item.CodigoPartNumber.ToLower().Contains("box"))
                        {
                            foreach(Partnumber subitem in listViewCajas.Items)
                            {
                                if (item.CodigoPartNumber == subitem.CodigoPartNumber &&  item.NumeroLote==((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    subitem.ValorSeleccionadoKanban = 0;
                                    subitem.SolicitadoPartnumber = subitem.SolicitadoPartnumberInicial+ item.RequeridoPartnumber;
                                }
                            }
                            listViewCajas.Items.Refresh();

                            foreach (Partnumber subitem in listViewCajasParciales.Items)
                            {
                                if (item.CodigoPartNumber == subitem.CodigoPartNumber && item.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    subitem.ValorSeleccionadoKanban = 0;
                                    subitem.SolicitadoPartnumber = subitem.SolicitadoPartnumberInicial + item.RequeridoPartnumber;
                                }
                            }
                            listViewCajasParciales.Items.Refresh();
                        }
                        else if (item.CodigoPartNumber.ToLower().Contains("hanger"))
                        {
                            foreach (KeyValuePair<string, Partnumber> subitem in listViewGancho.Items)
                            {
                                if (item.CodigoPartNumber == subitem.Value.CodigoPartNumber && item.NumeroLote == ((Lote)listBoxLote.SelectedItem).NumeroLote)
                                {
                                    subitem.Value.ValorSeleccionadoKanban = 0;
                                    subitem.Value.SolicitadoPartnumber = subitem.Value.SolicitadoPartnumberInicial + item.RequeridoPartnumber;
                                }
                            }
                            listViewGancho.Items.Refresh();
                        }
                        break;
                }
            }



        }
        private void limpiarListas()
        {
            labelEstilo.Content = "----";
            labelColor.Content = "----";
            labelTemporada.Content = "----";
            labelPiezas.Content = 0;
            ItemsControlAccesorios.ItemsSource= new List<Partnumber>();
            ItemsControlBinding.ItemsSource= new List<Partnumber>();
            ItemsControlHilos.ItemsSource= new List<Partnumber>();
            ItemsControlHilosSmed.ItemsSource= new List<Partnumber>();
            ItemsControlTela.ItemsSource= new List<Partnumber>();
            ItemsControlBra.ItemsSource= new List<Partnumber>();
            ItemsControlElastico.ItemsSource= new List<Partnumber>();
            listViewCajas.ItemsSource= new List<Partnumber>();
            listViewCajasParciales.ItemsSource= new List<Partnumber>();
            listViewGancho.ItemsSource= new List<Partnumber>();
            listViewTallasTela.ItemsSource = new List<Partnumber>();
            listViewTallasBra.ItemsSource= new List<Partnumber>();
        }

        private void chBoxCajaParcial_Checked(object sender, RoutedEventArgs e)
        {

            chBoxLoteSmed.IsChecked = false;
            chBoxLoteSmed.IsEnabled = false;


            listViewListaMateriales.Items.Clear();
            resetEtiquetas();
            tab.SelectedIndex = 6;
            tabAccesorios.Visibility = Visibility.Collapsed;
            tabBinding.Visibility = Visibility.Collapsed;
            tabHilos.Visibility = Visibility.Collapsed;
            tbHilosSmed.Visibility = Visibility.Collapsed;
            tabElastico.Visibility = Visibility.Collapsed;
            tabCajas.Visibility = Visibility.Collapsed;
            tabCajasParciales.Visibility = Visibility.Visible;
            tabGancho.Visibility = Visibility.Collapsed;
            tabTela.Visibility = Visibility.Collapsed;
            tabBra.Visibility = Visibility.Collapsed;
        }

        private void chBoxCajaParcial_Unchecked(object sender, RoutedEventArgs e)
        {
            chBoxLoteSmed.IsChecked = false;
            chBoxLoteSmed.IsEnabled = true;

            listViewListaMateriales.Items.Clear();
            resetEtiquetas();
            tab.SelectedIndex = 0;
            tabAccesorios.Visibility = Visibility.Visible;
            tabBinding.Visibility = Visibility.Visible;
            tabHilos.Visibility = Visibility.Visible;
            tbHilosSmed.Visibility = Visibility.Visible;
            tabElastico.Visibility = Visibility.Visible;
            tabCajas.Visibility = Visibility.Visible;
            tabCajasParciales.Visibility = Visibility.Collapsed;
            tabGancho.Visibility = Visibility.Visible;
            tabTela.Visibility = Visibility.Visible;
            tabBra.Visibility = Visibility.Visible;
        }
    }
}
