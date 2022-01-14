using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesTexops
{
    public class EquipoManto
    {
        int idEquipoManto;
        string nombreEquipo;

        public EquipoManto(int idEquipoManto, string nombreEquipo)
        {
            IdEquipoManto = idEquipoManto;
            NombreEquipo = nombreEquipo;
        }

        public int IdEquipoManto { get => idEquipoManto; set => idEquipoManto = value; }
        public string NombreEquipo { get => nombreEquipo; set => nombreEquipo = value; }
    }
}
