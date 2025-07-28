using HRR.Model;
using HRR.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace HRR
{
    public class HrDbContext : DbContext
    {
        public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
        {
        
        
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Called parent method
            //Seed
            modelBuilder.Entity<Lookup>().HasData(
                //Employee Postions (Mahor code = 0)
                new Lookup {Id = 1, MajorCode = 0 , MinorCode = 0, Name = "Employee Positions"},
                new Lookup {Id = 2, MajorCode = 0 , MinorCode = 1, Name = "HR"},
                new Lookup {Id = 3, MajorCode = 0 , MinorCode = 2, Name = "Manager"},
                new Lookup {Id = 4, MajorCode = 0 , MinorCode = 3, Name = "Developer"},

                //Department Types (Major Code = 1)
                new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Types" },
                new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "Finance" },
                new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" }
                );

        }
        

        //Define Tables

        //Employees Table
        public DbSet<Employee> Employees {get; set;}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup>Lookups { get; set;
        }
        
    }
}
