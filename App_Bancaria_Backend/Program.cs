using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Repositories.Admin;
using App_Bancaria_Backend.Repositories.GrupoAhorros;
using App_Bancaria_Backend.Repositories.Security;
using App_Bancaria_Backend.Repositories.Transacciones;
using App_Bancaria_Backend.Services.Admin;
using App_Bancaria_Backend.Services.GrupoAhorros;
using App_Bancaria_Backend.Services.Security;
using App_Bancaria_Backend.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args); // 🔥 PRIMERO

// 🔐 JWT CONFIG
var jwtSettings = builder.Configuration.GetSection("Jwt");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "CLAVE_TEMPORAL_123456")
    ),
        ClockSkew = TimeSpan.Zero // 🔥 IMPORTANTE
    };
});

// 🔹 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token así: Bearer {tu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
});
});

// 🔹 DI
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ITransaccionesRepository, TransaccionesRepository>();
builder.Services.AddScoped<TransaccionesService>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IGrupoAhorroRepository, GrupoAhorroRepository>();
builder.Services.AddScoped<GrupoAhorrosService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<AdminService>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Middleware
app.UseHttpsRedirection();
app.UseAuthentication(); // 🔥 IMPORTANTE
app.UseAuthorization();

app.MapControllers();

app.Run();