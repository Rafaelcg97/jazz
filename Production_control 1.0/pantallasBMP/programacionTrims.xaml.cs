using JazzCCO._0.clases;
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

namespace JazzCCO._0.pantallasBMP
{
    public partial class programacionTrims : Page
    {
        #region listasGenerales
        public List<tarjetaKanban> tarjetas = new List<tarjetaKanban>();
        public List<tarjetaKanban> lotes = new List<tarjetaKanban>();
        public List<String> partnumbersNoClasificados = new List<string>();
        List<tarjetaKanban> ordenTandasResultante = new List<tarjetaKanban>();
        #endregion
        public programacionTrims()
        {
            InitializeComponent();
            cargarListaDeTarjetas();
            datePickerFecha.DisplayDateStart = DateTime.Now;
            datePickerFecha.DisplayDateEnd = DateTime.Now.AddDays(5);
        }
        #region controlesGenerales
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new menuBMP());
        }
        #endregion
        #region controlesListViewTarjetas
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
            string cadenaLotes = "'";
            List<tarjetaKanban> elementosCoincidentes = new List<tarjetaKanban>();
            foreach (tarjetaKanban item in tarjetas)
            {
                if (item.seleccionado == true)
                {
                    existenDatosSeleccionados = true;
                    cadenaTarjetas = cadenaTarjetas + item.tarjeta + "', '";
                    elementosCoincidentes.Add(item);
                    cadenaLotes = cadenaLotes + item.lote + "', '";
                }
            }

            tarjetas.RemoveAll(l => elementosCoincidentes.Contains(l));

            if (existenDatosSeleccionados == true)
            {
                txtBuscarTarjeta.Clear();
                btnSeleccionarTodo.IsChecked = false;

                lstPartnumbersNoClasificados.ItemsSource = consultarPartnumbersNulos(cadenaLotes.Substring(0, cadenaLotes.Length - 3));
                lstPartnumbersNoClasificados.Items.Refresh();

                List<tarjetaKanban> listaTiempos = consultarTiempos(cadenaLotes.Substring(0, cadenaLotes.Length - 3));
                foreach (tarjetaKanban item in elementosCoincidentes)
                {
                    foreach (tarjetaKanban subitem in listaTiempos)
                    {
                        if (item.lote == subitem.lote)
                        {
                            item.sam = Math.Round(subitem.sam + subitem.pfdLoteTrims, 3);
                            item.conteoCategoria = subitem.conteoCategoria;
                        }
                    }
                }

                lotes.AddRange(elementosCoincidentes);
                lstTandas.ItemsSource = lotes;
                lstTarjetas.ItemsSource = tarjetas;
                lstTarjetas.Items.Refresh();
                lstTandas.Items.Refresh();
            }

        }
        #endregion
        #region metodosDeConsultaSQL
        private void cargarListaDeTarjetas()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT kanban, lote, estilo, color, cliente, temporada, make, copa FROM lotesNoProgramadosTrims WHERE  kanban like '%WK%'";
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
                        lote= dr["lote"].ToString(),
                        estilo = dr["estilo"].ToString(),
                        color=dr["color"].ToString(),
                        cliente=dr["cliente"].ToString(),
                        temporada=dr["temporada"].ToString(),
                        make= Convert.ToInt32(dr["make"] is DBNull ? 0: dr["make"]),
                        copa= Convert.ToBoolean(dr["copa"]),
                        seleccionado = false
                    });
                };
                dr.Close();
                cn.Close();

                lstTarjetas.ItemsSource = tarjetas;
                lstTarjetas.Items.Refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
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
            string sql = "SELECT lote, count(categoria) as conteoCategoria, round(sum(sam),5) as sam  FROM samPartNumbers where celula='trims' AND LOTE IN(" + cadenaLotes + ") GROUP BY lote";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tiemposResultado.Add(
                        new tarjetaKanban 
                        {
                            lote=dr["lote"].ToString(), 
                            sam=Convert.ToDouble(dr["sam"] is DBNull? 0: dr["sam"]), 
                            conteoCategoria=Convert.ToInt32(dr["conteoCategoria"]) });
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
        #endregion
        #region controlesListViewTandas
        private void generadorDeTandas(List<tarjetaKanban> listaLotes, DateTime fecha)
        {
            List<tarjetaKanban> listaOrdenada = (lotes.OrderBy(o => o.estilo).ThenBy(o => o.temporada).ThenBy(o => o.color)).ToList();
            lotes = listaOrdenada;
            lstTandas.ItemsSource = lotes;
            lstTandas.Items.Refresh();
            string idTandas = "A-" + fecha.Year + fecha.Month.ToString("00") + fecha.Day.ToString("00") + "-";
            string estilo = (lotes.First()).estilo;
            string temporada = (lotes.First()).temporada;
            string cliente = (lotes.First()).cliente;
            string color = (lotes.First()).color;
            int make = (lotes.First()).make;
            double sam = (lotes.First()).sam + 27.1;
            int tanda = 1;
            int conteoGrandes = 0;
            int conteoMedianos = 0;
            int conteoPequenios = 0;
            int conteoGeneral = 0;
            int lotesMaximosDeTanda = 0;

            int acumuladoCategorias = 0;
            int conteoLotesTnda = 0;

            foreach (tarjetaKanban item in lotes)
            {
                //validar si es REI pra verificar si debe o no tomarse en cuenta el color
                if (item.cliente == "REI")
                {
                    if (item.estilo == estilo && item.temporada == temporada && item.color==color && make <= 3500 && sam<=150)
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
                                item.color = color;
                                acumuladoCategorias = acumuladoCategorias + item.conteoCategoria;
                                conteoLotesTnda++;
                                sam = sam + item.sam+ ((Math.Round(Convert.ToDouble(acumuladoCategorias)/Convert.ToDouble(conteoLotesTnda),0)*6.25));
                            }
                            else
                            {
                                tanda = tanda + 1;
                                make = 0;
                                item.tanda = idTandas + tanda;
                                acumuladoCategorias = item.conteoCategoria;
                                conteoLotesTnda = 1;
                                sam = item.sam +27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
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
                            acumuladoCategorias = item.conteoCategoria;
                            conteoLotesTnda=1;
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
                        acumuladoCategorias = item.conteoCategoria;
                        conteoLotesTnda = 1;
                    }
                }
                //si no es REI
                else
                {
                    //validar si tiene copa para los limites
                    if (item.copa == false)
                    {
                        if (item.estilo == estilo && item.temporada == temporada && make <= 3500 && sam<150)
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
                                    acumuladoCategorias = acumuladoCategorias + item.conteoCategoria;
                                    conteoLotesTnda++;
                                    sam = sam+ item.sam + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
                                }
                                else
                                {
                                    tanda = tanda + 1;
                                    make = 0;
                                    item.tanda = idTandas + tanda;
                                    acumuladoCategorias = item.conteoCategoria;
                                    conteoLotesTnda = 1;
                                    sam = item.sam+27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
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
                                acumuladoCategorias = item.conteoCategoria;
                                conteoLotesTnda = 1;
                                sam = item.sam+27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
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
                            acumuladoCategorias = item.conteoCategoria;
                            conteoLotesTnda = 1;
                            sam = item.sam+27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
                        }
                    }
                    else
                    {
                        if (item.estilo == estilo && item.temporada == temporada && make <= 2400)
                        {
                            //determinar tamanio del lote
                            #region determinarTamanioDeLote
                            if (item.make <= 100)
                            {
                                conteoPequenios = conteoPequenios + 1;
                            }
                            else if (item.make <= 300)
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
                                lotesMaximosDeTanda = 2;
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
                                if (conteoGrandes <= 2 && conteoMedianos <= 4 && conteoPequenios <= 5)
                                {
                                    item.tanda = idTandas + tanda;
                                    acumuladoCategorias = acumuladoCategorias + item.conteoCategoria;
                                    conteoLotesTnda++;
                                    sam = sam+item.sam + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
                                }
                                else
                                {
                                    tanda = tanda + 1;
                                    make = 0;
                                    item.tanda = idTandas + tanda;
                                    acumuladoCategorias = item.conteoCategoria;
                                    conteoLotesTnda = 1;
                                    sam = item.sam+27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
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
                                else if (item.make <= 300)
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
                                acumuladoCategorias = item.conteoCategoria;
                                conteoLotesTnda = 1;
                                sam = item.sam+27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
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
                            else if (item.make <= 300)
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
                            acumuladoCategorias = item.conteoCategoria;
                            conteoLotesTnda = 1;
                            sam = item.sam+ 27.1 + ((Math.Round(Convert.ToDouble(acumuladoCategorias) / Convert.ToDouble(conteoLotesTnda), 0) * 6.25));
                        }
                    }
                }
                estilo = item.estilo;
                temporada = item.temporada;
                make = item.make;
                color = item.color;
                acumuladoCategorias = item.acumuladoCategoria;
                conteoLotesTnda = 0;
            }

            string tanda1 = ((tarjetaKanban)(lotes.First())).tanda;
            string color1 = "LightBlue";
            string color2 = "LightGray";
            foreach (tarjetaKanban item in lotes)
            {
                if(Convert.ToInt32(item.tanda.Substring(11, item.tanda.Length-11)) % 2 == 0)
                {
                    item.colorFondo = color1;
                }
                else
                {
                    item.colorFondo = color2;
                }
            }


            var result= lotes.GroupBy(d => d.tanda)
                .Select(
                g => 
                new tarjetaKanban
                {
                    tanda = g.First().tanda,
                    samTanda = (27.1+g.Sum(s => s.sam) + (g.Average(s => s.conteoCategoria)*6.75))* g.Max(s => s.pfdCliente),
                });
           
            foreach(tarjetaKanban item in result)
            {
                foreach(tarjetaKanban subitem in lotes)
                {
                    if (item.tanda == subitem.tanda)
                    {
                        subitem.samTanda =Math.Round(item.samTanda,3);
                    }
                }
            }


            lstTandas.Items.Refresh();
        }
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
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            btnSeleccionarTodo.IsChecked = false;
            cargarListaDeTarjetas();
            lotes.Clear();
            lstTandas.Items.Refresh();
            lstPartnumbersNoClasificados.Items.Refresh();
            partnumbersNoClasificados.Clear();
            ordenTandasResultante.Clear();
            lstOrden.Items.Refresh();
            pbarProgramado.Value = 0;
            lblMinutos.Content = "0 min de 528 min";
        }
        private void btnGenerarTandas_Click(object sender, RoutedEventArgs e)
        {
            if (lotes.Count > 0 && datePickerFecha.SelectedDate != null)
            {
                generadorDeTandas(lotes, datePickerFecha.SelectedDate.Value);
                lstTandas.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Asegures de tener lotes agregados y una fecha valida seleccionada", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnOrdenarTandas_Click(object sender, RoutedEventArgs e)
        {
            int conteoDatosVacios = 0;
            double totalMinutos = 0;
            foreach (tarjetaKanban item in lotes)
            {
                totalMinutos = totalMinutos + item.sam;
                if (string.IsNullOrEmpty(item.tanda))
                {
                    conteoDatosVacios = conteoDatosVacios + 1;
                }
            }
            if (conteoDatosVacios == 0)
            {
                lstOrden.ItemsSource = generarOrdenPrioridadesTandas((lotes.Select(o => o.tanda).Distinct().ToArray()).ToList());
                lstOrden.Items.Refresh();
                lblMinutos.Content=Math.Round(totalMinutos, 2)+" mintos de 528 de minutos";
                pbarProgramado.Value = totalMinutos;
            }
            else
            {
                MessageBox.Show("Parece que hay lotes con un codigo de tanda no valido");
            }

        }
        private void btnGuardarProgramacion_Click(object sender, RoutedEventArgs e)
        {
            if (ordenTandasResultante.Count > 0)
            {

                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                try
                {
                    cn.Open();
                    foreach (tarjetaKanban item in ordenTandasResultante)
                    {
                        string sql = "INSERT INTO ordenTandaTrims VALUES('" + item.tanda + "','" + item.ordenPrioridad + "')";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                try
                {
                    cn.Open();
                    foreach (tarjetaKanban item in lotes)
                    {
                        string sql = "INSERT INTO programacionTandasTrims VALUES('" + item.lote + "', '" + item.tanda + "', '"+item.cliente+"', '" + item.estilo + "', '"+item.modulo+"', '" + item.temporada + "', '" + item.color + "', '" + item.copa + "', '" + item.make + "', '" + item.sam + "')";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Operacion Terminada");
            }
            else
            {
                MessageBox.Show("Genere el orden de las tandas antes de guardar", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ordenTandasResultante.Clear();
            lstOrden.Items.Refresh();
        }
        private List<tarjetaKanban> generarOrdenPrioridadesTandas(List<string> listaTandas)
        {
            ordenTandasResultante.Clear();
            if (listaTandas.Count > 0)
            {
                int orden = 0;
                ordenTandasResultante.Clear();
                foreach (string item in listaTandas)
                {
                    orden = orden + 1;
                    ordenTandasResultante.Add(new tarjetaKanban { ordenPrioridad = orden, tanda = item });
                }
            }
            return ordenTandasResultante;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string tanda1 = ((tarjetaKanban)(lotes.First())).tanda;
            string color1 = "LightBlue";
            string color2 = "Pink";
            foreach (tarjetaKanban item in lotes)
            {
                if (Convert.ToInt32(item.tanda.Substring(11, item.tanda.Length - 11)) % 2 == 0)
                {
                    item.colorFondo = color1;
                }
                else
                {
                    item.colorFondo = color2;
                }
            }

            lstTandas.Items.Refresh();
        }
        #endregion
        #region controlesListViewOrdenTandas
        private void btnSubirTanda_Click(object sender, RoutedEventArgs e)
        {
            int indiceSeleccionado = lstOrden.SelectedIndex;
            if (indiceSeleccionado > -1)
            {
                tarjetaKanban itemSeleccionado = (tarjetaKanban)lstOrden.SelectedItem;

                if (indiceSeleccionado - 1 > -1)
                {
                    ordenTandasResultante.Remove(itemSeleccionado);
                    ordenTandasResultante.Insert(indiceSeleccionado - 1, itemSeleccionado);
                    int orden = 0;
                    foreach (tarjetaKanban item in ordenTandasResultante)
                    {
                        orden = orden + 1;
                        item.ordenPrioridad = orden;
                    }
                    lstOrden.Items.Refresh();
                }
            }
        }
        private void btnBajarTanda_Click(object sender, RoutedEventArgs e)
        {
            int indiceSeleccionado = lstOrden.SelectedIndex;
            if (indiceSeleccionado > -1)
            {
                tarjetaKanban itemSeleccionado = (tarjetaKanban)lstOrden.SelectedItem;

                if (indiceSeleccionado + 1 < lstOrden.Items.Count)
                {
                    ordenTandasResultante.Remove(itemSeleccionado);
                    ordenTandasResultante.Insert(indiceSeleccionado + 1, itemSeleccionado);

                    int orden= 0;
                    foreach(tarjetaKanban item in ordenTandasResultante)
                    {
                        orden = orden + 1;
                        item.ordenPrioridad = orden;
                    }
                    lstOrden.Items.Refresh();
                }
            }
        }
        #endregion

        private void btnNavegarNoClasificados_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new partNumbers());
        }


    }
}
