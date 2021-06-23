using System;
using Shambala.Core.Helphers;
namespace Shambala.Core.Models.DTOModel
{
    public class SchemeDTO
    {

        [RequiredWithNonDefault]
        public short Id { get; set; }
        
        [RequiredWithNonDefault]
        public string Title { get; set; }
        
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
       
        [RequiredWithNonDefault]
        public bool IsUserDefinedScheme { get; set; }
        
        [RequiredWithNonDefault]
        public SchemeType SchemeType { get; set; }
        
        public decimal Value { get; set; }
    }
}