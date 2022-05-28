using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp1.Models
{
    public class Product
    {
        public Product()
        {
            Orders = new List<Order>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
