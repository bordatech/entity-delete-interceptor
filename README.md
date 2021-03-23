# Borda.EntityDeleteInterceptor

The persistence layer is required for delete operations while business logics can handle create and update entities in the domain layer such as validating business rules and publishing domain events. This project has been developed to be a solution to the problem. Using this project that uses the ISaveChangesInterceptor feature that comes with EFCore 5.0, you can write your own entity delete interceptors and register them automatically.

# Usage

To scan and register all of your `IEntityDeleteInterceptor<>` implementations:

``` cs

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEntityDeleteInterceptor(Assembly.GetExecutingAssembly());
    }
}
```
And add entity delete interceptors in `OnConfiguring` method of your DbContext class:

``` cs
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
        optionsBuilder.AddEntityDeleteInterceptors(_serviceProvider);
    }
}
```

## Sample Interceptor

This is a sample entity delete interceptor for `Person` entity. It throws an exception if first name is "Emre" :smirk:

``` cs
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
```
