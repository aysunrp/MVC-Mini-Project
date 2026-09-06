using MCV_Mini_Project.Constants;
using MCV_Mini_Project.Data;
using MCV_Mini_Project.Filters;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Identity;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Options;
using MCV_Mini_Project.Services;
using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

EnvFile.Load(Path.Combine(builder.Environment.ContentRootPath, ".env"));
EnvFile.Load(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["Smtp:Host"] = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "",
    ["Smtp:Port"] = Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587",
    ["Smtp:User"] = Environment.GetEnvironmentVariable("SMTP_USER") ?? "",
    ["Smtp:Password"] = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "",
    ["Smtp:From"] = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "",
    ["Smtp:EnableSsl"] = Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL") ?? "true",
    ["Smtp:ConfirmationTo"] = Environment.GetEnvironmentVariable("SMTP_CONFIRMATION_TO") ?? "",
    ["SUPERADMIN_EMAIL"] = Environment.GetEnvironmentVariable("SUPERADMIN_EMAIL"),
    ["SUPERADMIN_PASSWORD"] = Environment.GetEnvironmentVariable("SUPERADMIN_PASSWORD"),
    ["ADMIN_EMAIL"] = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
    ["ADMIN_PASSWORD"] = Environment.GetEnvironmentVariable("ADMIN_PASSWORD"),
    ["AUTH_REQUIRE_CONFIRMED_EMAIL"] = Environment.GetEnvironmentVariable("AUTH_REQUIRE_CONFIRMED_EMAIL") ?? "true",
    ["EMAIL_CONFIRMATION_HOURS"] = Environment.GetEnvironmentVariable("EMAIL_CONFIRMATION_HOURS") ?? "24"
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));
builder.Services.AddScoped<IEmailService, EmailService>();

var requireConfirmedEmail = !string.Equals(
    builder.Configuration["AUTH_REQUIRE_CONFIRMED_EMAIL"],
    "false",
    StringComparison.OrdinalIgnoreCase);

var confirmationHours = 24;
if (int.TryParse(builder.Configuration["EMAIL_CONFIRMATION_HOURS"], out var parsedHours) && parsedHours > 0)
    confirmationHours = parsedHours;

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(confirmationHours));

builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.Name = ".eLearn.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.SuperAdminOnly, policy =>
        policy.RequireRole(AppRoles.SuperAdmin));

    options.AddPolicy(Policies.AdminPanel, policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.Identity?.IsAuthenticated == true &&
            ctx.User.FindFirst("email_confirmed")?.Value == "true" &&
            (ctx.User.IsInRole(AppRoles.SuperAdmin) ||
             ctx.User.IsInRole(AppRoles.Admin) ||
             (!ctx.User.IsInRole(AppRoles.Member) &&
              ctx.User.HasClaim(Permissions.ClaimType, Permissions.AccessAdminPanel)))));

    options.AddPolicy(Policies.ManageUsers, policy =>
        policy.RequireAssertion(ctx =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
                return false;
            if (ctx.User.FindFirst("email_confirmed")?.Value != "true")
                return false;
            if (ctx.User.IsInRole(AppRoles.SuperAdmin) || ctx.User.IsInRole(AppRoles.Admin))
                return true;
            if (ctx.User.IsInRole(AppRoles.Member))
                return false;
            return ctx.User.HasClaim(Permissions.ClaimType, Permissions.ManageUsers);
        }));
});

builder.Services.AddScoped<IIconService, IconService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IVisionService, VisionService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ICourseInfoService, CourseInfoService>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.Filters.Add<AdminAreaAuthorizationFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
