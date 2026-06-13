using CrudRestApi.Models;
using CrudRestApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRepository<User>>(
    new InMemoryRepository<User>(u => u.Id, (u, id) => u.Id = id));

builder.Services.AddSingleton<IRepository<Product>>(
    new InMemoryRepository<Product>(p => p.Id, (p, id) => p.Id = id));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();