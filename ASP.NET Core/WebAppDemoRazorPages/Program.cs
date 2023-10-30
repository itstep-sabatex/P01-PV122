using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebAppDemoRazorPages.Data;
using WebAppDemoRazorPages.Services;

const string defaultAdminName = "Administrator";
const string defaultRoleManagerName = "Manager";
const string defaultRoleUserName = "User";
const string defaultRoleGestName = "Guest"; // додається автоматично при самостійній реэстрації користувача
async Task InitialDatabase(IServiceProvider host)
{
    using (var serviceScope = host.CreateScope())
    {
        var services = serviceScope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var userStore = services.GetRequiredService<IUserStore<IdentityUser>>();
        var emailStorge = (IUserEmailStore<IdentityUser>)userStore;
        
        const string adminUser = "admin@myhost.com";
        const string adminPass = "Ss1234567890-";

var adminRole = await roleManager.FindByNameAsync(defaultAdminName);
        if (adminRole ==  null)
        {
            var adminR = await roleManager.CreateAsync(new IdentityRole(defaultAdminName));
            if (adminR.Succeeded == false) { throw new Exception("Error create Administrator role"); }
            adminRole = await roleManager.FindByNameAsync(defaultAdminName);
        }

        var userAdmin = await userManager.FindByNameAsync(adminUser);
        if (userAdmin == null)
        {
            var user = Activator.CreateInstance<IdentityUser>();

            await userStore.SetUserNameAsync(user, adminUser,CancellationToken.None);
            await emailStorge.SetEmailAsync(user,adminUser,CancellationToken.None);
            var result = await userManager.CreateAsync(user, adminPass);
            if (!result.Succeeded)
            {
                throw new Exception($"Error create user {adminUser} with password {adminPass}");
            }
            var userId = await userManager.GetUserIdAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var resultConfirm = await userManager.ConfirmEmailAsync(user, code);
            if (!resultConfirm.Succeeded)
            {
                throw new Exception("");
            }
            userAdmin = await userManager.FindByNameAsync(adminUser);

        }

        if (!await userManager.IsInRoleAsync(userAdmin, defaultAdminName))
        {
            await userManager.AddToRoleAsync(userAdmin, defaultAdminName);
            
        }
    
    }

}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication().AddMicrosoftAccount(options => {
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
});
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages().AddViewLocalization(); //uk-UA
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };
    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    //
});
    
    
builder.Services.AddScoped<IEmailSender, MailSender>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(new RequestLocalizationOptions()
{ ApplyCurrentCultureToResponseHeaders = true }
.AddSupportedCultures(new[] { "en-US", "uk-UA" })
.AddSupportedUICultures(new[] { "en-US", "uk-UA" })
.SetDefaultCulture("en-US"));

app.MapRazorPages();
app.MapControllers();
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
    var i = 10;

});
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
    var i = 10;

});

if (app.Environment.IsDevelopment())
{
    await InitialDatabase(app.Services);
}
app.Run();
