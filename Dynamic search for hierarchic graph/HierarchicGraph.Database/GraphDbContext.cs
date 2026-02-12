using HierarchicGraph.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicGraph.Database
{
    public class GraphDbContext : DbContext
    {
        public GraphDbContext(DbContextOptions<GraphDbContext> options)
            : base(options) { }

        public DbSet<GraphItem> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GraphItem>()
                .HasOne(g => g.Parent)
                .WithMany(g => g.Children)
                .HasForeignKey(g => g.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // prevents cascade loops
        }
    }
}