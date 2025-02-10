using Microsoft.EntityFrameworkCore;
using ItemsDeTrabajo.Models;
using ItemsDeTrabajo.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'DbConnection' no se encontró en la configuración.");
}
builder.Services.AddDbContext<ItemsTrabajoContext>(options =>
    options.UseNpgsql(connectionString));
// Registrar el servicio de negocio.
builder.Services.AddScoped<IItemstrabajoService, ItemstrabajoService>();
builder.Services.AddHttpClient<AsignacionService>(); // HttpClient inyectado en AsignacionService
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
