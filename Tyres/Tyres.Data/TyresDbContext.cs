using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tyres.Data.Models;

namespace Tyres.Data
{
    public class TyresDbContext : IdentityDbContext<User>
    {
        public TyresDbContext(DbContextOptions<TyresDbContext> options)
            : base(options)
        {
        }
    }
}
