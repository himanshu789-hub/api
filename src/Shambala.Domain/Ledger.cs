namespace Shambala.Domain
{
    public partial class Ledger
    {
        public int OutgoingShipmentIdFk { get; set; }
        public int Id { get; set; }
        public decimal OldCheque { get; set; }
        public decimal NewCheque { get; set; }
        public byte TotalOldCheque { get; set; }
        public byte TotalNewCheque { get; set; }
        public decimal OldCash{get;set;}
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
    }
}