using Orleans;

namespace Prdt.App.Contract.Dtos
{
    [GenerateSerializer]
    public class ProductCreationDto : ProductCreationAndUpdationDto
    {
        [Id(0)]
        public long StoreUid { get; set; }
    }
}
