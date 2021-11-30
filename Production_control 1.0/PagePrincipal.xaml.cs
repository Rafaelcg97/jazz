using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Input;
using Microsoft.Windows.Shell;
using com.sun.javadoc;
using System;
using System.Diagnostics;

namespace JazzCCO._0
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
                    GridPrincipal.Children.Add(new pantallasIniciales.inicio());
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
                    GridPrincipal.Children.Add(new pantallasIniciales.insumos());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.valiarUsuarioIngenieria());
                    break;
                case 5:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.kanban());
                    break;
                case 6:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.validarUsuarioBMP());
                    break;
                case 7:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.calidad());
                    break;
                case 8:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.muestras());
                    Process p = new Process();
                    p.StartInfo.FileName = @"\\FS-DESACOMP\Planificacion Desarrollo\Programacion Master\Archivos de Acceso\Control Desarrollo.accde";
                    p.Start();
                    break;
                case 9:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new pantallasIniciales.validacionUsuarioConfiguraciones());
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
    }

    public class Dummy
    {
        private static void dummy()
        {
            Action<System.Type> noop = _ => { };
            var dummy = typeof(Microsoft.Windows.Shell.JumpItem);
            noop(dummy);
        }
    }
}
