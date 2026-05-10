using Microsoft.EntityFrameworkCore;
using TurkmenAI.Domain.Entities;

namespace TurkmenAI.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<KnowledgeDocument> KnowledgeDocuments => Set<KnowledgeDocument>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
            e.Property(x => x.PreferredLanguage).HasMaxLength(8);
        });

        b.Entity<Conversation>(e =>
        {
            e.HasIndex(x => x.UserId);
            e.Property(x => x.Module).HasMaxLength(32).IsRequired();
            e.Property(x => x.Title).HasMaxLength(256);
        });

        b.Entity<Message>(e =>
        {
            e.HasIndex(x => x.ConversationId);
            e.Property(x => x.Role).HasMaxLength(16).IsRequired();
        });

        b.Entity<Subscription>(e =>
        {
            e.Property(x => x.PlanType).HasMaxLength(32).IsRequired();
        });

        b.Entity<KnowledgeDocument>(e =>
        {
            e.HasIndex(x => x.Module);
            e.Property(x => x.Module).HasMaxLength(32).IsRequired();
            e.Property(x => x.SourceName).HasMaxLength(256).IsRequired();
            e.Property(x => x.Language).HasMaxLength(8);
        });

        b.Entity<DocumentChunk>(e =>
        {
            e.HasIndex(x => x.DocumentId);
            e.HasOne(x => x.Document)
                .WithMany(d => d.Chunks)
                .HasForeignKey(x => x.DocumentId);
        });

        base.OnModelCreating(b);
    }
}
