using Email.Api.Utils;
using Email.App.Contract.Utils;
using Infra.Helper;
using RabbitMQ.Client;
using System.Net;

namespace Email.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        //builder.AddRabbitMQClient("messaging");

        builder.AddKeyedAzureTableClient("clustering");
        builder.AddKeyedAzureBlobClient("grain-state");
        builder.UseOrleans(orleansBuilder =>
        {
            if (builder.Environment.IsDevelopment())
            {
                //orleansBuilder.ConfigureEndpoints(Random.Shared.Next(10_000, 50_000), Random.Shared.Next(10_000, 50_000));
                orleansBuilder.ConfigureEndpoints(PortUtil.GetAvailablePort(IPAddress.Loopback), PortUtil.GetAvailablePort(IPAddress.Loopback));
                orleansBuilder.ConfigureLogging(logging => logging.AddConsole());             
            }
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddSingleton<RabbitMQReceiverService>();
        builder.Services.AddHostedService<RabbitMQBackgroundService>(); // ×¢²áÍÐ¹Ü·þÎñ

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();       

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
