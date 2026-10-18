
using Microsoft.EntityFrameworkCore;
using Infrastructure.DbContext;
using Infrastructure.Services;
using Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IAuthInterface, AuthService>();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS configuration — MUST be before builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Build the application
var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("React");

app.UseAuthorization();

app.MapControllers();

app.Run();
