using Microsoft.EntityFrameworkCore;
using ParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<NationalPark> NationalParks {get;set;}
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }



    }
}
