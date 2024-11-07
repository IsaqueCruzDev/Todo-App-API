using Microsoft.EntityFrameworkCore;

namespace todoApi.Models {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions options) : base(options) {

        }

        public DbSet<UserModel> User { get; set; }
    }
}