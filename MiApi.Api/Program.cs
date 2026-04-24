using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

// CORS (Igual que tu ejemplo de NestJS)
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
