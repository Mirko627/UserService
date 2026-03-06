using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserService.Business.Interfaces;
using UserService.Business.Mappers;
using UserService.Data.Context;
using UserService.Data.Repositories;
using UserService.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// DbContext + retry SQL
builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("UserDb"),
        sql => sql.EnableRetryOnFailure()
    )
);

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMapper>();
});

// JWT
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
        ValidIssuer = "UserService",
        ValidAudience = "ProjectMicroservizi",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("b133a0c0e9bee3be20163d2ad31d6248db292aa6dcb1ee087a2aa50e0fc75ae2")
        )
    };
});

// Services
builder.Services.AddScoped<IUserService, UserService.Business.Services.UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDBContext>();

    var retries = 10;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch
        {
            retries--;
            Thread.Sleep(3000);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
