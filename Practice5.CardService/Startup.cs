using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Practice5.CardService.Consumers;
using Shared;

namespace Practice5.CardService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IssueCardConsumer>();
            services.AddDbContext<CardContext>(builder =>
            {
                var connection = _configuration["Postgres"] ??
                                 "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=mydb;";
                builder.UseNpgsql(connection);
            });

            services.AddControllers();
            services.AddScoped<LogContextMiddleware>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<IssueCardConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Dns.GetHostAddresses(_configuration["Rabbit"] ?? "localhost")[0].ToString());
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<CardContext>().Database.Migrate();

            app.UseMiddleware<LogContextMiddleware>();
            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());
        }
    }
}