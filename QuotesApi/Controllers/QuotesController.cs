using Microsoft.AspNet.Identity;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace QuotesApi.Controllers
{
    [Authorize]
    public class QuotesController : ApiController
    {
        ApplicationDbContext quotesDbContext = new ApplicationDbContext();
        // GET: api/Quotes
        [AllowAnonymous]
        [HttpGet]
        [CacheOutput(ClientTimeSpan =60,ServerTimeSpan =60)]
        public IHttpActionResult LoadQuoutes(string sort)
        {
            IQueryable<Quotes> quotes;
            switch(sort)
            {
                case "desc":
                   quotes = quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes =quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = quotesDbContext.Quotes;
                    break;
            }
            //var quotes = quotesDbContext.Quotes;
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/Quotes/MyQuotes")]
        public IHttpActionResult MyQuotes()
        {
            string userId = User.Identity.GetUserId();
            var quotes = quotesDbContext.Quotes.Where(q=>q.UserId==userId);
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/Quotes/PagingQuotes/{pageNumber=1}/{pageSize=5}")]
        public IHttpActionResult PagingQuote(int pageNumber, int pageSize)
        {
            var quotes = quotesDbContext.Quotes.OrderBy(q=>q.Id);
            return Ok(quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
        [HttpGet]
        [Route("api/Quotes/SearchQuote/{type=}")]
        public IHttpActionResult SearchQuote(string type)
        {
           var quotes = quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        [HttpGet]
        public IHttpActionResult LoadQuote(int id)
        {
           var quote = quotesDbContext.Quotes.Find(id);
            if(quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quotes
        [HttpPost]
        public IHttpActionResult Post([FromBody]Quotes quote)
        {
         var userId =  User.Identity.GetUserId();
            quote.UserId = userId;
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            quotesDbContext.Quotes.Add(quote);
            quotesDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quotes/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Quotes quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.GetUserId();
            
            var entity= quotesDbContext.Quotes.FirstOrDefault(q => q.Id == id);


            if(entity == null)
            {
                return BadRequest("No record found against this ID");
            }


            if (userId != entity.UserId)
            {
                return BadRequest("You don't have right to update this records....!");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            quotesDbContext.SaveChanges();
            return Ok("Record updated sucessfully");
        }

        // DELETE: api/Quotes/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            var quote = quotesDbContext.Quotes.Find(id);
            if(quote == null)
            {
                return BadRequest("No record found against this ID");
            }

            if(userId!=quote.UserId)
            {
                return BadRequest("You don't have right to delete this records....!");
            }

            quotesDbContext.Quotes.Remove(quote);
            quotesDbContext.SaveChanges();
            return Ok("Quotes deleted");
        }
    }
}
