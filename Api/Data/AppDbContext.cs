using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<ContentItem> ContentItems => Set<ContentItem>();
    public DbSet<AudioVariant> AudioVariants => Set<AudioVariant>();
    public DbSet<Survey> Surveys => Set<Survey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.FirebaseUid).IsRequired();

            e.HasIndex(e => e.FirebaseUid).IsUnique();

            e.Property(e => e.Email);
            e.Property(e => e.CreatedAt).IsRequired();
            e.Property(e => e.HasCompletedOnboarding).IsRequired();
            e.Property(e => e.IsAnonymous).IsRequired();
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

        modelBuilder.Entity<Survey>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.ValueSignal).IsRequired();
            e.Property(e => e.ReturnIntent).IsRequired();
            e.Property(e => e.Feedback);
            e.Property(e => e.CreatedAt).IsRequired();

            e.Property(e => e.UserId).IsRequired();
            e.Property(e => e.ContentItemId).IsRequired();

            e.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne<ContentItem>()
                .WithMany()
                .HasForeignKey(e => e.ContentItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}