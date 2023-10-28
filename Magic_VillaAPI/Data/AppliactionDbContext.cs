using Magic_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Magic_VillaAPI.Data
{
    public class AppliactionDbContext : DbContext
    {
        public AppliactionDbContext(DbContextOptions<AppliactionDbContext> options) : base(options)
        {
           
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Test",
                    Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ullamcorper tortor id accumsan scelerisque. Praesent sed urna fringilla, congue lacus eu, accumsan massa.",
                    Sqft  = 1200,
                    Rate = 9.9,
                    Occupance = 9,
                    ImageUrl = "https://i.stack.imgur.com/PQ4bV.jpg",
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                    

                }
                );
        }
    }
}
