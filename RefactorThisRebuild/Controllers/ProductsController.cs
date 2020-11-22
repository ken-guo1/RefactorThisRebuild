using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefactorThisRebuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RefactorThisRebuild.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Dictionary<string, IList<Product>> Get([FromQuery(Name = "name")] string name=null)
        {

            List<Product> items;
            if(name==null)
            {
                items = _context.Products.ToList();
            }
            else
            {
                items = _context.Products.Where(p => p.Name.Contains(name)).ToList();
            }
            

            var dic = new Dictionary<string, IList<Product>>() {
                {"items",items}
            };
            return dic;
        }

        [HttpGet("{id}")]
        public Product Get(Guid id)
        {

            Product product = _context.Products.Where(i => i.Id == id).FirstOrDefault();

            return product;
        }


        [HttpPost]
        public IActionResult Post(Product product)
        {
            Product newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return Created("", newProduct);


        }
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Product product)
        {

            Product orig = _context.Products.Where(i => i.Id == id).FirstOrDefault();

            if (orig == null)
            {
                return NotFound();
            }
            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;

            _context.SaveChanges();

            return Created("", orig);


        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {

            var product = _context
                    .Products
                    .Include(p=>p.ProductOptions)
                    .SingleOrDefault(e => e.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Remove(product);
            _context.SaveChanges();
            return Ok();

        }


    }
}
