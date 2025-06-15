using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Syncfusion.Licensing;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;
using WUCSA.Infrastructure.Repositories;
using WUCSA.Infrastructure.Services;
using WUCSA.Web.Resources;
using WUCSA.Web.Utils;
using WUCSA.Web.ViewComponents;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────── журналирование ───────────────────
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// ──────────────────── лицензия Syncfusion ───────────────
SyncfusionLicenseProvider.RegisterLicense(
    builder.Configuration["SyncfusionLicenseKey"]);

// ──────────────────── Razor Pages + маршрутизация ───────
builder.Services.AddRazorPages(opt =>
{
    opt.Conventions.Add(new CultureTemplatePageRouteModelConvention());
})
.AddViewLocalization()
.AddDataAnnotationsLocalization(o =>
{
    o.DataAnnotationLocalizerProvider = (_, factory) =>
        factory.Create(typeof(SharedResource));
});

builder.Services.AddRouting(o => o.LowercaseUrls = true);

// ──────────────────── локализация ───────────────────────
builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = new[] { new CultureInfo("en"), new CultureInfo("ru"), new CultureInfo("uz") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supported;
    options.SupportedUICultures = supported;
    options.RequestCultureProviders.Insert(0,
        new RouteDataRequestCultureProvider { Options = options });
});

// ──────────────────── EF Core + DbContext ───────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("WUCSA_DB"))
           .UseLazyLoadingProxies());

// ──────────────────── Identity + аутентификация ─────────
builder.Services.AddDefaultIdentity<AppUser>()
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireDigit = true;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789_.-@";
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedAccount = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Lockout.AllowedForNewUsers = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    opt.Lockout.MaxFailedAccessAttempts = 3;
});

builder.Services.Configure<SecurityStampValidatorOptions>(o =>
    o.ValidationInterval = TimeSpan.Zero);

builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        var g = builder.Configuration.GetSection("Authentication:Google");
        o.ClientId     = g["ClientId"];
        o.ClientSecret = g["ClientSecret"];
        o.AccessDeniedPath = "/AccessDeniedPathInfo";
    });

// ──────────────────── инфраструктурные сервисы ──────────
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IRankRepository, RankRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ImageHelper>();
builder.Services.AddScoped<PDFFileHelper>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(o =>
{
    o.CheckConsentNeeded       = _ => true;
    o.MinimumSameSitePolicy    = SameSiteMode.None;
});

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

// ──────────────────── билд ──────────────────────────────
var app = builder.Build();

// ──────────────────── конвейер HTTP ─────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();                // замена UseDatabaseErrorPage
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// ──────────────────── инициализация БД/сидов ────────────
await using var scope = app.Services.CreateAsyncScope();
await SeedData.InitializeAsync(scope.ServiceProvider);

app.Run();