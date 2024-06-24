using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto : OutputDto
    {
        [Id(0)]
        /// <summary>
        /// 电邮
        /// </summary>
        public string Email { get; set; } = string.Empty;

        [Id(1)]
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        [Id(2)]
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
