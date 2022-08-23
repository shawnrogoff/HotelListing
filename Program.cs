global using Serilog;
global using Microsoft.EntityFrameworkCore;
using HotelListing.Data;
using HotelListing.Startup;

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


builder.AddNewtonsoftJson();
builder.AddStandardServices();
builder.AddUnitOfWork();
builder.AddDataContext();
builder.AddAutoMapper();
builder.AddCorsPolicy();

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
