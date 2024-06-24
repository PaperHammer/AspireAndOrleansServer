using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    [GenerateSerializer]
    public class UserSetRoleDto : InputDto
    {
        [Id(0)]
        public long[] RoleIds { get; set; } = [];
    }
}
