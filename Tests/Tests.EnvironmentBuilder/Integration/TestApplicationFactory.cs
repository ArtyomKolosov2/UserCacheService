using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Infrastructure.Database.Context;

namespace Tests.EnvironmentBuilder.Integration;

public class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configurationBuilder.AddJsonFile("appsettings.Tests.json", optional: false, reloadOnChange: true);
            });
            
            var databaseDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<UserCacheServiceDatabaseContext>));
            services.Remove(databaseDescriptor);

            services.AddDbContext<UserCacheServiceDatabaseContext>(options => { options.UseInMemoryDatabase(nameof(UserCacheServiceDatabaseContext)); });
            
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<UserCacheServiceDatabaseContext>();
            
            db.Database.EnsureCreated();
        });
    }
}