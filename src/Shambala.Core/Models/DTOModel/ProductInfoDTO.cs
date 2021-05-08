using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class ProductInfoDTO
    {
      public int Id{get;set;}
      public string Name{get;set;}
      public IEnumerable<FlavourInfoDTO> FlavourInfos{get;set;}
    
    }
    public class FlavourInfoDTO
    {
      public int QuantityDispatch{get;set;}
      public int QuantityInStock{get;set;}
      public string Title{get;set;}
      public int Id{get;set;}
    }
}