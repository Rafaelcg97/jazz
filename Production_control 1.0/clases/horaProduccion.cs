using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class horaProduccion
    {
        public int num_hh { get; set; }
        public string fecha { get; set; }
        public string turno { get; set; }
        public int hora { get; set; }
        public string modulo { get; set; }
        public int arteria { get; set; }
        public int coordinador { get; set; }
        public string estilo { get; set; }
        public string temporada { get; set; }
        public string empaque { get; set; }
        public string descripcion { get; set; }
        public double sam { get; set; }
        public double opeCostura { get; set; }
        public double opeManuales { get; set; }
        public double incapacitados { get; set; }
        public double permisos { get; set; }
        public double cita { get; set; }
        public double inasistencia { get; set; }
        public string lote { get; set; }
        public int piezas { get; set; }
        public int terminadas { get; set; }
        public string colorLote { get; set; }
        public int xxs { get; set; }
        public int xs { get; set; }
        public int s { get; set; }
        public int m { get; set; }
        public int l { get; set; }
        public int xl { get; set; }
        public int xxl { get; set; }
        public int xxxl { get; set; }
        public int totalDePiezas { get; set; }
        public double tiempoParo { get; set; }
        public string motivoParo { get; set; }
        public string custom { get; set; }
        public double minutosEfectivos { get; set; }
        public string ingresadoPor { get; set;}
        public string cambioEstilo { get; set; }
        public string[] turnos { get; set; }
        public int[] horas { get; set; }
        public string[] modulos { get; set; }
        public string[] eleccion { get; set; }
        public int[] arterias { get; set; }
        public string[] motivos { get; set; }
        public List<string> empaques { get; set; }
        public int valido { get; set; }
        public int diferencia { get; set; }
    }
}

