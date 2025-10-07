using Carter;
using DigitalWallet.Configuration;
using Scalar.AspNetCore;
using ServiceCollector.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigureDbContexts(builder.Configuration);
builder.Services.ConfigureValidator();
builder.Services.ConfigureSecondLevelCache();
builder.Services.AddCarter();
builder.Services.AddServiceDiscovery();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();

app.Run();


