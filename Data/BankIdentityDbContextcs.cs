using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UFS_BANK_FINAL.Data
{
    public class BankIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public BankIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
