namespace TurkmenAI.Domain.Ai;

/// <summary>
/// AI sağlayıcısı için tek arayüz. Faz 1'de API, Faz 2'de self-hosted modeli
/// bu arayüzü implement eden farklı sınıflarla çalıştırırız — uygulama kodu değişmez.
/// </summary>
public interface IAiProvider
{
    Task<AiResponse> CompleteAsync(AiRequest request, CancellationToken ct = default);

    /// <summary>Bir metnin embedding (vektör) temsilini döndürür. RAG için kullanılır.</summary>
    Task<float[]> EmbedAsync(string text, CancellationToken ct = default);
}

public sealed record AiRequest(
    IReadOnlyList<AiMessage> Messages,
    string? SystemPrompt = null,
    double Temperature = 0.4,
    int MaxTokens = 1024);

public sealed record AiMessage(string Role, string Content);

public sealed record AiResponse(
    string Content,
    int InputTokens,
    int OutputTokens,
    string ProviderName);
