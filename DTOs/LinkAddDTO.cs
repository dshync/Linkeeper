using System.ComponentModel.DataAnnotations;

namespace Linkeeper.DTOs
{
	public class LinkAddDTO
	{
		[Required]
		public string Address { get; set; }
		
		public string Representation { get; set; }
	}
}
