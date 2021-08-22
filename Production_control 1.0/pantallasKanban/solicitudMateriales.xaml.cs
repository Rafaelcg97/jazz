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
using Production_control_1._0.clases;
using Production_control_1._0.pantallasIniciales;

namespace Production_control_1._0.pantallasKanban
{
    public partial class solicitudMateriales : UserControl
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnManto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region listasGlobales
        List<solicitudKanban> listaCompletaLotes = new List<solicitudKanban>();
        List<materialesEstilos> listaDeMaterialesCompleta = new List<materialesEstilos>();
        #endregion
        #region datosIniciales
        public solicitudMateriales()
        {
            InitializeComponent();
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto
        private void letraAjustable1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letraAjustable2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.1 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letraAjustable4(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
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
        private void validarEntregados()
        {
            //limpiar etiquetas de estado de material catalogandolas todas como pendientes
            labelEstadoAccesorios.Content = "Pendiente";
            labelEstadoBinding.Content = "Pendiente";
            labelEstadoHilos.Content = "Pendiente";
            labelEstadoElastico.Content = "Pendiente";
            labelEstadoBra.Content = "Pendiente";
            labelEstadoTela.Content = "Pendiente";
            listViewTallasBra.IsEnabled = true;
            listViewTallasTela.IsEnabled = true;
            listViewCajas.IsEnabled = true;
            listViewGancho.IsEnabled = true;

            //validar si se necesitan accesorios
            if (ItemsControlAccesorios.Items.Count == 0)
            {
                labelEstadoAccesorios.Content = "No necesario";
            }
            //validar si se necesita Binding
            if (ItemsControlBinding.Items.Count == 0)
            {
                labelEstadoBinding.Content = "No necesario";
            }
            //validar si se necesitan elasticos
            if (ItemsControlElastico.Items.Count == 0)
            {
                labelEstadoElastico.Content = "No necesario";
            }
            //validar si se necesitan hilos
            if (ItemsControlHilos.Items.Count == 0)
            {
                labelEstadoHilos.Content = "No necesario";
            }
            //validar si se necesita tela
            if (ItemsControlTela.Items.Count == 0)
            {
                labelEstadoTela.Content = "No necesario";
                listViewTallasTela.IsEnabled = false;
            }
            //validar si se necesitan copas
            if (ItemsControlBra.Items.Count == 0)
            {
                labelEstadoBra.Content = "No necesario";
                listViewTallasBra.IsEnabled = false;
            }
            //validar si se necesitan cajas
            if (listViewCajas.Items.Count == 0)
            {
                labelEstadoCajas.Content = "No necesario"; 
                listViewCajas.IsEnabled = false;
            }
            //validar si se necesitan ganchos
            if (listViewGancho.Items.Count == 0)
            {
                labelEstadoGanchos.Content = "No necesario";
                listViewGancho.IsEnabled = false;
            }

            //cargar si ya se ha entregado
            cnKanban.Open();
            listaDeMaterialesCompleta.Clear();
            string sql = "select material, talla from detalleSolicitudeKanban where lote='" + ((solicitudKanban)listBoxLote.SelectedItem).lote + "'";

            SqlCommand cm = new SqlCommand(sql, cnKanban);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                switch (dr["material"].ToString())
                {
                    case "Accesorios":
                        labelEstadoAccesorios.Content = "Solicitado";
                        break;
                    case "Binding":
                        labelEstadoBinding.Content = "Solicitado";
                        break;
                    case "Hilos":
                        labelEstadoHilos.Content = "Solicitado";
                        break;
                    case "Tela":
                        List<solicitudKanban> listaActualizadaTela = new List<solicitudKanban>();
                        foreach (solicitudKanban talla in listViewTallasTela.Items)
                        {
                            if (talla.talla == dr["talla"].ToString())
                            {
                                listaActualizadaTela.Add(new solicitudKanban { talla = talla.talla, chequeado = true, habilitado = false, cantidad = talla.cantidad });
                            }
                            else
                            {
                                listaActualizadaTela.Add(talla);
                            }
                        }

                        labelEstadoTela.Content = "Solicitado";

                        listViewTallasTela.Items.Clear();
                        foreach (solicitudKanban elemento in listaActualizadaTela)
                        {
                            listViewTallasTela.Items.Add(elemento);
                        }
                        break;
                    case "Copas":
                        List<solicitudKanban> listaActualizadaCopas = new List<solicitudKanban>();
                        foreach (solicitudKanban talla in listViewTallasBra.Items)
                        {
                            if (talla.talla == dr["talla"].ToString())
                            {
                                listaActualizadaCopas.Add(new solicitudKanban { talla = talla.talla, chequeado = true, habilitado = false, cantidad = talla.cantidad });
                            }
                            else
                            {
                                listaActualizadaCopas.Add(talla);
                            }
                        }

                        labelEstadoTela.Content = "Solicitado";

                        listViewTallasBra.Items.Clear();
                        foreach (solicitudKanban elemento in listaActualizadaCopas)
                        {
                            listViewTallasBra.Items.Add(elemento);
                        }
                        break;
                }
            }
            dr.Close();
            cnKanban.Close();

            //validar si ya se encuentran en la lista por solicitar
            foreach(solicitudKanban item in listViewListaMateriales.Items)
            {
                if(item.lote== ((solicitudKanban)listBoxLote.SelectedItem).lote)
                {
                    switch (item.material)
                    {
                        case "Accesorios":
                            labelEstadoAccesorios.Content = "Agregado";
                            break;
                        case "Binding":
                            labelEstadoBinding.Content = "Agregado";
                            break;
                        case "Elastico":
                            labelEstadoElastico.Content = "Agregado";
                            break;
                        case "Hilos":
                            labelEstadoHilos.Content = "Agregado";
                            break;
                        case "Tela":
                            List<solicitudKanban> listaActualizadaTela = new List<solicitudKanban>();
                            foreach (solicitudKanban talla in listViewTallasTela.Items)
                            {
                                if (talla.talla == item.talla)
                                {
                                    listaActualizadaTela.Add(new solicitudKanban { talla = talla.talla, chequeado = true, habilitado = false, cantidad = talla.cantidad });
                                }
                                else
                                {
                                    listaActualizadaTela.Add(talla);
                                }
                            }

                            labelEstadoTela.Content = "Agregado";

                            listViewTallasTela.Items.Clear();
                            foreach (solicitudKanban elemento in listaActualizadaTela)
                            {
                                listViewTallasTela.Items.Add(elemento);
                            }
                            break;
                        case "Copas":
                            List<solicitudKanban> listaActualizadaCopas = new List<solicitudKanban>();
                            foreach (solicitudKanban talla2 in listViewTallasBra.Items)
                            {
                                if (talla2.talla == item.talla)
                                {
                                    listaActualizadaCopas.Add(new solicitudKanban { talla = talla2.talla, chequeado = true, habilitado = false, cantidad = talla2.cantidad });
                                }
                                else
                                {
                                    listaActualizadaCopas.Add(talla2);
                                }
                            }
                            labelEstadoBra.Content = "Agregado";

                            listViewTallasBra.Items.Clear();
                            foreach (solicitudKanban elemento2 in listaActualizadaCopas)
                            {
                                listViewTallasBra.Items.Add(elemento2);
                            }
                            break;
                        default:
                            if (item.material.Contains("BOXES"))
                            {
                                List<solicitudKanban> listaActualizadaCajas = new List<solicitudKanban>();
                                foreach (solicitudKanban subitem in listViewCajas.Items)
                                {
                                    if (item.material == subitem.material)
                                    {
                                        listaActualizadaCajas.Add(new solicitudKanban { material = subitem.material, cantidad = subitem.cantidad, agregado = 0, solicitado = subitem.solicitado, diferencia = subitem.diferencia-item.cantidad});
                                    }
                                    else
                                    {
                                        listaActualizadaCajas.Add(subitem);
                                    }
                                }

                                labelEstadoCajas.Content = "Agregado";

                                listViewCajas.Items.Clear();

                                foreach (solicitudKanban item4 in listaActualizadaCajas)
                                {
                                    listViewCajas.Items.Add(item4);
                                }
                            }
                            else
                            {
                                List<solicitudKanban> listaActualizadaGanchos = new List<solicitudKanban>();
                                foreach (solicitudKanban subitem in listViewGancho.Items)
                                {
                                    if (item.material == subitem.material)
                                    {
                                        listaActualizadaGanchos.Add(new solicitudKanban { material = subitem.material, cantidad = subitem.cantidad, agregado = 0, solicitado = subitem.solicitado, diferencia = subitem.diferencia - item.cantidad });
                                    }
                                    else
                                    {
                                        listaActualizadaGanchos.Add(subitem);
                                    }
                                }

                                labelEstadoGanchos.Content = "Agregado";

                                listViewGancho.Items.Clear();

                                foreach (solicitudKanban item4 in listaActualizadaGanchos)
                                {
                                    listViewGancho.Items.Add(item4);
                                }
                            }
                            break;
                    }
                }
            }
        }
        #endregion
        #region botonesDeAgregarMaterialLista
        private void buttonAgregarAccesorios_Click(object sender, RoutedEventArgs e)
        {
            //se verifica si aun no ha sido entregado
            if (labelEstadoAccesorios.Content.ToString() == "Pendiente")
            {
                listViewListaMateriales.Items.Add(new solicitudKanban
                {
                    lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                    modulo = listBoxModulo.SelectedItem.ToString(),
                    material = "Accesorios",
                    cantidad = 1,
                    talla = "Unica"
                });
            }
            validarEntregados();
        }
        private void buttonAgregarBinding_Click(object sender, RoutedEventArgs e)
        {
            //se verifica si aun no ha sido entregado
            if (labelEstadoBinding.Content.ToString() == "Pendiente")
            {
                listViewListaMateriales.Items.Add(new solicitudKanban
                {
                    lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                    modulo = listBoxModulo.SelectedItem.ToString(),
                    material = "Binding",
                    cantidad = 1,
                    talla = "Unica"
                });
            }
            validarEntregados();
        }
        private void buttonAgregarHilo_Click(object sender, RoutedEventArgs e)
        {
            //se verifica si aun no ha sido entregado
            if (labelEstadoHilos.Content.ToString() == "Pendiente")
            {
                listViewListaMateriales.Items.Add(new solicitudKanban
                {
                    lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                    modulo = listBoxModulo.SelectedItem.ToString(),
                    material = "Hilos",
                    cantidad = 1,
                    talla = "Unica"
                });
            }
            validarEntregados();
        }
        private void buttonAgregarElastico_Click(object sender, RoutedEventArgs e)
        {
            //se verifica si aun no ha sido entregado
            if (labelEstadoElastico.Content.ToString() == "Pendiente")
            {
                listViewListaMateriales.Items.Add(new solicitudKanban
                {
                    lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                    modulo = listBoxModulo.SelectedItem.ToString(),
                    material = "Elastico",
                    cantidad = 1,
                    talla = "Unica"
                });
            }
            validarEntregados();
        }
        private void buttonAgregarTela_Click(object sender, RoutedEventArgs e)
        {
            foreach (solicitudKanban item in listViewTallasTela.Items)
            {
                if (item.habilitado == true && item.chequeado == true)
                {
                    listViewListaMateriales.Items.Add(new solicitudKanban
                    {
                        lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                        modulo = listBoxModulo.SelectedItem.ToString(),
                        material = "Tela",
                        cantidad = item.cantidad,
                        talla = item.talla
                    });
                }
            }
            validarEntregados();
        }
        private void buttonAgregarBra_Click(object sender, RoutedEventArgs e)
        {
            foreach (solicitudKanban item in listViewTallasBra.Items)
            {
                if (item.habilitado == true && item.chequeado == true)
                {
                    listViewListaMateriales.Items.Add(new solicitudKanban
                    {
                        lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                        modulo = listBoxModulo.SelectedItem.ToString(),
                        material = "Copas",
                        cantidad = item.cantidad,
                        talla = item.talla
                    });
                }
            }
            validarEntregados();
        }
        private void buttonAgregarCajas_Click(object sender, RoutedEventArgs e)
        {
            foreach (solicitudKanban item in listViewCajas.Items)
            {
                if (item.agregado > 0)
                {
                    int cantidad = 0;
                    solicitudKanban itemAgregado = new solicitudKanban();
                    foreach (solicitudKanban subitem in listViewListaMateriales.Items)
                    {
                        if (item.material == subitem.material && subitem.lote == ((solicitudKanban)listBoxLote.SelectedItem).lote)
                        {
                            cantidad = cantidad + subitem.cantidad;
                            itemAgregado = subitem;
                        }
                    }

                    listViewListaMateriales.Items.Remove(itemAgregado);
                    listViewListaMateriales.Items.Add(new solicitudKanban
                    {
                        lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                        modulo = listBoxModulo.SelectedItem.ToString(),
                        material = item.material,
                        cantidad = item.agregado + cantidad,
                        talla = "unica",
                    });

                }
            }
            validarEntregados();
        }
        private void buttonAgregarGanchos_Click(object sender, RoutedEventArgs e)
        {
            foreach (solicitudKanban item in listViewGancho.Items)
            {
                if (item.agregado > 0)
                {
                    int cantidad = 0;
                    solicitudKanban itemAgregado = new solicitudKanban();
                    foreach (solicitudKanban subitem in listViewListaMateriales.Items)
                    {
                        if (item.material == subitem.material && subitem.lote == ((solicitudKanban)listBoxLote.SelectedItem).lote)
                        {
                            cantidad = cantidad + subitem.cantidad;
                            itemAgregado = subitem;
                        }
                    }

                    listViewListaMateriales.Items.Remove(itemAgregado);
                    listViewListaMateriales.Items.Add(new solicitudKanban
                    {
                        lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                        modulo = listBoxModulo.SelectedItem.ToString(),
                        material = item.material,
                        cantidad = item.agregado + cantidad,
                        talla = "unica",
                        diferencia = item.diferencia,
                        solicitado =item.cantidad,
                    });

                }
            }
            validarEntregados();
        }
        #endregion
        #region datosGeneralesDeFormulario
        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //mostrar datos general de el lote
                solicitudKanban loteSeleccionado = (solicitudKanban)listBoxLote.SelectedItem;
                labelEstilo.Content = loteSeleccionado.estilo;
                labelTemporada.Content = loteSeleccionado.temporada;
                labelPiezas.Content = loteSeleccionado.cantidad;
                labelColor.Content = loteSeleccionado.color;

                //limpiar datos cargados
                listViewTallasTela.Items.Clear();
                listViewTallasBra.Items.Clear();
                ItemsControlAccesorios.Items.Clear();
                ItemsControlBinding.Items.Clear();
                ItemsControlHilos.Items.Clear();
                ItemsControlBra.Items.Clear();
                ItemsControlElastico.Items.Clear();
                ItemsControlTela.Items.Clear();
                listViewCajas.Items.Clear();
                listViewGancho.Items.Clear();

                //cargar en lista todos los materiales que tiene lote seleccionado escepto cajas y ganchos
                cnKanban.Open();
                listaDeMaterialesCompleta.Clear();
                string sql = "select " +
                    "CategoryName, " +
                    "SubCategoryName, " +
                    "PartNumber, " +
                    "description " +
                    "from  componentesPorLote " +
                    "where manufactureId= '"+loteSeleccionado.manufactureId+"'";

                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    listaDeMaterialesCompleta.Add(new materialesEstilos {categoryName=dr["categoryName"].ToString(), subCategoryName=dr["SubCategoryName"].ToString(), partNumber=dr["partNumber"].ToString(), description=dr["description"].ToString()});
                }
                dr.Close();

                sql = "select " +
                    "a.talla, " +
                    "make, " +
                    "case when cantidad is null then 0 else cantidad end as cantidad " +
                    "from tallasLotes a " +
                    "left join detalleSolicitudeKanban  b " +
                    "on a.lote=b.lote and a.talla=b.talla where a.lote='"+ loteSeleccionado.lote +"'";
                cm = new SqlCommand(sql, cnKanban);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    bool chequeado_ = false;
                    bool habilitado_ = true;
                    if(Convert.ToInt32(dr["make"])== Convert.ToInt32(dr["cantidad"]))
                    {
                        chequeado_ = true;
                        habilitado_ = false;
                    }
                    solicitudKanban item = new solicitudKanban {talla=dr["talla"].ToString(), chequeado=chequeado_, habilitado=habilitado_, cantidad= Convert.ToInt32(dr["make"]) };
                    solicitudKanban item2 = new solicitudKanban { talla = dr["talla"].ToString(), chequeado = chequeado_, habilitado = habilitado_, cantidad = Convert.ToInt32(dr["make"]) };
                    listViewTallasTela.Items.Add(item);
                    listViewTallasBra.Items.Add(item2);
                }
                dr.Close();

                //consultar y agregar cajas, ganchos
                sql = "select " +
                    "a.lote, " +
                    "a.PartNumber, " +
                    "CEILING(a.quantity) as cantidad, " +
                    "case when b.cantidad is null then 0 else b.cantidad end as solicitado  " +
                    "from componentesPorLote a " +
                    "left join detalleSolicitudeKanban b " +
                    "on a.lote=b.lote and a.PartNumber=b.material " +
                    "where (a.SubCategoryName='Boxes' or a.SubCategoryName='Hangers') " +
                    "and a.lote='" + ((solicitudKanban)listBoxLote.SelectedItem).lote + "'";

                cm = new SqlCommand(sql, cnKanban);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    int diferencia_ = Convert.ToInt32(dr["cantidad"]) - Convert.ToInt32(dr["solicitado"]);
                    if (dr["partNumber"].ToString().Contains("BOXES"))
                    {
                        listViewCajas.Items.Add(new solicitudKanban { material = dr["partNumber"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"]), agregado = 0, solicitado = Convert.ToInt32(dr["solicitado"]), diferencia = diferencia_, habilitado = true });
                    }
                    else
                    {
                        listViewGancho.Items.Add(new solicitudKanban { material = dr["partNumber"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"]), agregado = 0, solicitado = Convert.ToInt32(dr["solicitado"]), diferencia = diferencia_, habilitado = true });
                    }
                }
                dr.Close();
                cnKanban.Close();
                //clasificar materiales
                foreach(materialesEstilos item in listaDeMaterialesCompleta)
                {
                    if(item.categoryName== "Thread")
                    {
                        ItemsControlHilos.Items.Add(item.partNumber);
                    }
                    else if (item.categoryName == "Trim" && item.subCategoryName== "Send Out")
                    {
                        ItemsControlBinding.Items.Add(item.partNumber);
                    }
                    else if (item.categoryName == "Trim" && item.subCategoryName == "Elastic" && (item.description.ToLower().Contains("neck")|| item.description.ToLower().Contains("clear") || item.description.ToLower().Contains("rubber") || item.description.ToLower().Contains("gripper")))
                    {
                        ItemsControlElastico.Items.Add(item.partNumber);
                    }
                    else if (item.categoryName == "Trim" && item.subCategoryName == "Bra Cups")
                    {
                        ItemsControlBra.Items.Add(item.partNumber);
                    }
                    else if (item.categoryName == "Fabric")
                    {
                        ItemsControlTela.Items.Add(item.partNumber);
                    }
                    else
                    {
                        ItemsControlAccesorios.Items.Add(item.partNumber);
                    }
                }
                validarEntregados();
            }
        }
        private void textBoxBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            //limpiar lista de lotes
            listBoxLote.Items.Clear();

            //revisar lotes que coinciden con la busqueda
            foreach (solicitudKanban lote in listaCompletaLotes)
            {
                if (!string.IsNullOrEmpty(textBoxBuscarLote.Text))
                {
                    if (lote.lote.StartsWith(textBoxBuscarLote.Text))
                    {
                        listBoxLote.Items.Add(lote);
                    }
                }
                else
                {
                    listBoxLote.Items.Add(lote);
                }
            }

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
                //carga ubicacion de 
                cnManto.Open();
                string sql = "select id from orden_modulos where modulo = '" + listBoxModulo.SelectedItem.ToString() + "'";
                SqlCommand cm = new SqlCommand(sql, cnManto);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    labelUbicacion.Content = dr["id"].ToString();
                }
                dr.Close();
                cnManto.Close();


                cnKanban.Open();
                sql = "select manufactureId, lote, make, estilo, estiloId, temporada, color from lotes where modulo='" + listBoxModulo.SelectedItem.ToString() + "'";
                listBoxLote.Items.Clear();
                listaCompletaLotes.Clear();
                cm = new SqlCommand(sql, cnKanban);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listBoxLote.Items.Add(
                      new solicitudKanban
                      {
                          manufactureId = Convert.ToInt32(dr["manufactureId"]),
                          lote = dr["lote"].ToString(),
                          cantidad = Convert.ToInt32(dr["make"]),
                          estilo = dr["estilo"].ToString(),
                          styleId = Convert.ToInt32(dr["estiloId"]),
                          temporada = dr["temporada"].ToString(),
                          color = dr["color"].ToString()
                      });

                    listaCompletaLotes.Add(
                        new solicitudKanban
                        {
                            manufactureId = Convert.ToInt32(dr["manufactureId"]),
                            lote = dr["lote"].ToString(),
                            cantidad = Convert.ToInt32(dr["make"]),
                            estilo = dr["estilo"].ToString(),
                            styleId = Convert.ToInt32(dr["estiloId"]),
                            temporada = dr["temporada"].ToString(),
                            color = dr["color"].ToString()
                        });
                };
                dr.Close();
                cnKanban.Close();
            }
        }
        private void passwordboxUsuario_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelUsuario.Content = "*";
            buttonEnviarSolicitud.IsEnabled = false;
            listBoxModulo.Items.Clear();
            listBoxLote.Items.Clear();
            #region variablesDeConexionn
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            #region consultarUsuario
            //consultar
            sql = "select modulosProduccion.modulo as modulo, usuarios.codigo as codigo from modulosProduccion left join ingenieria.dbo.usuarios on ";
            sql = sql + "modulosProduccion.ingenieroProcesosCodigo= usuarios.codigo or modulosProduccion.coordinadorCodigo= usuarios.codigo ";
            sql = sql + "where produccion=1 and contrasena='" + passwordboxUsuario.Password + "'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxModulo.Items.Add(dr["modulo"].ToString());
                labelUsuario.Content = dr["codigo"].ToString();
                buttonEnviarSolicitud.IsEnabled = true;
            }
            dr.Close();
            cnProduccion.Close();
            #endregion
        }
        private void buttonEnviarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "insert into solicitudesKanban(tipo, modulo, ubicacion, fechaSolicitud) values('solicitud', '" + listBoxModulo.SelectedItem.ToString() + "', '" + labelUbicacion.Content + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') SELECT SCOPE_IDENTITY()";
            cnKanban.Open();
            SqlCommand cm = new SqlCommand(sql, cnKanban);
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            int idIngreso = Convert.ToInt32(dr[0]);
            dr.Close();

            //ingresar detalles de la orden
            foreach (solicitudKanban item in listViewListaMateriales.Items)
            {
                sql = "insert into detalleSolicitudeKanban(solicitudKanbanId, lote, material, talla, cantidad) values('" + idIngreso + "', '" + item.lote + "', '" + item.material + "', '" + item.talla + "', '" + item.cantidad + "')";
                if (item.material.Contains("HANGER"))
                {
                    sql = "insert into detalleSolicitudeKanban(solicitudKanbanId, lote, material, talla, cantidad, diferencia, requerido) values('" + idIngreso + "', '" + item.lote + "', '" + item.material + "', '" + item.talla + "', '" + item.cantidad + "', '" + (item.diferencia - item.cantidad) + "','" + item.solicitado + "')";
                }
                cm = new SqlCommand(sql, cnKanban);
                cm.ExecuteNonQuery();
            }
            cnKanban.Close();

            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new estadoPlanta();
        }
        #endregion
    }
}
