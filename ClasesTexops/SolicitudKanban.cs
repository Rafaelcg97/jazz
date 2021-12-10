namespace ClasesTexops
{
    public class SolicitudKanban:Partnumber
    {
        Modulo modulo= new Modulo();
        Lote lote;

        public SolicitudKanban(Modulo modulo, Lote lote, string codigoPartnumber, string tallaPartnumber, double requeridoPartnumber, double solicitadoPartNumber)
        {
            this.lote = lote;
            this.modulo = modulo;
            CodigoPartNumber = codigoPartnumber;
            TallaPartnumber = tallaPartnumber;
            RequeridoPartnumber = requeridoPartnumber;
            SolicitadoPartnumber = solicitadoPartNumber;
        }

        public SolicitudKanban()
        {
        }

        public string NombreModulo { get => modulo.NombreModulo; set => modulo = new Modulo(value,1); }
        public string NumeroLote { get => lote.NumeroLote; set => lote = new ClasesTexops.Lote(value); }
    }
}
