using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class solicitudKanban
    {
        public int solicitudKanbanId { get; set; }
        public int manufactureId { get; set; }
        public string tipo { get; set; }
        public string modulo { get; set; }
        public int ubicacion { get; set; }
        public string lote { get; set; }
        public int styleId { get; set; }
        public string estilo { get; set; }
        public string temporada { get; set; }
        public string material { get; set; }
        public string talla { get; set; }
        public int cantidad{ get; set; }
        public string fechaSolicitud { get; set; }
        public string fechaInicio { get; set; }
        public string fechaEntrega { get; set; }
        public string color { get; set; }
        public string color2 { get; set; }
    }
}
