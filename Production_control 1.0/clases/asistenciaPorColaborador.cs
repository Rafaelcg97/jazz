using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class asistenciaPorColaborador
    {
        public string modulo { get; set; }
        public string semana { get; set; }
        public int codigo { get; set; }
        public string nombre{ get; set; }
        public string bonoLunes { get; set; }
        public string bonoMartes { get; set; }
        public string bonoMiercoles { get; set; }
        public string bonoJueves { get; set; }
        public string bonoViernes { get; set; }
        public string bonoSabado { get; set; }

        public string horasLunes { get; set; }
        public string horasMartes { get; set; }
        public string horasMiercoles { get; set; }
        public string horasJueves { get; set; }
        public string horasViernes { get; set; }
        public string horasSabado { get; set; }
        public string horas { get; set; }
        public string bono { get; set; }
    }
}
