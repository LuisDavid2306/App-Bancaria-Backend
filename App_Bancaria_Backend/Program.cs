using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.Repositories.Security;
using App_Bancaria_Backend.Services.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Servicios de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Inyección de dependencias
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// 🔹 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Map controllers
app.MapControllers();

app.Run();