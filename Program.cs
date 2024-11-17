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
      .AddEntityFrameworkStores<BankIdentityDbContext>()
      .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;               // Require at least one digit
    options.Password.RequireLowercase = true;           // Require at least one lowercase letter
    options.Password.RequireUppercase = false;           // Require at least one uppercase letter
    options.Password.RequireNonAlphanumeric = true;    // Disable special character requirement
    options.Password.RequiredLength = 6;               // Minimum length
    options.Password.RequiredUniqueChars = 0;           // Unique characters
});



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
