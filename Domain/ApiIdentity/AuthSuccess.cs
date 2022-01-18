using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Domain.ApiIdentity
{
    public class AuthSuccess
    {
        public string Token { get; set; }
        public bool Success { get; set; }
    }
}
