using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SQLInjectionDemo.Common
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            return int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

        }
    }
}
