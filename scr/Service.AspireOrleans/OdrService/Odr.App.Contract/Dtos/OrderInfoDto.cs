using Orleans;
using Shared.App.Dtos.Base;

namespace Odr.App.Contract.Dtos
{
    [GenerateSerializer]
    public class OrderInfoDto : OutputDto
    {
        [Id(0)]
        public long CustomerUid { get; set; }

        [Id(1)]
        public long StoreUid { get; set; }

        [Id(2)]
        public long RiderUid { get; set; }

        [Id(3)]
        public DateTime? CreateTime { get; set; }

        [Id(4)]
        public DateTime? FinishTime { get; set; }

        [Id(5)]
        public double StartPrice { get; set; }

        [Id(6)]
        public double DeliveryPrice { get; set; }

        [Id(7)]
        public int Statu { get; set; }
    }
}
