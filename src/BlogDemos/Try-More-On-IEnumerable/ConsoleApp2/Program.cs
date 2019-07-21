using System;
using System.Threading.Tasks;
using Mahua.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new ClientBuilder()
                .UseKubeGatewayListProvider()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "testcluster";
                    options.ServiceId = "testservice";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(IHelloGrain).Assembly).WithReferences())
                .Build();

            await client.Connect();
            for (int i = 0; i < 100; i++)
            {
                var helloGrain = client.GetGrain<IHelloGrain>(Guid.NewGuid().ToString());
                Console.WriteLine(await helloGrain.GetId());
            }

            await Task.Delay(TimeSpan.FromDays(1));
        }
    }
}