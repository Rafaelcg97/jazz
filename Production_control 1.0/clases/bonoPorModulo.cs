using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class bonoPorModulo
    {
        public string modart { get; set; }
        public string turno { get; set; }

        public int piezasLunes { get; set; }
        public int piezasMartes { get; set; }
        public int piezasMiercoles { get; set; }
        public int piezasJueves { get; set; }
        public int piezasViernes { get; set; }
        public int piezasSabado { get; set; }

        public string eficienciaLunes { get; set; }
        public string eficienciaMartes { get; set; }
        public string eficienciaMiercoles { get; set; }
        public string eficienciaJueves { get; set; }
        public string eficienciaViernes { get; set; }
        public string eficienciaSabado { get; set; }

        public string bonoLunes { get; set; }
        public string bonoMartes { get; set; }
        public string bonoMiercoles { get; set; }
        public string bonoJueves { get; set; }
        public string bonoViernes { get; set; }
        public string bonoSabado { get; set; }

        public double bonoLunesD { get; set; }
        public double bonoMartesD { get; set; }
        public double bonoMiercolesD { get; set; }
        public double bonoJuevesD { get; set; }
        public double bonoViernesD { get; set; }
        public double bonoSabadoD { get; set; }

        public string samLunes { get; set; }
        public string samMartes { get; set; }
        public string samMiercoles { get; set; }
        public string samJueves { get; set; }
        public string samViernes { get; set; }
        public string samSabado { get; set; }

        public int operariosLunes { get; set; }
        public int operariosMartes { get; set; }
        public int operariosMiercoles { get; set; }
        public int operariosJueves { get; set; }
        public int operariosViernes { get; set; }
        public int operariosSabado { get; set; }


        public double mtLunes { get; set; }
        public double mtMartes { get; set; }
        public double mtMiercoles { get; set; }
        public double mtJueves { get; set; }
        public double mtViernes { get; set; }
        public double mtSabado { get; set; }

        public double mdLunes { get; set; }
        public double mdMartes { get; set; }
        public double mdMiercoles { get; set; }
        public double mdJueves { get; set; }
        public double mdViernes { get; set; }
        public double mdSabado { get; set; }




        public int totalDePiezas { get; set; }
        public string samTotal { get; set; }
        public string eficienciaTotal { get; set; }
        public string bono { get; set; }
        public int operarios { get; set; }
        public double mt { get; set; }
        public double md { get; set; }
        public double bonoD { get; set; }

    }
}
