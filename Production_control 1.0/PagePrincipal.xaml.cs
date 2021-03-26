using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Input;

namespace Production_control_1._0
{

    public partial class PagePrincipal : Page
    {
        #region datos_iniciales
        public PagePrincipal()
        {
            InitializeComponent();
        }
        #endregion

        #region calculos_generales
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.produccion());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.mantenimiento());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    break;
                case 5:
                    passwordBoxContrasenaAdministrador.Password = "";
                    GridPrincipal.Children.Clear();
                    popUpConfiguraciones.IsOpen = true;
                    break;
                default:
                    break;
            }
        }
        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        #endregion

        #region control_general_del programa
        private void ButtonSalir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonMaximizar(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            };

        }
        private void ButtonMinimizar(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }
        #endregion

        #region pop_uo_administrador_a

        private void ButtonCerrarPopup_Click(object sender, RoutedEventArgs e)
        {
            popUpConfiguraciones.IsOpen = false;
        }

        private void ButtonIngresarContrasena_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxContrasenaAdministrador.Password == ConfigurationManager.AppSettings["administrador"])
            {
                popUpConfiguraciones.IsOpen = false;
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new pantallasIniciales.configuracion());
            }
            else
            {
                MessageBox.Show("Contraseña Incorrecta");
                passwordBoxContrasenaAdministrador.Password = "";
                popUpConfiguraciones.IsOpen = true;
            }

        }

        #endregion

    }
}
