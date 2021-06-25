using Production_control_1._0.clases;
using System;
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

namespace Production_control_1._0.pantallasIniciales
{
    public partial class produccionHora : UserControl
    {
        public produccionHora(List<elemento_grafica> lista )
        {
            InitializeComponent();
            int h1 = 0;
            int h2 = 0;
            int h3 = 0;
            int h4 = 0;
            int h5 = 0;
            int h6 = 0;
            int h7 = 0;
            int h8 = 0;
            int h9 = 0;
            int h10 = 0;
            int h11 = 0;
            int h12 = 0;
            int piezas = 0;
            listViewProduccionHora.ItemsSource = lista;
            foreach (elemento_grafica item in lista)
            {
                h1 = h1 + item.h1;
                h2 = h2 + item.h2;
                h3 = h3 + item.h3;
                h4 = h4 + item.h4;
                h5 = h5 + item.h5;
                h6 = h6 + item.h6;
                h7 = h7 + item.h7;
                h8 = h8 + item.h8;
                h9 = h9 + item.h9;
                h10 = h10 + item.h10;
                h11 = h11 + item.h11;
                h12 = h12 + item.h12;
                piezas = piezas + item.piezas;
            };
            listViewFooter.h1 = h1;
            listViewFooter.h2 = h2;
            listViewFooter.h3 = h3;
            listViewFooter.h4 = h4;
            listViewFooter.h5 = h5;
            listViewFooter.h6 = h6;
            listViewFooter.h7 = h7;
            listViewFooter.h8 = h8;
            listViewFooter.h9 = h9;
            listViewFooter.h10 = h10;
            listViewFooter.h11 = h11;
            listViewFooter.h12 = h12;
            listViewFooter.piezas = piezas;
        }
    }
}
