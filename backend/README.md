# Turkmen AI Backend

ASP.NET Core 8 Web API. Clean Architecture (Domain / Application / Infrastructure / Api).

## Çalıştırma

### Önkoşullar
- .NET 8 SDK
- SQL Server LocalDB (Visual Studio ile gelir) veya SQL Server Express

### Adımlar
```bash
cd backend
dotnet restore
dotnet build

# Veritabanını oluştur (ilk seferde)
dotnet ef migrations add InitialCreate --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api
dotnet ef database update --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api

# Çalıştır
dotnet run --project TurkmenAI.Api
```

Tarayıcıda: `https://localhost:7001/swagger`

## İlk Endpoint Testi

`POST /api/assistant/ask`:
```json
{
  "module": "language",
  "question": "Türkmen dilinde isim hallary nähili?",
  "history": []
}
```

Mock provider aktifken sahte cevap döner. Gerçek AI için:
1. https://console.groq.com adresinden ücretsiz API anahtarı al
2. `appsettings.json` içinde `Ai:Provider` = `"groq"` ve `Ai:Groq:ApiKey` doldur
3. Tekrar çalıştır

## Proje Yapısı

- **TurkmenAI.Domain** — Saf C# entity'ler ve interface'ler. Hiçbir kütüphaneye bağımlı değil.
- **TurkmenAI.Application** — Uygulama mantığı (AssistantService, ModulePrompts).
- **TurkmenAI.Infrastructure** — EF Core, AI provider'lar, dış servisler.
- **TurkmenAI.Api** — Controllers, DI, HTTP katmanı.

## AI Provider Değiştirme (Faz 2 hazırlığı)

`Infrastructure/DependencyInjection.cs` içinde yeni bir `case` ekle:
```csharp
case "localllama":
    services.AddHttpClient<IAiProvider, LocalLlamaProvider>();
    break;
```

`LocalLlamaProvider` sınıfı kendi sunucundaki Ollama / vLLM endpoint'ine bağlanır.
Uygulamanın geri kalanı haberdar bile olmaz.
