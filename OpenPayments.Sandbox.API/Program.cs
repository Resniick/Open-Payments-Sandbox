using OpenPayments.Sandbox.Core;
using OpenPayments.Sandbox.Infrastructure;
using OpenPayments.Sandbox.Infrastructure.Database;
using OpenPayments.Sandbox.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDataSource(ResourceNames.Database);

builder.Services.AddSingleton<IBankRepository, BankRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var connectionString = app.Configuration.GetConnectionString(ResourceNames.Database);
DatabaseUpgrader.upgrade(loggerFactory, connectionString);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
