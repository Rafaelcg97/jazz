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
using JazzCCO._0.clases;

namespace JazzCCO._0.pantallasProduccion
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

            double bonoLunes = 0;
            double bonoMartes = 0;
            double bonoMiercoles = 0;
            double bonoJueves = 0;
            double bonoViernes = 0;
            double bonoSabado = 0;
            double bono = 0;

            double mtLunes = 0;
            double mtMartes = 0;
            double mtMiercoles = 0;
            double mtJueves = 0;
            double mtViernes = 0;
            double mtSabado = 0;
            double mt = 0;

            double mdLunes = 0;
            double mdMartes = 0;
            double mdMiercoles = 0;
            double mdJueves = 0;
            double mdViernes = 0;
            double mdSabado = 0;
            double md = 0;

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
                if (!item.modart.Contains("-3"))
                {
                    piezasLunes = piezasLunes + item.piezasLunes;
                    piezasMartes = piezasMartes + item.piezasMartes;
                    piezasMiercoles = piezasMiercoles + item.piezasMiercoles;
                    piezasJueves = piezasJueves + item.piezasJueves;
                    piezasViernes = piezasViernes + item.piezasViernes;
                    piezasSabado = piezasSabado + item.piezasSabado;
                    piezas = piezas + item.totalDePiezas;
                    mtLunes = mtLunes + item.mtLunes;
                    mtMartes = mtMartes + item.mtMartes;
                    mtMiercoles = mtMiercoles + item.mtMiercoles;
                    mtJueves = mtJueves + item.mtJueves;
                    mtViernes = mtViernes + item.mtViernes;
                    mtSabado = mtSabado + item.mtSabado;
                    mt = mt + item.mt;

                    mdLunes = mdLunes + item.mdLunes;
                    mdMartes = mdMartes + item.mdMartes;
                    mdMiercoles = mdMiercoles + item.mdMiercoles;
                    mdJueves = mdJueves + item.mdJueves;
                    mdViernes = mdViernes + item.mdViernes;
                    mdSabado = mdSabado + item.mdSabado;
                    md = md + item.md;
                }

                bonoLunes = bonoLunes + item.bonoLunesD;
                bonoMartes = bonoMartes + item.bonoMartesD;
                bonoMiercoles = bonoMiercoles + item.bonoMiercolesD;
                bonoJueves = bonoJueves + item.bonoJuevesD;
                bonoViernes = bonoViernes + item.bonoViernesD;
                bonoSabado = bonoSabado + item.bonoSabadoD;
                bono = bono + item.bonoD;

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

            listViewFooter.bonoLunes = bonoLunes.ToString("C");
            listViewFooter.bonoMartes = bonoMartes.ToString("C");
            listViewFooter.bonoMiercoles = bonoMiercoles.ToString("C");
            listViewFooter.bonoJueves = bonoJueves.ToString("C");
            listViewFooter.bonoViernes = bonoViernes.ToString("C");
            listViewFooter.bonoSabado = bonoSabado.ToString("C");
            listViewFooter.bono = bono.ToString("C");

            listViewFooter.eficienciaLunes = (mtLunes/mdLunes).ToString("P");
            listViewFooter.eficienciaMartes = (mtMartes/mdMartes).ToString("P");
            listViewFooter.eficienciaMiercoles = (mtMiercoles/mdMiercoles).ToString("P");
            listViewFooter.eficienciaJueves = (mtJueves/mdJueves).ToString("P");
            listViewFooter.eficienciaViernes = (mtViernes/mdViernes).ToString("P");
            listViewFooter.eficienciaSabado = (mtSabado/mdSabado).ToString("P");
            listViewFooter.eficienciaTotal = (mt/md).ToString("P");

            listViewFooter.samLunes = Math.Round(mtLunes/piezasLunes, 4).ToString();
            listViewFooter.samMartes = Math.Round(mtMartes/piezasMartes, 4).ToString();
            listViewFooter.samMiercoles = Math.Round(mtMiercoles/piezasMiercoles, 4).ToString();
            listViewFooter.samJueves = Math.Round(mtJueves/piezasJueves, 4).ToString();
            listViewFooter.samViernes = Math.Round(mtViernes/piezasViernes, 4).ToString();
            listViewFooter.samSabado = Math.Round(mtSabado/piezasSabado, 4).ToString();
            listViewFooter.samTotal = Math.Round(mt/piezas, 4).ToString();
        }
    }
}
