using App.DataLayer.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace App.DataLayer.EF
{
    public class ProjectContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }

        static ProjectContext()
        {
            Database.SetInitializer<ProjectContext>(new StoreDbInitializer());
        }

        public ProjectContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProjectConfig());
            modelBuilder.Configurations.Add(new EmployeeConfig());
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }

    class ProjectConfig : EntityTypeConfiguration<Project>
    {
        public ProjectConfig()
        {
            HasKey(p => p.Id);
            Property<Guid>(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .Map(ep =>
                {
                    ep.MapLeftKey("ProjectId");
                    ep.MapRightKey("EmployeeId");
                    ep.ToTable("ProjectEmployee");
                });
        }
    }

    class EmployeeConfig : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfig()
        {
            HasKey(e => e.Id);
            Property<Guid>(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
