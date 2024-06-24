using System.Text;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Service.Config
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string configFileName = "appsettings.json";
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string rootDir = Path.GetFullPath(Path.Combine(currentDirectory, "..\\..\\..\\"));
            string configPath = Path.Combine(rootDir, configFileName);
            var rabbitMqHost = "config_rabbitmq_host";
            var watch = new ConfigFileWatcher(configPath, rabbitMqHost);

            await watch.NotifyConfigUpdateAsync();

            Console.WriteLine("Monitoring for configuration file changes...");
            Console.ReadLine();
        }
    }

    public class ConfigFileWatcher
    {
        private readonly string _rabbitMqHost;
        private FileSystemWatcher _watcher;
        private string _filePath;

        public ConfigFileWatcher(string filePath, string rabbitMqHost)
        {
            _filePath = filePath;
            _rabbitMqHost = rabbitMqHost;
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath))
            {
                Filter = Path.GetFileName(filePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"Configuration file changed: {e.FullPath}");
            try
            {
                await NotifyConfigUpdateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error notifying of config update: {ex.Message}");
            }
        }

        internal async Task NotifyConfigUpdateAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                ContinuationTimeout = new(int.MaxValue / 2),
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "config_updates", type: ExchangeType.Fanout);
            var newConfig = ReadEmailConfigFromJson();
            var messageBody = Encoding.UTF8.GetBytes(newConfig);
            channel.BasicPublish(exchange: "config_updates", routingKey: "", basicProperties: null, body: messageBody);

            Console.WriteLine("Notification sent.");
        }

        private string? ReadEmailConfigFromJson()
        {
            try
            {
                using (FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read))
                {
                    using var reader = new StreamReader(fs);
                    string jsonContent = reader.ReadToEnd(); 

                    JObject jsonObject = JObject.Parse(jsonContent); // 将字符串内容解析成JObject
                    JObject emailConfig = (JObject)jsonObject["EmailConfig"]; // 提取"EmailConfig"部分

                    return emailConfig.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return null;
            }
        }
    }
}
