using Learning.Interfaces;
using Learning.Models;
using Learning.Repositories;
using Learning.Ticker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddScoped<IAnimals, AnimalsRepository>();

// Add hosted service
builder.Services.AddHostedService<TickerBackgroundService>();

// Add singleton for the background service
builder.Services.AddSingleton<TickerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
