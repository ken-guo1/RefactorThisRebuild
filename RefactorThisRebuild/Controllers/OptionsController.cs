using Microsoft.AspNetCore.Mvc;
using RefactorThisRebuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThisRebuild.Controllers
{
    [Route("api/Products/{id}/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly string  _keyName = "items";

        public OptionsController(ProductContext context)
        {
            _context = context;
        }



        [HttpGet()]
        public Dictionary<string, IList<ProductOption>> Get(Guid id)
        {

            List<ProductOption> items;
            items = _context.ProductOptions.Where(o => o.ProductId == id).ToList();

            var dic = new Dictionary<string, IList<ProductOption>>() {
                {_keyName,items}
            };
            return dic;
        }
        [HttpGet("{optionId}")]
        public ProductOption Get( Guid id,Guid optionId)
        {

            ProductOption option = _context.ProductOptions.Where(o => o.Id == optionId)
                .Where(o => o.ProductId == id)
                .FirstOrDefault();
            return option;
        }

        [HttpPost]
        public IActionResult Post(Guid id, ProductOption option)
        {
            Product product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            if(product==null)
            {
                return NotFound();
            }
            option.ProductId = id;
            ProductOption newOption = new ProductOption
            {
                Name = option.Name,
                Description = option.Description,
                ProductId = option.ProductId

            };
            _context.ProductOptions.Add(newOption);
            _context.SaveChanges();

            return Created("", newOption);

        }
        [HttpPut("{optionId}")]
        public IActionResult Put(Guid optionId, ProductOption option)
        {
            ProductOption origOption = _context.ProductOptions.Where(o => o.Id == optionId)
                .FirstOrDefault();
            if (origOption == null)
            {
                return NotFound();
            }

            origOption.Name = option.Name;
            origOption.Description = option.Description;


            _context.SaveChanges();

            return Created("", origOption);
        }
        [HttpDelete("{optionId}")]
        public IActionResult Delete(Guid optionId)
        {

            ProductOption origOption = _context.ProductOptions.Where(o => o.Id == optionId)
                .FirstOrDefault();
            if (origOption == null)
            {
                return NotFound();
            }
            _context.Remove(origOption);


            _context.SaveChanges();

            return Ok();
        }
    }
}
