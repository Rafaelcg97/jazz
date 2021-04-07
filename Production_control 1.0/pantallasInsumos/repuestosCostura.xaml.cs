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
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasInsumos
{
    public partial class repuestosCostura : UserControl
    {
        #region conexionesConBasesSQL
        SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        string sql; //Consulta que se hace en sql
        SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
        SqlDataReader dr; //leer los resultados del comando sql
        #endregion

        #region datosIniciales
        public repuestosCostura(int codigo)
        {
            InitializeComponent();
            #region agregarUsuario
            labelCodigoSolicitante.Content = codigo.ToString();
            #endregion
            #region agregarNombreUsuario
            cnIngenieria.Open();
            sql = "select nombre from usuarios where codigo='"+ 9009 +"'";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            dr.Read();
            labelNombre.Content = dr["nombre"].ToString();
            dr.Close();
            cnIngenieria.Close();
            #endregion
            #region agregarRepuestos
            cnMantenimiento.Open();
            sql = "select top 100 PartNumber, Description, OnHand, Cost from spare_onhand where final_category='Repuestos de Costura'";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            List<solicitudInsumo> listaDeInsumos = new List<solicitudInsumo>();
            //agregar operaciones de consulta
            while (dr.Read())
            {
                string colorDisponible = "DarkGreen";
                if (Convert.ToInt32(dr["OnHand"]) >0)
                {
                    colorDisponible = "DarkGreen";
                }
                else
                {
                    colorDisponible = "Red";
                }
               listaDeInsumos.Add(new solicitudInsumo {partNumber=dr["partNumber"].ToString(), description=dr["Description"].ToString(), onHand=Convert.ToInt32(dr["OnHand"]), cost=Convert.ToDouble(dr["Cost"]), color=colorDisponible });
            };
            dr.Close();
            cnMantenimiento.Close();
            //agregar lista de respuestos a listBoxRepuesto
            listBoxRepuesto.ItemsSource = listaDeInsumos;
            #endregion
            #region agregarMaquinas
            cnMantenimiento.Open();
            sql = "select codigo from inventario_maquinas";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            List<maquina> listaDeMaquinas = new List<maquina>();
            //agregar maquinas existentes
            while (dr.Read())
            {

                listaDeMaquinas.Add(new maquina { codigoMaquina = dr["codigo"].ToString() });
            };
            dr.Close();
            cnMantenimiento.Close();
            //agregar lista de maquinas a listboxmaquinas
            listBoxMaquina.ItemsSource = listaDeMaquinas;
            #endregion
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
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion

        #region botonesControlFormulario

        private void aumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxCantidad.Text)==false)
            {
                textBoxCantidad.Text = (Convert.ToInt32(textBoxCantidad.Text) + 1).ToString();
            }
            else
            {
                textBoxCantidad.Text = "1";
            }
        }

        private void disminuirCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(textBoxCantidad.Text) > 1)
            {
                textBoxCantidad.Text = (Convert.ToInt32(textBoxCantidad.Text) - 1).ToString();
            }
        }

        private void listBoxMaquina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            maquina itemMaquinaSeleccionada = (maquina)listBoxMaquina.SelectedItem;

            if (listBoxMaquina.SelectedIndex > -1)
            {
                if (labelMaquinaSeleccionada.Content.ToString() == "----")
                {
                    labelMaquinaSeleccionada.Content = itemMaquinaSeleccionada.codigoMaquina;
                }
                else
                {
                    MessageBox.Show("Acabas de cambiar la máquina previamente seleccionada");
                    labelMaquinaSeleccionada.Content = itemMaquinaSeleccionada.codigoMaquina;
                }

            }



        }

        private void listViewRepuestosSolicitados_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar el repuesto solicitado al presionar la tecla d
            if (e.Key == Key.D)
            {
                int elementoSeleccionado = listViewRepuestosSolicitados.SelectedIndex + 1;
                List<solicitudInsumo> items = new List<solicitudInsumo>();
                int conteo = 0;
                foreach (solicitudInsumo item in listViewRepuestosSolicitados.Items)
                {
                    conteo = conteo + 1;
                    if (conteo == elementoSeleccionado)
                    {
                    }
                    else
                    {
                        items.Add(new solicitudInsumo { description = item.description, solicitado = item.solicitado, cost = item.cost });
                    }
                }
                listViewRepuestosSolicitados.ItemsSource = items;
            }
        }
        #endregion

        #region FiltrarListasMaquinaRepuestos

        private void TextBoXBuscarMaquina_TextChanged(object sender, TextChangedEventArgs e)
        {
            cnMantenimiento.Open();
            sql = "select codigo from inventario_maquinas where codigo like '%"+ TextBoXBuscarMaquina.Text +"%'";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            List<maquina> listaDeMaquinas = new List<maquina>();
            //agregar maquinas existentes
            while (dr.Read())
            {

                listaDeMaquinas.Add(new maquina { codigoMaquina = dr["codigo"].ToString() });
            };
            dr.Close();
            cnMantenimiento.Close();
            //agregar lista de maquinas a listboxmaquinas
            listBoxMaquina.ItemsSource = listaDeMaquinas;

        }

        private void TextBoXBuscarRepuesto_TextChanged(object sender, TextChangedEventArgs e)
        {
            cnMantenimiento.Open();
            sql = "select top 20 PartNumber, Description, OnHand, Cost from spare_onhand where final_category='Repuestos de Costura' and Description like '%" + TextBoXBuscarRepuesto.Text + "%'";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            List<solicitudInsumo> listaDeInsumos = new List<solicitudInsumo>();
            //agregar operaciones de consulta
            while (dr.Read())
            {
                string colorDisponible = "DarkGreen";
                if (Convert.ToInt32(dr["OnHand"]) > 0)
                {
                    colorDisponible = "DarkGreen";
                }
                else
                {
                    colorDisponible = "Red";
                }
                listaDeInsumos.Add(new solicitudInsumo { partNumber = dr["partNumber"].ToString(), description = dr["Description"].ToString(), onHand = Convert.ToInt32(dr["OnHand"]), cost = Convert.ToDouble(dr["Cost"]), color = colorDisponible });
            };
            dr.Close();
            cnMantenimiento.Close();
            //agregar lista de respuestos a listBoxRepuesto
            listBoxRepuesto.ItemsSource = listaDeInsumos;

        }

        #endregion

        #region agregarRepuestoLista
        private void ButtomIngresarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            //se crea lista consolidad de los repuestos pedidos
            List<solicitudInsumo> repuestosSolicitados = new List<solicitudInsumo>();

            //se obtiene el repuesto que se va a pedir
            solicitudInsumo itemSeleccionado = (solicitudInsumo)listBoxRepuesto.SelectedItem;

            //se revisa si ya existen repuestos agregados
            int conteoItemsAgregados = 0;
            //se revisa si el item era uno ya agregado o es otro
            int conteoItemsIguales = 0;
            foreach (solicitudInsumo item in listViewRepuestosSolicitados.Items)
            {
                int totalSeleccionado=0;
                //se verifica si existen items
                conteoItemsAgregados = conteoItemsAgregados + 1;

                //se verifica si se esta gregando un repuesto agregado anteriormente
                if (itemSeleccionado.description == item.description)
                {
                    conteoItemsIguales = conteoItemsIguales + 1;
                    totalSeleccionado = item.solicitado + Convert.ToInt32(textBoxCantidad.Text);

                    //si ya se ha agregado se suma lo que se habia agregado mas lo que se va a agregar y se revisa si alcanza lo que hay en sistema
                    if (totalSeleccionado > itemSeleccionado.onHand)
                    {
                        MessageBox.Show("No hay suficiente en sistema");
                        repuestosSolicitados.Add(new solicitudInsumo { description = item.description, solicitado = item.solicitado, cost = item.cost });
                    }
                    else
                    {
                        repuestosSolicitados.Add(new solicitudInsumo { description = item.description, solicitado = totalSeleccionado, cost=item.cost });
                    }
                }
                else
                {
                    repuestosSolicitados.Add(new solicitudInsumo { description = item.description, solicitado =item.solicitado, cost=item.cost });
                }
            }
            // si no existe ningun item solo se agrega el item lo que se ha elegido

            if (conteoItemsAgregados == 0)
            {
                if ((itemSeleccionado.onHand - Convert.ToInt32(textBoxCantidad.Text)) > 0)
                {
                    //se agrega a la lista
                    repuestosSolicitados.Add(new solicitudInsumo { description = itemSeleccionado.description, solicitado = Convert.ToInt32(textBoxCantidad.Text), cost = itemSeleccionado.cost });
                }
                else
                {
                    MessageBox.Show("No existe suficiente en sistema");
                }
            }

            //si ya existian items se revisa si el item ya estaba agregado y solo se consolido o hay que agregarlo como uno aparte
            else
            {
                if (conteoItemsIguales == 0)
                {
                    if ((itemSeleccionado.onHand - Convert.ToInt32(textBoxCantidad.Text)) > 0)
                    {
                        //se agrega a la lista
                        repuestosSolicitados.Add(new solicitudInsumo { description = itemSeleccionado.description, solicitado = Convert.ToInt32(textBoxCantidad.Text), cost = itemSeleccionado.cost });
                    }
                    else
                    {
                        MessageBox.Show("No existe suficiente en sistema");
                    }
                }
            }

            listViewRepuestosSolicitados.ItemsSource = repuestosSolicitados;
        }

        #endregion


    }
}
