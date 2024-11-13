using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public static class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<BankDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                new Customer
                {
                    CustomerName = "Client",
                    Role = "Client",
                    Email = "client@bank.co.za",
                    Address = "123 Zone Street",
                    City = "Zone Forge",
                    PostCode = "3749",
                });
                await context.SaveChangesAsync();
            }

            if (!context.Accounts.Any())
            {

                context.Accounts.AddRange(
                    new Account
                    {
                        AccountNumber = 4100,
                        CustomerName = "Client",
                        AccountType = (char)AccountType.Saving,
                        Balance = 5000,
                        Advice = "Spend money wisely",
                        UserId = "Client",
                        ModifyDate = DateTime.Now.ToUniversalTime(),
                    });

                await context.SaveChangesAsync();
            }

            const string format = "dd/MM/yyyy hh:mm:ss tt";
            if (!context.Transactions.Any())
            {
                context.Transactions.AddRange(
                    new Transaction
                    {
                        TransactionType = TransactionType.Deposit,
                        AccountNumber = 4100,
                        Amount = 100,
                        ModifyDate = DateTime.ParseExact("01/10/2024 08:00:00 PM", format, null).ToUniversalTime()
                    },
                    new Transaction
                    {
                        TransactionType = TransactionType.Transfer,
                        AccountNumber = 4100,
                        Amount = 500,
                        ModifyDate = DateTime.ParseExact("01/10/2024 08:30:00 PM", format, null).ToUniversalTime()
                    });

                await context.SaveChangesAsync();
            }
        }
    }
}
