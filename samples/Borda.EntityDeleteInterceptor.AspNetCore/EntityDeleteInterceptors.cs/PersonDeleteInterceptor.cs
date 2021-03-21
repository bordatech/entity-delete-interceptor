using Borda.EntityDeleteInterceptor.AspNetCore.Entities;
using Borda.EntityDeleteInterceptor.AspNetCore.Exceptions;

namespace Borda.EntityDeleteInterceptor.AspNetCore.EntityDeleteInterceptors.cs
{
    public class PersonDeleteInterceptor : IEntityDeleteInterceptor<Person>
    {
        public void DeletingEntity(Person person)
        {
            if (person.FirstName == "Emre")
            {
                throw new BusinessException("Cannot delete person Emre");
            }
        }
    }
}