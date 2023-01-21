using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebApplicationAutentRegist.Data
{
    public class ApplicationUser: IdentityUser
    {
        public virtual string Name { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
        public virtual bool Status { get; set; }
    }
}
