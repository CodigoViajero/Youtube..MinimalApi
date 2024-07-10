using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Minimal.Api.Extensions;

public static class AuthServices
{
    public static WebApplicationBuilder AddApplicationAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.TokenValidationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:Key"]!)),
                ValidIssuer = "http://localhost:5000",
                ValidAudience = "Product"
            };
            o.RequireHttpsMetadata = false;
            o.IncludeErrorDetails = true;
            o.SaveToken = true;

            o.Events = new JwtBearerEvents()
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.ContentType = "Application/json";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    var error = context.Error == null ? "No está autorizado" : context.Error;
                    var description = context.ErrorDescription == null ? "Debe proporcionar un token válido" : context.ErrorDescription;

                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        new
                        {
                            error,
                            description
                        }
                    ));
                },
            };
        });

        builder.Services.AddAuthorization(o =>
        {
            o.AddPolicy("MyPolicy", policy =>
            {
                policy.RequireClaim(ClaimTypes.Email, "viajero.ejemplo.com");
            });
        });

        return builder;
    }
}