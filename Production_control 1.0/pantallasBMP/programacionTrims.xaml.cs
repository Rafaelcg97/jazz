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
            string sql = "SELECT DISTINCT kanban FROM lotesNoProgramados";
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
    }
}
