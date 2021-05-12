using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class ProductInfoDTO
    {
      public int Id{get;set;}
      public string Name{get;set;}
      public byte CaretSize{get;set;}
      public IEnumerable<FlavourInfoDTO> FlavourInfos{get;set;}
    
    }
    public class FlavourInfoDTO
    {
      public int QuantityInDispatch{get;set;}
      public int QuantityInStock{get;set;}
      public string Title{get;set;}
      public int Id{get;set;}
    }
}