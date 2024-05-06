using Microsoft.OpenApi.Models;
using Serilog;
using UPB.LogicPatient.Manager;
using UPB.LogicPatient.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json"
    )
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration.GetSection("Logging").GetSection("FileLocation").Value+"",rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Information("La aplicación se ha iniciado correctamente.");
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<PatientManager>();
builder.Services.AddSingleton<PatientStorage>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["AppSettings:AppName"],
        Version = "v1"
    });
});

var app = builder.Build();
// Configuración del middleware para mostrar la página de error
app.Map("/error", app =>
{
    app.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        Log.Error($"Se ha producido un error en la ruta {exceptionHandlerPathFeature.Path}: {exceptionHandlerPathFeature.Error}");

        await context.Response.WriteAsync($"<h1>Error {context.Response.StatusCode}</h1>");
        await context.Response.WriteAsync($"<p>Se ha producido un error al procesar la solicitud: {exceptionHandlerPathFeature.Error.Message}</p>");
    });
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "QA")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Configuration["AppSettings:AppName"]);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//Run the application
app.Run();
