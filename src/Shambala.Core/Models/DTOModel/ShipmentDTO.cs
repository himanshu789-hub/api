using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class ShipmentDTO : EqualityComparer<ShipmentDTO>
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        [RequiredWithNonDefault]
        public int TotalRecievedPieces { get; set; }
        [Required]
        public int TotalDefectPieces { get; set; }
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        [RequiredWithNonDefault]
        public byte CaretSize { get; set; }
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        [RequiredWithNonDefault]
        public byte FlavourId { get; set; }

        public override bool Equals(ShipmentDTO x, ShipmentDTO y)
        {
            return x.FlavourId == y.FlavourId && x.ProductId == y.ProductId;
        }

        public override int GetHashCode(ShipmentDTO obj)
        {
            return base.GetHashCode();
        }
    }
}