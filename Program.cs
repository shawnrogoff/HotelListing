global using Serilog;
global using Microsoft.EntityFrameworkCore;
using HotelListing.Data;
using HotelListing.Startup;
using HotelListing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(path: "c:\\hotellistings\\logs\\log-.txt", 
        outputTemplate:"{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();


builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.AddScoped<IAuthManager, AuthManager>();

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

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
