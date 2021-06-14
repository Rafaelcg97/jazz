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

namespace Production_control_1._0.pantallasInsumos
{
    public partial class validacionUsuario : UserControl
    {
        #region datosIniciales
        public validacionUsuario(string areaSolicitada)
        {
            InitializeComponent();

            labelAreaSolicitada.Content = areaSolicitada;
        }
        #endregion
        #region calculos_generals
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
        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.insumos());
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region ingresarAreas
        private void ButtonIngresar_Click(object sender, RoutedEventArgs e)
        {
            //string de area a la que se desea ir
            string areaSolicitadaVerificar = labelAreaSolicitada.Content.ToString();
            //datos para realizar conexion
            SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select nivel from usuarios where bodega=1 and codigo='" + textBoxUsuario.Text + "' and contrasena='" + PasswordBoxContra.Password + "'";
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            SqlDataReader dr = cm.ExecuteReader();
            //se hace un conteo para ver si existe ese usuario con esa contrasena
            int conteoVerificacion = 0;
            int nivelUsuario = 0;
            while (dr.Read())
            {
                conteoVerificacion = conteoVerificacion + 1;
                nivelUsuario = Convert.ToInt32(dr["nivel"]);
            };
            dr.Close();
            cnIngenieria.Close();
            if (conteoVerificacion == 1)
            {
                if (areaSolicitadaVerificar== "Repuestos Costura")
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new repuestosCostura(Convert.ToInt32(textBoxUsuario.Text), labelAreaSolicitada.Content.ToString()));
                }
                else if(areaSolicitadaVerificar== "Administración Bodega de Insumos" && nivelUsuario == 1)
                {
                    estadoSolicitudesInsumos estadoSolicitudesInsumos = new estadoSolicitudesInsumos(Convert.ToInt32(textBoxUsuario.Text));
                    Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
                    GridPrincipal.Content = estadoSolicitudesInsumos;
                }
                else if(areaSolicitadaVerificar == "Administración Bodega de Insumos" && nivelUsuario < 1)
                {
                    MessageBox.Show("No tiene permiso de acceder a esta area");
                }
                else
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new insumosSolicitar(Convert.ToInt32(textBoxUsuario.Text), labelAreaSolicitada.Content.ToString()));
                }

            }
            else
            {
                MessageBox.Show("La contrasña es incorrecta o el usuario no tiene permiso para hacer solicitudes a bodega");
            }

        }
        #endregion
    }
}
