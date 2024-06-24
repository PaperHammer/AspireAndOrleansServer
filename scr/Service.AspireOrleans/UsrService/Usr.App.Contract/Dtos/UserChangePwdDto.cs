using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    /// <summary>
    /// 修改密码数据模型
    /// </summary>
    public class UserChangePwdDto : InputDto
    {
        [Id(0)]
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = string.Empty;

        [Id(1)]
        /// <summary>
        /// 当前密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        [Id(2)]
        /// <summary>
        /// 确认密码
        /// </summary>
        public string RePassword { get; set; } = string.Empty;
    }
}
