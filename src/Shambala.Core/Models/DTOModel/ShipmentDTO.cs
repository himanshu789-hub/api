using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    public class ShipmentDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public short TotalRecievedPieces { get; set; }
        [Required]
        public short TotalDefectPieces { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public byte CaretSize { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public byte FlavourId { get; set; }
    }
}