using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Practice2;

var host = Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(hostBuilder => hostBuilder.UseStartup<Startup>())
    .Build();

host.Run();