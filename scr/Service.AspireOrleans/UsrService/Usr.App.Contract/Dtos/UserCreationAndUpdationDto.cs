using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    public abstract class UserCreationAndUpdationDto : InputDto
    {
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Id(0)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        [Id(1)]
        public string UserName { get; set; } = string.Empty;
    }
}
