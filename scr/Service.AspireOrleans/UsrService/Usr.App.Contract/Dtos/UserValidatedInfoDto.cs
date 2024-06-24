using Usr.App.Contract.Util;
using Infra.Core.Core.Guard;
using Orleans;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    public class UserValidatedInfoDto : UserInfoDto
    {
        public string Token => Authenticater.JwtAuthenticate(
            JwtConfigUtil.Key, JwtConfigUtil.Iss, JwtConfigUtil.Aud);

        //public string Token { get; init; } = Guid.NewGuid().ToString("N");
    }
}
