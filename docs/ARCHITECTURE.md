# Mimari Dokümanı — Turkmen AI

## 1. Genel Bakış

Clean Architecture / Onion mimarisi kullanıyoruz. Bu, **bağımsızlık** ilkene en uygun mimaridir: AI sağlayıcısını değiştirsen, veritabanını değiştirsen, mobil UI değişse bile **iş mantığın** asla değişmez.

## 2. Katmanlar

```
┌──────────────────────────────────────────────────────────┐
│  TurkmenAI.Api  (Controllers, Endpoints, Auth, DI)       │  ← HTTP katmanı
├──────────────────────────────────────────────────────────┤
│  TurkmenAI.Application  (Use Cases, DTO, Services)       │  ← İş kuralları
├──────────────────────────────────────────────────────────┤
│  TurkmenAI.Domain  (Entities, Value Objects, Interfaces) │  ← Çekirdek
├──────────────────────────────────────────────────────────┤
│  TurkmenAI.Infrastructure  (SQL, AI providers, External) │  ← Dış dünya
└──────────────────────────────────────────────────────────┘
```

**Kural**: İçteki katman dıştakini tanımaz. Domain hiçbir şeyi import etmez. Bu sayede AI sağlayıcı, DB, hatta tüm web framework'ü değişebilir.

## 3. AI Provider Pattern (En Kritik Karar)

```csharp
// Domain katmanında - sadece interface
public interface IAiProvider
{
    Task<AiResponse> CompleteAsync(AiRequest request, CancellationToken ct);
    Task<float[]> EmbedAsync(string text, CancellationToken ct);
}

// Infrastructure katmanında - 3 farklı implementation
public class OpenAiProvider : IAiProvider { /* Faz 1 */ }
public class GroqProvider : IAiProvider { /* Faz 1, ucuz */ }
public class LocalLlamaProvider : IAiProvider { /* Faz 2, self-hosted */ }

// Hangi provider kullanılacağını appsettings.json ile değiştir
// Kod değişikliği YOK
```

Bu pattern sayesinde Faz 2'de self-hosted modele geçerken **tek bir satır config değiştireceksin**. Uygulamanın geri kalanı haberdar bile olmayacak.

## 4. RAG (Retrieval-Augmented Generation) Mimarisi

RAG, AI'ın **senin verinle** cevap vermesini sağlar. Bu senin gerçek değerin.

```
Kullanıcı sorusu
      │
      ▼
[Embedding üret] ──→ Vektörü hesapla
      │
      ▼
[SQL Server'da ara] ──→ En yakın 5 doküman parçası
      │
      ▼
[Prompt'a ekle] ──→ "Bu bilgilere dayanarak cevap ver: ..."
      │
      ▼
[AI Provider çağır] ──→ Cevap üret
      │
      ▼
Kullanıcıya cevap
```

### RAG Veri Depolaması

SQL Server 2022 **vektör arama** desteği var (VECTOR datatype, COSINE distance). Eğer eski sürümdeysek:
- Seçenek A: PostgreSQL + pgvector (daha popüler RAG için)
- Seçenek B: SQL Server + harici vektör indeks (Qdrant, Milvus)
- Seçenek C: SQL Server'da float[] olarak sakla, küçük veri için yeterli (10K dökümana kadar)

**Karar**: MVP için Seçenek C ile başla, sonra ölçek arttıkça Qdrant ekleriz.

## 5. Modül Yapısı

Her modül (Dil, Muhasebe, Hukuk, Banka) aynı şablonu kullanır:

```
Application/
  Modules/
    Language/
      LanguageAssistantService.cs
      Documents/        (sözlükler, dilbilgisi kuralları)
    Accounting/
      AccountingAssistantService.cs
      Documents/        (vergi mevzuatı, fatura kuralları)
    Law/
      LawAssistantService.cs
      Documents/        (kanunlar, lisans süreçleri)
    Banking/
      BankingAssistantService.cs
      Documents/        (banka prosedürleri)
```

Her modülün:
- Kendi bilgi tabanı (RAG için)
- Kendi sistem promptu (uzmanlık alanı)
- Kendi prompt zinciri (chain)

Ama tamamı **aynı `IAiProvider` ve `IRagService`'i kullanır**. DRY prensibi.

## 6. Veri Güvenliği

| Veri | Nerede saklanır | Erişim |
|------|-----------------|--------|
| Bilgi tabanı (mevzuat, sözlük vb.) | SQL Server (senin sunucun) | Sadece backend |
| Kullanıcı bilgileri | SQL Server | Hashlenmiş, GDPR uyumlu |
| Konuşma geçmişi | SQL Server | Şifrelenmiş (AES-256 at rest) |
| API anahtarları | Azure Key Vault / .env | Sadece backend |
| AI'a gönderilen istek | Geçici | Provider'a göre (Faz 2'de hiç çıkmaz) |

## 7. Ölçeklenebilirlik

MVP tek sunucuda çalışır. Büyüdükçe:

1. **Load balancer** + 2-3 API sunucusu
2. **Redis** cache (sık sorulan sorular)
3. **Background job** queue (RAG indexleme için Hangfire)
4. **CDN** (Cloudflare) — statik içerik
5. **Read replica** SQL Server

## 8. Deployment

- **Geliştirme**: Senin makinende (Visual Studio / Rider + SQL Server LocalDB)
- **Production**: Hetzner / DigitalOcean / Azure (Türkiye/Avrupa lokasyonu)
- **Container**: Docker (gelecekte K8s kolaylaştırır)
- **CI/CD**: GitHub Actions ücretsiz, başlangıç için yeterli

## 9. İlk Sprint'te Yapılacaklar

1. Solution iskeletini kur (✅ hazır geliyor)
2. SQL Server bağlantısı (EF Core)
3. Basit "hello world" endpoint
4. `IAiProvider` interface ve mock implementation
5. İlk modül: Language — basit bir Türkmence soru-cevap endpoint
6. Next.js'te basit chat UI

Hepsi tamamlandığında çalışan bir prototipin olur — sonra üzerine inşa edersin.
