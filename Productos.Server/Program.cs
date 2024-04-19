using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Productos.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n del servicio de base de datos (Entity Framework Core)
builder.Services.AddDbContext<ProductosContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Productos API", Version = "v1" });
});

// Agregar el servicio de Autorizaci�n
builder.Services.AddAuthorization();

// Agregar el servicio de Controladores
builder.Services.AddControllers(); // Agrega el servicio de controladores

var app = builder.Build();

// Configuraci�n del middleware de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productos API v1"));
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("ReactPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
