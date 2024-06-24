using System.ComponentModel.DataAnnotations.Schema;

namespace Prdt.Repository.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        /// <summary>
        /// 编号
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 类别 Id
        /// </summary>
        public long CategoryUid { get; set; }

        /// <summary>
        /// 所属商店 Id
        /// </summary>
        public long StoreUid { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public byte[] ProductImage { get; set; } = [];

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string ProductDesc { get; set; } = string.Empty;

        /// <summary>
        /// 商品价格
        /// </summary>
        public double ProductPrice { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
