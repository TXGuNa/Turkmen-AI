# C# + SQL Öğrenme Yol Haritası (Java/JS/TS bilen biri için)

Java biliyorsan C# %80 tanıdık gelir. Birkaç gün içinde rahatça yazabilirsin. Bu plan **projeyle paralel** ilerlemen için tasarlandı: her gün biraz öğren, biraz da gerçek projede uygula.

## Hafta 0 — Kurulum (1 gün)

### Kurulması gerekenler:
1. **.NET 8 SDK** — https://dotnet.microsoft.com/download
2. **Visual Studio 2022 Community** (ücretsiz) veya **JetBrains Rider** (Java'dan biliyorsan)
3. **SQL Server 2022 Developer Edition** (ücretsiz) — https://www.microsoft.com/sql-server/sql-server-downloads
4. **SQL Server Management Studio (SSMS)** veya **Azure Data Studio**
5. **Git** (büyük ihtimalle var)

### Doğrulama:
```bash
dotnet --version    # 8.0.x görmeli
sqlcmd -?            # SQL Server CLI çalışıyorsa OK
```

## Hafta 1 — C# Temelleri (Java karşılaştırmalı)

### Java vs C# Hızlı Geçiş Notları:
| Java | C# | Not |
|------|-----|-----|
| `System.out.println()` | `Console.WriteLine()` | |
| `String` | `string` (alias of `String`) | |
| `ArrayList<T>` | `List<T>` | |
| `HashMap<K,V>` | `Dictionary<K,V>` | |
| `@Override` | `override` (zorunlu keyword) | |
| `final` | `readonly` veya `const` | |
| `package` | `namespace` | |
| `import` | `using` | |
| Maven/Gradle | NuGet + `dotnet` CLI | |
| Spring Boot | ASP.NET Core | Çok benzer felsefe |

### Öğrenmen gereken C# özgün özellikleri:
- **Properties**: `public string Name { get; set; }` (Java'da getter/setter)
- **LINQ**: SQL benzeri sorgular — `list.Where(x => x.Age > 18).Select(x => x.Name)`
- **async/await**: JS gibi (Java'da CompletableFuture'a benzer ama daha temiz)
- **Records**: `public record User(string Name, int Age);` (immutable veri sınıfları)
- **Nullable reference types**: `string?` ve `string` farkı

### Ödev:
- Microsoft Learn'in C# fundamentals ücretsiz: https://learn.microsoft.com/en-us/training/paths/csharp-first-steps/
- 2-3 saat yeterli, geri kalanını projede uygulayarak öğreneceksin

## Hafta 2 — ASP.NET Core Web API

Spring Boot biliyorsan bu çok tanıdık gelecek. Karşılaştırma:

| Spring Boot | ASP.NET Core | Not |
|-------------|--------------|-----|
| `@RestController` | `[ApiController]` + Controller class | |
| `@GetMapping` | `[HttpGet]` | |
| `@Autowired` | Constructor injection (DI built-in) | |
| `application.properties` | `appsettings.json` | |
| Spring Data JPA | Entity Framework Core | ORM |
| `@Service` | DI'da `AddScoped<T,Impl>()` | |

### Ödev:
- Microsoft Learn ASP.NET Core: https://learn.microsoft.com/en-us/training/paths/create-web-api-with-aspnet-core/
- Bu projedeki `backend/` klasörünü incele, çalıştır

## Hafta 3 — SQL + Entity Framework Core

SQL bilmiyorsan önce SQL temelleri (1-2 gün yeterli):

### Mutlaka öğren:
- `SELECT, FROM, WHERE, JOIN (INNER/LEFT), GROUP BY, ORDER BY, HAVING`
- `INSERT, UPDATE, DELETE`
- `PRIMARY KEY, FOREIGN KEY, INDEX`
- Subqueries ve CTEs (Common Table Expressions)

### Önerdiğim kaynak:
- SQLBolt — https://sqlbolt.com/ (interaktif, 1-2 saat)
- Mode SQL Tutorial — https://mode.com/sql-tutorial/

### Sonra EF Core:
- Code-First migration
- DbContext kavramı (Java JPA EntityManager gibi)
- LINQ ile sorgu (SQL üretiyor altta)
- Eager vs Lazy loading

### Ödev:
- `DATABASE_SCHEMA.md`'yi oku
- Backend projesindeki `DbContext` ve entity sınıflarını incele

## Hafta 4 — AI Entegrasyonu

Bu aşamada artık projenin gerçek kalbine geliyoruz:
- HTTP istemcileri (HttpClient)
- JSON serialization (System.Text.Json)
- OpenAI/Groq/Anthropic SDK'ları
- Streaming responses (Server-Sent Events)
- Embedding hesaplama

## Hafta 5+ — Frontend + Mobil

- Next.js: TypeScript bildiğin için sorun yok, 1 hafta yeterli
- .NET MAUI: C# bildiğin için sorun yok, ama XAML UI öğrenmek gerek (Android XML'e benzer)

---

## Günlük Pratik Önerisi

- **30 dk teori** (video / makale)
- **60 dk gerçek projede kod yaz**
- **30 dk eski koda dön, refactor et / anla**

Toplam günlük 2 saat = ayda 60 saat = ciddi ilerleme.

## Hata Yapma Korkusu

Bu proje **senin için**. Hata yap, geri al, tekrar yaz. Bu nasıl öğrenildiğinin tek yolu. Java/JS bildiğin için çok hızlı öğreneceksin.

## En Önemli Tek Şey

**Her gün biraz** — günde 30 dk düzenli > haftada 8 saat dağınık.
