using TurkmenAI.Domain.Ai;

namespace TurkmenAI.Infrastructure.Ai;

/// <summary>
/// Geliştirme için sahte AI sağlayıcısı. API anahtarına ihtiyaç duymaz.
/// İlk endpoint'i test etmek için kullanılır. Gerçek sağlayıcıya geçince bu silinmez,
/// unit testlerde de işe yarar.
/// </summary>
public class MockAiProvider : IAiProvider
{
    public Task<AiResponse> CompleteAsync(AiRequest request, CancellationToken ct = default)
    {
        var lastUser = request.Messages.LastOrDefault(m => m.Role == "user")?.Content ?? "";
        var reply = $"[Mock cevap] Soruňyz: \"{lastUser}\". " +
                    "Hakyky AI saglaýjy heniz konfigurirlenmedi. " +
                    "Lütfen appsettings.json içinde Ai:Provider'ı değiştirin.";

        return Task.FromResult(new AiResponse(
            Content: reply,
            InputTokens: lastUser.Length / 4,
            OutputTokens: reply.Length / 4,
            ProviderName: "mock"));
    }

    public Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        // Deterministic fake embedding (sabit boyutta rastgele ama tekrarlanabilir)
        var rng = new Random(text.GetHashCode());
        var v = new float[384];
        for (int i = 0; i < v.Length; i++) v[i] = (float)(rng.NextDouble() * 2 - 1);
        return Task.FromResult(v);
    }
}
