# Turkmen AI Mobile (.NET MAUI)

Tek C# kod tabanından **iOS + Android** uygulaması. C# pratiği yaparken hem mobil çıkarsın.

## Neden MAUI?

| Seçenek | Artısı | Eksisi |
|---------|--------|--------|
| **.NET MAUI** | C# bildiğin için tek dil; backend ile kod paylaşımı | Topluluk React Native kadar büyük değil |
| React Native | TS bildiğin için kolay | Bridge sorunları, native UI farklılıkları |
| Flutter | Hızlı UI | Dart öğrenmen gerek |

Senin durumun için MAUI mantıklı çünkü:
1. Backend zaten C# — DTO'ları paylaşabilirsin
2. C# öğrenmek için tek proje
3. Visual Studio + tek tıkla iOS/Android build

## Geliştirme Aşamaları

### Aşama 1 — Önce web'i tamamla (önerilen)
MVP'yi web olarak çıkar, kullanıcı topla, gelir gelsin. Sonra mobile başla.
Sebep: Mobil store'a koymak (Google Play 25$, Apple 99$/yıl) ve test cihazı maliyetli.

### Aşama 2 — MAUI projeyi kur
```bash
# Visual Studio 2022'de "Mobile development with .NET" workload kur
# Yeni proje: .NET MAUI App, isim: TurkmenAI.Mobile
```

Veya CLI ile:
```bash
dotnet workload install maui
dotnet new maui -n TurkmenAI.Mobile -o mobile/TurkmenAI.Mobile
```

### Aşama 3 — Ekran yapısı

```
TurkmenAI.Mobile/
├── App.xaml(.cs)
├── AppShell.xaml(.cs)         ← Tab navigation
├── Pages/
│   ├── LoginPage.xaml
│   ├── ChatPage.xaml          ← Ana ekran (modül seçici + chat)
│   ├── HistoryPage.xaml       ← Konuşma geçmişi
│   └── ProfilePage.xaml       ← Abonelik, ayarlar
├── ViewModels/
│   ├── ChatViewModel.cs
│   └── ...
├── Services/
│   └── TurkmenAiApiClient.cs  ← Backend ile konuşur
└── Models/
    └── Module.cs, Message.cs, ...
```

### Aşama 4 — Paylaşılan kod
`TurkmenAI.SharedDtos` adında bir class library oluştur, hem backend hem mobile referans versin.
DTO'lar (AskRequest, AskResponse) burada yaşar — duplicate kod yok.

## İlk Sprint için Backlog
1. .NET MAUI workload kur, boş proje oluştur, telefonda "Hello World" çalıştır
2. `TurkmenAiApiClient` — HttpClient ile `/api/assistant/ask` çağrısı
3. `ChatPage` — basit liste + input + buton
4. Modül seçici (segmented control)
5. Yerel SQLite cache (offline çalışma için)
6. Push notifications (sonraki sprintte)

## Maliyet Notu

Mobil yayına alma:
- Google Play Developer: **$25 tek seferlik**
- Apple Developer Program: **$99/yıl**

Web tarafı gelir getirene kadar mobil çıkışı erteleyebilirsin.
