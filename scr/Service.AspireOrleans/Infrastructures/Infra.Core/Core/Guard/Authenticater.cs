using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infra.Core.Core.Guard
{
    public class Authenticater
    {
        public static string JwtAuthenticate(string key, string iss, string aud)
        {
            // var jwtConfig = Configuration.GetSection("Jwt");
            //秘钥，就是标头，这里用Hmacsha256算法，需要256bit的密钥
            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            //Claim，JwtRegisteredClaimNames中预定义了好多种默认的参数名，也可以像下面的Guid一样自己定义键名.
            //ClaimTypes也预定义了好多类型如role、email、name。Role用于赋予权限，不同的角色可以访问不同的接口
            //相当于有效载荷
            var claims = new Claim[] {
                new(JwtRegisteredClaimNames.Iss, iss),
                new(JwtRegisteredClaimNames.Aud, aud),
                new("Guid",Guid.NewGuid().ToString("D")),
                new(ClaimTypes.Role,"paperhammer"),
                // new(ClaimTypes.Role,"admin"),
            };
            SecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: securityKey,
                expires: DateTime.Now.AddMinutes(10),//过期时间
                claims: claims
            );

            string content = new JwtSecurityTokenHandler().WriteToken(securityToken);
            //生成jwt令牌
            return content;
        }
    }
}
