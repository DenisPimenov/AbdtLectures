using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Practice5.ApiGateway.Apis;
using Shared;
using StackExchange.Redis;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Practice5.ApiGateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        private ConnectionMultiplexer ConnectToRedis() =>
            ConnectionMultiplexer.Connect(_configuration["Redis"] ?? "localhost");

        public void ConfigureServices(IServiceCollection services)
        {
            var redis = ConnectToRedis();

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddScoped<LogContextMiddleware>();

            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<Cache>();

            services.AddHttpClient<ICardApiClient, CardApiClient>(
                    client => { client.BaseAddress = new Uri(_configuration["CardApi"] ?? "http://localhost:5001"); })
                .AddHttpMessageHandler(provider =>
                    new LoggingHandler(provider.GetRequiredService<IHttpContextAccessor>()))
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(25)))
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i)))
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .CircuitBreakerAsync(10, TimeSpan.FromMinutes(5)))
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5)));

            services.AddMassTransit(x =>
            {
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
            app.UseMiddleware<LogContextMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}