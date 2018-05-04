using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AmudhaApp.Library.Models;
using System.Net;

namespace AmudhaApp.Server.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private LiteDatabase _db;
        private LiteCollection<Account> AccountDatabase;
        public AccountController(LiteDatabase db)
        {
            _db = db;
            AccountDatabase = _db.GetCollection<Account>("acccounts");
        }

        [HttpGet("accounts", Name = "GetAllAccounts")]
        public async Task<ActionResult<Account>> GetAllAccounts()
        {
            try
            {
                var result = await Task.FromResult(AccountDatabase.FindAll());
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

        [HttpGet("account/{id:guid}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> GetAccountByCustomerID([FromRoute]Guid customerId)
        {
            try
            {
                var result = await Task.FromResult(AccountDatabase.FindById(customerId));
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


        [HttpPost("account", Name = "PostAccount")]
        public async Task<ActionResult<Account>> CreateAccount([FromBody]Account account)
        {
            try
            {
                account.UpdatedAt = DateTimeOffset.Now;
                await Task.FromResult(AccountDatabase.Insert(account));
                return CreatedAtRoute(nameof(GetAccountByCustomerID),  account.Customer.Id);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpPut("account/{id:guid}", Name = "PutAccount")]
        public async Task<ActionResult<Account>> CreateOrUpdateAccount([FromRoute]Guid customerId, [FromBody]Account account)
        {
            try
            {
                if (customerId == default(Guid) || account.Customer.Id == default(Guid) || account.Customer.Id != account.Id)
                {
                    return new BadRequestResult();
                }
                account.UpdatedAt = DateTimeOffset.Now;
                await Task.FromResult(AccountDatabase.Upsert(account));
                return CreatedAtRoute(nameof(GetAccountByCustomerID), account.Id);
            }

            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { message = $"Query failed with error code {e.HResult.ToString()}." });
            }
        }

        [HttpDelete("account/{id:guid}", Name = "DeleteAccount")]
        public async Task<ActionResult<InventoryItem>> DeleteAccountByProductId([FromRoute]Guid customerId)
        {
            try
            {
                await Task.FromResult(AccountDatabase.Delete(customerId));
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
