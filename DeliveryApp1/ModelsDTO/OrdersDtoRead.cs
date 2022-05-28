using DeliveryApp1.Models;

namespace DeliveryApp1.ModelsDTO
{
    public class OrdersDtoRead
    {
        public int Id { get; set; }
        public DateTime DateSale { get; set; }

        public int UserId { get; set; }
 
        public int DeliveryDriverId { get; set; }

        public int ProductId { get; set; }

        public virtual string User { get; set; }
        public virtual string DeliveryDriver { get; set; }
        public virtual string Product { get; set; }
    }
}
