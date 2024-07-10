using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data;

namespace Minimal.Api.Extensions;

public static class ContextServices
{
    public static WebApplicationBuilder AddAppContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ProductDbContext>(options => 
            options.UseSqlServer(builder.Configuration["ConnectionString"])
        );

        return builder;
    }
}