using Orleans;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    public class UserCreationDto : UserCreationAndUpdationDto
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Id(0)]
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// 确认密码
        /// </summary>
        [Id(1)]
        public string RePassword { get; set; } = string.Empty;
    }
}
