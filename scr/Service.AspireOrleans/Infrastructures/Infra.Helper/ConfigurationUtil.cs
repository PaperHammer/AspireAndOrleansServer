using Microsoft.Extensions.Configuration;

namespace Infra.Helper
{
    public class ConfigurationUtil
    {
        public static readonly IConfiguration Configuration;

        static ConfigurationUtil()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Configuration = configurationBuilder;
        }
    }
}
