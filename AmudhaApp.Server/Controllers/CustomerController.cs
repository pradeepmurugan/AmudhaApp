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
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private LiteDatabase _db;
        private LiteCollection<Customer> CustomerDatabase;
        public CustomerController(LiteDatabase db)
        {
            _db = db;
            CustomerDatabase = _db.GetCollection<Customer>("Customers");
        }

        [HttpGet("customers", Name = "GetAllCustomers")]
        public async Task<ActionResult<Customer>> GetAllCustomers()
        {
            try
            {
                var result = await Task.FromResult(CustomerDatabase.FindAll());
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


        [HttpGet("customer/{id:guid}", Name = "GetCustomer")]
        public async Task<ActionResult<Customer>> GetCustomerById([FromRoute]Guid id)
        {
            try
            {
                var result =  await Task.FromResult(CustomerDatabase.FindById(id));
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

        [HttpPost("customer", Name = "PostCustomer")]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody]Customer customer)
        {
            customer.Id = Guid.NewGuid();
            customer.UpdatedAt = DateTimeOffset.Now;
            try
            {
                await Task.FromResult(CustomerDatabase.Insert(customer));
                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("customer/{id:guid}", Name = "PutCustomer")]
        public async Task<ActionResult<Customer>> CreateOrUpdateCustomer([FromRoute]Guid id, [FromBody]Customer customer)
        {
            if (id == default(Guid))
            {
                return new BadRequestResult();
            }
            customer.UpdatedAt = DateTimeOffset.Now;

            try
            {
                await Task.FromResult(CustomerDatabase.Upsert(customer));
                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("customer/{id:guid}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer([FromRoute]Guid id)
        {
            try
            {
                await Task.FromResult(CustomerDatabase.Delete(id));
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
