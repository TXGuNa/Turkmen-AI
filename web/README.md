# Turkmen AI Web

Next.js 14 + TypeScript + Tailwind CSS.

## Çalıştırma

```bash
cd web
npm install
npm run dev
```

Tarayıcıda: http://localhost:3000

Backend `https://localhost:7001` adresinde çalışmalı. Farklı port için
`NEXT_PUBLIC_API_URL` environment değişkenini ayarla.

## Yapı

```
src/app/
  layout.tsx     — kök layout (Türkmen dili, font, meta)
  globals.css    — Tailwind
  page.tsx       — Ana sayfa: modül seçici + chat UI
```

## Sonraki Adımlar

- Kullanıcı kayıt / giriş sayfası (`/auth/login`, `/auth/register`)
- Konuşma geçmişi paneli (`/conversations`)
- Abonelik / ödeme sayfası (`/billing`)
- Markdown render (asistan cevabını biçimlendirmek için `react-markdown`)
- Streaming cevap (Server-Sent Events)
