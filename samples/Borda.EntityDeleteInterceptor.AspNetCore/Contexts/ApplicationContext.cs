using System;
using Borda.EntityDeleteInterceptor.AspNetCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Borda.EntityDeleteInterceptor.AspNetCore.Contexts
{
    public class ApplicationContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationContext(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.AddEntityDeleteInterceptors(_serviceProvider);
        }

        public DbSet<Person> Persons { get; set; }
    }
}