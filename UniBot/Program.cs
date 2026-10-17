using Microsoft.EntityFrameworkCore;
using Infrastructure.DbContext;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Services;
using Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


   

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


app.UseCors("React");
app.UseAuthorization();

app.MapControllers();

app.Run();
