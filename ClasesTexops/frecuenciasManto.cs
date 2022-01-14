using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesTexops
{
    public class frecuenciasManto
    {
        int idFrecuencia;
        string nombreFrecuencia;
        int equivalenteEnDias;

        public frecuenciasManto(int idFrecuencia, string nombreFrecuencia, int equivalenteEnDias)
        {
            this.idFrecuencia = idFrecuencia;
            this.nombreFrecuencia = nombreFrecuencia;
            this.equivalenteEnDias = equivalenteEnDias;
        }

        public int IdFrecuencia { get => idFrecuencia; set => idFrecuencia = value; }
        public string NombreFrecuencia { get => nombreFrecuencia; set => nombreFrecuencia = value; }
        public int EquivalenteEnDias { get => equivalenteEnDias; set => equivalenteEnDias = value; }
    }
}
