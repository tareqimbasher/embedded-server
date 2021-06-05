using EmbeddedServer;
using EmbeddedServer.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("products")]
        public ActionResult GetProducts()
        {
            return ActionResult.Ok("GET products alive!");
        }

        [HttpGet("products/things")]
        public ActionResult GetProductThings()
        {
            return ActionResult.Ok("GET products/things alive!");
        }

        [HttpPost("products/things")]
        public ActionResult PostProductThings()
        {
            return ActionResult.Ok("POST products/things alive!");
        }

        [HttpPost("products/things/{id}/new")]
        public async Task<ActionResult> PostProductThingsAsync(int id, Person model)
        {
            return ActionResult.Ok("POST products/things/new alive!");
        }

        [HttpDelete("products/things")]
        public ActionResult DeleteProductThings()
        {
            return ActionResult.Ok("Delete products/things/new alive!");
        }
    }
}
