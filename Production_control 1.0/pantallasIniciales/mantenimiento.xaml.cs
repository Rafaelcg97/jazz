﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Production_control_1._0.pantallasMantenimiento;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class mantenimiento : UserControl
    {
        #region datos_iniciales
        public mantenimiento()
        {
            InitializeComponent();
        }
        #endregion

        #region entrar_a_pantallas
        private void BorderMantenimientoPreventivo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new mantenimientoPreventivo());
        }

        private void BorderReporteMaquinaMala_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new reporteMaquinaMala());
        }

        private void BorderEstadoPlanta_MouseUp(object sender, MouseButtonEventArgs e)
        {
            estadoPlantaProduccion estadoPlantaProduccion = new estadoPlantaProduccion();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = estadoPlantaProduccion;
        }

        private void BorderReporteTPM_MouseUp(object sender, MouseButtonEventArgs e)
        {
            formularioTPM formularioTPM = new formularioTPM();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = formularioTPM;

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

        #endregion


    }
}
