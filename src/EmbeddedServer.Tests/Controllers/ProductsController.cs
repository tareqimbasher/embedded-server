using EmbeddedServer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmbeddedServer.Tests.Controllers
{
    public class ProductsController : Controller
    {
        public static List<Product> Products;

        static ProductsController()
        {
            Products = new List<Product>()
            {
                new Product(1, "Motherboard"),
                new Product(2, "CPU"),
                new Product(3, "Graphics Card"),
                new Product(4, "Hard Drive"),
                new Product(5, "Monitor")
            };
        }




        [HttpGet("products")]
        public ActionResult Get()
        {
            return ActionResult.Ok(Products);
        }

        [HttpGet("products/first")]
        public ActionResult GetThings()
        {
            return ActionResult.Ok(Products.First());
        }

        [HttpGet("products/{id}")]
        public ActionResult Get(int id)
        {
            return ActionResult.Ok(Products.FirstOrDefault(x => x.Id == id));
        }
    }

    public class Product
    {
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
