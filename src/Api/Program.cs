using Api.Modules;
using Api.Modules.RouteFiltering;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddOpenApi();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
});

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference("/", options =>
    {
        // Can add UI customization here
        options.Theme = ScalarTheme.DeepSpace;
    });
}

await app.InitialiseDb();

app.MapControllers();

app.UseHttpsRedirection();

await app.RunAsync();

public partial class Program;