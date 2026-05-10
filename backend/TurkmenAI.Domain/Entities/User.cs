namespace TurkmenAI.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string PreferredLanguage { get; set; } = "tk";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    public Subscription? Subscription { get; set; }
}
