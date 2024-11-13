using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UFS_BANK_FINAL.Data
{
    public static class SeedDataIdentity
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Bank123$";
        private const string adminEmail = "admin@bank.co.za";
        private const string adminRole = "Admin";

        private const string clientUser = "Client";
        private const string clientPassword = "Bank123$";
        private const string clientEmail = "client@bank.co.za";
        private const string clientRole = "Client";

        private const string consultantUser = "Consultant";
        private const string consultantPassword = "Bank123$";
        private const string consultantEmail = "consultant@bank.co.za";
        private const string consultantRole = "Consultant";

        private const string financialAdvisorUser = "FinancialAdvisor";
        private const string financialAdvisorPassword = "Bank123$";
        private const string financialAdvisorEmail = "financialadvisor@bank.co.za";
        private const string financialAdvisorRole = "FinancialAdvisor";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankIdentityDbContext>();
                await context.Database.MigrateAsync();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Seed roles if they do not exist
                await EnsureRoleExists(roleManager, adminRole);
                await EnsureRoleExists(roleManager, clientRole);
                await EnsureRoleExists(roleManager, consultantRole);
                await EnsureRoleExists(roleManager, financialAdvisorRole); // Add Financial Advisor role

                // Seed Admin user
                await EnsureUserExists(userManager, adminUser, adminEmail, adminPassword, adminRole);

                // Seed Client user
                await EnsureUserExists(userManager, clientUser, clientEmail, clientPassword, clientRole);

                // Seed Consultant user
                await EnsureUserExists(userManager, consultantUser, consultantEmail, consultantPassword, consultantRole);

                // Seed Financial Advisor user
                await EnsureUserExists(userManager, financialAdvisorUser, financialAdvisorEmail, financialAdvisorPassword, financialAdvisorRole);
            }
        }

        // Ensure the role exists, if not, create it
        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Ensure the user exists, if not, create the user and assign the role
        private static async Task EnsureUserExists(UserManager<IdentityUser> userManager, string userName, string email, string password, string roleName)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = new IdentityUser
                {
                    UserName = userName,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
