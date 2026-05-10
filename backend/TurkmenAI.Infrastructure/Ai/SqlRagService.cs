using Microsoft.EntityFrameworkCore;
using TurkmenAI.Domain.Ai;
using TurkmenAI.Infrastructure.Persistence;

namespace TurkmenAI.Infrastructure.Ai;

/// <summary>
/// MVP için basit SQL tabanlı RAG.
/// Tüm chunkları belleğe çekip cosine similarity hesaplar.
/// Ölçek büyüdüğünde Qdrant / SQL Server VECTOR datatype'a geçilecek.
/// </summary>
public class SqlRagService : IRagService
{
    private readonly AppDbContext _db;
    private readonly IAiProvider _ai;

    public SqlRagService(AppDbContext db, IAiProvider ai)
    {
        _db = db;
        _ai = ai;
    }

    public async Task<IReadOnlyList<RagChunk>> RetrieveAsync(
        string module, string query, int topK = 5, CancellationToken ct = default)
    {
        // 1) Sorgunun embedding'ini hesapla
        float[] queryEmbedding;
        try
        {
            queryEmbedding = await _ai.EmbedAsync(query, ct);
        }
        catch (NotSupportedException)
        {
            // Embedding sağlayıcısı yoksa RAG devre dışı, boş döner — AI sadece kendi bilgisiyle cevaplar
            return Array.Empty<RagChunk>();
        }

        // 2) Modüldeki tüm chunkları çek (MVP: küçük veri için OK)
        var chunks = await _db.DocumentChunks
            .Include(c => c.Document)
            .Where(c => c.Document.Module == module)
            .ToListAsync(ct);

        if (chunks.Count == 0) return Array.Empty<RagChunk>();

        // 3) Cosine similarity hesapla
        var scored = chunks
            .Select(c =>
            {
                var emb = DeserializeEmbedding(c.Embedding);
                var sim = CosineSimilarity(queryEmbedding, emb);
                return new RagChunk(c.Id, c.Document.SourceName, c.Content, sim);
            })
            .OrderByDescending(x => x.Similarity)
            .Take(topK)
            .ToList();

        return scored;
    }

    private static float[] DeserializeEmbedding(byte[] bytes)
    {
        var floats = new float[bytes.Length / sizeof(float)];
        Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
        return floats;
    }

    private static double CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length) return 0;
        double dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB) + 1e-10);
    }
}
