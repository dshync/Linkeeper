using Linkeeper.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Linkeeper.Data
{
	public interface ILinkeeperRepo
	{
		IEnumerable<Link> GetAllLinks();
		IEnumerable<Link> GetAllUserLinks(IdentityUser user);
		Link GetLinkById(int id);
		void AddLink(Link link);
		void UpdateLink(Link link);
		void DeleteLink(Link link);

		bool SaveChanges();
	}
}
