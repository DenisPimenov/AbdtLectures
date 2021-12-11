using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Practice5.ApiGateway.Apis;
using Shared;
using StackExchange.Redis;
using MassTransit;

namespace Practice5.ApiGateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        private ConnectionMultiplexer ConnectToRedis() => ConnectionMultiplexer.Connect(_configuration["Redis"]);
        
        public void ConfigureServices(IServiceCollection services)
        {
            var redis = ConnectToRedis();

            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<Cache>();
            services.AddHttpClient<ICardApiClient, CardApiClient>(
                client => client.BaseAddress = new Uri(_configuration["CardApi"]));

            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context,cfg) =>
                {
                    cfg.Host(Dns.GetHostAddresses(_configuration["Rabbit"])[0].ToString());
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}