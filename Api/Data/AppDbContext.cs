using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<ContentItem> ContentItems => Set<ContentItem>();
    public DbSet<AudioVariant> AudioVariants => Set<AudioVariant>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.FirebaseUid).IsRequired();
            e.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<ContentItem>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.Title).IsRequired();
            e.Property(e => e.Description).IsRequired();
            e.Property(e => e.SituationTag).IsRequired();
            e.Property(e => e.IsPremium).IsRequired();
            e.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<AudioVariant>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.ContentItemId).IsRequired();
            e.Property(e => e.EmotionalTone).IsRequired();
            e.Property(e => e.AudioUrl).IsRequired();
            e.Property(e => e.DurationSeconds).IsRequired();

            e.HasOne(e => e.ContentItem)
                  .WithMany(c => c.AudioVariants)
                  .HasForeignKey(e => e.ContentItemId);
        });
    }
}
