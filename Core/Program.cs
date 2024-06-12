using Core.Jw;
using Core.Jwt;
using Core.Repository;
using Core.Repository.Contracts;
using Core.Security;
using Core.Services;
using Core.Services.Contracts;
using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//DbContext
var connString = builder.Configuration.GetConnectionString("DevelopmentConnection");
builder.Services.AddDbContext<AuthDbContext>(options => 
{
    options.UseNpgsql(connString, builder => builder.MigrationsAssembly("Core"));
});

//JWT configuration
var jwtConfig = builder.Configuration.GetRequiredSection("JwtToken");
builder.Services.Configure<JwtOptions>(jwtConfig);

//Dependencies
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<JwtService>();

//Authentication with JWT Bearer Tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options => 
    {
        var key = Convert.FromBase64String(jwtConfig["Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    }
);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

