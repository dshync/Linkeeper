using Linkeeper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Linkeeper.Data
{
	public class LinkeeperContext : IdentityDbContext
	{
		public LinkeeperContext(DbContextOptions<LinkeeperContext> opt)
			:base(opt)
		{
		}

		public DbSet<Link> Links { get; set; }
	}
}
