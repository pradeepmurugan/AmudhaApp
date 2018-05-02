using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AmudhaApp.Library.Models;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmudhaApp.Server.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class ProductController : Controller
    {
        private LiteDatabase _db;
        private LiteCollection<Product> ProductDatabase;
        public ProductController(LiteDatabase db)
        {
            _db = db;
            ProductDatabase = _db.GetCollection<Product>("products");
        }

        [HttpGet("products", Name = "GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await Task.FromResult(ProductDatabase.FindAll());
                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return new NoContentResult();
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpGet("product/{id:guid}", Name = "GetProduct")]
        public async Task<IActionResult> GetProductByID([FromRoute]Guid id)
        {
            try
            {
                var result = await Task.FromResult(ProductDatabase.FindById(id));
                if (result == null)
                {
                    return new NotFoundResult();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }


        [HttpPost("product", Name = "PostProduct")]
        public async Task<IActionResult> CreateProduct([FromBody]Product product)
        {
            try
            {
                product.Id = Guid.NewGuid();
                product.UpdatedAt = DateTimeOffset.Now;
                product.Price.UpdatedAt = DateTimeOffset.Now;
                await Task.FromResult(ProductDatabase.Insert(product));
                return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("product/{id:guid}", Name = "PutProduct")]
        public async Task<IActionResult> CreateOrUpdateProduct([FromRoute]Guid id, [FromBody]Product product)
        {
            try
            {
                if (id == default(Guid))
                {
                    return new BadRequestResult();
                }
                if(product.Price.UpdatedAt == default(DateTimeOffset))
                {
                    product.Price.UpdatedAt = DateTimeOffset.Now;
                }
                product.UpdatedAt = DateTimeOffset.Now;
                await Task.FromResult(ProductDatabase.Upsert(product));
                return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("product/{id:guid}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid id)
        {
            try
            {
                await Task.FromResult(ProductDatabase.Delete(id));
                return Ok();
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }
    }
}
