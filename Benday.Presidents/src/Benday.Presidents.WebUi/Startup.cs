using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Features;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Benday.DataAccess;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Benday.Presidents.Api.Models;
using Benday.Presidents.WebUI.Controllers;
using Benday.Presidents.WebUi.Data;
using Microsoft.AspNetCore.Identity;
using Benday.Presidents.WebUi.Security;
using Microsoft.AspNetCore.Authorization;

namespace Benday.Presidents.WebUi
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
                            options.UseSqlServer(
                                Configuration.GetConnectionString("default")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 2;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            RegisterTypes(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddXmlSerializerFormatters();
            
            /*
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddXmlSerializerFormatters();
            */
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SecurityConstants.PolicyName_EditPresident,
                                  policy => policy.Requirements.Add(
                                      new EditPresidentRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, EditPresidentHandler>();            
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UsePopulateSubscriptionClaimsMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=President}/{action=Index}/{id?}");
            });

            CheckDatabaseHasBeenDeployed(app);
        }

        private void CheckDatabaseHasBeenDeployed(IApplicationBuilder app)
        {
            using (var scope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<PresidentsDbContext>())
                {
                    context.Database.Migrate();
                }

                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();

            services.AddTransient<IFeatureManager, FeatureManager>();

            services.AddTransient<Api.Services.ILogger, Logger>();

            services.AddDbContext<PresidentsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddTransient<IPresidentsDbContext, PresidentsDbContext>();

            services.AddTransient<IRepository<Person>, SqlEntityFrameworkPersonRepository>();

            services.AddTransient<IValidatorStrategy<President>, DefaultValidatorStrategy<President>>();
            services.AddTransient<IDaysInOfficeStrategy, DefaultDaysInOfficeStrategy>();

            services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();

            services.AddTransient<IPresidentService, PresidentService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();

            services.AddTransient<ITestDataUtility, TestDataUtility>();

            services.AddTransient<PopulateSubscriptionClaimsMiddleware>();

            services.AddTransient<IUserAuthorizationStrategy, DefaultUserAuthorizationStrategy>();

            services.AddTransient<IUserClaimsPrincipalProvider, 
                HttpContextUserClaimsPrincipalProvider>();            
        }
    }
}
