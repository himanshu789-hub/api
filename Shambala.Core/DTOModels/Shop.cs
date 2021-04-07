using System.Collections.Generic;

namespace Shambala.Core.DTOModels
{
    public class ShopDTO
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public short Id { get; set; }
        public virtual SchemeDTO Scheme { get; set; }

    }
    public class ShopWithInvoicesDTO : ShopDTO
    {
        public ICollection<InvoiceDTO> Invoice { get; set; }
    }
}