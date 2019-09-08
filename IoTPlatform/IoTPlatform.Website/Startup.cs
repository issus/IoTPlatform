using IoTPlatform.Data.EntityFramework;
using IoTPlatform.Data.Identity;
using IoTPlatform.Website.Configuration;
using IoTPlatform.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartBreadcrumbs.Extensions;

namespace IoTPlatform.Website
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));

            /*****
             * SQLite is used for an easy demo, no database server is required for it to work.
             * You can find the connection strings in appsettings.json
             * 
             * Alternative Database Options.
             *****************************************
             *** Microsoft SQL Server              ***
             * services.AddDbContext<ApplicationDbContext>(options =>
             *    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
             * 
             *** MySQL Server                      ***
             * services.AddDbContext<ApplicationDbContext>(options =>
             *     options.UseMySQL();
             *     
             *** SQLite File                       ***
             * services.AddDbContext<ApplicationDbContext>(options =>
             *****/

            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Load SendGrid settings from appsettings.json
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));
            services.AddSingleton(Configuration);


            services.AddDefaultIdentity<WebProfileUser>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register email service. Configured in appsettings.json
            services.AddTransient<IEmailSender, SendGridEmailService>();


            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                options.TagName = "small";
                options.TagClasses = "";
                options.OlClasses = "";
                options.LiClasses = "";
                options.ActiveLiClasses = "c-white";
                options.SeparatorElement = "<br >";
            });

            // Register our user in role handler for security
            services.AddSingleton<IAuthorizationHandler, UserInRoleHandler>();

            // Create an admins policy group for high level security requirements
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admins", policy =>
                    policy.Requirements.Add(new UserInRoleRequirement(UserRoles.Administrator)));
            });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                // High level pages security setup
                options.Conventions.AllowAnonymousToPage("/Privacy");
                options.Conventions.AllowAnonymousToPage("/Terms");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}