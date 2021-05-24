using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class bonoPorOperario
    {
        public string asignado { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string bonoLunes { get; set; }
        public string bonoMartes { get; set; }
        public string bonoMiercoles { get; set; }
        public string bonoJueves { get; set; }
        public string bonoViernes { get; set; }
        public string bonoSabado { get; set; }
        public double tiempoLunes { get; set; }
        public double tiempoMartes { get; set; }
        public double tiempoMiercoles { get; set; }
        public double tiempoJueves { get; set; }
        public double tiempoViernes { get; set; }
        public double tiempoSabado { get; set; }
        public string bonoBruto { get; set; }
        public string aqlG { get; set; }
        public string aqlI { get; set; }
        public string bp { get; set; }
        public string inasistencias { get; set; }
        public string amonestaciones{ get; set; }
        public string bonoNeto { get; set; }
    }
}
