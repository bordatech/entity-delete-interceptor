namespace Borda.EntityDeleteInterceptor
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityDeleteInterceptor<in TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void DeletingEntity(TEntity entity);
    }
}