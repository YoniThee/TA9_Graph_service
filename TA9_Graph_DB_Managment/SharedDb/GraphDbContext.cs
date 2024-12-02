using TA9_Graph_DB_Managment.Shaphes;
using Microsoft.EntityFrameworkCore;


namespace TA9_Graph_DB_Managment.SharedDb
{
    public class GraphDbContext : DbContext
    {

        public GraphDbContext(DbContextOptions<GraphDbContext> options) : base(options) { }

        public DbSet<Edge> Edges { get; set; }
        public DbSet<Node> Nodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-6PQJSFF;Database=TA9_HomeAssignment_Graph;Encrypt=false;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edge>()
                .HasKey(e => e.EdgeId);
            modelBuilder.Entity<Node>()
                .HasKey(n => n.NodeId);
        }
    }
}
