using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp1.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
