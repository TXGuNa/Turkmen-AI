using Microsoft.Extensions.Logging;
using TurkmenAI.Domain.Ai;

namespace TurkmenAI.Application.Modules;

/// <summary>
/// Tüm modüller için ortak asistan servisi.
/// Module parametresi ile hangi uzmanlık alanında cevap vereceğini belirler.
/// RAG ile bilgi tabanından bağlam çekip prompt'a ekler.
/// </summary>
public class AssistantService
{
    private readonly IAiProvider _aiProvider;
    private readonly IRagService _ragService;
    private readonly ILogger<AssistantService> _logger;

    public AssistantService(
        IAiProvider aiProvider,
        IRagService ragService,
        ILogger<AssistantService> logger)
    {
        _aiProvider = aiProvider;
        _ragService = ragService;
        _logger = logger;
    }

    public async Task<AssistantReply> AskAsync(
        string module,
        string userQuestion,
        IReadOnlyList<AiMessage> history,
        CancellationToken ct = default)
    {
        // 1) RAG: bilgi tabanından ilgili parçaları çek
        var chunks = await _ragService.RetrieveAsync(module, userQuestion, topK: 5, ct);

        // 2) Sistem promptunu modüle göre belirle ve RAG bağlamını ekle
        var systemPrompt = ModulePrompts.Get(module);
        if (chunks.Count > 0)
        {
            var context = string.Join("\n\n", chunks.Select((c, i) =>
                $"[Kaynak {i + 1}: {c.SourceName}]\n{c.Content}"));
            systemPrompt += "\n\nAşağıdaki bilgi tabanı parçalarını kullanarak cevap ver:\n" + context;
        }

        // 3) Geçmiş + yeni soruyu birleştir
        var messages = history.ToList();
        messages.Add(new AiMessage("user", userQuestion));

        // 4) AI'a gönder
        var request = new AiRequest(messages, systemPrompt);
        var response = await _aiProvider.CompleteAsync(request, ct);

        _logger.LogInformation(
            "Module={Module} Provider={Provider} InTok={In} OutTok={Out}",
            module, response.ProviderName, response.InputTokens, response.OutputTokens);

        return new AssistantReply(
            response.Content,
            response.InputTokens,
            response.OutputTokens,
            response.ProviderName,
            chunks.Select(c => c.SourceName).ToList());
    }
}

public sealed record AssistantReply(
    string Content,
    int InputTokens,
    int OutputTokens,
    string ProviderName,
    IReadOnlyList<string> Sources);
