using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Borda.EntityDeleteInterceptor
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        public static void AddEntityDeleteInterceptor(this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.Scan(scan =>
            {
                assemblies ??= new[] {Assembly.GetEntryAssembly()};

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IEntityDeleteInterceptor<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder AddEntityDeleteInterceptors(this DbContextOptionsBuilder builder,
            IServiceProvider serviceProvider)
        {
            return builder.AddInterceptors(new EntityDeleteInterceptor(serviceProvider));
        }
    }
}