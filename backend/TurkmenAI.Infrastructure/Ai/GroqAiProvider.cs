using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TurkmenAI.Domain.Ai;

namespace TurkmenAI.Infrastructure.Ai;

/// <summary>
/// Groq API sağlayıcısı (Llama 3.1 70B vb. çok ucuz/hızlı çalıştırır).
/// Faz 1 için iyi başlangıç. OpenAI-uyumlu API kullanır, ileride
/// OpenAiProvider, AnthropicProvider veya LocalLlamaProvider eklemek kolay.
/// </summary>
public class GroqAiProvider : IAiProvider
{
    private readonly HttpClient _http;
    private readonly GroqOptions _opt;

    public GroqAiProvider(HttpClient http, IOptions<GroqOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
        _http.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _opt.ApiKey);
    }

    public async Task<AiResponse> CompleteAsync(AiRequest request, CancellationToken ct = default)
    {
        var messages = new List<object>();
        if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
            messages.Add(new { role = "system", content = request.SystemPrompt });
        foreach (var m in request.Messages)
            messages.Add(new { role = m.Role, content = m.Content });

        var body = new
        {
            model = _opt.Model,
            messages,
            temperature = request.Temperature,
            max_tokens = request.MaxTokens
        };

        var response = await _http.PostAsJsonAsync("chat/completions", body, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<GroqChatResponse>(cancellationToken: ct)
                   ?? throw new InvalidOperationException("Groq'tan boş cevap");

        var content = json.Choices.FirstOrDefault()?.Message.Content ?? "";
        return new AiResponse(
            Content: content,
            InputTokens: json.Usage?.PromptTokens ?? 0,
            OutputTokens: json.Usage?.CompletionTokens ?? 0,
            ProviderName: $"groq:{_opt.Model}");
    }

    public Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        // Groq henüz embedding API sunmuyor — embedding için ayrı bir provider
        // (örn. OpenAI text-embedding-3-small veya self-hosted bge-m3) kullanılır.
        // Şimdilik mock döner.
        throw new NotSupportedException(
            "Groq embedding desteklemiyor. EmbeddingProvider'ı ayrı yapılandırın.");
    }

    // --- DTOs ---
    private sealed record GroqChatResponse(
        [property: JsonPropertyName("choices")] List<Choice> Choices,
        [property: JsonPropertyName("usage")] Usage? Usage);
    private sealed record Choice([property: JsonPropertyName("message")] ChoiceMessage Message);
    private sealed record ChoiceMessage(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("content")] string Content);
    private sealed record Usage(
        [property: JsonPropertyName("prompt_tokens")] int PromptTokens,
        [property: JsonPropertyName("completion_tokens")] int CompletionTokens);
}

public class GroqOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "llama-3.1-70b-versatile";
}
