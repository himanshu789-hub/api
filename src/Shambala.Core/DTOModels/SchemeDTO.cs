using System;

namespace Shambala.Core.DTOModels
{
    public class SchemeDTO
    {

        public short Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsUserDefinedScheme { get; set; }
        public byte SchemeType { get; set; }
        public decimal Value { get; set; }
    }
}