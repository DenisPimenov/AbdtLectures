using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Practice2.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Practice2
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{nameof(Practice2)}.xml");
                c.IncludeXmlComments(filePath);

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "API-KEY",
                    Description = "Api key auth",
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);

                c.SchemaFilter<EnumSchemaFilter>();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseReDoc(c => { c.RoutePrefix = "docs"; });
            app.Use(async (context, func) =>
            {
                if (!context.Request.Headers.TryGetValue("API-KEY", out var key) || !key.Equals("secret-api-key"))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new ApiError(10, "Invalid api key"));
                    return;
                }

                await func();
            });
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }


    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            var desc = Enum.GetNames(context.Type)
                .Aggregate("", (acc, c) => acc + $"{Convert.ToInt64(Enum.Parse(context.Type, c))} - {c} ");

            schema.Description = $"{schema.Description} : {desc}";
        }
    }
}