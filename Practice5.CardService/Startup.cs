using System.Net;
using System.Reflection;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Practice5.CardService.Consumers;

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
            services.AddDbContext<CardContext>(builder => builder.UseNpgsql(_configuration["Postgres"]));

            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<IssueCardConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Dns.GetHostAddresses(_configuration["Rabbit"])[0].ToString());
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<CardContext>().Database.Migrate();

            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());
        }
    }
}