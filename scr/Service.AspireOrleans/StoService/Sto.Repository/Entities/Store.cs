using System.ComponentModel.DataAnnotations.Schema;

namespace Sto.Repository.Entities
{
    public class Store
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Uid { get; set; }

        public long OwnerId { get; set; }

        public byte[]? StoreImage { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public double Score { get; set; }

        public string? StoreDesc { get; set; }

        public DateTime OpeningTime { get; set; }
        
        public DateTime ClosingTime { get; set; }
        
        public double StartPrice { get; set; }
        
        public double DeliveryPrice { get; set; }
        
        public bool IsDelete { get; set; }
    }
}
