using System.ComponentModel.DataAnnotations;

namespace Linkeeper.DTOs
{
    public class LinkUpdateDTO
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string Representation { get; set; }
    }
}
