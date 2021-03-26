using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    class operacion:maquina
    {
        public int correlativoOperacion { get; set; }
        public string nombreOperacion { get; set; }
        public string tituloOperacion { get; set; }
        public double samOperacion { get; set; }
        public double asignadoOperacion { get; set; }
        public double requeridoOperacion { get; set; }

    }
}
