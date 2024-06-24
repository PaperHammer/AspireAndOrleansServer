using Infra.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Odr.Repository.Core;
using System.Net;
using System.Text;

namespace Odr.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.AddKeyedAzureTableClient("clustering");
        builder.AddKeyedAzureBlobClient("grain-state");
        builder.UseOrleans(orleansBuilder =>
        {
            if (builder.Environment.IsDevelopment())
            {
                //orleansBuilder.ConfigureEndpoints(Random.Shared.Next(10_000, 50_000), Random.Shared.Next(10_000, 50_000));
                orleansBuilder.ConfigureEndpoints(PortUtil.GetAvailablePort(IPAddress.Loopback), PortUtil.GetAvailablePort(IPAddress.Loopback));
                orleansBuilder.Services.AddSingleton<DbContext, OrderDbContext>();
                orleansBuilder.ConfigureLogging(logging => logging.AddConsole());
            }
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var s = builder.Configuration.GetValue<string>("SqlConnectionStrings");
        builder.Services.AddDbContextPool<OrderDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetValue<string>("SqlConnectionStrings"), sqlOptions =>
            {
                // 如果DBContext和启动程序不在一个程序集，需要指定要迁移的程序集。代码中添加要迁移的程序集名称
                sqlOptions.MigrationsAssembly("Odr.Repository");
                // Workround for https://github.com/dotnet/aspire/issues/1023
                sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
            })
        );
        builder.EnrichSqlServerDbContext<OrderDbContext>(settings =>
            // Disable Aspire default retries as we're using a custom execution strategy
            settings.DisableRetry = true);

        var symmetricKeyAsBase64 = builder.Configuration.GetValue<string>("JwtSecret");
        var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        //认证参数
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//是否验证签名
                    IssuerSigningKey = signingKey,//解密的密钥
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("JwtIss"),//发行人
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetValue<string>("JwtAud"),//订阅人
                    ValidateLifetime = true,//是否验证过期时间
                    ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间
                    RequireExpirationTime = true,
                };
            });

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
