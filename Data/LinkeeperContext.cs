using Linkeeper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linkeeper.Data
{
    public class LinkeeperContext : DbContext
    {
        public LinkeeperContext(DbContextOptions<LinkeeperContext> opt)
            :base(opt)
        {
        }

        public DbSet<Link> Links { get; set; }
    }
}
