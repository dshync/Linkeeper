using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkeeper.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Column(TypeName="varchar(250)")]
        public string Address { get; set; }

        [MaxLength(250)]
        [Column(TypeName = "varchar(250)")]
        public string Representation { get; set; }        
    }
}
