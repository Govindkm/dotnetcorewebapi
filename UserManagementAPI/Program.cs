using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Middlewares;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure in-memory database for simplicity.
builder.Services.AddDbContext<UserContext>(options =>
    options.UseInMemoryDatabase("UserDB"));

builder.Services.AddSingleton<TokenService>();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
