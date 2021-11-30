using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class solicitudKanban
    {
        public int atiendeSolicitud { get; set; }
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
        public bool chequeado { get; set; }
        public bool habilitado { get; set; }
        public bool habilitadoEntrega { get; set; }
        public int solicitado { get; set; }
        public int diferencia { get; set; }
        public int agregado { get; set; }
        public bool validadoSmed { get; set; }
        public string[] areas { get; set; }
        public string[] motivos { get; set; }
        public string area { get; set; }
        public string motivo { get; set; }
        public string ubicacionM { get; set; }
        public int responsable { get; set; }
        public string movimiento { get; set; }
    }
}
