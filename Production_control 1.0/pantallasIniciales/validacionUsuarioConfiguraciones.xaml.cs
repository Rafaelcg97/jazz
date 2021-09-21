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

namespace Production_control_1._0.pantallasIniciales
{
    public partial class validacionUsuarioConfiguraciones : UserControl
    {
        public validacionUsuarioConfiguraciones()
        {
            InitializeComponent();
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
        private void ButtonIngresarContrasena_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select cargo from usuarios where contrasena='"+passwordBoxContrasenaAdministrador.Password+"'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            if (dr.Read())
            {
                if (dr["cargo"].ToString()=="ADMINISTRADOR1")
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new configuracion("ADMINISTRADOR1"));
                }
                else if(dr["cargo"].ToString() == "ADMINISTRADOR2")
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new configuracion("ADMINISTRADOR2"));
                }
                else if(dr["cargo"].ToString() == "ADMINISTRADORGENERAL")
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new configuracion("ADMINISTRADORGENERAL"));
                }
                else if (dr["cargo"].ToString() == "ADMIN_KANBAN")
                {
                    Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new configuracion("ADMIN_KANBAN"));
                }
                else
                {
                    MessageBox.Show("Su usuario no tiene autorizacion para acceder a las configuraciones");
                }
            }
            else
            {
                MessageBox.Show("El usuario ingresado no existe");
            };
            dr.Close();
            cn.Close();
        }
    }
}
