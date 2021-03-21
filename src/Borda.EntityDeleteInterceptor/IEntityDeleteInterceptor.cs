namespace Borda.EntityDeleteInterceptor
{
    /// <summary>
    /// Allows deletion interception for given entity type.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity class.</typeparam>
    public interface IEntityDeleteInterceptor<in TEntity>
    {
        /// <summary>
        /// Called at the start of DbContext.SaveChanges for each entity if entity state is Deleted.
        /// </summary>
        /// <param name="entity">Tracked entity instance.</param>
        void DeletingEntity(TEntity entity);
    }
}