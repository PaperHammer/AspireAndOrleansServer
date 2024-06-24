using Orleans;
using Shared.App.Dtos.Base;

namespace Sto.App.Contract.Dtos
{
    [GenerateSerializer]
    public class StoreCreationAndUpdationDto : InputDto
    {
        [Id(0)]
        public long OwnerId { get; set; }

        [Id(1)]
        public byte[]? StoreImage { get; set; }

        [Id(2)]
        public string StoreName { get; set; } = string.Empty;

        [Id(3)]
        public string Position { get; set; } = string.Empty;

        [Id(4)]
        public string StoreDesc { get; set; } = string.Empty;

        [Id(5)]
        public DateTime OpeningTime { get; set; }

        [Id(6)]
        public DateTime ClosingTime { get; set; }

        [Id(7)]
        public double StartPrice { get; set; }

        [Id(8)]
        public double DeliveryPrice { get; set; }
    }
}
