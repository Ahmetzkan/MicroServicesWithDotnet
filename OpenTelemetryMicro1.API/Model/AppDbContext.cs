using Microsoft.EntityFrameworkCore;

namespace OpenTelemetryMicro1.API.Model
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }


    public class Order
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
    }
}