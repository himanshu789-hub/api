using System;
using System.ComponentModel.DataAnnotations;

namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class CreditDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }

        [RequiredWithNonDefault]
        public DateTime DateRecieved { get; set; }

        [RequiredWithNonDefault]
        public decimal Amount { get; set; }

        [RequiredWithNonDefault]
        public short ShopId { get; set; }

        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }

    }
}