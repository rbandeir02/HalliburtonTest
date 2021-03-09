using Microsoft.EntityFrameworkCore;
namespace HalliburtonTest
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
          : base(options)
        { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
