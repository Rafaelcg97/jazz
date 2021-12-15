using SQLConnection;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JazzCCO._0.pantallasKanban
{
    public partial class validacionUsuarioKanban : UserControl
    {
        public validacionUsuarioKanban()
        {
            InitializeComponent();
        }
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
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection cn = ConexionTexopsServer.Ingenieria())
            {
                try
                {
                    string sql = "validarUsuarioKanbanL1 '" + Properties.Settings.Default.desencriptarContrasenia + "', '" + pboxContrasenia.Password.ToString() +"'";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    if(dr.Read())
                    {
                        int codigoResponsable = Convert.ToInt32(dr[0]);
                        movimientosLotes movimientosLotes = new movimientosLotes(codigoResponsable);
                        Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
                        GridPrincipal.Content = movimientosLotes;
                    }
                    else
                    {
                        MessageBox.Show("Parece que no tienes permiso de entrar a esta area", "Atencion", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
