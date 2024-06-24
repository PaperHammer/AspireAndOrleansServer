using Orleans;
using Shared.App.Dtos.Base;

namespace Odr.App.Contract.Dtos
{
    [GenerateSerializer]
    public class OrderCreationDto : InputDto
    {
        [Id(0)]
        public long CustomerUid { get; set; }

        [Id(1)]
        public long StoreUid { get; set; }

        [Id(2)]
        public DateTime? CreateTime { get; set; }

        [Id(3)]
        public double StartPrice { get; set; }

        [Id(4)]
        public double DeliveryPrice { get; set; }
    }
}
