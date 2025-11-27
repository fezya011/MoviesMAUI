using Microsoft.AspNetCore.Builder;
using MyMediator.Extension;
using MyMediator.Interfaces;
using MyMediator.Types;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IMediator, Mediator>();

var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatorHandlers(assembly);

builder.Services.AddScoped<MoviesAPI.CQRS.Commands.AddMovieCommand.AddMovieCommandHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
