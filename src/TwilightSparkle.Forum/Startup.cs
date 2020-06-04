using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Forum.Configurations;
using TwilightSparkle.Forum.DatabaseSeed;
using TwilightSparkle.Forum.Foundation.Authentication;
using TwilightSparkle.Forum.Foundation.ImageService;
using TwilightSparkle.Forum.Foundation.ImageStorage;
using TwilightSparkle.Forum.Foundation.ThreadsManagement;
using TwilightSparkle.Forum.Foundation.UserProfile;
using TwilightSparkle.Forum.Repository.DbContexts;
using TwilightSparkle.Forum.Repository.Interfaces;
using TwilightSparkle.Forum.Repository.UnitOfWork;

namespace TwilightSparkle.Forum
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<DbContext, DatabaseContext>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IImageStorageService, ImageStorageService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IThreadsManagementService, ThreadsManagementService>();

            services.AddScoped<IForumUnitOfWork, ForumUnitOfWork>();

            services.AddSingleton<IHasher, Sha256>();



            services.Configure<ImageStorageConfiguration>(Configuration.GetSection("ImageUploading"));
            services.AddSingleton<IImageStorageConfiguration>(provider =>
            {
                var imageUploadConfigOptions = provider.GetService<IOptions<ImageStorageConfiguration>>();
                var imageUploadConfig = imageUploadConfigOptions.Value;

                return imageUploadConfig;
            });


            services.AddSingleton(Configuration);
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = new PathString("/Home/Login"); });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext appContext)
        {
            DatabaseMigrationSeed.SeedMigrateDatabase(appContext);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "API",
                    pattern: "api/{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{*url}",
                    defaults: new { controller = "App", action = "Index" });
            });
        }
    }
}
