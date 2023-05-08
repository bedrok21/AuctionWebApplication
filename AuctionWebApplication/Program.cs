using AuctionWebApplication;
using AuctionWebApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AuctionWebApplication.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbauctionContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHostedService<ExpirationService>();

builder.Services.AddDbContext<IdentityContext> (option => option.UseSqlServer(
builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentity<UserIdent, IdentityRole>().AddEntityFrameworkStores <IdentityContext>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var userManager = services.GetRequiredService <UserManager<UserIdent>> ();
		var rolesManager = services.GetRequiredService <RoleManager<IdentityRole>>();
		await RoleInitializer.InitializeAsync(userManager, rolesManager);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while seeding the database." +DateTime.Now.ToString());
	}
}
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();
