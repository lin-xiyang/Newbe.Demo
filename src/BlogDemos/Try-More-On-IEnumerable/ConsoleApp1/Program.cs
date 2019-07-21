using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Mahua.Implements;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;

namespace ConsoleApp1
{
    class Program
    {
        private static ISiloHost silo;
        private static readonly ManualResetEvent siloStopped = new ManualResetEvent(false);

        static async Task Main(string[] args)
        {
            Console.WriteLine("starting");
            var builder = new SiloHostBuilder();
            var host = builder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "testcluster";
                    options.ServiceId = "testservice";
                })
                .ConfigureLogging(logging => logging.AddConsole())
#if DEBUG
                    .UseLocalhostClustering()
#else
                .ConfigureEndpoints(new Random(1).Next(10001, 10100), new Random(1).Next(20001, 20100))
                .UseKubeMembership(opt =>
                {
                    opt.CanCreateResources = true;
//                        opt.DropResourcesOnInit = true;
                })
#endif
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .AddMemoryGrainStorageAsDefault()
                .UseDashboard(x => { x.Port = 9090; })
                .Build();
            Console.WriteLine("started");
            await host.StartAsync();
            AssemblyLoadContext.Default.Unloading += context =>
            {
                Task.Run(StopSilo);
                siloStopped.WaitOne();
            };

            siloStopped.WaitOne();
        }

        private static async Task StopSilo()
        {
            await silo.StopAsync();
            Console.WriteLine("Silo stopped");
            siloStopped.Set();
        }
    }
}