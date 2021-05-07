using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.DTOModels
{
    public class ShipmentDTO
    {
        public int Id { get; set; }
        public short TotalRecievedPieces { get; set; }
        public short TotalDefectPieces { get; set; }
        public DateTime DateCreated { get; set; }
        public byte CaretSize { get; set; }
        public int ProductId { get; set; }
        public byte FlavourId { get; set; }
    }
}