using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFS_BANK_FINAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BankConnection")));

builder.Services.AddDbContext<BankIdentityDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
      .AddEntityFrameworkStores<BankIdentityDbContext>();


builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await SeedData.EnsurePopulated(app);
await SeedDataIdentity.EnsurePopulated(app);

app.Run();
