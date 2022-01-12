using Linkeeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Data
{
    public interface ILinkeeperRepo
    {
        IEnumerable<Link> GetAllLinks();
        Link GetLinkById(int id);
        void AddLink(Link link);

        bool SaveChanges();
    }
}
