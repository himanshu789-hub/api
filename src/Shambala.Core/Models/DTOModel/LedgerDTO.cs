namespace Shambala.Core.Models.DTOModel
{
    public class LedgerDTO
    {
        
        public int OutgoingShipmentId { get; set; }
        public decimal OldCheque { get; set; }
        public decimal NewCheque { get; set; }
        public byte TotalOldCheque { get; set; }
        public byte TotalNewCheque { get; set; }
        public decimal OldCash { get; set; }
        public short RowVersion { get; set; }
        public decimal NetPrice { get; set; }

    }
}