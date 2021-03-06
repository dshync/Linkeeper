using System.ComponentModel.DataAnnotations;

namespace Linkeeper.Domain.ApiIdentity
{
	//used only for API
	public class UserRegistrationRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
