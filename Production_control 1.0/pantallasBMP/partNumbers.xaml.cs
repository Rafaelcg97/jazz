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

namespace Production_control_1._0.pantallasBMP
{
    public partial class partNumbers : Page
    {
        List<partNumber> categoriasPartNumber = new List<partNumber>();
        List<partNumber> PartNumbers = new List<partNumber>();
        public partNumbers()
        {
            InitializeComponent();

            cargarCategorias();
            cargarPartNumbersFaltantes();
        }

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
                        subPaquete = Convert.ToInt32(dr["subPaquete"]),
                        tiempoUnitario = Convert.ToDouble(dr["tiempoUnitario"]),
                        tiempoPaquete = Convert.ToDouble(dr["tiempoPaquete"]),
                        tiempoComplementario = Convert.ToDouble(dr["tiempoComplementario"]),
                        macroCategoria = dr["macroCategoria"].ToString()
                    });
                };
                dr.Close();
                cn.Close();

                lstvCategorias.ItemsSource = categoriasPartNumber;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void cargarPartNumbersFaltantes()
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_bmp"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "SELECT DISTINCT partNumber FROM samPartNumbers WHERE categoria IS NULL";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    PartNumbers.Add(new partNumber
                    {
                        partNumberNombre = dr["partNumber"].ToString()
                    });
                };
                dr.Close();
                cn.Close();

                lstvPartNumbers.ItemsSource = PartNumbers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBuscarCategoria_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<partNumber> categoriasBuscadas = new List<partNumber>();
            string letrasIntroducidas = "";
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
                    if (item.categoria.Contains(letrasIntroducidas))
                    {
                        categoriasBuscadas.Add(item);
                    }
                }
            }

            lstvCategorias.ItemsSource = categoriasBuscadas;
            lstvCategorias.Items.Refresh();
        }
    }
}
