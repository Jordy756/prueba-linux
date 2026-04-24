using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using MiApi.Application.Commands;
using MiApi.Domain.Ports;
using MiApi.Infrastructure.Adapters;
using MiApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

// Inyección de Dependencias (Patrón de tu ejemplo HiCupon)
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<RegisterUserCommand>();

// Configuración de Base de Datos (PostgreSQL)
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        corsBuilder => corsBuilder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigin"); 
app.UseAuthorization();
app.MapControllers();

app.Run();
