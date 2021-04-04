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

namespace Production_control_1._0.pantallasProduccion
{
    /// <summary>
    /// Lógica de interacción para reporteProduccion.xaml
    /// </summary>
    public partial class reporteProduccion : Page
    {
        public reporteProduccion()
        {
            InitializeComponent();
            comboBoxModulo.Items.Add("abir");
            comboBoxModulo.Items.Add("aco");
            comboBoxModulo.Items.Add("casa");
            comboBoxModulo.Items.Add("de");
            comboBoxModulo.Items.Add("2458-237");
            comboBoxModulo.Items.Add("2458-236");

        }
    }
}
