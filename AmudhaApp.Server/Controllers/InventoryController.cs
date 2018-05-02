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
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private LiteDatabase _db;
        private LiteCollection<InventoryItem> InventoryDatabase;
        public InventoryController(LiteDatabase db)
        {
            _db = db;
            InventoryDatabase = _db.GetCollection<InventoryItem>("Inventory");
        }

        [HttpGet("Inventory", Name = "GetAllItems")]
        public async Task<ActionResult<InventoryItem>> GetAllItems()
        {
            try
            {
                var result = await Task.FromResult(InventoryDatabase.FindAll());
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

        [HttpGet("item/{id:guid}", Name = "GetItem")]
        public async Task<ActionResult<InventoryItem>> GetItemByProductID([FromRoute]Guid productId)
        {
            try
            {
                var result = await Task.FromResult(InventoryDatabase.FindById(productId));
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


        [HttpPost("item", Name = "PostItem")]
        public async Task<ActionResult<InventoryItem>> CreateInventory([FromBody]InventoryItem item)
        {
            try
            {
                item.UpdatedAt = DateTimeOffset.Now;
                await Task.FromResult(InventoryDatabase.Insert(item));
                return CreatedAtRoute(nameof(GetItemByProductID), new { id = item.Id }, item);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("item/{id:guid}", Name = "PutItem")]
        public async Task<ActionResult<InventoryItem>> CreateOrUpdateInventory([FromRoute]Guid productId, [FromBody]InventoryItem item)
        {
            if (productId == default(Guid))
            {
                return new BadRequestResult();
            }
            item.UpdatedAt = DateTimeOffset.Now;

            try
            {
                await Task.FromResult(InventoryDatabase.Upsert(item));
                return CreatedAtRoute(nameof(GetItemByProductID), new { id = item.Id }, item);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("item/{id:guid}", Name = "DeleteItem")]
        public async Task<ActionResult<InventoryItem>> DeleteItemByProductId([FromRoute]Guid productId)
        {
            try
            {
                await Task.FromResult(InventoryDatabase.Delete(productId));
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
