using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AmazonWebApplication1.Models;

namespace AmazonWebApplication1.Database
{
    public class RDSContext : DbContext
    {
        public RDSContext(DbContextOptions<RDSContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ImageModel> ImageModel { get; set; }

    }
}
