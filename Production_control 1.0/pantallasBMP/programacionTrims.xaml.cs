using Production_control_1._0.clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Production_control_1._0.pantallasBMP
{
    public partial class programacionTrims : Page
    {
        public List<tarjetaKanban> tarjetas = new List<tarjetaKanban>();
        public List<tarjetaKanban> lotes = new List<tarjetaKanban>();
        public List<String> partnumbersNoClasificados = new List<string>();
        public programacionTrims()
        {
            InitializeComponent();
            cargarListaDeTarjetas();
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new menuBMP());
        }


        private void cargarListaDeTarjetas()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT DISTINCT kanban FROM lotesNoProgramadosTrims";
            try
            {
                tarjetas.Clear();
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tarjetas.Add(new tarjetaKanban
                    {
                        tarjeta = dr["kanban"].ToString(),
                        seleccionado = false
                    }) ;
                };
                dr.Close();
                cn.Close();

                lstTarjetas.ItemsSource = tarjetas;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void txtBuscarTarjeta_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<tarjetaKanban> coincidencias = new List<tarjetaKanban>();
            foreach(tarjetaKanban item in tarjetas)
            {
                if (string.IsNullOrEmpty(txtBuscarTarjeta.Text))
                {
                    coincidencias.Add(item);
                }
                else
                {
                    if (item.tarjeta.ToLower().Contains(txtBuscarTarjeta.Text.ToLower()))
                    {
                        coincidencias.Add(item);
                    }
                }
            }

            btnSeleccionarTodo.IsChecked = false;
            lstTarjetas.ItemsSource = coincidencias;
        }
        private void btnSeleccionarTodo_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (tarjetaKanban item in lstTarjetas.Items)
            {
                item.seleccionado = false;
            }
            lstTarjetas.Items.Refresh();
            imgSeleccionar.Source = new BitmapImage(new Uri("/imagenes/combinar.png", UriKind.RelativeOrAbsolute));
        }
        private void btnSeleccionarTodo_Checked(object sender, RoutedEventArgs e)
        {
            foreach (tarjetaKanban item in lstTarjetas.Items)
            {
                item.seleccionado = true;
            }
            lstTarjetas.Items.Refresh();
            imgSeleccionar.Source = new BitmapImage(new Uri("/imagenes/separar.png", UriKind.RelativeOrAbsolute));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cadenaTarjetas = "'";
            bool existenDatosSeleccionados = false;
            List<tarjetaKanban> elementosCoincidentes = new List<tarjetaKanban>();
            foreach (tarjetaKanban item in tarjetas)
            {
                if (item.seleccionado == true)
                {
                    existenDatosSeleccionados = true;
                    cadenaTarjetas = cadenaTarjetas + item.tarjeta + "', '";
                    elementosCoincidentes.Add(item);
                }
            }

            tarjetas.RemoveAll(l => elementosCoincidentes.Contains(l));

            if (existenDatosSeleccionados == true)
            {
                txtBuscarTarjeta.Clear();
                btnSeleccionarTodo.IsChecked = false;
                lstTandas.ItemsSource = consultarLotes(cadenaTarjetas.Substring(0, cadenaTarjetas.Length - 3));
                lstTarjetas.ItemsSource = tarjetas;
                lstTarjetas.Items.Refresh();
                lstTandas.Items.Refresh();

                //generar cadena de lotes para verificar si todos los partNumbers tienen datos
                string cadenaLotes = "'";
                bool existenLotesAgregados = false;
                foreach (tarjetaKanban item in lstTandas.Items)
                {
                    existenLotesAgregados = true;
                    cadenaLotes = cadenaLotes + item.lote + "', '";
                }
                if (existenLotesAgregados == true)
                {
                    lstPartnumbersNoClasificados.ItemsSource = consultarPartnumbersNulos(cadenaLotes.Substring(0, cadenaLotes.Length - 3));
                    lstPartnumbersNoClasificados.Items.Refresh();

                    List<tarjetaKanban> listaTiempos = consultarTiempos(cadenaLotes.Substring(0, cadenaLotes.Length - 3));
                    foreach(tarjetaKanban item in lotes)
                    {
                        foreach(tarjetaKanban subitem in listaTiempos)
                        {
                            if (item.lote == subitem.lote)
                            {
                                item.sam = subitem.sam;
                            }
                        }
                    }
                }
            }

        }





        private List<tarjetaKanban> consultarLotes(string cadenaTarjetas)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT lote, estilo, color, temporada, cliente, make, copa FROM lotesNoProgramadosTrims WHERE kanban IN(" + cadenaTarjetas+ ") ORDER BY estilo, temporada, color";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lotes.Add(new tarjetaKanban
                    {
                        lote = dr["lote"].ToString(),
                        cliente = dr["cliente"].ToString(),
                        estilo = dr["estilo"].ToString(),
                        color = dr["color"].ToString(),
                        temporada = dr["temporada"].ToString(),
                        copa = Convert.ToBoolean(dr["copa"]),
                        make = Convert.ToInt32(dr["make"] is DBNull ? 0 : dr["make"])
                    });
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return lotes;
        }
        private List<string> consultarPartnumbersNulos(string cadenaLotes)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT DISTINCT partnumber FROM samPartNumbers WHERE celula IS NULL AND LOTE IN("+cadenaLotes+ ")";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    partnumbersNoClasificados.Add(dr["partnumber"].ToString());
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return partnumbersNoClasificados;
        }
        private List<tarjetaKanban> consultarTiempos(string cadenaLotes)
        {
            List<tarjetaKanban> tiemposResultado = new List<tarjetaKanban>();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT lote, sum(sam) as sam  FROM samPartNumbers where celula='trims' AND LOTE IN(" + cadenaLotes + ") GROUP BY lote";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tiemposResultado.Add(new tarjetaKanban {lote=dr["lote"].ToString(), sam=Convert.ToDouble(dr["sam"] is DBNull? 0: dr["sam"]) });
                };
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return tiemposResultado;
        }
        private void generadorDeTandas(List<tarjetaKanban> listaLotes, DateTime fecha)
        {
            List<tarjetaKanban> listaOrdenada= (lotes.OrderBy(o => o.estilo).ThenBy(o=>o.temporada).ThenBy(o=>o.color)).ToList();
            lotes = listaOrdenada;
            lstTandas.ItemsSource = lotes;
            lstTandas.Items.Refresh();
            string idTandas = "A-" + fecha.Year + fecha.Month + fecha.Day + "-";
            string estilo = (lotes.First()).estilo;
            string temporada = (lotes.First()).temporada;
            string cliente = (lotes.First()).cliente;
            string color = (lotes.First()).color;
            int make = (lotes.First()).make;
            double sam = (lotes.First()).sam;
            int tanda = 1;
            int conteoGrandes = 0;
            int conteoMedianos = 0;
            int conteoPequenios = 0;
            int conteoGeneral = 0;

            int lotesMaximosDeTanda = 0;

            foreach (tarjetaKanban item in lotes)
            {
                if (item.estilo==estilo && item.temporada==temporada && make<=3500)
                {
                    //determinar tamanio del lote
                    #region determinarTamanioDeLote
                    if (item.make <= 100)
                    {
                        conteoPequenios = conteoPequenios + 1;
                    }
                    else if (item.make <= 600)
                    {
                        conteoMedianos = conteoMedianos + 1;
                    }
                    else
                    {
                        conteoGrandes = conteoGrandes + 1;
                    }
                    #endregion

                    if (conteoGrandes > 0)
                    {
                        lotesMaximosDeTanda = 3;
                    }
                    else if (conteoMedianos > 0)
                    {
                        lotesMaximosDeTanda = 4;
                    }
                    else
                    {
                        lotesMaximosDeTanda = 5;
                    }

                    conteoGeneral = conteoGeneral + 1;

                    if (conteoGeneral <= lotesMaximosDeTanda)
                    {
                        if (conteoGrandes <= 3 && conteoMedianos <= 4 && conteoPequenios <= 5)
                        {
                            item.tanda = idTandas + tanda;
                        }
                        else
                        {
                            tanda = tanda + 1;
                            make = 0;
                            item.tanda = idTandas + tanda;
                        }
                    }
                    else
                    {
                        conteoGrandes = 0;
                        conteoMedianos = 0;
                        conteoPequenios = 0;
                        conteoGeneral = 0;

                        //determinar tamanio del lote
                        #region determinarTamanio
                        if (item.make <= 100)
                        {
                            conteoPequenios = conteoPequenios + 1;
                        }
                        else if (item.make <= 600)
                        {
                            conteoMedianos = conteoMedianos + 1;
                        }
                        else
                        {
                            conteoGrandes = conteoGrandes + 1;
                        }
                        #endregion

                        tanda = tanda + 1;
                        make = 0;
                        item.tanda = idTandas + tanda;
                    }

                }
                else
                {
                    conteoGrandes = 0;
                    conteoMedianos = 0;
                    conteoPequenios = 0;
                    conteoGeneral = 0;

                    //determinar tamanio del lote
                    #region determinarTamanio
                    if (item.make <= 100)
                    {
                        conteoPequenios = conteoPequenios + 1;
                    }
                    else if (item.make <= 600)
                    {
                        conteoMedianos = conteoMedianos + 1;
                    }
                    else
                    {
                        conteoGrandes = conteoGrandes + 1;
                    }
                    #endregion

                    tanda = tanda + 1;
                    make = 0;
                    item.tanda = idTandas + tanda;
                }
                estilo = item.estilo;
                temporada = item.temporada;
                make = item.make;
            }
        }


        #region controlesListViewTandas
        private void btbSubir_Click(object sender, RoutedEventArgs e)
        {
            int indexSeleccionado = lstTandas.SelectedIndex;
            if (indexSeleccionado > -1)
            {
                tarjetaKanban itemSeleccionado = (tarjetaKanban)lstTandas.SelectedItem;

                if (indexSeleccionado - 1 > -1)
                {
                    lotes.Remove(itemSeleccionado);
                    lotes.Insert(indexSeleccionado - 1, itemSeleccionado);
                    lstTandas.ItemsSource = lotes;
                    lstTandas.Items.Refresh();
                }
            }
        }
        private void btnBajar_Click(object sender, RoutedEventArgs e)
        {
            int indexSeleccionado = lstTandas.SelectedIndex;
            if (indexSeleccionado > -1)
            {
                tarjetaKanban itemSeleccionado = (tarjetaKanban)lstTandas.SelectedItem;

                if (indexSeleccionado + 1 < lstTandas.Items.Count)
                {
                    lotes.Remove(itemSeleccionado);
                    lotes.Insert(indexSeleccionado + 1, itemSeleccionado);
                    lstTandas.ItemsSource = lotes;
                    lstTandas.Items.Refresh();
                }
            }
        }
        #endregion

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            btnSeleccionarTodo.IsChecked = false;
            cargarListaDeTarjetas();
            lotes.Clear();
            lstTandas.Items.Refresh();
            lstPartnumbersNoClasificados.Items.Refresh();
            partnumbersNoClasificados.Clear();
            lstOrden.Items.Clear();
        }

        private void btnGenerarTandas_Click(object sender, RoutedEventArgs e)
        {
            generadorDeTandas(lotes, datePickerFecha.SelectedDate.Value);
            lstTandas.Items.Refresh();
        }
    }
}
