using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using WUCSA.Web.Utils;
using WUCSA.Web.Resources;
using WUCSA.Core.Interfaces;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;
using WUCSA.Infrastructure.Services;
using WUCSA.Infrastructure.Repositories;
using WUCSA.Web.ViewComponents;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WUCSA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
            });
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("uz")
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });
            

            ConfigureDatabases(services);
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IRankRepository, RankRepository>();
            services.AddScoped<ISportTypeRepository, SportTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            ConfigureIdentity(services);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ImageHelper>();
            services.AddScoped<PDFFileHelper>();
            services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            services.AddHttpContextAccessor();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/Blog/List", "/Blog");
                options.Conventions.AddPageRoute("/Rank/List", "/Rank");
                options.Conventions.AddPageRoute("/Rank/SubList/{loc}/{stype}", "/Rank/List/{loc}/{stype}");
                options.Conventions.AddPageRoute("/Rank/Index/{slug}/{gender}/{weight?}", "/Rank/{slug}/{gender}/{weight?}");
                options.Conventions.AddPageRoute("/Event/List", "/Event");
                options.Conventions.AddPageRoute("/SportType/List", "/SportType");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });  
        }
        
        private void ConfigureDatabases(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                        Configuration.GetConnectionString("WUCSA_DB"))
                        .UseLazyLoadingProxies());
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<AppUser>()
                .AddRoles<UserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789_.-@";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
                options.ClaimActions.MapJsonKey("image", "picture");
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            })
            .AddFacebook(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            });
        }
    }
}
