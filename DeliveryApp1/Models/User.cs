using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp1.Models
{
    public class User
    {
        public class MinimumAgeAttribute : ValidationAttribute
        {
            int _minimumAge;

            public MinimumAgeAttribute(int minimumAge)
            {
                _minimumAge = minimumAge;
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_minimumAge) < DateTime.Now;
                }

                return false;
            }
        }
        public User()
        {
            Orders = new List<Order>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Неправильний формат номеру телефону")]
        public string PhoneNumber { get; set; }
        [MinimumAge(16, ErrorMessage = "Менше 16 років")]
        //[Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateBirth { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
