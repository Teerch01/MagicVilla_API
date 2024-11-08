using MagicVilla_VillaAPI.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//use serilog for logging
//Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    //options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


builder.Services.AddScoped<ILogging, Logging>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
