using Quotes_WebApi.Data;
using Quotes_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Quotes_WebApi.Controllers
{
    public class QuotesController : ApiController
    {
        QuotesDbContext quotesDbContext = new QuotesDbContext();
        // GET: api/Quotes
        public IHttpActionResult Get()
        {
            var quotes = quotesDbContext.Quotes;
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        public IHttpActionResult Get(int id)
        {
           var quote = quotesDbContext.Quotes.Find(id);
            if(quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quotes
        public IHttpActionResult Post([FromBody]Quotes quote)
        {
            quotesDbContext.Quotes.Add(quote);
            quotesDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quotes/5
        public IHttpActionResult Put(int id, [FromBody]Quotes quote)
        {
           var entity= quotesDbContext.Quotes.FirstOrDefault(q => q.Id == id);
            if(entity == null)
            {
                return BadRequest("No record found against this ID");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            quotesDbContext.SaveChanges();
            return Ok("Record updated sucessfully");
        }

        // DELETE: api/Quotes/5
        public IHttpActionResult Delete(int id)
        {
            var quote = quotesDbContext.Quotes.Find(id);
            if(quote == null)
            {
                return BadRequest("No record found against this ID");
            }

            quotesDbContext.Quotes.Remove(quote);
            quotesDbContext.SaveChanges();
            return Ok("Quotes deleted");
        }
    }
}
