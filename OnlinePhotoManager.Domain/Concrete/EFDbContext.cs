using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("PhotoManagerDb") { }
        
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>().
                Map<FreeUser>(m => m.Requires("User")).HasEntitySetName("FreeUser");
            modelBuilder.Entity<UserProfile>().
                Map<PremiumUser>(m => m.Requires("User")).HasEntitySetName("PremiumUser");
        }
    }
}
