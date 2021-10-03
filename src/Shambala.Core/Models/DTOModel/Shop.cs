using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class ShopDTO
    {
        [Required,MinLength(1)]
        public string Title { get; set; }
        [Required,MinLength(1)]
        public string Address { get; set; }
        [RequiredWithNonDefault]  
        public short Id { get; set; }
    }
    // public class ShopWithInvoicesDTO : ShopDTO
    // {
    //     public ICollection<ShipmentLedgerDetail> Invoice { get; set; }
    // }
}