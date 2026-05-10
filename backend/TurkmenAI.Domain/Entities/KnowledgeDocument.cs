namespace TurkmenAI.Domain.Entities;

public class KnowledgeDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>language | accounting | law | banking</summary>
    public string Module { get; set; } = "language";
    public string SourceName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Language { get; set; } = "tk";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublic { get; set; } = true;

    public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();
}

public class DocumentChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; set; }
    public KnowledgeDocument Document { get; set; } = null!;
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;

    /// <summary>Embedding vector as a serialized float[].</summary>
    public byte[] Embedding { get; set; } = Array.Empty<byte>();
    public int TokenCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
