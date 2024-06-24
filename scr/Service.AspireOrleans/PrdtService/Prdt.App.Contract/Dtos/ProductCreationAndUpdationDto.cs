using Orleans;
using Shared.App.Dtos.Base;

namespace Prdt.App.Contract.Dtos
{
    [GenerateSerializer]
    public class ProductCreationAndUpdationDto : InputDto
    {
        [Id(0)]
        public long CategoryUid { get; set; }
        
        [Id(1)]
        public byte[] ProductImage { get; set; } = [];

        [Id(2)]
        public string ProductName { get; set; } = string.Empty;

        [Id(3)]
        public string ProductDesc { get; set; } = string.Empty;
            
        [Id(4)]
        public double ProductPrice { get; set; }
    }
}
