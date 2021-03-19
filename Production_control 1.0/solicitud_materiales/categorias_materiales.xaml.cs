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


namespace Production_control_1._0.solicitud_materiales
{

    public partial class categorias_materiales : Page
    {
        public categorias_materiales()
        {
            InitializeComponent();
            
        }


        private void letra_responsiva(object sender, SizeChangedEventArgs e)
        {
            tamanosLetras.tamanosLetras.letra1(sender, e);
        }

        
    }
}
