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
using Xceed.Wpf.Toolkit;

namespace Production_control_1._0.pantallasBMP
{
    public partial class partNumbers : Page
    {
        #region listasGlobales
        List<partNumber> categoriasPartNumber = new List<partNumber>();
        List<partNumber> PartNumbers = new List<partNumber>();
        List<partNumber> PartNumbersAgregados = new List<partNumber>();
        List<string> categorias = new List<string>();
        #endregion
        #region datosIniciales
        public partNumbers()
        {
            InitializeComponent();

            cargarCategorias();
            cargarPartNumbersFaltantes();
            cargarPartNumbersExistentes();
        }
        #endregion
        #region calculosGenerales
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pantallasBMP.menuBMP());
        }
        private void soloNumerosDecimales(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Decimal))
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region eventosCategorias
        private void cargarCategorias()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT categoria, subPaquete, tiempoUnitario, tiempoPaquete, tiempoComplementario, macroCategoria  FROM categoriasPartNumber";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    categoriasPartNumber.Add(new partNumber
                    {
                        categoria = dr["categoria"].ToString(),
                        categoriaOriginal = dr["categoria"].ToString(),
                        subPaquete = Convert.ToInt32(dr["subPaquete"]),
                        subPaqueteOriginal= Convert.ToInt32(dr["subPaquete"]),
                        tiempoUnitario = Convert.ToDouble(dr["tiempoUnitario"]),
                        tiempoPaquete = Convert.ToDouble(dr["tiempoPaquete"]),
                        tiempoComplementario = Convert.ToDouble(dr["tiempoComplementario"]),
                        macroCategoria = dr["macroCategoria"].ToString(),
                        modificado = false
                    });
                };
                dr.Close();
                cn.Close();

                lstvCategorias.ItemsSource = categoriasPartNumber;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void txtBuscarCategoria_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<partNumber> categoriasBuscadas = new List<partNumber>();
            string letrasIntroducidas = ((TextBox)sender).Text;
            if (letrasIntroducidas == "")
            {
                foreach (partNumber item in categoriasPartNumber)
                {
                    categoriasBuscadas.Add(item);
                }
            }
            else
            {
                foreach (partNumber item in categoriasPartNumber)
                {
                    if (item.categoria.ToLower().Contains(letrasIntroducidas.ToLower()))
                    {
                        categoriasBuscadas.Add(item);
                    }
                }
            }

            lstvCategorias.ItemsSource = categoriasBuscadas;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            lstvCategorias.SelectedItem = textBox.DataContext;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            partNumber itemSeleccionado = (partNumber)(lstvCategorias.SelectedItem);
            itemSeleccionado.modificado = true;
        }
        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            lstvCategorias.SelectedItem = comboBox.DataContext;
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            partNumber itemSeleccionado = (partNumber)(lstvCategorias.SelectedItem);
            itemSeleccionado.modificado = true;
        }
        private void btnAgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            categoriasPartNumber.Add(new partNumber
            {
                categoria = "",
                categoriaOriginal = "",
                subPaquete = 1,
                subPaqueteOriginal = 1,
                tiempoUnitario = 0,
                tiempoPaquete = 0,
                tiempoComplementario = 0,
                macroCategoria = "",
                modificado = true
            });

            lstvCategorias.Items.Refresh();
        }
        private void btnGuardarCategoria_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            foreach (partNumber item in categoriasPartNumber)
            {
                if (item.modificado == true)
                {
                    if (item.categoriaOriginal == "")
                    {
                        string sql = "INSERT INTO categoriasPartNumber VALUES('" + item.categoria + "', '" + item.subPaquete + "', '" + item.tiempoUnitario + "', '" + item.tiempoPaquete + "', '" + item.tiempoComplementario + "', '" + item.macroCategoria + "')";
                        try
                        {
                            cn.Open();
                            SqlCommand cm = new SqlCommand(sql, cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            item.categoriaOriginal = item.categoria;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        string sql = "UPDATE categoriasPartNumber set categoria='" + item.categoria + "', subpaquete='" + item.subPaquete + "', tiempoUnitario='" + item.tiempoUnitario + "', tiempoPaquete='" + item.tiempoPaquete + "', tiempoComplementario='" + item.tiempoComplementario + "', macroCategoria='" + item.macroCategoria + "' WHERE categoria='" + item.categoriaOriginal + "' and subPaquete='" + item.subPaqueteOriginal + "'";
                        try
                        {
                            cn.Open();
                            SqlCommand cm = new SqlCommand(sql, cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            item.categoriaOriginal = item.categoria;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            categorias = crearListaCategorias();

            foreach(partNumber item in PartNumbers)
            {
                item.categorias = categorias;
            }

            foreach(partNumber item in PartNumbersAgregados)
            {
                item.categorias = categorias;
            }

            lstvPartNumbers.Items.Refresh();
            lstvPartNumbersAgregados.Items.Refresh();


            System.Windows.MessageBox.Show("Categorias Actualizadas");

        }
        private List<string> crearListaCategorias()
        {
            List<string> categorias = new List<string>();
            categorias = categoriasPartNumber.Select(m => m.categoria).Distinct().ToList();
            return categorias;
        }
        #endregion
        #region EventosPartNumberFaltantes
        private void cargarPartNumbersFaltantes()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT DISTINCT partNumber FROM samPartNumbers WHERE categoria IS NULL";
            try
            {
                PartNumbers.Clear();
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                categorias = crearListaCategorias();
                while (dr.Read())
                {
                    PartNumbers.Add(new partNumber
                    {
                        partNumberNombre = dr["partNumber"].ToString(),
                        categorias = categorias
                    });
                };
                dr.Close();
                cn.Close();

                lstvPartNumbers.ItemsSource = PartNumbers;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void btnAgregarPartNumber_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            foreach (partNumber item in PartNumbers)
            {
                if(item.paquete>0 && !string.IsNullOrEmpty(item.categoria) && item.subPaquete>0)
                {
                    string sql = "INSERT INTO partNumbers VALUES('" + item.partNumberNombre + "', '" + item.categoria + "', '" + item.paquete + "', '" + item.subPaquete + "')";
                    try
                    {
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }
                }
            }

            txtBuscarPartNumber.Clear();
            cargarPartNumbersFaltantes();

            System.Windows.MessageBox.Show("PartNumbers agregados");
        }
        #endregion
        #region EventosPartNumberExistentes
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            cargarPartNumbersExistentes();
        }
        private void txtBuscarPartNumberAgregado_TextChanged(object sender, TextChangedEventArgs e)
        {
            indicador.Fill = Brushes.Red;
        }
        private void cargarPartNumbersExistentes()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            if (string.IsNullOrEmpty(txtBuscarPartNumberAgregado.Text))
            {
                try
                {
                    string sql = "SELECT TOP 200 partNumber, categoria, paquete, subpaquete FROM partNumbers";
                    PartNumbersAgregados.Clear();
                    cn.Open();
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        PartNumbersAgregados.Add(new partNumber
                        {
                            partNumberNombre = dr["partNumber"].ToString(),
                            categoria = dr["categoria"].ToString(),
                            subPaquete = Convert.ToInt32(dr["subPaquete"]),
                            paquete = Convert.ToInt32(dr["paquete"]),
                            categorias = crearListaCategorias(),
                            modificado=false
                        });
                    };
                    dr.Close();
                    cn.Close();

                    lstvPartNumbersAgregados.ItemsSource = PartNumbersAgregados;
                    lstvPartNumbersAgregados.Items.Refresh();
                    indicador.Fill = Brushes.Green;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    string sql = "SELECT TOP 200 partNumber, categoria, paquete, subpaquete FROM partNumbers where partNumber like '%" + txtBuscarPartNumberAgregado.Text + "%'";
                    PartNumbersAgregados.Clear();
                    cn.Open();
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        PartNumbersAgregados.Add(new partNumber
                        {
                            partNumberNombre = dr["partNumber"].ToString(),
                            categoria = dr["categoria"].ToString(),
                            subPaquete = Convert.ToInt32(dr["subPaquete"]),
                            paquete = Convert.ToInt32(dr["paquete"]),
                            categorias = crearListaCategorias(),
                            modificado=false,
                        });
                    };
                    dr.Close();
                    cn.Close();

                    lstvPartNumbersAgregados.ItemsSource = PartNumbersAgregados;
                    lstvPartNumbersAgregados.Items.Refresh();
                    indicador.Fill = Brushes.Green;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }
        private void txtBuscarPartNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<partNumber> coincidencias = new List<partNumber>();
            string letrasIntroducidas = ((TextBox)sender).Text;
            if (letrasIntroducidas == "")
            {
                foreach (partNumber item in PartNumbers)
                {
                    coincidencias.Add(item);
                }
            }
            else
            {
                foreach (partNumber item in PartNumbers)
                {
                    if (item.partNumberNombre.ToLower().Contains(letrasIntroducidas.ToLower()))
                    {
                        coincidencias.Add(item);
                    }
                }
            }

            lstvPartNumbers.ItemsSource = coincidencias;
        }
        private void ComboBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            lstvPartNumbersAgregados.SelectedItem = comboBox.DataContext;
        }
        private void IntegerUpDown_GotFocus(object sender, RoutedEventArgs e)
        {
            IntegerUpDown integerUpDown = (IntegerUpDown)sender;
            lstvPartNumbersAgregados.SelectedItem = integerUpDown.DataContext;
        }
        private void btnGuardarPartNumbers_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            foreach (partNumber item in lstvPartNumbersAgregados.Items)
            {
                if (item.modificado == true)
                {
                    string sql = "UPDATE partNumbers set categoria='" + item.categoria + "', subpaquete='" + item.subPaquete + "', paquete='" + item.paquete + "' WHERE partNumber='" + item.partNumberNombre + "'";
                    try
                    {
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        System.Windows.MessageBox.Show("PartNumbers Guardados");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void ComboBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            partNumber itemSeleccionado = (partNumber)(lstvPartNumbersAgregados.SelectedItem);
            itemSeleccionado.modificado = true;
        }
        private void IntegerUpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            partNumber itemSeleccionado = (partNumber)(lstvPartNumbersAgregados.SelectedItem);
            itemSeleccionado.modificado = true;
        }
        #endregion
    }
}
