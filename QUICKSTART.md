# Hızlı Başlangıç — Turkmen AI

## 1. Bu hafta yapacakların (sıralı)

### Gün 1: Kurulum
- [ ] .NET 8 SDK kur — https://dotnet.microsoft.com/download
- [ ] Visual Studio 2022 Community kur (ücretsiz). Workload'ları seç:
  - ASP.NET and web development
  - .NET desktop development
  - (Sonra) Mobile development with .NET (MAUI için)
- [ ] SQL Server LocalDB kur (VS ile gelir)
- [ ] Node.js 20+ kur — https://nodejs.org
- [ ] Bu klasörü VS'de aç: `backend/TurkmenAI.sln`

### Gün 2: Backend'i çalıştır
```bash
cd backend
dotnet restore
dotnet build
# EF Core tool yoksa: dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api
dotnet ef database update --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api
dotnet run --project TurkmenAI.Api
```
- [ ] Tarayıcıda `https://localhost:7001/swagger` aç
- [ ] `POST /api/assistant/ask` endpoint'ini Swagger'dan dene (mock cevap dönecek)

### Gün 3: Frontend'i çalıştır
```bash
cd web
npm install
npm run dev
```
- [ ] Tarayıcıda `http://localhost:3000` aç
- [ ] Bir modül seç, soru yaz, mock cevabı gör

### Gün 4: Gerçek AI ekle
- [ ] https://console.groq.com — ücretsiz hesap aç, API anahtarı oluştur
- [ ] `backend/TurkmenAI.Api/appsettings.json`:
  - `Ai:Provider` = `"groq"`
  - `Ai:Groq:ApiKey` = `"gsk_..."`
- [ ] Backend'i yeniden başlat, gerçek Türkmence cevabı al

### Gün 5-7: Anlama ve özelleştirme
- [ ] `docs/LEARNING_PATH.md` oku, C# temellerini gözden geçir
- [ ] `AssistantService.cs` ve `ModulePrompts.cs` üzerinde oyna — prompt'ları geliştir
- [ ] İlk bilgi belgeni `KnowledgeDocuments` tablosuna ekle (basit bir Türkmence kural)

## 2. Önümüzdeki ay

- Hafta 2: Kullanıcı kayıt/giriş (JWT auth)
- Hafta 3: Konuşma geçmişi (DB'ye kaydetme)
- Hafta 4: RAG için ilk gerçek dökümanları yükleme (Türkmen dilbilgisi PDF/Word'lardan parse)

## 3. Önemli Komutlar Cep Notu

```bash
# Backend
dotnet run --project backend/TurkmenAI.Api
dotnet ef migrations add YeniMigration --project backend/TurkmenAI.Infrastructure --startup-project backend/TurkmenAI.Api
dotnet ef database update --project backend/TurkmenAI.Infrastructure --startup-project backend/TurkmenAI.Api

# Web
cd web && npm run dev

# Git (henüz repo değilse)
cd "Turkmen AI"
git init && git add . && git commit -m "Initial scaffold"
```

## 4. Yardım

- Bu projedeki tüm dokümanları `docs/` klasöründe bul
- C#/SQL ile ilgili her takılma noktasında bana sor
- Strateji/ürün kararlarında yine bana sor

İyi şanslar. Başlangıcı yaptık, bundan sonra adım adım büyütüyoruz.
