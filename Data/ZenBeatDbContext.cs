using Microsoft.EntityFrameworkCore;
using zenBeat.Models;
using zenBeat.Models.History;
using zenBeat.Data.Interceptors;

namespace zenBeat.Data
{
    public class ZenBeatDbContext : DbContext
    {
        public ZenBeatDbContext(DbContextOptions<ZenBeatDbContext> options) : base(options)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<ProjectStepsMapping> ProjectStepsMappings { get; set; }
        
        // History tables
        public DbSet<LanguageHistory> LanguageHistories { get; set; }
        public DbSet<ProjectHistory> ProjectHistories { get; set; }
        public DbSet<StepHistory> StepHistories { get; set; }
        public DbSet<ProjectStepsMappingHistory> ProjectStepsMappingHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Language entity
            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EnUS).HasColumnName("en-US").HasMaxLength(200);
                entity.Property(e => e.ZhCN).HasColumnName("zh-CN").HasMaxLength(200);
                entity.Property(e => e.ZhTW).HasColumnName("zh-TW").HasMaxLength(200);
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).IsRequired().HasMaxLength(100);
                
                entity.HasOne(e => e.Language)
                    .WithMany(l => l.Projects)
                    .HasForeignKey(e => e.Name)
                    .HasPrincipalKey(l => l.Name)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Step entity
            modelBuilder.Entity<Step>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).IsRequired().HasMaxLength(100);
                
                entity.HasOne(e => e.Language)
                    .WithMany(l => l.Steps)
                    .HasForeignKey(e => e.Name)
                    .HasPrincipalKey(l => l.Name)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ProjectStepsMapping entity
            modelBuilder.Entity<ProjectStepsMapping>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.StepId });
                
                entity.HasOne(e => e.Project)
                    .WithMany(p => p.ProjectStepsMappings)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Step)
                    .WithMany(s => s.ProjectStepsMappings)
                    .HasForeignKey(e => e.StepId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure History entities
            modelBuilder.Entity<LanguageHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FieldName).HasMaxLength(50);
                entity.Property(e => e.FromValue).HasMaxLength(200);
                entity.Property(e => e.ToValue).HasMaxLength(200);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });

            modelBuilder.Entity<ProjectHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FieldName).HasMaxLength(50);
                entity.Property(e => e.FromValue).HasMaxLength(200);
                entity.Property(e => e.ToValue).HasMaxLength(200);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });

            modelBuilder.Entity<StepHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FieldName).HasMaxLength(50);
                entity.Property(e => e.FromValue).HasMaxLength(200);
                entity.Property(e => e.ToValue).HasMaxLength(200);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });

            modelBuilder.Entity<ProjectStepsMappingHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FieldName).HasMaxLength(50);
                entity.Property(e => e.FromValue).HasMaxLength(200);
                entity.Property(e => e.ToValue).HasMaxLength(200);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });
        }
    }
}