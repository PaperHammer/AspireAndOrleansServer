using Orleans;

namespace Shared.App.Dtos.Base
{
    [GenerateSerializer]
    public abstract class OutputDto : IDto
    {
        [Id(0)]
        public virtual long Uid { get; set; }
    }
}
