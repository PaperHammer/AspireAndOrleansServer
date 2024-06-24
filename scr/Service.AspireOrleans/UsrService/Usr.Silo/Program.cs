using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Runtime;
using Usr.Repository.Core;

namespace Usr.Silo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("silo.json")
                .Build();
            var sections = configuration.GetSection("host");

            int siloPort = int.Parse(sections["SiloPort"]);
            int gatewayPort = int.Parse(sections["GatewayPort"]);
            string serviceId = sections["ServiceId"];
            string clusterId = sections["ClusterId"];

            await InitSilo(siloPort, gatewayPort, serviceId, clusterId);
        }

        private static async Task InitSilo(int siloPort, int gatewayPort, string seviceId, string clusterId)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder()
                .UseOrleans(silo =>
                {
                    silo.UseLocalhostClustering(
                        siloPort,
                        gatewayPort,
                        null,
                        seviceId,
                        clusterId)
                        .ConfigureLogging(logging => logging.AddConsole());
                    silo.Services.AddSingleton<DbContext, UserDbContext>();
                    silo.Services.AddDbContextPool<UserDbContext>(options =>
                        options.UseSqlServer(silo.Configuration.GetValue<string>("SqlConnectionStrings"), sqlOptions =>
                        {
                            // 如果DBContext和启动程序不在一个程序集，需要指定要迁移的程序集。代码中添加要迁移的程序集名称
                            sqlOptions.MigrationsAssembly("Usr.Repository");
                            // Workround for https://github.com/dotnet/aspire/issues/1023
                            sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
                        }));
                })
                .UseConsoleLifetime();

            using IHost host = builder.Build();

            await host.RunAsync();
        }
    }
}
