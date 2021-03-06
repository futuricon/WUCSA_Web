using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WUCSA.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WUCSA.Core.Interfaces;
using WUCSA.Infrastructure.Repositories;
using WUCSA.Infrastructure.Services;
using WUCSA.Core.Entities.UserModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using WUCSA.Web.Utils;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Reflection;
using WUCSA.Web.Resources;
using Microsoft.Extensions.Options;
using WUCSA.Core.Interfaces.Repositories;

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
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("WUCSA_DB")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLocalization(options => options.ResourcesPath = "resources");
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
            });
            services.AddSingleton<CommonLocalizationService>();

            services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
                    return factory.Create(nameof(CommonResources), assemblyName.Name);
                };
            });

            ConfigureDatabases(services);
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IRankRepository, RankRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            ConfigureIdentity(services);

            services.AddScoped<ImageHelper>();

            services.AddHttpContextAccessor();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/Blog/List", "/Blog");
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
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
        }
        
        private void ConfigureDatabases(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                        Configuration.GetConnectionString("WUCSA_DB")));
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

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
