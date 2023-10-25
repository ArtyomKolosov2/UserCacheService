using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using UserCacheService.Application;
using UserCacheService.Handlers;
using UserCacheService.Infrastructure;
using UserCacheService.Infrastructure.Authentication;
using UserCacheService.Infrastructure.Database;
using UserCacheService.Infrastructure.Database.Context;
using UserCacheService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, null);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<UnhandledExceptionMiddleware>();

if (app.Environment.IsDevelopment()) await PrepareDatabase(app);

await app.RunAsync();

return;

async Task PrepareDatabase(IHost webApplication)
{
    await using var scope = webApplication.Services.CreateAsyncScope();
    var databaseContext = scope.ServiceProvider.GetRequiredService<UserCacheServiceDatabaseContext>();
    if (databaseContext.Database.GetPendingMigrations().Any())
        await databaseContext.Database.MigrateAsync();
    await DatabaseDataSeeder.SeedDatabaseWithDataIfEmpty(databaseContext);
}

public partial class Program { }