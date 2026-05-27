using Api.Endpoints;
using Data.Configuration;
using Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddQueries()
    .AddServices()
    .AddRepositories()
    .AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

app.MapBookEndpoints();
app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
