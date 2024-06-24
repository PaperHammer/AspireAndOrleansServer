using Infra.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using Usr.App.Contract.Util;
using Usr.Repository.Core;

namespace Usr.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        //builder.AddKeyedRedisClient("redis");
        builder.AddKeyedAzureTableClient("clustering");
        builder.AddKeyedAzureBlobClient("grain-state");
        builder.UseOrleans(orleansBuilder =>
        {
            if (builder.Environment.IsDevelopment())
            {
                //orleansBuilder.ConfigureEndpoints(Random.Shared.Next(10_000, 50_000), Random.Shared.Next(10_000, 50_000));
                orleansBuilder.ConfigureEndpoints(PortUtil.GetAvailablePort(IPAddress.Loopback), PortUtil.GetAvailablePort(IPAddress.Loopback));
                //orleansBuilder.Services.AddSingleton<DbContext, UserDbContext>();
                orleansBuilder.ConfigureLogging(logging => logging.AddConsole());            
            }
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddDbContextPool<UserDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetValue<string>("SqlConnectionStrings"), sqlOptions =>
            {
                // ���DBContext������������һ�����򼯣���Ҫָ��ҪǨ�Ƶĳ��򼯡����������ҪǨ�Ƶĳ�������
                sqlOptions.MigrationsAssembly("Usr.Repository");
                // Workround for https://github.com/dotnet/aspire/issues/1023
                sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
            })
        );
        builder.EnrichSqlServerDbContext<UserDbContext>(settings =>
            // Disable Aspire default retries as we're using a custom execution strategy
            settings.DisableRetry = true);

        var symmetricKeyAsBase64 = builder.Configuration.GetValue<string>("JwtSecret");
        var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        //��֤����
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��
                    IssuerSigningKey = signingKey,//���ܵ���Կ
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("JwtIss"),//������
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetValue<string>("JwtAud"),//������
                    ValidateLifetime = true,//�Ƿ���֤����ʱ��
                    ClockSkew = TimeSpan.Zero,//����ǻ������ʱ��
                    RequireExpirationTime = true,
                };
            });

        JwtConfigUtil.Key = builder.Configuration.GetValue<string>("JwtSecret");
        JwtConfigUtil.Iss = builder.Configuration.GetValue<string>("JwtIss");
        JwtConfigUtil.Aud = builder.Configuration.GetValue<string>("JwtAud");

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

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
