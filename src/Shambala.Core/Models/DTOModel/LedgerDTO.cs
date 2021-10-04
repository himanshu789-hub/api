using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class LedgerDTO
    {
        public int Id{get;set;}
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        public decimal OldCheque { get; set; }
        public decimal NewCheque { get; set; }
        public byte TotalOldCheque { get; set; }
        public byte TotalNewCheque { get; set; }
        public decimal OldCash { get; set; }
        [Range(0,int.MaxValue)]
        public short RowVersion { get; set; }
        [RequiredWithNonDefault]
        public decimal NetPrice { get; set; }
    }
}