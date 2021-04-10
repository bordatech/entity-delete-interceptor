using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Borda.EntityDeleteInterceptor
{
    public static class Extensions
    {
        /// <summary>
        /// Scans and adds <see cref="IEntityDeleteInterceptor{TEntity}"/> implementation instances to the service provider as scoped.
        /// </summary>
        /// <param name="services">to add services to.</param>
        /// <param name="assemblies">Assemblies to scan <see cref="IEntityDeleteInterceptor{TEntity}"/> implementations.</param>
        public static void AddEntityDeleteInterceptor(this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.Scan(scan =>
            {
                if (!assemblies.Any())
                {
                    assemblies = new[] {Assembly.GetEntryAssembly()};
                }

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IEntityDeleteInterceptor<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        /// <summary>
        /// Adds the interceptor to handle <see cref="IEntityDeleteInterceptor{TEntity}"/> DeletingEntity method.
        /// </summary>
        /// <param name="builder">to add interceptor.</param>
        /// <param name="serviceProvider">To get registered <see cref="IEntityDeleteInterceptor{TEntity}"/> implementations.</param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static DbContextOptionsBuilder AddEntityDeleteInterceptors(this DbContextOptionsBuilder builder,
            IServiceProvider serviceProvider)
        {
            return builder.AddInterceptors(new EntityDeleteInterceptor(serviceProvider));
        }
    }
}