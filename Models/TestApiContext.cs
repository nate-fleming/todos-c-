using Microsoft.EntityFrameworkCore;

namespace test_api.Models
{
    public class TestApiContext : DbContext
    {
        public TestApiContext(DbContextOptions<TestApiContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}