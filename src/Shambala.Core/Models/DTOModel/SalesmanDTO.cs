using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    public class SalesmanDTO
    {
        [Required]
        public short Id { get; set; }
        [Required]
        public string FullName { get; set; }

        public bool IsActive { get; set; }
    }

}