using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class preparacionTanda: tarjetaKanban
    {
        public int idTanda { get; set; }
        public string nombrePreparador { get; set; }
        public int codigoPreparador { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public double pausa { get; set; }
        public string imagenStatus { get; set; }
        public string imagenTerminar { get; set; }
        public bool terminar { get; set; }
    }
}
