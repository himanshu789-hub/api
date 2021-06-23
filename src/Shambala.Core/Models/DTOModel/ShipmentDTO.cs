using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class ShipmentDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        [RequiredWithNonDefault]
        public short TotalRecievedPieces { get; set; }
        [Required]
        public short TotalDefectPieces { get; set; }
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        [RequiredWithNonDefault]
        public byte CaretSize { get; set; }
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        [RequiredWithNonDefault]
        public byte FlavourId { get; set; }
    }
}