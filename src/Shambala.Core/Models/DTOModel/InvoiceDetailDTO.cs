using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    public class InvoiceDetailDTO
    {
        public int Id{get;set;}
        public System.DateTime DateCreated{get;set;}
        public decimal CostPrice{get;set;}
        public SchemeDTO Scheme{get;set;}
        public decimal SellingPrice{get;set;}
        public decimal DuePrice{get;set;}
    }
}