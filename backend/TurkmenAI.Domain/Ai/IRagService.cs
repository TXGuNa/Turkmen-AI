namespace TurkmenAI.Domain.Ai;

/// <summary>
/// RAG (Retrieval-Augmented Generation) servisi.
/// Bir soruyu alır, bilgi tabanından en uygun parçaları bulur ve döndürür.
/// </summary>
public interface IRagService
{
    Task<IReadOnlyList<RagChunk>> RetrieveAsync(
        string module,
        string query,
        int topK = 5,
        CancellationToken ct = default);
}

public sealed record RagChunk(
    Guid ChunkId,
    string SourceName,
    string Content,
    double Similarity);
