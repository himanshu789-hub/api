using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class SalesmanDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        [Required,MinLength(1)]
        public string FullName { get; set; }
        
        public bool IsActive { get; set; }
    }

}