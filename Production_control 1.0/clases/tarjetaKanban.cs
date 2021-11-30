using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class tarjetaKanban
    {
        public string tarjeta { get; set; }
        public int ordenPrioridad { get; set; }
        public string tanda { get; set; }
        public string lote { get; set; }
        public string cliente { get; set; }
        public string estilo { get; set; }
        public string modulo { get; set; }
        public string temporada { get; set; }
        public string color { get; set; }
        public int make { get; set; }
        public double sam { get; set; }
        public double samTanda { get; set; }
        public bool copa { get; set; }
        public bool seleccionado { get; set; }
        public string colorFondo { get; set; }
        public int conteoCategoria {get; set;}
        public double pfdLoteTrims { get => 6.75; }
        public double pfdTandaTrims { get => 27.1; }
        public double pfdSubcategoriaTrims { get => 6.75; }
        public int acumuladoCategoria { get; set; }
        public double pfdCliente
        {
            get
            {
                switch (cliente)
                {
                    case "REI":
                        return 0.75*0.88*0.75;
                    case "ACADEMYSPS":
                        return 0.90*0.88*0.75;
                    case "DICKS":
                        return 0.92*0.88*0.75;
                    case "WITH-P":
                        return 0.95*0.88*0.75;
                    default:
                        return 0.88*0.75;
                }
            }
        }
    }
}
