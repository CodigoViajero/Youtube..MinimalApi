using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Minimal.Api.Apis;
using Minimal.Api.Data;
using Minimal.Api.Dtos;
using Minimal.Api.Extensions;
using Minimal.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppContext();

builder.AddApplicationAuth();

builder.Services.AddTransient<IRepo, ProductRepo>();

builder.Services.AddAutoMapper(typeof(Profiles));

builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapProductApi();

app.Run();
