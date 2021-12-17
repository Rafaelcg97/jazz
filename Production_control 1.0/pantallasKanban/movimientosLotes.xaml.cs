using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SQLConnection;
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

namespace JazzCCO._0.pantallasKanban
{
    public partial class movimientosLotes : Page
    {
        List<string> listaCompletaDeLotes = new List<string>();
        List<string> listaCompletaDeUbicaciones = new List<string>(); 
        public movimientosLotes(int codigoResponsable)
        {
            InitializeComponent();
            lblResponsable.Content = codigoResponsable.ToString();
            lblFecha.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            cmbxTipoMovimiento.Items.Add("Entrada");
            cmbxTipoMovimiento.Items.Add("Salida");

            using (SqlConnection cn = ConexionTexopsServer.Kanban())
            {
                try
                {
                    string sql = "SELECT lote FROM lotes";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        listaCompletaDeLotes.Add(dr["lote"].ToString());
                    }
                    dr.Close();

                    sql = "SELECT nombreUbicacion FROM ubicacionesKanban";
                    cm = new SqlCommand(sql, cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        listaCompletaDeUbicaciones.Add(dr["nombreUbicacion"].ToString());
                    }
                    dr.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            ltbxLotes.ItemsSource = listaCompletaDeLotes;
            ltbxUbicaciones.ItemsSource = listaCompletaDeUbicaciones;
        }
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.White;
            // navegarInicioKanBan
            PagePrincipal pagePrincipal = new PagePrincipal();
            Grid gridInicio = (Grid)pagePrincipal.Content;
            foreach (object objeto in gridInicio.Children)
            {
                if (objeto.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)objeto;
                    if (grid.Name == "gridListaAreas")
                    {
                        foreach (object objeto2 in grid.Children)
                        {
                            if (objeto2.GetType() == typeof(ListView))
                            {
                                ListView listviewMenu = (ListView)objeto2;
                                listviewMenu.SelectedIndex = 5;
                            }
                        }
                    }
                }
            }
            NavigationService.Navigate(pagePrincipal);
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
        private void ltbxLotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ltbxLotes.SelectedIndex > -1)
            {
                cmbxMaterial.SelectedIndex = -1;
                cmbxTipoMovimiento.SelectedIndex = -1;
                ltbxUbicaciones.SelectedIndex = -1;
                nudCantidadPaquetes.Value = 0;
                cmbxMaterial.Items.Clear();
                using (SqlConnection cn = ConexionTexopsServer.Kanban())
                {
                    try
                    {
                        cmbxMaterial.SelectedIndex = -1;
                        string sql = "SELECT PartNumber FROM componentesPorLote WHERE (SubCategoryName='Hangers' OR SubCategoryName='Boxes') AND lote='"+ ltbxLotes.SelectedItem.ToString() +"'";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            cmbxMaterial.Items.Add(dr["PartNumber"].ToString());
                        }
                        dr.Close();
                        cmbxMaterial.Items.Add("Accesorios");
                        cmbxMaterial.Items.Add("Binding");
                        cmbxMaterial.Items.Add("Copas");
                        cmbxMaterial.Items.Add("Drawcord/Wraphia");
                        cmbxMaterial.Items.Add("Elastico");
                        cmbxMaterial.Items.Add("ElasticoSecundario");
                        cmbxMaterial.Items.Add("Hilos");
                        cmbxMaterial.Items.Add("HilosSmed");
                        cmbxMaterial.Items.Add("PiezasCortadas");
                        cmbxMaterial.Items.Add("PiezasCortadasSmed");
                        cmbxMaterial.Items.Add("TelaSmed");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(ltbxLotes.SelectedIndex>-1 && ltbxUbicaciones.SelectedIndex > -1 && cmbxTipoMovimiento.SelectedIndex>-1 && cmbxMaterial.SelectedIndex > -1 && nudCantidadPaquetes.Value>0)
            {
                using (SqlConnection cn = ConexionTexopsServer.Kanban())
                {
                    try
                    {
                        int tipoMovimiento = 1;
                        if (cmbxTipoMovimiento.SelectedItem.ToString() == "Salida")
                        {
                            tipoMovimiento = -1;
                        }

                        string sql = "INSERT INTO movimientosKanban(tipoMovimiento, lote, material, paquetes, responsable, fechaMovimiento, ubicacion) " +
                            "VALUES('" + tipoMovimiento + "', '" + ltbxLotes.SelectedItem.ToString() + "', '" + cmbxMaterial.SelectedItem.ToString() + "', '" + nudCantidadPaquetes.Value + "', '" + lblResponsable.Content.ToString() + "', '" + lblFecha.Content.ToString() + "', '" + ltbxUbicaciones.SelectedItem.ToString() + "')";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();


                        //limpiar 
                        lblFecha.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ltbxUbicaciones.SelectedIndex = -1;
                        cmbxMaterial.SelectedIndex = -1;
                        cmbxTipoMovimiento.SelectedIndex = -1;
                        nudCantidadPaquetes.Value = 0;

                        MessageBox.Show("Datos ingresados", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Parece que hay datos incompletos", "Aviso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txtBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> listaAuxiliar = new List<string>();
            foreach(string item in listaCompletaDeLotes)
            {
                if (txtBuscarLote.Text == "")
                {
                    listaAuxiliar.Add(item);
                }
                else
                {
                    if (item.ToLower().Contains(txtBuscarLote.Text.ToLower()))
                    {
                        listaAuxiliar.Add(item);
                    }
                }
            }

            ltbxLotes.ItemsSource = listaAuxiliar;
            ltbxLotes.Items.Refresh();
        }
        private void txtBuscarUbicacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> listaAuxiliar = new List<string>();
            foreach (string item in listaCompletaDeUbicaciones)
            {
                if (txtBuscarUbicacion.Text == "")
                {
                    listaAuxiliar.Add(item);
                }
                else
                {
                    if (item.ToLower().Contains(txtBuscarUbicacion.Text.ToLower()))
                    {
                        listaAuxiliar.Add(item);
                    }
                }
            }

            ltbxUbicaciones.ItemsSource = listaAuxiliar;
            ltbxUbicaciones.Items.Refresh();
        }
    }
}
