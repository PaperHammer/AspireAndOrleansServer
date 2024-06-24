using System.ComponentModel.DataAnnotations.Schema;

namespace Odr.Repository.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Uid { get; set; }

        public long CustomerUid { get; set; }

        public long StoreUid { get; set; }

        public long? RiderUid { get; set; }

        public DateTime? CreateTime { get; set; }
        
        public DateTime? FinishTime { get; set; }

        public double StartPrice { get; set; }

        public double DeliveryPrice { get; set; }

        public int Statu { get; set; }

        public bool IsDelete { get; set; }
    }
}
