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
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasProduccion
{
    public partial class bonoPorModuloSemana : UserControl
    {
        public bonoPorModuloSemana(List<bonoPorModulo> lista)
        {
            InitializeComponent();
            int piezasLunes = 0;
            int piezasMartes = 0;
            int piezasMiercoles = 0;
            int piezasJueves = 0;
            int piezasViernes = 0;
            int piezasSabado = 0;
            int piezas = 0;
            int bonoLunes = 0;
            int bonoMartes = 0;
            int bonoMiercoles = 0;
            int bonoJueves = 0;
            int bonoViernes = 0;
            int bonoSabado = 0;
            int bono = 0;
            int operariosLunes = 0;
            int operariosMartes = 0;
            int operariosMiercoles = 0;
            int operariosJueves = 0;
            int operariosViernes = 0;
            int operariosSabado = 0;
            int operarios = 0;
            listViewBomoPorModulo.ItemsSource = lista;
            foreach(bonoPorModulo item in lista)
            {
                piezasLunes = piezasLunes + item.piezasLunes;
                piezasMartes = piezasMartes + item.piezasMartes;
                piezasMiercoles = piezasMiercoles + item.piezasMiercoles;
                piezasJueves = piezasJueves + item.piezasJueves;
                piezasViernes = piezasViernes + item.piezasViernes;
                piezasSabado = piezasSabado + item.piezasSabado;
                piezas = piezas + item.totalDePiezas;

                operariosLunes = operariosLunes + item.operariosLunes;
                operariosMartes = operariosMartes + item.operariosMartes;
                operariosMiercoles = operariosMiercoles + item.operariosMiercoles;
                operariosJueves = operariosJueves + item.operariosJueves;
                operariosViernes = operariosViernes + item.operariosViernes;
                operariosSabado = operariosSabado + item.operariosSabado;
                operarios = operarios + item.operarios;

            }
            listViewFooter.piezasLunes = piezasLunes;
            listViewFooter.piezasMartes = piezasMartes;
            listViewFooter.piezasMiercoles = piezasMiercoles;
            listViewFooter.piezasJueves = piezasJueves;
            listViewFooter.piezasViernes = piezasViernes;
            listViewFooter.piezasSabado = piezasSabado;
            listViewFooter.totalDePiezas = piezas;

            listViewFooter.operariosLunes = operariosLunes;
            listViewFooter.operariosMartes = operariosMartes;
            listViewFooter.operariosMiercoles = operariosMiercoles;
            listViewFooter.operariosJueves = operariosJueves;
            listViewFooter.operariosViernes = operariosViernes;
            listViewFooter.operariosSabado = operariosSabado;
            listViewFooter.operarios = operarios;
        }
    }
}
