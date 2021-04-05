using System;

namespace Production_control_1._0.clases
{
    public class balance : estilo 
    {
        public DateTime fechaCreacion {get; set;}
        public string modulo {get; set;}
        public int corrida {get; set;}
        public int horas { get; set; }
        public int operarios { get; set; }
        public string eficiencia { get; set; }
        public int piezas { get; set; }
        public string ingeniero { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int version { get; set; }
        public string tipo { get; set; }
        public string identificador { get; set; }
        public string sub { get; set; }

        public string sobre { get; set; }

        public string lote { get; set; }
    }
}
