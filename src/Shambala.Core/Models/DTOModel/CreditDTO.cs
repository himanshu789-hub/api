using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    public class CreditDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DateRecieved { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public short ShopId { get; set; }

        [Required]
        public int OutgoingShipmentId { get; set; }

    }
}