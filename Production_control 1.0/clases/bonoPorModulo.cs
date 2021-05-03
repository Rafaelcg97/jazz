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

        public int totalDePiezas { get; set; }
        public string bono { get; set; }

    }
}
