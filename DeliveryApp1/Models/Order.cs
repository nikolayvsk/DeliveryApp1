using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp1.Models
{
    public class Order
    {
        public int Id { get; set; }
        //[Required]
        public DateTime DateSale { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]

        public int UserId { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int DeliveryDriverId { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int ProductId { get; set; }

        public virtual User User { get; set; }
        public virtual DeliveryDriver DeliveryDriver { get; set; }
        public virtual Product Product { get; set; }

    }
}
