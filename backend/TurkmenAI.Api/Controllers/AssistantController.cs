using Microsoft.AspNetCore.Mvc;
using TurkmenAI.Application.Modules;
using TurkmenAI.Domain.Ai;

namespace TurkmenAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssistantController : ControllerBase
{
    private readonly AssistantService _assistant;

    public AssistantController(AssistantService assistant)
    {
        _assistant = assistant;
    }

    /// <summary>
    /// Bir modüle (language/accounting/law/banking) Türkmence soru sor.
    /// </summary>
    [HttpPost("ask")]
    public async Task<ActionResult<AskResponse>> Ask(
        [FromBody] AskRequest req,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Question))
            return BadRequest("Soru boş olamaz.");

        var history = req.History?.Select(h => new AiMessage(h.Role, h.Content)).ToList()
                      ?? new List<AiMessage>();

        var reply = await _assistant.AskAsync(req.Module, req.Question, history, ct);

        return Ok(new AskResponse(
            Answer: reply.Content,
            Module: req.Module,
            Provider: reply.ProviderName,
            Sources: reply.Sources,
            InputTokens: reply.InputTokens,
            OutputTokens: reply.OutputTokens));
    }
}

public sealed record AskRequest(
    string Module,
    string Question,
    List<HistoryMessage>? History);

public sealed record HistoryMessage(string Role, string Content);

public sealed record AskResponse(
    string Answer,
    string Module,
    string Provider,
    IReadOnlyList<string> Sources,
    int InputTokens,
    int OutputTokens);
