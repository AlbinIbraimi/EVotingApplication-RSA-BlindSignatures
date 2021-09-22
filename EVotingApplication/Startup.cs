using EVotingApplication.Data;
using EVotingApplication.Models.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication
{
    public class Startup
    {
        public static Guid LogedInUserId;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<VotersIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //populate the VotersCredentials table with testData, in real world this data should be send via phone number to the voters.          
            CreateDefaultVoters_PopulateVotersCredentials(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void CreateDefaultVoters_PopulateVotersCredentials(IServiceProvider serviceProvider) 
        {
            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<VotersIdentity>>();

            List<VotersIdentity> voters = new List<VotersIdentity>()
            {
                new VotersIdentity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Albin",
                    Surname = "Ibraimi",
                    UserName = "190", // the username its equal with the EMBG, now we can search for users using the _userManager.FindByName() using embg
                    EMBG = "190",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber="077-000-000"
                },

                 new VotersIdentity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "David",
                    Surname = "Anastasov",
                    UserName = "290", // the username its equal with the EMBG, now we can search for users using the _userManager.FindByName() using embg
                    EMBG = "290",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber="077-000-000"
                },

                  new VotersIdentity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Novica",
                    Surname = "Jovanovski",
                    UserName = "390", // the username its equal with the EMBG, now we can search for users using the _userManager.FindByName() using embg
                    EMBG = "390",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber="077-000-000"
                },

                   new VotersIdentity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Simona",
                    Surname = "Anchova",
                    UserName = "490", // the username its equal with the EMBG, now we can search for users using the _userManager.FindByName() using embg
                    EMBG = "490",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber="077-000-000"
                }
            };

            foreach(var voter in voters) 
            {
                if (_userManager.FindByNameAsync(voter.EMBG).Result == null) 
                {
                    var create = _userManager.CreateAsync(voter);
                    create.Wait();

                    // this object can be creared with random userName and randomPassword.
                    VotersCredentials credentials = new VotersCredentials()
                    {
                        Id = Guid.NewGuid(),
                        VoterId = Guid.Parse(voter.Id),
                        username = voter.Name + voter.EMBG,
                        password = voter.Surname + voter.EMBG,
                        voted = false
                    };
                    _context.VotersCredentials.Add(credentials);
                    _context.SaveChanges();
                }
            }

            var credentialsList = _context.VotersCredentials.ToList();
            foreach(var c in credentialsList)
            {
                c.voted = false;
                _context.VotersCredentials.Update(c);
                _context.SaveChanges();
            }
        }
    }
}
