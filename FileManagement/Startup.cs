
using FileManagement.AppDbContext;
using FileManagement.Models;
using FileManagement.Repositories;
using FileManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace FileManagement
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
            services.AddControllers();

            // For Entity Framework
            services.AddDbContext<FileManagementDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddHttpClient<IUserManagementService, UserManagementService>(c =>
                 c.BaseAddress = new Uri(Configuration["ApiConfigs:UserManagement:Uri"]));

            services.AddScoped<ICustomAuthorizeService, CustomAuthorizeService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IFileDetailsRepository, FileDetailsRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
