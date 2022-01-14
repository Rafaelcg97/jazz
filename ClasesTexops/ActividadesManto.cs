using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesTexops
{
    public class ActividadesManto
    {
        int idActividadManto;
        string nombreActividad;
        string nombreFrecuencia;
        string nombreEquipo;
        int idStatusActividad;
        string inicio;

        public ActividadesManto(int idActividadManto, string nombreActividad, string nombreFrecuencia, string nombreEquipo, int idStatusActividad, string inicio)
        {
            this.idActividadManto = idActividadManto;
            this.nombreActividad = nombreActividad;
            this.nombreFrecuencia = nombreFrecuencia;
            this.nombreEquipo = nombreEquipo;
            this.idStatusActividad = idStatusActividad;
            this.inicio = inicio;
        }

        public int IdActividadManto { get => idActividadManto; set => idActividadManto = value; }
        public string NombreActividad { get => nombreActividad; set => nombreActividad = value; }
        public string NombreFrecuencia { get => nombreFrecuencia; set => nombreFrecuencia = value; }
        public string NombreEquipo { get => nombreEquipo; set => nombreEquipo = value; }
        public int IdStatusActividad { get => idStatusActividad; set => idStatusActividad = value; }
        public string Inicio { get => inicio; set => inicio = value; }
    }
}
