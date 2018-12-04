using Quotes_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Quotes_WebApi.Data
{
    public class QuotesDbContext : DbContext
    {
        public DbSet<Quotes> Quotes { get; set; }
    }
}