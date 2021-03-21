using System.Reflection;
using Borda.EntityDeleteInterceptor.AspNetCore.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Borda.EntityDeleteInterceptor.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationContext>(opt => { opt.UseSqlite("Data Source=database.sqlite"); });

            services.AddEntityDeleteInterceptor(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            AutoMigrate(app);
        }

        private static void AutoMigrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationContext>();

            context.Database.Migrate();
        }
    }
}