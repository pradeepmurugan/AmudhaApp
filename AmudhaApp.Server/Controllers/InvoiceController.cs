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
    public class InvoiceController : Controller
    {
        private LiteDatabase _db;
        private LiteCollection<Invoice> InvoiceDatabase;
        public InvoiceController(LiteDatabase db)
        {
            _db = db;
            InvoiceDatabase = _db.GetCollection<Invoice>("invoices");
        }

        [HttpGet("invoices", Name = "Getinvoices")]
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
        public async Task<IActionResult> GetProductByID([FromRoute]Guid id)
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
            invoice.Id = Guid.NewGuid();
            invoice.UpdatedAt = DateTimeOffset.Now;
            try
            {
                await Task.FromResult(InvoiceDatabase.Insert(invoice));
                return CreatedAtRoute("PostInvoice", new { id = invoice.Id }, invoice);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("invoice/{id:guid}", Name = "PutInvoice")]
        public async Task<IActionResult> CreateOrUpdateProduct([FromRoute]Guid id, [FromBody]Invoice invoice)
        {
            if (id == default(Guid))
            {
                return new BadRequestResult();
            }
            invoice.UpdatedAt = DateTimeOffset.Now;

            try
            {
                await Task.FromResult(InvoiceDatabase.Upsert(invoice));
                return CreatedAtRoute("PutInvoice", new { id = invoice.Id }, invoice);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("invoice/{id:guid}", Name = "DeleteInvoice")]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid id)
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
