using System;
using Shambala.Core.Helphers;
namespace Shambala.Core.Models.DTOModel
{
    public class SchemeDTO
    {

        [RequiredWithNonDefault]
        public short Id { get; set; }
        
        [RequiredWithNonDefault]
        public int Quantity { get; set; }
        
        [RequiredWithNonDefault]
        public int ProductId{get;set;}
        
        [RequiredWithNonDefault]
        public DateTime DateCreated{get;set;}
    }
}