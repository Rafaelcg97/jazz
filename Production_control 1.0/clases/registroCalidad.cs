using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class registroCalidad
    {
        public string fecha { get; set; }
        public string codigo { get; set; }
        public string defecto { get; set; }
        public int anio { get; set; }
        public int semana { get; set; }
        public string modulo { get; set; }
        public int arteria { get; set; }
        public int muestraP { get; set; }
        public int rechazosP { get; set; }
        public double aqlP { get; set; }
        public int muestraE { get; set; }
        public int rechazosE { get; set; }
        public double aqlE { get; set; }
        public int muestraF { get; set; }
        public int rechazosF { get; set; }
        public double aqlF { get; set; }
    }
}
