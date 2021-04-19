using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Borda.EntityDeleteInterceptor
{
    internal class EntityDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityDeleteInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            InvokeInterceptorServices(eventData.Context);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            InvokeInterceptorServices(eventData.Context);
            return ValueTask.FromResult(result);
        }

        private void InvokeInterceptorServices(DbContext context)
        {
            var deletedEntityEntries = context.ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Deleted);

            foreach (var entityEntry in deletedEntityEntries)
            {
                InvokeInterceptorService(entityEntry.Entity);
            }
        }
        
        private void InvokeInterceptorService(object entity)
        {
            var entityType = entity.GetType();
            var interceptorType = typeof(IEntityDeleteInterceptor<>).MakeGenericType(entityType);
            var interceptorService = _serviceProvider.GetService(interceptorType);

            if (interceptorService is null)
            {
                return;
            }

            try
            {
                interceptorType
                    .GetMethod(nameof(IEntityDeleteInterceptor<object>.DeletingEntity))
                    ?.Invoke(interceptorService, new[] {entity});
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}