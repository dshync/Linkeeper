using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Domain.ApiIdentity
{
    public class AuthFailed
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
