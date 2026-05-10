import "./globals.css";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "Türkmen AI",
  description: "Türkmen dilinde dil, muhasebe, hukuk ve bankacılık asistanı"
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="tk">
      <body className="min-h-screen bg-neutral-50 text-neutral-900 antialiased">
        {children}
      </body>
    </html>
  );
}
