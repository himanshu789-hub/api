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
        public short? SchemeId{get;set;}

    }
    public class ShopInfoDTO : ShopDTO
    {
        public SchemeDTO Scheme { get; set; }
    }
    public class ShopWithInvoicesDTO : ShopDTO
    {
        public ICollection<PostInvoiceDTO> Invoice { get; set; }
    }
}