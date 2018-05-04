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
    [Route("api/billing")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private LiteDatabase _db;
        private LiteCollection<Invoice> InvoiceDatabase;
        public InvoiceController(LiteDatabase db)
        {
            _db = db;
            InvoiceDatabase = _db.GetCollection<Invoice>("invoices");
            var invoice = new Invoice();
            var deliveryChargePrice = new ProductPrice
            {
                CalculatedPrice = 100
            };
            var deliveryCharge = new Product
            {
                Name = "Delivery Charge",
                Nickname = "DelCharge",
                UpdatedAt = DateTimeOffset.Now,
                Price = deliveryChargePrice
            };
            var pipePrice = new ProductPrice
            {
                CalculatedPrice = 200
            };
            var pipe = new Product
            {
                Name = "Delivery Charge",
                Nickname = "DelCharge",
                UpdatedAt = DateTimeOffset.Now,
                Price = pipePrice
            };
            invoice.Products.Add(new ProductsListItem(deliveryCharge, 1));
            invoice.Products.Add(new ProductsListItem(pipe, 20));
            InvoiceDatabase.Insert(invoice);
        }

        [HttpGet("invoices", Name = "GetAllInvoices")]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var result = await Task.FromResult(InvoiceDatabase.FindAll());
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

        [HttpGet("invoice/{id:guid}", Name = "GetInvoice")]
        public async Task<IActionResult> GetInvoiceByID([FromRoute]Guid id)
        {
            try
            {
                var result = await Task.FromResult(InvoiceDatabase.FindById(id));
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


        [HttpPost("invoice", Name = "PostInvoice")]
        public async Task<IActionResult> CreateInvoice([FromBody]Invoice invoice)
        {
            try
            {
                invoice.Id = Guid.NewGuid();
                invoice.CreatedAt = DateTimeOffset.Now;
                await Task.FromResult(InvoiceDatabase.Insert(invoice));
                return CreatedAtRoute("GetInvoice", new { id = invoice.Id }, invoice);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("invoice/{id:guid}", Name = "PutInvoice")]
        public async Task<IActionResult> CreateOrUpdateInvoice([FromRoute]Guid id, [FromBody]Invoice invoice)
        {
            try
            {
                if (id == default(Guid))
                {
                    return new BadRequestResult();
                }
                if(invoice.CreatedAt == default(DateTimeOffset))
                {
                    invoice.CreatedAt = DateTimeOffset.Now;
                }

                await Task.FromResult(InvoiceDatabase.Upsert(invoice));
                return CreatedAtRoute("GetInvoice", new { id = invoice.Id }, invoice);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("invoice/{id:guid}", Name = "DeleteInvoice")]
        public async Task<IActionResult> DeleteInvoice([FromRoute]Guid id)
        {
            try
            {
                await Task.FromResult(InvoiceDatabase.Delete(id));
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