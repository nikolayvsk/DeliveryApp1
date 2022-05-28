//using Microsoft.AspNetCore.Mvc;
using Xunit;
using DeliveryApp1;
using DeliveryApp1.Models;
using DeliveryApp1.Controllers;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

namespace UnitTestsAPP
{
    public class Class1
    {
        [Fact]
        public void GetProductsResultNotNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryAPIContext>();
            optionsBuilder.UseSqlServer("Server= DESKTOP-NIT; Database=DeliveryAppDB111; Trusted_Connection=True; MultipleActiveResultSets=true");
            var controller = new ProductsController(new DeliveryAPIContext(optionsBuilder.Options));
            var result = controller.GetProducts();
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetProductsContainsAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryAPIContext>();
            optionsBuilder.UseSqlServer("Server= DESKTOP-NIT; Database=DeliveryAppDB111; Trusted_Connection=True; MultipleActiveResultSets=true");
            var controller = new ProductsController(new DeliveryAPIContext(optionsBuilder.Options));
            var result = await controller.GetProducts();
            Assert.Contains(result.Value, a => a.Name.Equals("testID"));
        }
    }
}