using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.AddServiceDefaults();

        builder.AddKeyedAzureTableClient("clustering");
        builder.UseOrleansClient();

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

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
