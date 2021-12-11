using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Practice5.ApiGateway;

var host = Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(hostBuilder => hostBuilder.UseStartup<Startup>())
    .Build();

host.Run();




