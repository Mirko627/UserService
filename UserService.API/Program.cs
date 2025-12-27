using Microsoft.EntityFrameworkCore;
using UserService.Business.Interfaces;
using UserService.Business.Mappers;
using UserService.Data.Context;
using UserService.Data.Repositories;
using UserService.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDb") ?? throw new InvalidOperationException("Connection string 'UserDb' not found.")));

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMapper>();
});

// Services e repository
builder.Services.AddScoped<IUserService, UserService.Business.Services.UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
