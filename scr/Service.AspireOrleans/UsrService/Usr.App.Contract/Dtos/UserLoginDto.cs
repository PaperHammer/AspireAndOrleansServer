using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    /// <summary>
    /// 登录信息
    /// </summary>
    public class UserLoginDto : InputDto
    {
        [Id(0)]
        /// <summary>
        /// 账户
        /// </summary>
        public string Email { get; set; } = string.Empty;

        [Id(1)]
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
