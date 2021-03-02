﻿using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Configuration;

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for abrir.xaml
    /// </summary>
    public partial class abrir : Page
    {
        #region clases_especiales()
        public class elementos
        {
            public string estilo { get; set; }
            public string temporada { get; set; }
            public decimal sam { get; set; }
            public string modulo { get; set; }
            public int corrida { get; set; }
            public int horas { get; set; }
            public int operarios { get; set; }
            public int ingeniero { get; set; }
            public string version { get; set; }
        }
        #endregion

        #region control_general_programa()
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        #endregion

        #region datos_iniciales()
        public abrir()
        {
            InitializeComponent();
            //se cargan los modulos que tienen guardados layouts

            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modulo from lista_balances group by modulo";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                modulo_.Items.Add(dr["modulo"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();
            cn.Close();
        }

        #endregion

        #region filtros_de_list_box

        private void modulo__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // se limpian los datos cargados en temporada estilo y versiones
            temporada_.Items.Clear();
            estilo_.Items.Clear();
            vers_.Items.Clear();
            // se limipian los datos de las versiones consultadas
            corrida_c.Content = "----";
            horas_c.Content = "----";
            operarios_c.Content = "----";
            eficiencia_c.Content = "----";
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            foto_vista.Source = new BitmapImage(fileUri);

            //se cpnsultan las tempordas que tiene el modulo que se escoge
            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select temporada from lista_balances where modulo= '" + modulo_.SelectedItem.ToString() + "' group by temporada";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                temporada_.Items.Add(dr["temporada"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();
            cn.Close();
        }

        private void temporada__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // se limpian los datos cargados en estilo y versiones
            estilo_.Items.Clear();
            vers_.Items.Clear();
            // se limipian los datos de las versiones consultadas
            corrida_c.Content = "----";
            horas_c.Content = "----";
            operarios_c.Content = "----";
            eficiencia_c.Content = "----";
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            foto_vista.Source = new BitmapImage(fileUri);

            if (temporada_.SelectedIndex>-1 & modulo_.SelectedIndex>-1)
            {
                //se cpnsultan los estilos que tiene el modulo temporada que se escoge
                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select estilo from lista_balances where modulo= '" + modulo_.SelectedItem.ToString() + "' and temporada= '" + temporada_.SelectedItem.ToString() + "' group by estilo";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();

                // se llenan la lista de modulos con los datos de la consulta
                while (dr.Read())
                {
                    estilo_.Items.Add(dr["estilo"].ToString());
                };
                //se termina la conexion a la base
                dr.Close();
                cn.Close();
            }
            else
            {

            }
        }

        private void estilo__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // se limpian los datos cargados de las versiones
            vers_.Items.Clear();
            // se limipian los datos de las versiones consultadas
            corrida_c.Content = "----";
            horas_c.Content = "----";
            operarios_c.Content = "----";
            eficiencia_c.Content = "----";

            if (estilo_.SelectedIndex>-1 & modulo_.SelectedIndex>-1 & temporada_.SelectedIndex>-1)
            {
                //se cpnsultan las versione que tiene el modulo, temporada, estilo que se escoge
                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select version from lista_balances where modulo= '" + modulo_.SelectedItem.ToString() + "' and temporada= '" + temporada_.SelectedItem.ToString() + "' and estilo= '" + estilo_.SelectedItem.ToString() + "' group by version";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();

                // se llenan la lista de modulos con los datos de la consulta
                while (dr.Read())
                {
                    vers_.Items.Add(dr["version"].ToString());
                };
                //se termina la conexion a la base
                dr.Close();
                cn.Close();

                // se carga la imagen desde archivo
                try
                {
                    Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + temporada_.SelectedItem.ToString() + "/" + estilo_.SelectedItem.ToString() + ".jpg");
                    foto_vista.Source = new BitmapImage(fileUri);
                }

                // si no encuentra la imagen del estilo carga la imagen inicial
                catch
                {
                    Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                    foto_vista.Source = new BitmapImage(fileUri);
                }
            }
            else
            {

            }
        }

        private void vers__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // se limipian los datos de las versiones consultadas
            corrida_c.Content = "----";
            horas_c.Content = "----";
            operarios_c.Content = "----";
            eficiencia_c.Content = "----";

            if (estilo_.SelectedIndex > -1 & modulo_.SelectedIndex > -1 & temporada_.SelectedIndex > -1 & vers_.SelectedIndex>-1)
            {
                //se cpnsultan los datos de la version que se escoge
                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_balances"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select corrida, horas, operarios, eficiencia from lista_balances where modulo= '" + modulo_.SelectedItem.ToString() + "' and temporada= '" + temporada_.SelectedItem.ToString() + "' and estilo= '" + estilo_.SelectedItem.ToString() + "' and version= '" + vers_.SelectedItem.ToString() +"'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();

                // se llenan la lista de modulos con los datos de la consulta
                while (dr.Read())
                {
                    corrida_c.Content= dr["corrida"].ToString();
                    horas_c.Content = dr["horas"].ToString();
                    operarios_c.Content = dr["operarios"].ToString();
                    eficiencia_c.Content = dr["eficiencia"].ToString() + "%";
                };
                //se termina la conexion a la base
                dr.Close();
                cn.Close();

            }


        }

        #endregion

        #region abrir_balance

        private void abrir_c_Click(object sender, RoutedEventArgs e)
        {
            if(modulo_.SelectedIndex<0 || estilo_.SelectedIndex<0 || temporada_.SelectedIndex<0 || vers_.SelectedIndex < 0)
            {
                MessageBox.Show("Datos seleccionados incompletos");
            }
            else
            {


            Global.identificador = modulo_.SelectedItem.ToString() + estilo_.SelectedItem.ToString() + temporada_.SelectedItem.ToString() + vers_.SelectedItem.ToString();
            balance balance = new balance();
            NavigationService.Navigate(balance);
            }
        }

        #endregion
    }
}