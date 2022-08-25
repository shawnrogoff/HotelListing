global using Microsoft.EntityFrameworkCore;
global using Serilog;
using AspNetCoreRateLimit;
using HotelListing.Core.Services;
using HotelListing.Core.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(path: "c:\\hotellistings\\logs\\log-.txt",
        outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureRateLimiting();
builder.Services.ConfigureAutoMapper();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();
builder.Services.AddAuthentication();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddResponseCaching();

builder.AddNewtonsoftJson();
builder.AddStandardServices();
builder.AddUnitOfWork();
builder.AddDataContext();
builder.AddAutoMapper();
builder.AddCorsPolicy();
builder.AddSwaggerDoc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();

app.ConfigureExceptionHandler();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
