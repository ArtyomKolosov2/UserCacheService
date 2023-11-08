using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UserCacheService.Application;
using UserCacheService.Application.UserInfo.Cache.Background;
using UserCacheService.Handlers;
using UserCacheService.Infrastructure;
using UserCacheService.Infrastructure.Authentication;
using UserCacheService.Infrastructure.Database;
using UserCacheService.Infrastructure.Database.Context;
using UserCacheService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddXmlSerializerFormatters()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHostedService<UserInfoCacheBackgroundUpdateService>();

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, options => {  });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(BasicAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.UseMiddleware<UnhandledExceptionMiddleware>();

if (app.Environment.IsDevelopment()) await PrepareDatabase(app);

await app.RunAsync();

return;

async Task PrepareDatabase(IHost webApplication)
{
    await using var scope = webApplication.Services.CreateAsyncScope();
    var databaseContext = scope.ServiceProvider.GetRequiredService<UserCacheServiceDatabaseContext>();
    if (databaseContext.Database.IsRelational() && databaseContext.Database.GetPendingMigrations().Any())
        await databaseContext.Database.MigrateAsync();
    await DatabaseDataSeeder.SeedDatabaseWithDataIfEmpty(databaseContext);
}

public partial class Program { }