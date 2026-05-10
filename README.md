# Turkmen AI — Stratejik Yol Haritası

> Türkmenistan vatandaşları için Türkmen dilinde dil/edebiyat, muhasebe, hukuk ve bankacılık konularında uzman yapay zekâ asistanı. Bağımsız altyapı, kapalı veri, ücretli hizmet modeli.

---

## 1. Vizyon

Türkmenistan halkına kendi dillerinde profesyonel düzeyde yardım veren — dilbilgisinden vergi beyanına, lisans almaktan banka işlemlerine kadar — bir AI platformu. Veriler **kimseyle paylaşılmaz**, tüm bilgi tabanı senin sunucunda kalır, gelir modeli abonelik / kullanım başına ödeme.

## 2. Ana İlkeler

1. **Veri bağımsızlığı**: Bilgi tabanı (RAG verisi) hiçbir 3. tarafa gitmez. SQL Server'da, senin sunucunda durur.
2. **AI sağlayıcı bağımsızlığı**: AI katmanı interface ile soyutlanır. Bugün API, yarın kendi self-hosted modelin — uygulama kodu değişmez.
3. **Küçük bütçe ile başla, büyüdükçe geçiş yap**: Faz 1'de API maliyeti düşük; gelir geldikçe Faz 2'de self-host modeline geç.
4. **Tek kişi yönetilebilir**: Senin başlangıçta tek başına yönetebileceğin sade bir mimari.

---

## 3. Faz Faz Plan

### Faz 0 — Hazırlık (1-2 hafta)
- C# + ASP.NET Core temellerini öğren (Java bildiğin için hızlı geçer)
- SQL Server temelleri
- Proje iskeletlerini oluştur (✅ bu dokümanla başlıyoruz)

### Faz 1 — MVP (1-3 ay) — Küçük bütçe
- Backend: ASP.NET Core Web API (C#)
- DB: SQL Server (kendi sunucunda, veri burada)
- Web: Next.js (TypeScript bildiğin için hızlı)
- Mobil: .NET MAUI (C# pratiği + tek kod tabanı iOS/Android)
- AI: **Provider pattern** ile soyutlanmış. Başlangıçta:
  - Çeviri ve hafif görevler için: ücretsiz/ucuz API (örn. Gemini Flash, Groq)
  - Karmaşık görevler: OpenAI/Anthropic API (data retention=OFF, zero-retention anlaşması ile)
- RAG: SQL Server + ücretsiz embedding modeli (örn. multilingual-e5, kendi sunucunda)
- Maliyet: ~$50-150/ay (sunucu + API)

> **Önemli not**: API kullanmak "verini paylaşmak" demek DEĞİL — kullanıcının sorduğu soruyu gönderirsin, modelin senin bilgi tabanın yok. Hassas içerik için bile OpenAI/Anthropic ile **zero data retention** anlaşması yapılabilir. Yine de Faz 2'de tamamen kapatacağız.

### Faz 2 — Bağımsızlaşma (3-9 ay) — Gelir başladıktan sonra
- Self-hosted LLM: Llama 3.1 8B / Qwen 2.5 7B / Aya-23 (multilingual) — kendi GPU sunucunda
- Sunucu: 1x RTX 4090 veya A6000'li VPS (~$300-500/ay)
- AI provider değişir, uygulama kodu aynı kalır (Provider pattern sayesinde)
- Veriler ve modelin tamamı sende

### Faz 3 — Türkmence Uzmanlaşma (9-18 ay)
- Türkmence veri seti topla (kitap, gazete, mevzuat, sözlük)
- Mevcut açık kaynak modeli **fine-tune** et (LoRA ile, görece ucuz)
- Eşsiz bir Türkmence modelin olur — rakipsiz

### Faz 4 — Ölçek
- Mobil uygulamada öne çıkış, B2B (firma muhasebe danışmanlığı)
- Hükümet/kurumsal anlaşmalar
- API olarak başka ürünlere sat

---

## 4. Ürün Modülleri

| Modül | İçerik | Öncelik |
|-------|--------|---------|
| **Dil ve Edebiyat** | Türkmence dilbilgisi, çeviri, edebiyat analizi, kompozisyon yardımı, sözlük | 🟢 İlk |
| **Muhasebe** | Defter tutma, fatura, vergi beyanı, KDV, gelir/gider takibi (Türkmenistan mevzuatı) | 🟡 İkinci |
| **Hukuk ve Lisans** | Lisans/izin alma süreçleri, sözleşme hazırlama, hukuki danışmanlık | 🟡 İkinci |
| **Bankacılık** | Banka işlemleri, kredi, transfer, döviz, mevzuat | 🟠 Üçüncü |
| **Ses ve Şarkı** | TTS (text-to-speech), şarkı sözü yazma. Bu daha sonra. | 🔵 İleride |

> **Strateji**: Modülleri **paralel altyapıyla** kur (aynı veritabanı, aynı backend, aynı UI iskeleti) ama içerik/veri toplamayı sırayla yap. Hepsi aynı anda yayın yapması şart değil — biri hazır olunca aç.

---

## 5. Gelir Modeli

- **Ücretsiz katman**: Günde 5 soru (deneme)
- **Bireysel abonelik**: Aylık ~50-100 TMT, sınırsız soru, tek modül
- **Premium**: Aylık ~200 TMT, tüm modüller, mobil + web
- **İşletme**: Aylık ~500-2000 TMT, çoklu kullanıcı, API erişimi
- **Ödeme**: Yerel banka kartı entegrasyonu (Rysgal, Halkbank vb.) + uluslararası için Stripe

---

## 6. Teknoloji Yığını (Karar)

```
┌─────────────────────────────────────────────────┐
│  Web (Next.js + TS)    Mobil (.NET MAUI / C#)   │
└──────────────────┬──────────────────┬───────────┘
                   │                  │
                   └──────┬───────────┘
                          │ HTTPS
                ┌─────────▼──────────┐
                │  ASP.NET Core API  │
                │  (C# 12, .NET 8)   │
                └─────┬──────────┬───┘
                      │          │
              ┌───────▼──┐   ┌───▼─────────┐
              │ SQL      │   │ AI Provider │
              │ Server   │   │ (Interface) │
              │ (Veri)   │   └──┬───┬──────┘
              └──────────┘      │   │
                           ┌────▼─┐ ▼─────────┐
                           │ API  │ Self-host │
                           │ (F1) │ (F2+)     │
                           └──────┘└──────────┘
```

## 7. Klasör Yapısı

```
Turkmen AI/
├── README.md (bu dosya)
├── docs/
│   ├── ARCHITECTURE.md
│   ├── LEARNING_PATH.md      (C# + SQL öğrenme planı)
│   └── DATABASE_SCHEMA.md
├── backend/                  (ASP.NET Core solution)
│   ├── TurkmenAI.Api/
│   ├── TurkmenAI.Application/
│   ├── TurkmenAI.Domain/
│   └── TurkmenAI.Infrastructure/
├── web/                      (Next.js)
└── mobile/                   (MAUI - sonra)
```

## 8. Sıradaki Adım

1. `docs/ARCHITECTURE.md` ve `docs/LEARNING_PATH.md` dosyalarını oku
2. .NET 8 SDK ve SQL Server'ı kur (talimatlar `LEARNING_PATH.md`'de)
3. Backend iskeletini incele (`backend/` klasörü)
4. İlk endpoint'i çalıştır

---

**Lisans / Gizlilik**: Bu proje sana ait. Hiçbir kod, veri, model dış kaynağa açılmıyor. Tüm IP senin.
