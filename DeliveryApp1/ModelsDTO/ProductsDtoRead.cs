using DeliveryApp1.Models;

namespace DeliveryApp1.ModelsDTO
{
    public class ProductsDtoRead
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int ManufacturerId { get; set; }

        public virtual string Manufacturer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
