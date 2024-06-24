using Orleans;
using Shared.App.Dtos.Base;

namespace Prdt.App.Contract.Dtos
{
    [GenerateSerializer]
    public class ProductInfoDto : OutputDto
    {
        [Id(0)]
        /// <summary>
        /// 类别 Id
        /// </summary>
        public long CategoryUid { get; set; }

        [Id(1)]
        /// <summary>
        /// 所属商店 Id
        /// </summary>
        public long StoreUid { get; set; }

        [Id(2)]
        /// <summary>
        /// 商品图片
        /// </summary>
        public byte[] ProductImage { get; set; } = [];

        [Id(3)]
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        [Id(4)]
        /// <summary>
        /// 商品描述
        /// </summary>
        public string ProductDesc { get; set; } = string.Empty;

        [Id(5)]
        /// <summary>
        /// 商品价格
        /// </summary>
        public double ProductPrice { get; set; }
    }
}
