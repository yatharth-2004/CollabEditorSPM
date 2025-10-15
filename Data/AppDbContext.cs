using Microsoft.EntityFrameworkCore;
using CollabTextEditor.Models;

namespace CollabTextEditor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
    }
}
