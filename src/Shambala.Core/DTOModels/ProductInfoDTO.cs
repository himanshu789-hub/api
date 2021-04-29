using System.Collections.Generic;
namespace Shambala.Core.DTOModels
{
    public class ProductInfoDTO
    {
      public int Id{get;set;}
      public string Name{get;set;}
      public IEnumerable<FlavourInfoDTO> FlavourInfos{get;set;}
    
    }
    public class FlavourInfoDTO
    {
      public string QuantityDispatch{get;set;}
      public string QuantityInStock{get;set;}
      public string Title{get;set;}
      public int Id{get;set;}
    }
}