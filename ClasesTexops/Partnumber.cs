namespace ClasesTexops
{
    public class Partnumber:Celulabmp
    {
        string codigoPartNumber = "";
        string categoryPartNumber = "";
        string subcategoryPartNumber = "";
        string tallaPartnumber = "";
        double samConteoPartnumber = 0;
        double requeridoPartnumber = 0;
        int valorSeleccionadoKanban= 0;
        double solicitadoPartnumber = 0;
        //solo usado como base para ir validando lista de agregados
        double solicitadoPartnumberInicial = 0;
        bool solicitadoKanban = false;
        bool habilitadoKanban = true;

        public Partnumber()
        {

        }

        public Partnumber(string codigoPartNumber, string categoryPartNumber, string tallaPartnumber, double requeridoPartnumber, double solicitadoPartnumber, double solicitadoPartnumberInicial)
        {
            this.codigoPartNumber = codigoPartNumber;
            this.categoryPartNumber = categoryPartNumber;
            this.tallaPartnumber = tallaPartnumber;
            this.requeridoPartnumber = requeridoPartnumber;
            this.solicitadoPartnumber = solicitadoPartnumber;
            this.solicitadoPartnumberInicial = solicitadoPartnumberInicial;
        }

        public string CodigoPartNumber { get => codigoPartNumber; set => codigoPartNumber = value; }
        public double SamConteoPartnumber { get => samConteoPartnumber; set => samConteoPartnumber = value; }
        public string TallaPartnumber { get => tallaPartnumber; set => tallaPartnumber = value; }
        public double RequeridoPartnumber { get => requeridoPartnumber; set => requeridoPartnumber = value; }
        public string CategoryPartNumber { get => categoryPartNumber; set => categoryPartNumber = value; }
        public string SubcategoryPartNumber { get => subcategoryPartNumber; set => subcategoryPartNumber = value; }
        public double SolicitadoPartnumber { get => solicitadoPartnumber; set => solicitadoPartnumber = value; }
        public bool HabilitadoKanban
        {
            get
            {
                if (requeridoPartnumber > solicitadoPartnumber)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set => habilitadoKanban = value;
        }

        public bool SolicitadoKanban { get => solicitadoKanban; set => solicitadoKanban = value; }
        public double RestantePartnumber
        {
            get
            {
                return requeridoPartnumber - solicitadoPartnumber;
            }
        }

        public int ValorSeleccionadoKanban { get => valorSeleccionadoKanban; set => valorSeleccionadoKanban = value; }
        public double SolicitadoPartnumberInicial { get => solicitadoPartnumberInicial; set => solicitadoPartnumberInicial = value; }
    }
}
