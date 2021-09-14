using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Production_control_1._0.pantallasMantenimiento
{
    public partial class reporteFinalTPM : Page
    {
        #region varibalesConexion
        public SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        int mecanico_ = 0;
        int id_ = 0;
        public reporteFinalTPM(int id, int mecanico)
        {
            InitializeComponent();
            mecanico_ = mecanico;
            id_ = id;
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void letra_pop_cerrar(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.White;
            this.NavigationService.GoBack();
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
        #endregion
        #region calculosGenerales

        private void Checkear(object sender, RoutedEventArgs e)
        {
            ToggleButton boton = sender as ToggleButton;
            StackPanel contenedor = (StackPanel)boton.Parent;

            foreach (ToggleButton pareja in contenedor.Children)
            {
                 if (pareja.Name != boton.Name)
                    {
                        pareja.IsChecked = false;
                    }                 
            }
        }

        private void Uncheckear(object sender, RoutedEventArgs e)
        {
            ToggleButton boton = sender as ToggleButton;
            StackPanel contenedor = (StackPanel)boton.Parent;

            foreach (ToggleButton pareja in contenedor.Children)
            {
                if (pareja.Name != boton.Name)
                {
                    pareja.IsChecked = true;
                }
            }
        }

        #endregion
        #region habilitarFormulario
        private void checkedCompleto_Checked(object sender, RoutedEventArgs e)
        {
            BorderFormularioCompleto.IsEnabled = true;
        }
        private void checkedCompleto_Unchecked(object sender, RoutedEventArgs e)
        {
            BorderFormularioCompleto.IsEnabled = false;
            s23.IsChecked = false;
            s24.IsChecked = false;
            s25.IsChecked = false;
            s26.IsChecked = false;
            s27.IsChecked = false;
            s28.IsChecked = false;
            s29.IsChecked = false;
            s30.IsChecked = false;
            s31.IsChecked = false;
            s32.IsChecked = false;
            s33.IsChecked = false;
            s34.IsChecked = false;
            s35.IsChecked = false;
            s36.IsChecked = false;
            s37.IsChecked = false;
            s38.IsChecked = false;
        }
        private void BorderFormularioCompleto_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Border borde = (Border)sender;
            if (borde.IsEnabled == true)
            {
                borde.Opacity = 1;
            }
            else
            {
                borde.Opacity = 0.2;
            }
        }
        #endregion
        #region terminarSolicitud
        private void buttonIngresar_Click(object sender, RoutedEventArgs e)
        {
            #region variables
            int p1 = 0;
            int p2 = 0;
            int p3 = 0;
            int p4 = 0;
            int p5 = 0;
            int p6 = 0;
            int p7 = 0;
            int p8 = 0;
            int p9 = 0;
            int p10 = 0;
            int p11 = 0;
            int p12 = 0;
            int p13 = 0;
            int p14 = 0;
            int p15 = 0;
            int p16 = 0;
            int p17 = 0;
            int p18 = 0;
            int p19 = 0;
            int p20 = 0;
            int p21 = 0;
            int p22 = 0;
            int p23 = 0;
            int p24 = 0;
            int p25 = 0;
            int p26 = 0;
            int p27 = 0;
            int p28 = 0;
            int p29 = 0;
            int p30 = 0;
            int p31 = 0;
            int p32 = 0;
            int p33 = 0;
            int p34 = 0;
            int p35 = 0;
            int p36 = 0;
            int p37 = 0;
            int p38 = 0;
            string tipo = "";
            string observaciones_="";

            #endregion

            if (s1.IsChecked == true) { p1 = 1; } else { p1 = 0; }
            if (s2.IsChecked == true) { p2 = 1; } else { p2 = 0; }
            if (s3.IsChecked == true) { p3 = 1; } else { p3 = 0; }
            if (s4.IsChecked == true) { p4 = 1; } else { p4 = 0; }
            if (s5.IsChecked == true) { p5 = 1; } else { p5 = 0; }
            if (s6.IsChecked == true) { p6 = 1; } else { p6 = 0; }
            if (s7.IsChecked == true) { p7 = 1; } else { p7 = 0; }
            if (s8.IsChecked == true) { p8 = 1; } else { p8 = 0; }
            if (s9.IsChecked == true) { p9 = 1; } else { p9 = 0; }
            if (s10.IsChecked == true) { p10 = 1; } else { p10 = 0; }
            if (s11.IsChecked == true) { p11 = 1; } else { p11 = 0; }
            if (s12.IsChecked == true) { p12 = 1; } else { p12 = 0; }
            if (s13.IsChecked == true) { p13 = 1; } else { p13 = 0; }
            if (s14.IsChecked == true) { p14 = 1; } else { p14 = 0; }
            if (s15.IsChecked == true) { p15 = 1; } else { p15 = 0; }
            if (s16.IsChecked == true) { p16 = 1; } else { p16 = 0; }
            if (s17.IsChecked == true) { p17 = 1; } else { p17 = 0; }
            if (s18.IsChecked == true) { p18 = 1; } else { p18 = 0; }
            if (s19.IsChecked == true) { p19 = 1; } else { p19 = 0; }
            if (s20.IsChecked == true) { p20 = 1; } else { p20 = 0; }
            if (s21.IsChecked == true) { p21 = 1; } else { p21 = 0; }
            if (s22.IsChecked == true) { p22 = 1; } else { p22 = 0; }
            if (s23.IsChecked == true) { p23 = 1; } else { p23 = 0; }
            if (s24.IsChecked == true) { p24 = 1; } else { p24 = 0; }
            if (s25.IsChecked == true) { p25 = 1; } else { p25 = 0; }
            if (s26.IsChecked == true) { p26 = 1; } else { p26 = 0; }
            if (s27.IsChecked == true) { p27 = 1; } else { p27 = 0; }
            if (s28.IsChecked == true) { p28 = 1; } else { p28 = 0; }
            if (s29.IsChecked == true) { p29 = 1; } else { p29 = 0; }
            if (s30.IsChecked == true) { p30 = 1; } else { p30 = 0; }
            if (s31.IsChecked == true) { p31 = 1; } else { p31 = 0; }
            if (s32.IsChecked == true) { p32 = 1; } else { p32 = 0; }
            if (s33.IsChecked == true) { p33 = 1; } else { p33 = 0; }
            if (s34.IsChecked == true) { p34 = 1; } else { p34 = 0; }
            if (s35.IsChecked == true) { p35 = 1; } else { p35 = 0; }
            if (s36.IsChecked == true) { p36 = 1; } else { p36 = 0; }
            if (s37.IsChecked == true) { p37 = 1; } else { p37 = 0; }
            if (s38.IsChecked == true) { p38 = 1; } else { p38 = 0; }
            if(checkedCompleto.IsChecked == true) { tipo = "COMPLETO"; } else { tipo = "BASICO"; }
            observaciones_ = observaciones.Text.Replace("'", "");
            string sql = "insert into solicitudesTPMDetalles(idSolicitud, tipo, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32, p33, p34, p35, p36, p37, p38, observaciones) ";
            sql = sql + "values( '" + id_ + "', '" + tipo +"', " + p1 + ", " + p2 + ", " + p3 + ", " + p4 + ", " + p5 + ", " + p6 + ", " + p7 + ", " + p8 + ", " + p9 + ", " + p10 + ", " + p11 + ", " + p12 + ", " + p13 + ", " + p14 + ", " + p15 + ", " + p16 + ", " + p17 + ", " + p18 + ", " + p19 + ", " + p20 + ", " + p21 + ", " + p22 + ", " + p23 + ", " + p24 + ", " + p25 + ", " + p26 + ", " + p27 + ", " + p28 + ", " + p29 + ", " + p30 + ", " + p31 + ", " + p32 + ", " + p33 + ", " + p34 + ", " + p35 + ", " + p36 + ", " + p37 + ", " + p38 + ", '" + observaciones_ + "')";
            string sql2 = "update solicitudesTPM set fechaFin = '" + DateTime.Now + "' where id= '" + id_ + "'";
            string sql3 = "insert into tiemposPorMecanicoTPM (num_solicitud, mecanico, hora, tipo) values( '" + id_ + "', '" + mecanico_ + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            cm = new SqlCommand(sql2, cnMantenimiento);
            cm.ExecuteNonQuery();
            cm = new SqlCommand(sql3, cnMantenimiento);
            cm.ExecuteNonQuery();
            cnMantenimiento.Close();

            formularioTPM formularioTPM = new formularioTPM();
            this.NavigationService.Navigate(formularioTPM);
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2F3134");
        }
    }
}
