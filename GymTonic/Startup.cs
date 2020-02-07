using GymTonic.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;

namespace GymTonic
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
            createDb();
            services.AddDbContext<GymDataContest>(options => options.UseSqlite("Data Source=C:\\Users\\dani1\\source\\repos\\GymTonic\\GymTonic\\GymTonic.db"));
            services.AddIdentity<IdentityUser, IdentityRole>().
                    AddEntityFrameworkStores<GymDataContest>();
            services.AddControllersWithViews();
            services.AddMvc( options=> {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
        }
        private  void CreateAdmin(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                IdentityUser user = new IdentityUser()
                {
                    UserName = "Admin@gymTonic.com",
                    Email = "Admin@gymTonic.com",
                    EmailConfirmed = true
                };
                userManager.CreateAsync(user, "Admin12345!");
            }
         }

        private void createDb()
        {
            DbContextOptionsBuilder<GymDataContest> options = new DbContextOptionsBuilder<GymDataContest>();
            options = options.UseSqlite("Data Source=C:\\Users\\dani1\\source\\repos\\GymTonic\\GymTonic\\GymTonic.db");
            using (var context = new GymDataContest(options.Options))
            {
                context.Database.EnsureCreated();
                //context.Database.Migrate();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
             
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            CreateAdmin(app);
        }
        
    }
}
